using UnityEngine;

public enum PlayerState { Idle, Walk, Jump, Fall, Dash, Attak, Defend, Hurt, Die }

public class PlayerInputController : MonoBehaviour
{
    public CollectablePlayerAbilities _collectableAbilities;
    private PlayerMovement _movement;

    [Space(5)]
    [Header("Player State")]
    public PlayerState CurrentState;
    public bool isCanDoDifferentWork;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        _theSetPlayerState();
    }
    private void _theSetPlayerState()
    {
        if (isCanDoDifferentWork)
        {
            if (_movement.isWalled)
            {
                if (_movement.isGrounded)
                    CurrentState = PlayerState.Idle;
                else
                    CurrentState = PlayerState.Fall;
            }
            if (Input.GetMouseButtonDown(0))
            {/*
                 && _attackLevel < 4
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
                }*/
                CurrentState = PlayerState.Attak;
            }
            else if (Input.GetMouseButtonDown(1) && _collectableAbilities.isCanDashing)
            {
                CurrentState = PlayerState.Dash;
            }
            else if (Input.GetKey(KeyCode.Space) && (_movement.isGrounded || _collectableAbilities.isCanDubleJumping))
                CurrentState = PlayerState.Jump;

            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && _movement.isGrounded && !_movement.isWalled)
                CurrentState = PlayerState.Walk;

            else if (!_movement.isGrounded && CurrentState != PlayerState.Attak)
            {
                CurrentState = PlayerState.Fall;
            }
            else if (!isAnyInput() && _movement.isGrounded)
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
                _movement.TheIdle();
                break;

            case PlayerState.Walk:
                isCanDoDifferentWork = true;
                _movement.TheWalk();
                break;

            case PlayerState.Jump:
                isCanDoDifferentWork = true;
                //CoolDownTime = 1;
                _movement.TheJump();
                break;

            case PlayerState.Fall:
                isCanDoDifferentWork = true;
                _movement.TheFall();
                break;

            case PlayerState.Dash:
                isCanDoDifferentWork = false;
                _movement.TheDash();
                break;
            case PlayerState.Attak:
                isCanDoDifferentWork = true;
                //_movement.TheAttak();
                break;
            case PlayerState.Defend:
                isCanDoDifferentWork = false;
                //_movement.TheDefend();
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
