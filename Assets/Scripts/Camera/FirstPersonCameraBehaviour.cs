using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraBehaviour : CameraBehaviour
{
    public float CameraHorizPosEaseSpeed = 5.0f;
    public float CameraVertPosEaseSpeed = 2.0f;
    public float LookPosEaseSpeed = 5.0f;

    public Vector3 PlayerMaxDistLocalLookPos = Vector3.zero;
    public Vector3 PlayerMinDistLocalLookPos = Vector3.zero;

    public Vector3 PlayerLocalPivotPos = Vector3.zero;

    public float YawRotateSpeed = 1.0f;
    public float PitchRotateSpeed = 1.0f;
    public float MaxVerticalAngle = 89.0f;

    public float MaxDistFromPlayer = 5.0f;
    public float MinHorizDistFromPlayer = 4.0f;
    public float AutoRotateDelayTime = 1.0f;

    public FirstPersonCameraBehaviour()
    {

    }

    public override void Activate()
    {
        base.Activate();

        m_GoalPos = m_Camera.transform.position;
        m_AllowAutoRotate = false;
        m_TimeTillAutoRotate = AutoRotateDelayTime;
        m_XRotation = m_Camera.transform.rotation.eulerAngles.x;
        m_YRotation = m_Camera.transform.rotation.eulerAngles.y;
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    public override void UpdateRotation(float yawAmount, float pitchAmount)
    {
        m_YawInput = yawAmount;
        m_PitchInput = pitchAmount;    
    }

    public override void SetFacingDirection(Vector3 direction)
    {

    }

    public override Vector3 GetControlRotation()
    {
        return base.GetControlRotation();
    }

    public override bool UsesStandardControlRotation()
    {
        return false;
    }

    public override void UpdateCamera()
    {
        //Position
        m_GoalPos = m_Player.transform.position + (Vector3.up * 0.5f);
        
        m_Camera.transform.position = m_GoalPos;

        bool isColorWheelOpen = m_Player.Controller.IsOpeningColorMenu();
        if (isColorWheelOpen) { return; }

        //Simple Rotation
        float mouseX = m_YawInput * YawRotateSpeed;
        float mouseY = m_PitchInput * PitchRotateSpeed;

        m_YRotation += mouseX;
        m_XRotation += mouseY;
        m_XRotation = Mathf.Clamp(m_XRotation, -MaxVerticalAngle, MaxVerticalAngle);

        Vector3 targetCameraRot = new Vector3(m_XRotation, m_YRotation, 0f);

        m_Camera.transform.eulerAngles = targetCameraRot;

    }

    Vector3 m_GoalPos;

    float m_YawInput;
    float m_PitchInput;

    float m_XRotation;
    float m_YRotation;

    float m_TimeTillAutoRotate;
    bool m_AllowAutoRotate;
}
