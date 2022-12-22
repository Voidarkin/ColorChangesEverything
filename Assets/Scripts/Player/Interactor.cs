using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

[RequireComponent(typeof(Player))]
public class Interactor : MonoBehaviour
{

    public float CheckDistance = 5.0f;

    void Start()
    {
        StartCoroutine(BeginLateStart());
    }

    IEnumerator BeginLateStart()
    {
        yield return new WaitForFixedUpdate();
        LateStart();
    }
    void LateStart()
    {
        Player player = GetComponent<Player>();
        if (player)
        {
            m_Controller = player.Controller;
        }
    }

    void Update()
    {
        if(m_Controller == null) { return; }

        bool isInteracting = m_Controller.IsInteracting();
        if (isInteracting)
        {
            CheckForInteractable();
            Interact();
        }
    }

    void FixedUpdate()
    {
        //CheckForInteractable();
    }

    void CheckForInteractable()
    {
        if(m_Controller == null) { return; }

        //m_CurrentInteractable.HideInteraction();
        m_CurrentInteractable = null;
        Vector3 rayStart = Camera.main.transform.position;
        Vector3 rayDir = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(rayStart, rayDir, out hitInfo, CheckDistance))
        {
            IInteractable interactable = hitInfo.collider.GetComponentInChildren<IInteractable>();
            if (interactable != null)
            {
                m_CurrentInteractable = interactable;
                //m_CurrentInteractable.DisplayInteraction();
            }
        }
    }

    void Interact()
    {
        if (m_CurrentInteractable != null)
        {
            m_CurrentInteractable.Interact();
        }
    }

    IInteractable m_CurrentInteractable;
    IController m_Controller;
}
