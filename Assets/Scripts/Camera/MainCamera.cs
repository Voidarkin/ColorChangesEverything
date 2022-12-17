using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public FirstPersonCameraBehaviour MainCameraBehaviour;

    void Awake()
    {
        if (MainCameraBehaviour == null)
        {
            MainCameraBehaviour = new FirstPersonCameraBehaviour();
        }

    }

    void Update()
    {
        
    }

    void LateUpdate()
    {
        if (m_Player == null)
        {
            return;
        }

        if (m_CurrentBehaviour != null)
        {
            m_CurrentBehaviour.UpdateCamera();

            ControlRotation = m_CurrentBehaviour.GetControlRotation();
        }
    }

    public void SetPlayer(Player player)
    {
        m_Player = player;

        if (m_Player != null)
        {
            LookPos = m_Player.transform.position;

            MainCameraBehaviour.Init(this, m_Player);
        }

        SetCameraBehaviour(MainCameraBehaviour);
    }

    public void UpdateRotation(float yawAmount, float pitchAmount)
    {
        if (m_CurrentBehaviour != null)
        {
            m_CurrentBehaviour.UpdateRotation(yawAmount, pitchAmount);
        }
    }

    public void SetFacingDirection(Vector3 direction)
    {
        if (m_CurrentBehaviour != null)
        {
            m_CurrentBehaviour.SetFacingDirection(direction);
        }
    }

    public Vector3 ControlRotation { get; private set; }
    public Vector3 LookPos { get; set; }

    public Vector3 PivotRotation { get; set; }

    void SetCameraBehaviour(CameraBehaviour behaviour)
    {
        if (m_CurrentBehaviour == behaviour)
        {
            return;
        }

        if (m_CurrentBehaviour != null)
        {
            m_CurrentBehaviour.Deactivate();
        }

        m_CurrentBehaviour = behaviour;

        if (m_CurrentBehaviour != null)
        {
            m_CurrentBehaviour.Activate();
        }
    }

    CameraBehaviour m_CurrentBehaviour;
    Player m_Player;
}
