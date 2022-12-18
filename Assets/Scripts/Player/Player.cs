using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using System;

public class Player : MonoBehaviour
{

    public Health Health;

    public float OnGroundMoveAccel = 10.0f;
    public float OnGroundMaxSpeed = 10.0f;
    public float OnGroundStopEaseSpeed = 20.0f;
    public float OnGroundMaxSprintSpeed = 20.0f;
    public float OnGroundSprintMoveAccel = 20.0f;

    public float GroundCheckStartOffsetY = 0.5f;
    public float CheckForGroundRadius = 0.5f;
    public float GroundResolutionOverlap = 0.05f;

    public bool  InstantStepUp = false;
    public float StepUpEaseSpeed = 10.0f;
    public float MinAllowedSurfaceAngle = 15.0f;

    public float JumpSpeed = 10.0f;
    public float JumpPushOutOfGroundAmount = 0.5f;

    public float InAirMoveAccel = 5.0f;
    public float InAirMaxSpeed = 7.0f;
    public float InAirSprintMoveAccel = 10.0f;
    public float InAirMaxSprintSpeed = 14.0f;

    public float InvulnTimer = 0.0f;

    public GameObject FootLocationObj;

    public Vector3 GroundVelocity { get; private set; }
    public Vector3 GroundAngularVelocity { get; private set; }
    public Vector3 GroundNormal { get; private set; }
    public IController Controller { get; set; }

    void Start()
    {
        if (!SetupHumanPlayer())
        {
            return;
        }

        m_GroundCheckMask = ~LayerMask.GetMask("Player", "Ignore Raycast");
        m_RigidBody = GetComponent<Rigidbody>();
        m_Velocity = Vector3.zero;
        m_AllowJump = true;

        if(Health)
        {
            Health.SetHealthToMax();
        }

        m_Invuln = false;
        m_InvulnTimer = 0.4f;

        (Controller as DefaultController).DisableMouseControl();
    }

    bool SetupHumanPlayer()
    {
        if (LevelManager.Instance.GetPlayer() == null)
        {
            DontDestroyOnLoad(gameObject);

            LevelManager.Instance.RegisterPlayer(this);

            Controller = new DefaultController();

            Controller.Init(this);
            return true;
        }
        else
        {
            Destroy(gameObject);
            return false;
        }
    }

    void Update()
    {
        if(m_Invuln)
        {
            InvulnTimer += Time.deltaTime;
            if(InvulnTimer >= m_InvulnTimer)
            {
                m_Invuln = false;
                InvulnTimer = 0.0f;
            }
        }

        ////Temp Input
        //if (Controller.SwitchToItem1())
        //{
        //    ColorManager.Instance.ChangeColor(Color.red);
        //}
        //else if (Controller.SwitchToItem2())
        //{
        //    ColorManager.Instance.ChangeColor(Color.blue);
        //}
        //else if (Controller.SwitchToItem3())
        //{
        //    ColorManager.Instance.ChangeColor(Color.green);
        //}
        //else if (Controller.SwitchToItem4())
        //{
        //    ColorManager.Instance.ChangeColor(Color.white);
        //}

    }

    void FixedUpdate()
    {
        if(m_MovementState == MovementState.Disable) { return; }

        m_Velocity = m_RigidBody.velocity;

        UpdateGroundInfo();

        Controller.UpdateControls();

        Vector3 localMoveDir = Controller.GetMoveInput();

        localMoveDir.Normalize();

        bool isJumping = Controller.IsJumping();
        bool isSprinting = Controller.ToggleSprint();

        switch (m_MovementState)
        {
            case MovementState.OnGround:
                UpdateOnGround(localMoveDir, isJumping, isSprinting);
                break;

            case MovementState.InAir:
                UpdateInAir(localMoveDir, isJumping);
                break;

            case MovementState.Disable:
                break;

            default:
                break;
        }

    }


    void UpdateGroundInfo()
    {
        GroundAngularVelocity = Vector3.zero;
        GroundVelocity = Vector3.zero;
        GroundNormal.Set(0.0f, 0.0f, 1.0f);

        m_CenterHeight = transform.position.y;

        float footHeight = FootLocationObj.transform.position.y;

        float halfCapsuleHeight = m_CenterHeight - footHeight;

        Vector3 rayStart = transform.position;
        rayStart.y += GroundCheckStartOffsetY;

        Vector3 rayDir = -Vector3.up;

        float rayDist = halfCapsuleHeight + GroundCheckStartOffsetY - CheckForGroundRadius;

        RaycastHit[] hitInfos = Physics.SphereCastAll(rayStart, CheckForGroundRadius, rayDir, rayDist, m_GroundCheckMask);

        RaycastHit groundHitInfo = new RaycastHit();
        bool validGroundFound = false;
        float minGroundDist = float.MaxValue;

        foreach (RaycastHit hitInfo in hitInfos)
        {
            float surfaceAngle = MathUtils.CalcVerticalAngle(hitInfo.normal);
            if (surfaceAngle < MinAllowedSurfaceAngle || hitInfo.distance <= 0.0f)
            {
                continue;
            }

            if (hitInfo.distance < minGroundDist)
            {
                minGroundDist = hitInfo.distance;

                groundHitInfo = hitInfo;

                validGroundFound = true;
            }
        }

        if (!validGroundFound)
        {
            if (m_MovementState != MovementState.Disable)
            {
                
            }
            return;
        }

        Vector3 bottomAtHitPoint = MathUtils.ProjectToBottomOfCapsule(
                groundHitInfo.point,
                transform.position,
                halfCapsuleHeight * 2.0f,
                CheckForGroundRadius
                );

        float stepUpAmount = groundHitInfo.point.y - bottomAtHitPoint.y;

        m_CenterHeight += stepUpAmount - GroundResolutionOverlap;

        GroundNormal = groundHitInfo.normal;

        if (m_MovementState != MovementState.Disable)
        {
            SetMovementState(MovementState.OnGround);
        }

    }

