using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{

    public interface IInteractable
    {
        void DisplayInteraction();
        void HideInteraction();
        void Interact();
    }

    public interface IController
    {
        void Init(Player player);

        MainCamera GetMainCamera();

        void UpdateControls();

        void SetFacingDirection(Vector3 direction);

        void AddLedgeDir(Vector3 ledgeDir);

        Vector3 GetControlRotation();

        Vector3 GetMoveInput();

        Vector3 GetLookInput();

        Vector3 GetAimTarget();

        bool IsJumping();

        bool IsFiring();

        //bool IsAiming();

        bool ToggleCrouch();

        bool IsInteracting();

        bool ToggleSprint();

        bool ToggleMenu();

        bool IsAttacking();

        bool IsOpeningColorMenu();

        
        bool SwitchToItem1();

        bool SwitchToItem2();

        bool SwitchToItem3();
        
        bool SwitchToItem4();
/*
        bool SwitchToItem5();

        bool SwitchToItem6();

        bool SwitchToItem7();

        bool SwitchToItem8();

        bool SwitchToItem9();
        */
    }



}