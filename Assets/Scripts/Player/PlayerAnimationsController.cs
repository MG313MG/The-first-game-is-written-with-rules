using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{
    private Animator _animator;
    private PlayerAttackAndDefend _attackAndDefend;
    [SerializeField]
    private PlayerInputController _playerInputController;
    [SerializeField]
    private PlayerState[] _movementStates, _takeDamage;

    private float _state;
    private bool isAttaking;
    private bool isHurtingorDie;
    private bool isDashing;

    private bool isDefending;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerInputController = GetComponent<PlayerInputController>();
        _attackAndDefend = GetComponent<PlayerAttackAndDefend>();
    }

    void Update()
    {
        TheSetAanimations(_playerInputController.CurrentState);
    }

    private void TheSetAanimations(PlayerState currentState)
    {
        for(int i = 0; i < _movementStates.Length; i++)
        {
            if (currentState == _movementStates[i])
            {
                Rigidbody2D rigidBody2D;
                rigidBody2D = GetComponent<Rigidbody2D>();
                if (rigidBody2D.linearVelocity.y > 0)
                    _state = 2;
                else if(rigidBody2D.linearVelocity.y < 0)
                {
                    _state = 3;
                }
                else
                {
                    _state = i;
                }
                _animator.SetFloat("State", _state);
            }
        }
        for (int i = 0;i < _takeDamage.Length;i++)
        {
            if(currentState == _takeDamage[i])
            {
                _state = i;
                _animator.SetFloat("State", _state);
            }
        }
        if (currentState == PlayerState.Attak)
        {
            isAttaking = true;
            _animator.SetBool("isAttaking", isAttaking);
            _state = _attackAndDefend.AttackLevel;
            _animator.SetFloat("State", _state);
        }
        else if (currentState == PlayerState.Defend)
        {
            isDefending = true;
            _animator.SetBool("isDefending", isDefending);
        }
        else if (currentState == PlayerState.Dash)
        {
            isDashing = true;
            _animator.SetBool("isDashing", isDashing);
        }
    }
    public void TheAnimationChangerForStaticAnimations()
    {
        Debug.Log("Method called");
        isAttaking = false;
        isDefending = false;
        isDashing = false;
        _animator.SetBool("isDefending", isDefending);
        _animator.SetBool("isAttaking", isAttaking);
        _animator.SetBool("isDashing", isDashing);
        PlayerPosition playerPosition;
        playerPosition = GetComponent<PlayerPosition>();
        if (playerPosition.isGrounded)
            _playerInputController.CurrentState = PlayerState.Idle;
        else if (!playerPosition.isGrounded)
        {
            _playerInputController.CurrentState = PlayerState.Fall;
        }
    }
}
