using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    State currentState;
    public bool dead;

    [Header("RaycastInfo")]
    public LayerMask everything;
    public LayerMask climbing;
    public Vector3 lastCachedhit;
    public float distanceFromWall;
    public float grabHeight;

    [Header("GroundedInfo")]
    public bool grounded;
    public bool ccGrounded;

    [Header("IK")]
    public bool useIK;
    public Vector3 leftHandPos;
    public Vector3 rightHandPos;

    #region public hidden
    [HideInInspector] public CharacterController cc;
    [HideInInspector] public OrbitCamera oc;
    [HideInInspector] public LockOnLookat lol;
    [HideInInspector] public TargetingSystem ts;
    [HideInInspector] public Animator anim;
    [HideInInspector] public bool lockedOn;
    [HideInInspector] public PlayerControls pc;
    [HideInInspector] public StateMachine stateMachine;

    [HideInInspector] public MonoBehaviour mono;
    #endregion

    void Start()
    {
        stateMachine = new StateMachine();

        mono = this;

        cc = GetComponent<CharacterController>();
        ts = GetComponent<TargetingSystem>();
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerControls>();

        oc = GetComponentInChildren<OrbitCamera>();
        lol = GetComponentInChildren<LockOnLookat>();

        lol.gameObject.SetActive(false);

        SetupStateMachine();
    }

    void SetupStateMachine()
    {
        Locomotion lm = new Locomotion();
        InAir ia = new InAir();
        
        stateMachine.AddState(lm);
        stateMachine.AddState(ia);

        stateMachine.GoToState(this, "Locomotion");
    }

    void Update()
    {
        ccGrounded = cc.isGrounded;
        currentState.StateUpdate(this);
        Targeting();
        anim.SetBool("Land", grounded);
    }

    void Targeting()
    {
        if (lockedOn)
        {
            RotateTowardsCamera();
            if (Input.mouseScrollDelta.y != 0)
            {
                ts.SwitchTarget(oc);
                lol.target = ts.currentTarget;
            }
            if (Vector3.Distance(transform.position, ts.currentTarget.transform.position) > ts.loseTargetRange)
            {
                LoseTarget();
            }
        }
    }
    
    public void CanTarget()
    {
        if (Input.GetKeyDown(pc.target))
        {
            switch (oc.currentState)
            {
                case OrbitCamera.CamState.onPlayer:
                    if (ts.SelectTarget(oc))
                    {
                        anim.SetBool("Target", true);
                        lockedOn = true;
                        lol.gameObject.SetActive(true);
                        lol.target = ts.currentTarget;
                    }
                    break;
                case OrbitCamera.CamState.onTarget:
                    LoseTarget();
                    break;
                default:
                    break;
            }
        }
    }

    void LoseTarget()
    {
        lockedOn = false;
        anim.SetBool("Target", false);
        oc.ChangeCamState(OrbitCamera.CamState.onPlayer);
        ts.currentTarget = null;
        lol.gameObject.SetActive(false);
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    public void RotateTowardsCamera()
    {
        Quaternion newLookAt = Quaternion.LookRotation(ts.currentTarget.transform.position - transform.position);
        newLookAt.x = 0;
        newLookAt.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, newLookAt, Time.deltaTime * 5);
    }

    [Header("Ground Raycast")]
    public float startHeight = 0.5f;
    public float range = 1;
    public float airtime;

    public bool Grounded()
    {
        if    (RayHit(transform.position + (transform.right * 0.2f) + (transform.up * startHeight), (Vector3.down), range, everything)
            || RayHit(transform.position + (-transform.right * 0.2f) + (transform.up * startHeight), (Vector3.down), range, everything)
            || RayHit(transform.position + (transform.forward * 0.2f) + (transform.up * startHeight), (Vector3.down), range, everything)
            || RayHit(transform.position + (-transform.forward * 0.2f) + (transform.up * startHeight), (Vector3.down), range, everything)
            || RayHit(transform.position + (transform.up * startHeight), (Vector3.down), range, everything))
        {
            grounded = true;
            if (airtime != 0)
            {
                //Debug.Log("Total airtime was " + airtime.ToString("F2"));
            }
            airtime = 0;
            return true;
        }
        airtime += Time.deltaTime;
        grounded = false;
        return false;
    }

    public bool RayHit(Vector3 start, Vector3 dir, float length, LayerMask lm)
    {
        RaycastHit hit;
        Ray ray = new Ray(start, dir);
        Debug.DrawRay(start, dir * length, Color.magenta, 0.1f);
        if (Physics.Raycast(ray, out hit, length, lm))
        {
            lastCachedhit = hit.point;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LerpToPosition(Vector3 pos, float lerpTime)
    {
        Debug.DrawLine(transform.position, pos, Color.red, 5);
        StartCoroutine(LerpToPos(pos, lerpTime));
    }

    IEnumerator LerpToPos(Vector3 pos, float lerpTime)
    {
        anim.applyRootMotion = false;
        Vector3 startPos = transform.position;

        for (float t = 0; t < 1; t += Time.deltaTime / lerpTime)
        {
            transform.position = Vector3.Lerp(startPos, pos, t);
            yield return new WaitForEndOfFrame();
        }

        //transform.position = pos;
        anim.applyRootMotion = true;
    }

    public bool PlayerToWall(Player pb, Vector3 dir, bool lerp, float checkYOffset)
    {
        RaycastHit hit;
        float range = 2;
        Vector3 playerHeight = new Vector3(pb.transform.position.x, pb.transform.position.y + checkYOffset, pb.transform.position.z);
        Debug.DrawRay(playerHeight, dir * range, Color.green);
        if (Physics.Raycast(playerHeight, dir, out hit, range))
        {
            Vector3 temp = pb.transform.position - hit.point;
            temp.y = 0;
            Vector3 positionToSend = pb.transform.position - temp;
            positionToSend -= (pb.transform.forward * distanceFromWall);
            if (lerp)
            {
                pb.LerpToPosition(positionToSend, 0.25f);
            }
            else
            {
                transform.position = positionToSend;
            }
            return true;
        }
        return false;
    }

    public bool PlayerFaceWall(Player pb, Vector3 startOffset, Vector3 dir, float range)
    {
        RaycastHit hit;
        Vector3 playerHeight = pb.transform.position;
        playerHeight += startOffset;
        Debug.DrawRay(playerHeight, dir * range, Color.cyan, 5);
        if (Physics.Raycast(playerHeight, dir, out hit, range))
        {
            pb.transform.rotation = Quaternion.LookRotation(-hit.normal, Vector3.up);
            return true;
        }
        return false;
    }

    public bool LedgeInfo()
    {
        if (!RayHit(transform.position + (transform.up * 1.7f), transform.forward, 0.45f, climbing))
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position + (transform.forward * 0.45f) + (transform.up * 1.7f), Vector3.down);
            Debug.DrawRay(transform.position + (transform.forward * 0.45f) + (transform.up * 1.7f), Vector3.down * 0.35f, Color.red, 0.25f);
            if (Physics.Raycast(ray, out hit, 0.35f, climbing))
            {
                lastCachedhit = hit.point;
                string tagObject = hit.collider.tag;
                switch (tagObject)
                {
                    case "Ledge":
                        if (stateMachine.IsInState("InAir"))
                        {
                            anim.Play("AirToClimb");
                        }
                        stateMachine.GoToState(this, "Climbing");
                        break;
                    case "Example":
                        break;
                    default:
                        break;
                }
                return true;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator LerpCharacterControllerCenter(float to, float lerpTime)
    {
        float timeElapsed = 0;
        float beginFloat = cc.center.y;
        while (timeElapsed < lerpTime)
        {
            beginFloat = Mathf.Lerp(beginFloat, to, timeElapsed / 0.25f);
            Vector3 temp = new Vector3(0, beginFloat, 0);
            cc.center = temp;
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Vector3 finalY = new Vector3(0, to, 0);
        cc.center = finalY;
    }

    public void DelayFunction(string functionName, float delay)
    {
        Invoke(functionName, delay);
    }

    void DelayedRoot()
    {
        anim.applyRootMotion = true;
    }

    public void _GoToState(string stateName)
    {
        stateMachine.GoToState(this, stateName);
    }

    public void _DoubleJump(float targetFloat)
    {
        StartCoroutine(LerpCharacterControllerCenter(targetFloat, 0.2f));
    }

    public void AddRotation(Vector3 rot)
    {
        transform.Rotate(rot);
    }
    
    private void OnAnimatorIK(int layerIndex)
    {
        if (useIK)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);

            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
        }
    }

}
