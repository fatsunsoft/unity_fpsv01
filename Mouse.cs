using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Mouse
{
    //public vars
    public float XSensitivity = 1.0f;
    public float YSensitivity = 1.0f;
    public bool clampVerticalRotation = true;
    public float MinX = -90f;
    public float MaxX = 90f;
    public bool lockCursor = true;

    //internal vars
    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private bool m_IsCursorLocked = true;

    //same as awake, get our player and camera
    public void Init(Transform character, Transform camera)
    {
        m_CharacterTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;
    }

    //rotate the view
    public void LookRotation(Transform character, Transform camera)
    {
        if (m_IsCursorLocked)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            //clamping
            if (clampVerticalRotation)
            {
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
            }

            character.localRotation = m_CharacterTargetRot;
            camera.localRotation = m_CameraTargetRot;
        }

        //update our cursor lock if we need it
        UpdateCursorLock();
    }


    //sets the cursor lock mode
    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //update if we need to check for a new cursor lock
    public void UpdateCursorLock()
    {
        if (lockCursor)
        {
            InternalLockUpdate();
        }
    }

    //control cursor lock and visibility
    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_IsCursorLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_IsCursorLocked = true;
        }

        if (m_IsCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_IsCursorLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //clamps rotation
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinX, MaxX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}