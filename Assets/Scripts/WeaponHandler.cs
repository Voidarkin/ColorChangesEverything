using System.Collections;
using UnityEngine;
using Interfaces;

public class WeaponHandler : MonoBehaviour
{

    public Weapon ActiveWeapon;
    public int ActiveWeaponIndex = 0;
    public Weapon[] Weapons;

    void Start()
    {
        m_CanFire = true;
        m_FireTimer = 0.0f;

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

    public void Init(IController controller)
    {
        if (controller == null) { return; }

        m_Controller = controller;
        m_HoldingObject = transform;

        if(Weapons.Length == 0) { return; }

        ActiveWeaponIndex = 0;
        ActiveWeapon = Weapons[0];

        ActiveWeapon.Init();
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

        ////Temp Input
        //if (m_Controller.SwitchToItem1())
        //{
        //    ActiveWeapon.SetSelectedColor(0);
        //}
        //else if (m_Controller.SwitchToItem2())
        //{
        //    ActiveWeapon.SetSelectedColor(1);
        //}
        //else if (m_Controller.SwitchToItem3())
        //{
        //    ActiveWeapon.SetSelectedColor(2);
        //}

        m_Line.positionCount = 0;

        CheckIfCanFire();

        bool isFiring = m_Controller.IsFiring();
        if (isFiring && m_CanFire)
        {
            FireWeapon();
        }
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
        if (m_HoldingObject == null) { return; }

        //Vector3 currentRot = transform.rotation.eulerAngles;
        Vector3 controlRotation = m_Controller.GetControlRotation();

        //currentRot = MathUtils.LerpTo(5.0f, currentRot, controlRotation, Time.fixedDeltaTime);
        Quaternion targetRot = Quaternion.Euler(controlRotation);

        m_HoldingObject.transform.rotation = Quaternion.RotateTowards(m_HoldingObject.transform.rotation, targetRot, 100.0f * Time.fixedDeltaTime); //Quaternion.Euler(currentRot);
    }

    bool m_CanFire;
    float m_FireTimer;

    IController m_Controller;
    Transform m_HoldingObject;
    LineRenderer m_Line;
    int m_SegmentCount;

}
