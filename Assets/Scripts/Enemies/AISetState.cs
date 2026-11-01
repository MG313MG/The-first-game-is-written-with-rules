using System.Collections;
using UnityEngine;

public enum EnemyStates { Idle, Walk, Run, Jump, Fall, Dash, Attack, Hit, Die }

public class AISetState : MonoBehaviour
{
    private AIMovement _movement;
    private AIPosition _position;

    private Rigidbody2D _rigidbody2D;

    public EnemyStates CurrentState;

    [Header("Abilities")]
    [SerializeField] private bool isCanIdle;
    [SerializeField] private bool isCanRun;
    [SerializeField] private bool isCanDash;
    [SerializeField] private bool isCanJump;
    [Space(5)]

    [Header("Following a player and more ...")] 
    public bool isPlayerHere;
    [SerializeField] private bool _canFollowPlayer;

    [SerializeField] private float _playerFollowTime = 2;
    [Space(5)]
    [Header("Enemy")]

    public bool CanDoDifferentWork;
    [SerializeField] private float _idleTime = 1.5f;

    void Start()
    {
        _movement = GetComponent<AIMovement>();
        _position = GetComponent<AIPosition>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(_canFollowPlayer && _position.PlayerRaycast)
        {
            isPlayerHere = true;
            StartCoroutine(_thePLayerIsNotHere());
        }
        if(CanDoDifferentWork)
        {
            if (_position.PlayerRaycast && _position.PlayerRaycast.distance < 2.75f)
                CurrentState = EnemyStates.Attack;
            else if (isCanDash && _position.PlayerRaycast)
                CurrentState = EnemyStates.Dash;
            else if (_position.PlayerRaycast && !isCanDash)
                CurrentState = EnemyStates.Run;
            else if (!_position.isGrounded && _rigidbody2D.linearVelocityY < 0)
                CurrentState = EnemyStates.Fall;
            else if (!_position.isGrounded && (_position.isWallInYourFront || !_position.isGroundInYourFront) && isPlayerHere)
                CurrentState = EnemyStates.Jump;
            else if (_position.isGrounded && CurrentState != EnemyStates.Idle)
                CurrentState = EnemyStates.Walk;
        }
    }

    private void FixedUpdate()
    {
        _theSwitchStates();
    }

    private void _theSwitchStates()
    {
        switch (CurrentState)
        {
            case EnemyStates.Idle:
                _rigidbody2D.linearVelocity = new Vector2(0, _rigidbody2D.linearVelocity.y);
                break;
            case EnemyStates.Walk:
                CanDoDifferentWork = true;
                _movement.TheWalk();
                break;
            case EnemyStates.Run:
                CanDoDifferentWork = true;
                _movement.TheRun();
                break;
            case EnemyStates.Jump:
                CanDoDifferentWork = false;
                _movement.TheJump();
                break;
            case EnemyStates.Fall:
                CanDoDifferentWork = false;
                break;
            case EnemyStates.Dash:
                CanDoDifferentWork = false;
                _movement.TheDash();
                break;
            case EnemyStates.Hit:
                CanDoDifferentWork = false;
                break;
            case EnemyStates.Die:
                CanDoDifferentWork = false;
                break;
        }
    }

    public void TheSetCanDoDifferentWork(bool canDoDifferentWork)
    {
        CanDoDifferentWork &= canDoDifferentWork;
    }

    public void TheSetIDLEMode()
    {
        if (isCanIdle)
        {
            int randomNumber = UnityEngine.Random.Range(1, 6);
            if(randomNumber == 3)
            {
                CurrentState = EnemyStates.Idle;
            }
            StartCoroutine(_theIDLETimer());
        }
    }

    private IEnumerator _thePLayerIsNotHere()
    {
        yield return new WaitForSeconds(_playerFollowTime);
        isPlayerHere = false;
    }
    private IEnumerator _theIDLETimer()
    {
        yield return new WaitForSeconds(_idleTime);
        CurrentState = EnemyStates.Walk;
    }
}
