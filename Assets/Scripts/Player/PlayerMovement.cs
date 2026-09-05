using System;
using UnityEngine;

public enum PlayerState { Idle, Walk, Jump, Fall, Dash, Attak , Defend, Hurt, Die}

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D;

    [Space(5)]
    [Header("Player State")]
    public PlayerState CurrentState;

    [Space(5)]
    [Header("Public Floats")]
    public float CoolDownTime;
    public float MoveSpeed;
    public float JumpSpeed;
    public float FaceDir;
    public float Damage;

    [Space(5)]
    [Header("Public Bools")]
    public bool isCanDubleJumping;
    public bool isCanDashing;

    [Space(5)]
    [Header("Bool of Distances")]
    public bool isOnAir;
    public bool isWalled;
    public bool isGrounded;

    [Space(5)]
    [Header("Layers")]
    [SerializeField] private LayerMask _groundLayer;

    [Space(5)]
    [Header("Float of Distances")]
    [SerializeField] private float _checkAirDistance;
    [SerializeField] private float _checkWallDistance;
    [SerializeField] private float _checkGroundDistance;

    [Space(5)]
    [Header("Game objects for check distances")]
    [SerializeField] private GameObject _wallDistanceCheckerGameObject;

    [Space(5)]
    [Header("Tornado")]
    [SerializeField] private GameObject _tornado;
    [SerializeField] private GameObject _tornadoSP;
    [SerializeField] private float _tornadoCostStamina;

    private float _xScale;
    private int _attackLevel;
    private bool isAttackLevelReseted;
    private bool isCanDoDifferentWork;

    public Action<float> DamagetoEnemy;
    public Action<PlayerState> SendState;

    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _xScale = transform.localScale.x;
    }

    private void Update()
    {
        _theChangeFace();
        _theCheckDistance();
        _theSetPlayerState();
        if (CoolDownTime > 0)
            _theCoolDownTimer();
        if (isGrounded)
            _theSetAbilitiesToTrue();
        if (_attackLevel != 0 && !isAttackLevelReseted && CoolDownTime <= 0)
            _theResetAttackLevel();
    }


    private void _theChangeFace()
    {
        int move = 0;

        if (Input.GetKey(KeyCode.D))
            move = 1;
        else if (Input.GetKey(KeyCode.A))
            move = -1;

        if (move != 0)
            FaceDir = (int)Mathf.Sign(move);
        if (FaceDir != transform.localScale.x)
            transform.localScale = new Vector3(_xScale * FaceDir, transform.localScale.y, transform.localScale.z);
    }
    private void _theCheckDistance()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, _checkGroundDistance, _groundLayer);
        isWalled = Physics2D.Raycast(_wallDistanceCheckerGameObject.transform.position, Vector2.right * FaceDir, _checkWallDistance, _groundLayer);
        isOnAir = Physics2D.Raycast(transform.position, Vector2.down, _checkAirDistance, _groundLayer);
    }
    private void _theSetPlayerState()
    {
        if(isCanDoDifferentWork)
        {
            if (isWalled)
            {
                if (isGrounded)
                    CurrentState = PlayerState.Idle;
                else
                    CurrentState = PlayerState.Fall;
            }
            if (Input.GetMouseButtonDown(0) && _attackLevel < 4)
            {
                isAttackLevelReseted = false;
                if (_attackLevel == 0)
                    CoolDownTime = 1.7f;
                else if (_attackLevel == 1)
                    CoolDownTime = 3f;
                else if (_attackLevel == 2)
                    CoolDownTime = 3f;
                else if (_attackLevel == 3)
                    CoolDownTime = 1.1f;
                if (isGrounded)
                {
                    _attackLevel += 1;
                    print(_attackLevel);
                }
                CurrentState = PlayerState.Attak;
            }
            else if (Input.GetMouseButtonDown(1) && isCanDashing)
            {
                CurrentState = PlayerState.Dash;
            }
            else if (Input.GetKey(KeyCode.Space) && (isGrounded || isCanDubleJumping))
                CurrentState = PlayerState.Jump;

            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && isGrounded && !isWalled)
                CurrentState = PlayerState.Walk;

            else if (!isGrounded && CurrentState != PlayerState.Attak)
            {
                _rigidBody2D.linearVelocity = new Vector2(0, _rigidBody2D.linearVelocity.y);
                CurrentState = PlayerState.Fall;
            }
            else if (!isAnyInput() && isGrounded)
            {
                _rigidBody2D.linearVelocity = new Vector2(0, _rigidBody2D.linearVelocity.y);
                CurrentState = PlayerState.Idle;
            }
            //print(CurrentState);
        }
    }
    private void _theResetAttackLevel()
    {
        _attackLevel = 0;
        isAttackLevelReseted = true;
    }
    private void _theSetAbilitiesToTrue()
    {
        isCanDubleJumping = true;
        isCanDashing = true;
    }

    void FixedUpdate()
    {
        _theSwitchOnPlayerStates();
    }

    private void _theSwitchOnPlayerStates()
    {
        switch (CurrentState)
        {
            case PlayerState.Idle:
                isCanDoDifferentWork = true;
                _theIdle();
                break;

            case PlayerState.Walk:
                isCanDoDifferentWork = true;
                _theWalk();
                break;

            case PlayerState.Jump:
                isCanDoDifferentWork = true;
                CoolDownTime = 1;
                _theJump();
                break;

            case PlayerState.Fall:
                isCanDoDifferentWork = true;
                _theFall();
                break;

            case PlayerState.Dash:
                isCanDoDifferentWork = false;
                _theDash();
                break;
            case PlayerState.Attak:
                isCanDoDifferentWork = true;
                _theAttak();
                break;
            case PlayerState.Defend:
                isCanDoDifferentWork = false;
                _theDefend();
                break;
        }
    }

    //MovementMods
    private void _theIdle()
    {
        _rigidBody2D.linearVelocity = Vector2.zero;
    }

    private void _theWalk()
    {
        _rigidBody2D.linearVelocity = new Vector2(MoveSpeed * FaceDir, _rigidBody2D.linearVelocity.y);
    }
    private void _theJump()
    {
        if (isGrounded)
        {
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, JumpSpeed);
            CurrentState = PlayerState.Fall;
            return;
        }
        else if (isCanDubleJumping)
        {
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, JumpSpeed);
            isCanDubleJumping = false;
            CurrentState = PlayerState.Fall;
        }
    }

    private void _theFall()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            _rigidBody2D.linearVelocity = new Vector2(MoveSpeed * FaceDir, _rigidBody2D.linearVelocity.y);
        if (_rigidBody2D.linearVelocity.y > 0) { }
        else
            if (isGrounded)
                CurrentState = PlayerState.Idle;
    }
    private void _theDash()
    {
        if (isCanDashing)
        {
            isCanDashing = false;
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, 0);
        }
    }
    private void _theAttak()
    {

    }
    private void _theDefend()
    {

    }

    private bool isAnyInput()
    {
        if (CurrentState == PlayerState.Attak)
            return true;
        if (Input.anyKey)
            return true;

        if (Input.GetMouseButton(0) ||
            Input.GetMouseButton(1) ||
            Input.GetMouseButton(2))
            return true;

        return false;
    }

    private void _theCoolDownTimer()
    {
            CoolDownTime -= Time.deltaTime;
    }

    public void TheResetCanDoDifferentWork()
    {
        isCanDoDifferentWork = true;
    }

    //Draw the raycast line
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - _checkGroundDistance, transform.position.z));
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - _checkAirDistance, transform.position.z));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_wallDistanceCheckerGameObject.transform.position, new Vector3(_wallDistanceCheckerGameObject.transform.position.x + (_checkWallDistance * FaceDir), _wallDistanceCheckerGameObject.transform.position.y, _wallDistanceCheckerGameObject.transform.position.z));
    }
}

