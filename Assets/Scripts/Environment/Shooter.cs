using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    public float CheckRadius = 5.0f;
    public float ShotDelay = 2.0f;
    public float SpinSpeed = 1.0f;

    public GameObject ProjectilePrefab;
    public Color Color;
    public Transform Muzzle;

    void Start()
    {
        m_CheckMask = LayerMask.GetMask("Player");
        m_ShotTimer = 0.0f;
        m_CanShoot = true;
    }

    void Update()
    {
        if (!m_CanShoot)
        {
            m_ShotTimer += Time.deltaTime;
            if(m_ShotTimer >= ShotDelay)
            {
                m_CanShoot = true;
                m_ShotTimer = 0.0f;
            }
        }
    }

    void FixedUpdate()
    {
        if(m_Target)
        {
            //Rotate Turret
            Vector3 lookDir = m_Target.transform.position - Muzzle.position;
            lookDir.y = 0;

            float singleStep = SpinSpeed * Time.fixedDeltaTime;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, lookDir, singleStep, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        if(!m_CanShoot) { return; }

        Vector3 rayStart = Muzzle.position;
        Vector3 rayDir = Muzzle.forward;
        float rayDist = 20.0f;

        RaycastHit[] hitInfos = Physics.SphereCastAll(rayStart, CheckRadius, rayDir, rayDist, m_CheckMask);
        Debug.DrawRay(rayStart, rayDir, Color.black, 0.5f);

        m_Target = null;

        foreach (RaycastHit hitInfo in hitInfos)
        {
            GameObject go = hitInfo.collider.gameObject;
            Player player = go.GetComponent<Player>();
            if (!player)
            {
                continue;
            }

            m_Target = go;
            
        }

        if(m_Target)
            FireShot();
    }

    void FireShot()
    {
        Vector3 firePos = Muzzle.position + Muzzle.forward;
        Vector3 fireDir = m_Target.transform.position - Muzzle.position;
        fireDir.Normalize();
        Quaternion fireRot = Quaternion.Euler(fireDir);
        GameObject Bullet = PoolManager.Get(ProjectilePrefab, firePos, fireRot);
        Bullet.GetComponent<Bullet>().Init(firePos, fireDir, Color);
        m_CanShoot = false;
    }

    [SerializeField] GameObject m_Target;

    [SerializeField] bool m_CanShoot;
    [SerializeField] float m_ShotTimer;
    int m_CheckMask;
}
