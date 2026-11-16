using Unity.VisualScripting;
using UnityEngine;

public class AIAnimationsController : MonoBehaviour
{
    private Animator _animator;
    private AISetState _aiSetState;

    [Header("bools of Animations")]
    [SerializeField]
    private bool isAttacking;
    [SerializeField]
    private bool isHurtingOrDie;
    [SerializeField]
    private bool isDashing;
    [SerializeField]
    private bool isJumpingOrFalling;
    [Space(5)]
    [Header("State Number")]
    [SerializeField]
    private float _state;

    [Space(5)]
    [Header("Attacks")]
    [SerializeField]
    private float _attackLevel;
    [SerializeField]
    private float _attackNumber;
    

    void Start()
    {
        _animator = GetComponent<Animator>();
        _aiSetState = GetComponent<AISetState>();
    }


    void Update()
    {
        switch (_aiSetState.CurrentState)
        {
            case EnemyStates.Fall:
            case EnemyStates.Jump:
                _theJumpAndFall();
                break;
            case EnemyStates.Die:
            case EnemyStates.Hit:
                _theSetGroup(false, false, true, false);
                break;
            case EnemyStates.Dash:
                _theSetGroup(false, true, false, false);
                break;
            case EnemyStates.Attack:
                _theSetGroup(true, false, false, false);
                _animator.SetFloat("State",_attackLevel);
                break;
            default:
                _theSetGroup(false, false, false, false);
                _animator.SetFloat("State", (int)_aiSetState.CurrentState);
                break;
        }
    }

    private void _theJumpAndFall()
    {
        if (_aiSetState.CurrentState == EnemyStates.Jump)
            _state = 0;
        else if (_aiSetState.CurrentState == EnemyStates.Fall)
            _state = 1;
        _animator.SetFloat("State", _state);
        _theSetGroup(false, false, false, true);
    }

    void _theSetGroup(bool attack, bool dash, bool hurtDie, bool jumpFall)
    {
        _animator.SetBool("isAttacking", attack);
        _animator.SetBool("isDashing", dash);
        _animator.SetBool("isHurtingOrDie", hurtDie);
        _animator.SetBool("isJumpingOrFalling", jumpFall);
    }

    public void TheAddAttackLevel()
    {
        _attackLevel++;
        if( _attackLevel == _attackNumber ) 
            _attackLevel = 0;
    }
}
