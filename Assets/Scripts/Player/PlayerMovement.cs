using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerPosition _playerPosition;
    private PlayerInputController _playerInputController;
    private Rigidbody2D _rigidBody2D;

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
    [Header("Tornado")]
    [SerializeField] private GameObject _tornado;
    [SerializeField] private GameObject _tornadoSP;
    [SerializeField] private float _tornadoCostStamina;

    private float _xScale;

    public Action<float> DamagetoEnemy;
    

    void Start()
    {
        _playerPosition = GetComponent<PlayerPosition>();
        _playerInputController = GetComponent<PlayerInputController>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _xScale = transform.localScale.x;
    }

    private void Update()
    {
        _theChangeFace();
    }

    //This function must defind in player input controller
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
    
    //This function must defind in ability manager and optimize with it
    private void _theSetAbilitiesToTrue()
    {
        isCanDubleJumping = true;
        isCanDashing = true;
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
        if (_playerPosition.isGrounded)
        {
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, JumpSpeed);
            _playerInputController.CurrentState = PlayerState.Fall;
            return;
        }
        else if (isCanDubleJumping)
        {
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, JumpSpeed);
            isCanDubleJumping = false;
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
            if (_playerPosition.isGrounded)
                _playerInputController.CurrentState = PlayerState.Idle;
    }
    public void TheDash()
    {
        if (isCanDashing)
        {
            isCanDashing = false;
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, 0);
        }
    }
    public void TheCoolDownTimer()
    {
            CoolDownTime -= Time.deltaTime;
    }

    public void TheResetCanDoDifferentWork()
    {
        _playerInputController.isCanDoDifferentWork = true;
    }

}

