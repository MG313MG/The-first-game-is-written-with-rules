using UnityEngine;

public enum PlayerState { Idle, Walk, Jump, Fall, Dash, Attak, Defend, Hurt, Die }

public class PlayerInputController : MonoBehaviour
{
    public CollectablePlayerAbilities _collectableAbilities;
    private PlayerMovement _playerMovement;

    private PlayerAttackAndDefend _attackAndDefend;

    [Space(5)]
    [Header("Player State")]
    public PlayerState CurrentState;
    public bool isCanDoDifferentWork;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _attackAndDefend = GetComponent<PlayerAttackAndDefend>();
    }

    private void Update()
    {
        _theSetPlayerState();
    }
    private void _theSetPlayerState()
    {
        if (isCanDoDifferentWork)
        {
            if (_playerMovement.isWalled)
            {
                if (_playerMovement.isGrounded)
                    CurrentState = PlayerState.Idle;
                else
                    CurrentState = PlayerState.Fall;
            }
            if (Input.GetMouseButtonDown(0) && _attackAndDefend.AttackLevel < 4)
            {
                if (_playerMovement.isGrounded)
                {
                    _attackAndDefend.TheAttak();
                }
                CurrentState = PlayerState.Attak;
            }
            else if (Input.GetMouseButtonDown(1) && _collectableAbilities.isCanDashing)
            {
                CurrentState = PlayerState.Dash;
            }
            else if (Input.GetKey(KeyCode.Space) && (_playerMovement.isGrounded || _collectableAbilities.isCanDubleJumping))
                CurrentState = PlayerState.Jump;

            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && _playerMovement.isGrounded && !_playerMovement.isWalled)
                CurrentState = PlayerState.Walk;

            else if (!_playerMovement.isGrounded && CurrentState != PlayerState.Attak)
            {
                CurrentState = PlayerState.Fall;
            }
            else if (!isAnyInput() && _playerMovement.isGrounded)
            {
                Rigidbody2D _rigidBody2D = GetComponent<Rigidbody2D>();
                _rigidBody2D.linearVelocity = new Vector2(0, _rigidBody2D.linearVelocity.y);
                CurrentState = PlayerState.Idle;
            }
        }
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
                _playerMovement.TheIdle();
                break;

            case PlayerState.Walk:
                isCanDoDifferentWork = true;
                _playerMovement.TheWalk();
                break;

            case PlayerState.Jump:
                isCanDoDifferentWork = true;
                //CoolDownTime = 1;
                _playerMovement.TheJump();
                break;

            case PlayerState.Fall:
                isCanDoDifferentWork = true;
                _playerMovement.TheFall();
                break;

            case PlayerState.Dash:
                isCanDoDifferentWork = false;
                _playerMovement.TheDash();
                break;
            case PlayerState.Attak:
                isCanDoDifferentWork = true;
                break;
            case PlayerState.Defend:
                isCanDoDifferentWork = false;
                //_playerMovement.TheDefend();
                break;
        }
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
}
