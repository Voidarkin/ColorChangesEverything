using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float MaxLifeTime = 3.0f;
    public float Speed = 20.0f;
    public int Damage = 1;

    void Awake()
    {

        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr)
        {
            m_Renderer = mr;
        }

        Collider collider = GetComponent<Collider>();
        if (collider)
        {
            m_Collider = collider;
        }
    }

    public void Init(Vector3 position, Vector3 velocity, Color color)
    {
        transform.position = position;
        m_Velocity = velocity;
        m_TimeLeftTillDestroy = MaxLifeTime;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if(mr)
        {
            mr.material.color = color;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        transform.position += m_Velocity * Speed * Time.deltaTime;

        m_TimeLeftTillDestroy -= Time.deltaTime;

        if(m_TimeLeftTillDestroy <= 0.0f)
        {
            DisableObject();
        }
	}

    void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject != null)
        {
            DisableObject();
        }
    }

    void DisableObject()
    {
        if (m_Renderer)
        {
            m_Renderer.enabled = true;
        }

        if (m_Collider)
        {
            m_Collider.enabled = true;
        }

        this.gameObject.SetActive(false);
    }

    MeshRenderer m_Renderer;
    Collider m_Collider;

    float m_TimeLeftTillDestroy;
    Vector3 m_Velocity;
}
