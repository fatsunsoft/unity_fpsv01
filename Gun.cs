using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    //enum for our ways to fire a gun, damage of the gun
    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode;
    public float damage;

    //where we shoot from, what we shoot, delay, and speed of the shot
    public Transform[] muzzles;
    public float msShotDelay = 100;
    public int burstCount;
    public int shellsPerClip;

    //public Transform shell;
    public Transform shellPort;

    //effects
    //MuzzleFlash muzzleFlash;
    //public AudioClip shootAudio;

    //private float for when we shoot again
    float nextShotTime;
    bool triggerReleasedSinceShot;
    int burstShotsRemaining;
    int shellsInClip;

    Camera playerCamera;

    void Start()
    {
        //where we cast the ray from
        playerCamera = Camera.main;

        //muzzleFlash = GetComponent<MuzzleFlash>();

        //the number of shots remaining to fire is equal to the total number of shots to fire
        burstShotsRemaining = burstCount;
        shellsInClip = shellsPerClip;
    }

    //Shoot the Gun
    void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            if (fireMode == FireMode.Burst)
            {
                if (burstShotsRemaining == 0)
                {
                    return;
                }
                burstShotsRemaining--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceShot)
                {
                    return;
                }
            }
            for (int i = 0; i < muzzles.Length; i++)
            {
                //set next shot time
                nextShotTime = Time.time + msShotDelay / 1000;

                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100f))
                {
                    //Debug.Log("Found an object at distance: " + hit.distance);

                    OnHitObject(hit.collider, hit.point);
                }

                Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward, Color.green, .1f);
            }
            

            //Graphic - shell and muzzle flash
            //Instantiate(shell, shellPort.position, shellPort.rotation);
            //muzzleFlash.Activate();

            //Audio - gunshot audio
            //AudioManager.instance.PlaySound(shootAudio, transform.position);
        }       
    }

    //Trigger - Holding
    public void OnTriggerHold()
    {
        if (shellsInClip > 0)
        {
            Shoot();

            //subtract a shell
            shellsInClip--;

            Debug.Log("Shot: " + shellsInClip + " shells left in clip");
        }
        else if (shellsInClip == 0)
        {
            Reload();
        }
        
        // we have not release the trigger
        triggerReleasedSinceShot = false;
    }

    //Trigger - Release
    public void OnTriggerRelease()
    {
        // we have release the trigger
        triggerReleasedSinceShot = true;

        //reset the burst shots remaining back to the total burst value
        burstShotsRemaining = burstCount;
    }

    //Reload
    public void Reload()
    {
        shellsInClip = shellsPerClip;

        Debug.Log("Reloaded");
    }

    //Hit - deal hit and damage
    void OnHitObject(Collider c, Vector3 hitPoint)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hitPoint, transform.forward);
        }
    }
}
