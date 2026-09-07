using System;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputController _playerInputController;
    private Rigidbody2D _rigidBody2D;

    [Header("Scriptable objects")]
    private CollectablePlayerAbilities _collectedPlayerAbilities;

    [Space(5)]
    [Header("Public Floats")]
    public float CoolDownTime;
    public float MoveSpeed;
    public float JumpSpeed;
    public float FaceDir;
    public float Damage;

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


    public Action<float> DamagetoEnemy;
    public Action<PlayerState> SendState;

    void Start()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _xScale = transform.localScale.x;
    }

    private void Update()
    {
        _theChangeFace();
        _theCheckDistance();
        if (isGrounded)
            _collectedPlayerAbilities.The_Set_Movement_To_True();
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
    
    private void _theResetAttackLevel()
    {
        _attackLevel = 0;
        isAttackLevelReseted = true;
    }
    

    

    //MovementMods
    public void TheIdle()
    {
        _rigidBody2D.linearVelocity = Vector2.zero;
    }

    public void TheWalk()
    {
        _rigidBody2D.linearVelocity = new Vector2(MoveSpeed * FaceDir, _rigidBody2D.linearVelocity.y);
    }
    public void TheJump()
    {
        if (isGrounded)
        {
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, JumpSpeed);
            _playerInputController.CurrentState = PlayerState.Fall;
            return;
        }
        else if (_collectedPlayerAbilities.isCanDubleJumping)
        {
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, JumpSpeed);
            _collectedPlayerAbilities.isCanDubleJumping = false;
            _playerInputController.CurrentState = PlayerState.Fall;
        }
    }

    public void TheFall()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            _rigidBody2D.linearVelocity = new Vector2(MoveSpeed * FaceDir, _rigidBody2D.linearVelocity.y);
        if (_rigidBody2D.linearVelocity.y > 0) { 
        //Do nothing
        }
        else
            if (isGrounded)
                _playerInputController.CurrentState = PlayerState.Idle;
    }
    public void TheDash()
    {
        if (_collectedPlayerAbilities.isCanDashing)
        {
            _collectedPlayerAbilities.isCanDashing = false;
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, 0);
        }
    }
    public void TheAttak()
    {

    }
    public void TheDefend()
    {

    }

    public void TheCoolDownTimer()
    {
            CoolDownTime -= Time.deltaTime;
    }

    public void TheResetCanDoDifferentWork()
    {
        _playerInputController.isCanDoDifferentWork = true;
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

