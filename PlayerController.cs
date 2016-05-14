using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GunController))]
[RequireComponent(typeof(InventoryController))]
//[RequireComponent(typeof(AudioSource))]

public class PlayerController : LivingEntity
{
    [SerializeField]
    private bool m_IsWalking;
    [SerializeField]
    private float m_WalkSpeed;
    [SerializeField]
    private float m_SprintSpeed;
    [SerializeField]
    private float m_GravityMultiplier;
    [SerializeField]
    private float m_PushPower;
    [SerializeField]
    private Mouse m_Mouse;
    [SerializeField]
    private float m_JumpSpeed;

    //[SerializeField]
    //private AudioClip m_LandSound;

    private Camera m_Camera;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private GunController m_GunController;
    private InventoryController m_InventoryController;
    private CollisionFlags m_CollisionFlags;

    //used for returning from things like crouching
    private Vector3 m_OriginalWeaponHold;
    private Vector3 m_OriginalCameraPosition;

    private bool m_PreviouslyGrounded;
    private bool m_Jump;
    private bool m_Jumping;

    private float damage = 1;

    //crouching
    private bool m_Crouch = false;
    
    //private AudioSource m_AudioSource;

    protected override void Start()
    {
        //base class (living entity) start method
        base.Start();        

        //begin by acquiring reference to the attached objects CharacterController
        m_CharacterController = GetComponent<CharacterController>();
        m_GunController = GetComponent<GunController>();
        m_InventoryController = GetComponent<InventoryController>();
        m_Camera = Camera.main;

        //original placement of children
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_OriginalWeaponHold = m_GunController.weaponHold.transform.localPosition;

        m_Mouse.Init(transform, m_Camera.transform);
        m_IsWalking = true;
        m_Jumping = false;

        //m_AudioSource = GetComponent<AudioSource>();
    }

    //update checks 
    private void Update()
    {
        RotateView();

        //MouseButton0
        if (Input.GetMouseButtonDown(0))
        {
            m_GunController.OnTriggerHold();

        }
        if (Input.GetMouseButtonUp(0))
        {
            m_GunController.OnTriggerRelease();
        }

        //MouseButton1
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, 100f))
            {
                print("Found an object at distance: " + hit.distance);
                OnUseObject(hit.collider, hit.point);
                OnHitObject(hit.collider, hit.point);
            }

            Debug.DrawRay(m_Camera.transform.position, m_Camera.transform.forward, Color.green * 20f, 0.3f);
            Debug.Log("Pressed right click.");
        }

        //flashlight
        if (Input.GetKeyDown(KeyCode.F))
        {
            m_GunController.ToggleFlashlight();
        }

        //use
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, 2))
            {
                print("Found an object at distance: " + hit.distance);

                OnUseObject(hit.collider, hit.point);


                //Debug.DrawRay(m_Camera.transform.position, m_Camera.transform.forward * 2, Color.yellow, 0.3f);
                //Debug.Log("Pressed 'E' and Hit with Raycast.");
            }
        }

        //reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_GunController.Reload();
        }

        //sprinting
        if (Input.GetKey(KeyCode.LeftShift) && !m_Crouch)
        {
            m_IsWalking = false;
        }
        else
        {
            m_IsWalking = true;
        }

        //crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            m_Crouch = !m_Crouch;
            Crouch(m_Crouch);
        }

        //jumping
        if (!m_Jump)
        {
            m_Jump = Input.GetKeyDown(KeyCode.Space);
        }

        //jumping
        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            //PlayLandingSound();
            m_MoveDir.y = 0f;
            m_Jumping = false;
        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;

        //inventory checks
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            m_InventoryController.ActivateSlot(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            m_InventoryController.ActivateSlot(2);
        }
    }


    //character controller movement
    private void FixedUpdate()
    {
        //get our input * speed
        float speed;
        GetInput(out speed);

        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                           m_CharacterController.height / 2f, ~0, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;

        //if conditions are met to be able to jump
        if (m_CharacterController.isGrounded && m_Jump)
        {
            m_MoveDir.y = m_JumpSpeed;
            m_Jump = false;
            m_Jumping = true;
        }

        //moving, including jump portion
        m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        //mouse update for camera position and whether we unlock cursor
        UpdateCameraPosition(speed);
        m_Mouse.UpdateCursorLock();
    }

    /*private void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
    }*/

    
    //move the camera with our controller
    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;

        newCameraPosition = m_Camera.transform.localPosition;

        m_Camera.transform.localPosition = newCameraPosition;

    }

    //movement input
    private void GetInput(out float speed)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (!m_IsWalking)
        {
            speed = m_SprintSpeed;
        }
        else
        {
            speed = m_WalkSpeed;
        }

        m_Input = new Vector2(horizontal, vertical);

        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }
    }

    //rotate the view by calling mouse.lookrotation
    private void RotateView()
    {
        m_Mouse.LookRotation(transform, m_Camera.transform);
    }

    //check for controller collisions
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        
        //how much force we add to a body by pushing, can be 0
        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }

    //crouching
    private void Crouch(bool crouch)
    {
        if (crouch)
        {
            m_CharacterController.height = 1.0f;
            m_Camera.transform.localPosition = Vector3.zero;

            //gun crouch
            m_GunController.weaponHold.localPosition = new Vector3 (m_OriginalWeaponHold.x, -0.2f, m_OriginalWeaponHold.z);
        }
        else
        {
            m_CharacterController.height = 1.8f;
            m_Camera.transform.localPosition = m_OriginalCameraPosition;

            //gun crouch
            m_GunController.weaponHold.localPosition = m_OriginalWeaponHold;
        }
    }

    /*void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }*/

    public void OnHitObject(Collider c, Vector3 hitPoint)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hitPoint, transform.forward);
        }
    }

    void OnUseObject(Collider c, Vector3 hitPoint)
    {
        IUsable usableObject = c.GetComponent<IUsable>();
        if(usableObject != null)
        {
            usableObject.TakeUseHit(hitPoint, transform.forward);
        }
    }
}
