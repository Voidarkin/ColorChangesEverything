using System;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class DefaultController : IController
{
    public MainCamera ControlCamera { get; private set; }
    public void Init(Player player)
    {
        ControlCamera = Camera.main.GetComponent<MainCamera>();

        if(player != null)
            ControlCamera.SetPlayer(player);
    }

    public MainCamera GetMainCamera()
    {
        return ControlCamera;
    }

    public void UpdateControls()
    {
        if (ControlCamera == null)
        {
            return;
        }
        //UpdateMouseControlToggle();
        Vector3 lookInput = GetLookInput();
        ControlCamera.UpdateRotation(lookInput.y, lookInput.x);
    }

    public void SetFacingDirection(Vector3 direction)
    {
        ControlCamera.SetFacingDirection(direction);
    }

    public void AddLedgeDir(Vector3 ledgeDir)
    {

    }

    public Vector3 GetControlRotation()
    {
        return ControlCamera.ControlRotation;
    }

    public Vector3 GetMoveInput()
    {
        return new Vector3(
            Input.GetAxis("Horizontal"),
            0.0f,
            Input.GetAxis("Vertical")
            );
    }

    public Vector3 GetLookInput()
    {
        if (!m_EnableMouseControl)
        {
            return Vector3.zero;
        }

        Vector3 lookInput = new Vector3(
            Input.GetAxis("Mouse Y"),
            Input.GetAxis("Mouse X"),
            0.0f
            );

        //Prevent from starting Inverted
        lookInput.x *= -1;

        ////Start Invert
        //bool xInvert = DataManager.Instance.Options.GetInvertMouseX();
        //bool yInvert = DataManager.Instance.Options.GetInvertMouseY();

        //if (xInvert) { lookInput.y *= -1; }
        //if (yInvert) { lookInput.x *= -1; }

        return lookInput;
    }

    public Vector3 GetAimTarget()
    {
        return Vector3.zero;
    }

    public bool IsJumping()
    {
        return Input.GetButton("Jump");
    }

    public bool IsFiring()
    {
        if (!m_EnableMouseControl)
        {
            return false;
        }

        return Input.GetButton("Fire1");
    }
    public bool IsAttacking()
    {
        if (!m_EnableMouseControl)
        {
            return false;
        }

        return Input.GetButtonDown("Attack");
    }

    //public bool IsAiming()
    //{
    //    return Input.GetButton("Aim");
    //}

    public bool IsInteracting()
    {
        return Input.GetButtonDown("Interact");
    }

    public bool ToggleCrouch()
    {
        return Input.GetKeyDown(KeyCode.C);
    }

    public bool ToggleSprint()
    {
        return Input.GetButton("Sprint");
    }

    public bool ToggleGroundPound()
    {
        return Input.GetButton("GroundPound");
    }

    public bool ToggleMenu()
    {
        return Input.GetButtonDown("Menu");
    }
    public bool SwitchToItem1()
    {
        //TODO: Getting input from the keyboard directly is convenient for getting features in quickly for prototyping etc.
        //      but it isn't usually ideal for final products.  This should be changed to use Unity's regular input system.
        return Input.GetKeyDown(KeyCode.Alpha1);
    }

    public bool SwitchToItem2()
    {
        //TODO: Getting input from the keyboard directly is convenient for getting features in quickly for prototyping etc.
        //      but it isn't usually ideal for final products.  This should be changed to use Unity's regular input system.
        return Input.GetKeyDown(KeyCode.Alpha2);
    }

    public bool SwitchToItem3()
    {
        //TODO: Getting input from the keyboard directly is convenient for getting features in quickly for prototyping etc.
        //      but it isn't usually ideal for final products.  This should be changed to use Unity's regular input system.
        return Input.GetKeyDown(KeyCode.Alpha3);
    }
    public bool SwitchToItem4()
    {
        //TODO: Getting input from the keyboard directly is convenient for getting features in quickly for prototyping etc.
        //      but it isn't usually ideal for final products.  This should be changed to use Unity's regular input system.
        return Input.GetKeyDown(KeyCode.Alpha4);
    }

    void UpdateMouseControlToggle()
    {
        //if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //}

        //m_EnableMouseControl = Cursor.lockState == CursorLockMode.Locked;

        //Cursor.visible = !m_EnableMouseControl;
    }

    public void EnableMouseControl()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = m_EnableMouseControl;
    }

    public void DisableMouseControl()
    {
        Cursor.lockState = CursorLockMode.Locked;
        m_EnableMouseControl = Cursor.lockState == CursorLockMode.Locked;

        Cursor.visible = !m_EnableMouseControl;
    }

    bool m_EnableMouseControl;

}
