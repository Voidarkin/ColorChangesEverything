using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class WeaponHandler : MonoBehaviour
{

    public Weapon ActiveWeapon;
    public int ActiveWeaponIndex = 0;
    public List<Weapon> Weapons;

    public Transform HoldingObject;

    void Start()
    {
        m_CanFire = true;
        m_FireTimer = 0.0f;
        m_IsFiring = false;

        StartCoroutine(BeginLateStart());
    }

    IEnumerator BeginLateStart()
    {
        yield return new WaitForFixedUpdate();
        LateStart();
    }

    void LateStart()
    {
        Player player = GetComponentInParent<Player>();
        if (player)
        {
            Init(player.Controller);
        }

        LineRenderer line = GetComponentInChildren<LineRenderer>();
        if (line)
        {
            m_Line = line;
            m_Line.positionCount = 0;
        }

        m_SegmentCount = 2;
    }

    public void AddWeapon(Weapon newWeapon)
    {
        if(Weapons.Count != 0)
        {
            Weapons.Clear();
        }

        Weapons.Add(newWeapon);
        ChangeWeapon(0);
        
    }

    public void ChangeWeapon(int newIndex)
    {
        if(newIndex < 0 || newIndex >= Weapons.Count) { return; }

        ActiveWeaponIndex = newIndex;
        ActiveWeapon = Weapons[newIndex];
        ActiveWeapon.Init();

        if (m_CurrentlyHeldWeapon)
        {
            Destroy(m_CurrentlyHeldWeapon.gameObject);
        }
        m_CurrentlyHeldWeapon = Instantiate(ActiveWeapon.WeaponObject, HoldingObject.position, HoldingObject.rotation, HoldingObject);

    }

    public void Init(IController controller)
    {
        if (controller == null) { return; }

        m_Controller = controller;

        m_Pivot = transform;

        if(Weapons.Count == 0) { return; }

        ActiveWeaponIndex = 0;
        ActiveWeapon = null;
        m_IsFiring = false;
    }

    void Update()
    {
        if (m_Controller == null) { return; }
        if (!ActiveWeapon) { return; }

        UI_Controller UIControl = FindObjectOfType<UI_Controller>();
        bool isColorMenuOpen = m_Controller.IsOpeningColorMenu();
        if (UIControl)
        {
            if (isColorMenuOpen)
            {
                if (!UIControl.IsCircularMenuOpen())
                {
                    UIControl.OpenCircularMenu(m_Controller, ActiveWeapon);
                }
            }
            else
            {
                if (UIControl.IsCircularMenuOpen())
                {
                    UIControl.CloseCircularMenu();

                }
            }
        }

        m_IsFiring = m_Controller.IsFiring();
    }

    void CheckIfCanFire()
    {
        if (!m_CanFire)
        {
            m_FireTimer += Time.deltaTime;
            if (m_FireTimer >= ActiveWeapon.GetFireRate())
            {
                m_CanFire = true;
                m_FireTimer = 0.0f;
            }
        }
    }

    void FireWeapon()
    {
        m_Line.materials[0].color = Color.white;
        //Transform muzzlePos = transform.Find("Muzzle");
        Transform muzzlePos = GetComponentInChildren<WeaponAdjustments>().GetMuzzle();
        if(muzzlePos == null) { return; }

        Vector3 rayStart = Camera.main.transform.position;
        Vector3 rayDir = Camera.main.transform.forward;

        Vector3 hitLocation;

        RaycastHit hitInfo;
        if (Physics.Raycast(rayStart, rayDir, out hitInfo, 100.0f))
        {
            hitLocation = hitInfo.point;

            m_Line.positionCount = m_SegmentCount;
            m_Line.materials[0].color = ActiveWeapon.GetSelectedColor();
            m_Line.SetPosition(0, muzzlePos.position);
            m_Line.SetPosition(1, hitLocation);

            ColorCanBeChanged hit = hitInfo.collider.GetComponent<ColorCanBeChanged>();
            if(hit)
            {
                hit.ChangeMaterial(ActiveWeapon.GetSelectedMaterial());
            }

        }

        m_CanFire = false;
    }

    void FixedUpdate()
    {
        if (m_Controller == null) { return; }
        if (m_Pivot == null) { return; }
        if (HoldingObject == null) { return; }

        if (ActiveWeapon)
        {
            m_Line.positionCount = 0;

            CheckIfCanFire();

            
            if (m_IsFiring && m_CanFire)
            {
                FireWeapon();
            }
        }

        //Vector3 currentRot = transform.rotation.eulerAngles;
        Vector3 controlRotation = m_Controller.GetControlRotation();

        //currentRot = MathUtils.LerpTo(5.0f, currentRot, controlRotation, Time.fixedDeltaTime);
        Quaternion targetRot = Quaternion.Euler(controlRotation);

        m_Pivot.transform.rotation = Quaternion.RotateTowards(HoldingObject.transform.rotation, targetRot, 200.0f * Time.fixedDeltaTime); //Quaternion.Euler(currentRot);
    }

    bool m_CanFire;
    float m_FireTimer;

    IController m_Controller;
    GameObject m_CurrentlyHeldWeapon;
    Transform m_Pivot;
    LineRenderer m_Line;
    int m_SegmentCount;

    bool m_IsFiring;

}
