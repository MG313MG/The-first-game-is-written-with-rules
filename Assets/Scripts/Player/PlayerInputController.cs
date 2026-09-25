using UnityEngine;
using System;

public enum PlayerState { Idle, Walk, Jump, Fall, Dash, Attak, Defend, Heal, Hurt, Dead }

public class PlayerInputController : MonoBehaviour
{
    public CollectablePlayerAbilities _collectableAbilities;
    private PlayerMovement _playerMovement;
    private PlayerPosition _playerPosition;
    private PlayerAttackAndDefend _attackAndDefend;

    [Space(5)]
    [Header("Player State")]
    public PlayerState CurrentState;
    [SerializeField]
    private PlayerState _lastState;

    public bool isCanDoDifferentWork;

    public Action<PlayerState> SendState;
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerPosition = GetComponent<PlayerPosition>();
        _attackAndDefend = GetComponent<PlayerAttackAndDefend>();
    }

    private void Update()
    {
        _theSetPlayerState();
        _theStateSender();
    }
    private void _theSetPlayerState()
    {
        if (isCanDoDifferentWork)
        {
            if (_playerPosition.isWalled)
            {
                if (_playerPosition.isGrounded)
                    CurrentState = PlayerState.Idle;
                else
                    CurrentState = PlayerState.Fall;
            }
            if (Input.GetMouseButtonDown(0) && _attackAndDefend.AttackLevel < 5)
            {
                _attackAndDefend.TheAttak();
                CurrentState = PlayerState.Attak;
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _attackAndDefend.TheDefend();
                CurrentState = PlayerState.Defend;
            }
            else if (Input.GetMouseButtonDown(1) && _collectableAbilities.isCanDashing)
            {
                CurrentState = PlayerState.Dash;
            }
            else if (Input.GetKey(KeyCode.Space) && (_playerPosition.isGrounded || _collectableAbilities.isCanDubleJumping))
                CurrentState = PlayerState.Jump;

            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && _playerPosition.isGrounded && !_playerPosition.isWalled)
                CurrentState = PlayerState.Walk;

            else if (!_playerPosition.isGrounded && CurrentState != PlayerState.Attak)
            {
                CurrentState = PlayerState.Fall;
            }
            else if (!isAnyInput() && _playerPosition.isGrounded)
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
                isCanDoDifferentWork = false;
                break;
            case PlayerState.Defend:
                isCanDoDifferentWork = false;
                break;
            case PlayerState.Heal:
                isCanDoDifferentWork = false;
                break;
            case PlayerState.Hurt: 
                isCanDoDifferentWork = false;
                break;
            case PlayerState.Dead:
                isCanDoDifferentWork = false;
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

    private void _theStateSender()
    {
        if (_lastState != CurrentState)
        {
            _lastState = CurrentState;
            SendState?.Invoke(CurrentState);
        }
    }
}
