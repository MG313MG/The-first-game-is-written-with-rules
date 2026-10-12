using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public AbilitiesSO[] AbilitiesSO;
    public AbilityManager ValueOfAbilityManage;
    private PlayerPosition _playerPosition;
    private PlayerInputController _playerInputController;
    private Rigidbody2D _rigidBody2D;

    private PlayerStaminaController _playerStaminaController;

    private PlayerStaminaController _staminaController;

    public CollectablePlayerAbilities CollectedAbilities;

    [Space(5)]
    [Header("Public Floats")]
    public float CoolDownTime;
    public float MoveSpeed;
    public float JumpSpeed;
    public float FaceDir;
    public float Damage;


    private float _xScale;

    public Action<float> DamagetoEnemy;
    

    void Start()
    {
        _playerPosition = GetComponent<PlayerPosition>();
        _playerInputController = GetComponent<PlayerInputController>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        ValueOfAbilityManage = GetComponent<AbilityManager>();
        _staminaController = GetComponent<PlayerStaminaController>();
        _playerStaminaController = GetComponent<PlayerStaminaController>();
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
        }
        else if (CollectedAbilities.isCanDubleJumping && AbilitiesSO[1].isActivated && _playerStaminaController.CurrentStamina > AbilitiesSO[1].StaminaCost )
        {
            //_rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, JumpSpeed);
            AbilitiesSO[1].Activate(gameObject);
            _playerStaminaController.ThePlayerStaminaConsumption(AbilitiesSO[1].StaminaCost);
            _playerInputController.CurrentState = PlayerState.Fall;
            AbilitiesSO[1].isActivated = false;
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
        //The player's stamina is reduced via a function within the dash animation event.س
        if (CollectedAbilities.isCanDashing)
        {
            AbilitiesSO[0].Activate(gameObject);
            AbilitiesSO[0].isActivated = false;
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

    //If you want use from default velocity input zero in fields
    public void TheSetXSpeed(float xSpeed)
    {
        if (xSpeed < 0) xSpeed = _rigidBody2D.linearVelocity.x;
        _rigidBody2D.linearVelocity = new Vector2(xSpeed * FaceDir, _rigidBody2D.linearVelocity.y);
    }
    public void THeSetYSpeed(float ySpeed)
    {
        if(ySpeed < 0) ySpeed = _rigidBody2D.linearVelocity.y;
        _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, ySpeed);

    }
}