    private void UpdateOnGround(Vector3 localMoveDir, bool isJumping, bool isSprinting)
    {
        {
            if (localMoveDir.sqrMagnitude > MathUtils.CompareEpsilon)
            {
                Vector3 localVelocity = m_Velocity - GroundVelocity;

                Vector3 moveAccel = CalcMoveAccel(localMoveDir);
                Vector3 groundTangent = moveAccel - Vector3.Project(moveAccel, GroundNormal);
                groundTangent.Normalize();
                moveAccel = groundTangent;

                Vector3 velAlongMoveDir = Vector3.Project(localVelocity, moveAccel);

                if (Vector3.Dot(velAlongMoveDir, moveAccel) > 0.0f) 
                {
                    localVelocity = MathUtils.LerpTo(OnGroundStopEaseSpeed, localVelocity, velAlongMoveDir, Time.fixedDeltaTime);
                }
                else
                {
                    localVelocity = MathUtils.LerpTo(OnGroundStopEaseSpeed, localVelocity, Vector3.zero, Time.fixedDeltaTime);
                }

                if (isSprinting)
                {
                    moveAccel *= OnGroundSprintMoveAccel;
                }
                else
                {
                    moveAccel *= OnGroundMoveAccel;
                }


                localVelocity += moveAccel * Time.fixedDeltaTime;


                if (isSprinting)
                {
                    localVelocity = Vector3.ClampMagnitude(localVelocity, OnGroundMaxSprintSpeed);
                }
                else
                {
                    localVelocity = Vector3.ClampMagnitude(localVelocity, OnGroundMaxSpeed);
                }

                m_Velocity = localVelocity + GroundVelocity;
            }
            else
            {
                UpdateStopping(OnGroundStopEaseSpeed);
            }

            if (isJumping)
            {
                ActivateJump();
            }
            else
            {
                m_AllowJump = true;
            }

            ApplyVelocity(m_Velocity);

            Vector3 playerCenter = transform.position;

            if (InstantStepUp)
            {
                playerCenter.y = m_CenterHeight;
            }
            else
            {
                playerCenter.y = MathUtils.LerpTo(StepUpEaseSpeed, playerCenter.y, m_CenterHeight, Time.deltaTime);
            }

            transform.position = playerCenter;
        }
    }

    private void UpdateInAir(Vector3 localMoveDir, bool isJumping)
    {
        if (localMoveDir.sqrMagnitude > MathUtils.CompareEpsilon)
        {
            Vector3 localVelocity = m_Velocity;
            Vector3 moveAccel = CalcMoveAccel(localMoveDir);

            localVelocity += moveAccel * Time.fixedDeltaTime;

            m_Velocity = localVelocity;
            ApplyVelocity(m_Velocity);
        }
        else
        {
            if (m_Velocity.y >= 7.5f)
            {
                m_Velocity.y = 7.5f;
                ApplyVelocity(m_Velocity);
            }
        }

    }

    public void UpdateStopping(float stopEaseSpeed)
    {
        m_Velocity = MathUtils.LerpTo(stopEaseSpeed, m_Velocity, GroundVelocity, Time.fixedDeltaTime);
    }

    void ActivateJump()
    {
        if (m_AllowJump)
        {
            m_Velocity.y = JumpSpeed + GroundVelocity.y;

            transform.position += new Vector3(0.0f, JumpPushOutOfGroundAmount, 0.0f);

            m_AllowJump = false;
            SetMovementState(MovementState.InAir);

        }
    }

    Vector3 CalcMoveAccel(Vector3 localMoveDir)
    {
        Vector3 controlRotation = Controller.GetControlRotation();
        controlRotation.x = 0;

        Vector3 moveAccel = Quaternion.Euler(controlRotation) * localMoveDir;

        return moveAccel;
    }
    void ApplyVelocity(Vector3 velocity)
    {
        Vector3 velocityDiff = velocity - m_RigidBody.velocity;

        m_RigidBody.AddForce(velocityDiff, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.collider.GetComponent<Bullet>();
        if (bullet)
        {
            if(!m_Invuln)
            {
                Health.TakeDamage(bullet.Damage);
                m_Invuln = true;
            }
        }
    }

    public enum MovementState
    {
        OnGround,
        InAir,
        Disable
    }

    void SetMovementState(MovementState movementState)
    {
        switch (movementState)
        {
            case MovementState.OnGround:
                (Controller as DefaultController).DisableMouseControl();
                break;

            case MovementState.InAir:
                (Controller as DefaultController).DisableMouseControl();
                break;
            case MovementState.Disable:
                m_Velocity = Vector3.zero;
                m_RigidBody.velocity = Vector3.zero;
                ApplyVelocity(m_Velocity);
                (Controller as DefaultController).EnableMouseControl();
                break;

            default:
                break;
        }

        m_MovementState = movementState;
    }

    [SerializeField] MovementState m_MovementState;
    Vector3 m_Velocity;
    [SerializeField] bool m_AllowJump;
    Rigidbody m_RigidBody;
    float m_CenterHeight;
    int m_GroundCheckMask;

    bool m_Invuln;
    float m_InvulnTimer;

}
