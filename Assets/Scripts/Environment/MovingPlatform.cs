using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public float Speed = 10.0f;
    public float CheckRadius = 1.0f;
    public Transform[] MovementPoints;

    void Start()
    {
        m_OriginalPosition = transform.position;
        GetStartingDestination();
        m_Paused = false;
        m_PauseTimer = 0.0f;
        m_PauseTimeMax = 1.5f;
    }

    void Update()
    {
        float difference = (transform.position - m_NextPosition).magnitude;
        if(difference <= CheckRadius)
        {
            int newIndex = m_CurrentIndex + 1;
            newIndex = (newIndex + MovementPoints.Length) % MovementPoints.Length;
            m_CurrentIndex = newIndex;
            m_NextPosition = MovementPoints[m_CurrentIndex].position;
            m_Paused = true;
        }
    }

    void FixedUpdate()
    {
        if (m_Paused)
        {
            m_PauseTimer += Time.fixedDeltaTime;
            if (m_PauseTimer >= m_PauseTimeMax)
            {
                m_PauseTimer = 0.0f;
                m_Paused = false;
            }
        }
        else
        {
            Vector3 dir = m_NextPosition - transform.position;
            dir.Normalize();
            dir *= Speed * Time.fixedDeltaTime;

            transform.position += dir;
        }
    }

    void GetStartingDestination()
    {
        if(MovementPoints.Length == 0) { transform.position = m_OriginalPosition; }
        else
        {
            float difference = (transform.position - MovementPoints[0].position).magnitude;
            int index = 0;
            for(int i = 1; i < MovementPoints.Length; i++)
            {
                float newDifference = (transform.position - MovementPoints[i].position).magnitude;
                if( newDifference < difference)
                {
                    index = i;
                    difference = newDifference;
                }
            }
            m_CurrentIndex = index;
            m_NextPosition = MovementPoints[m_CurrentIndex].position;
        }
    }

    bool m_Paused;
    float m_PauseTimer;
    float m_PauseTimeMax;
    int m_CurrentIndex;
    [SerializeField] Vector3 m_NextPosition;
    Vector3 m_OriginalPosition;
}
