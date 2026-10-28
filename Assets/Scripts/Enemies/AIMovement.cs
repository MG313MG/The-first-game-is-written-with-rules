using Unity.VisualScripting;
using UnityEngine;

public enum EnemyStates { Idle, Walk, Run, Jump, Fall, Dash, Attack, Hit, Die }

public class AIMovement : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D;
    private AIPosition _position;

    [Header("Abilities")]
    [SerializeField] private bool _canRun;
    [SerializeField] private bool _canDash;
    [SerializeField] private bool _canJump;
    [Space(5)]
    [Header("Move Variables")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _dashPower;

    private bool isDashing;


    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _position = GetComponent<AIPosition>();
    }

    
    void Update()
    {

    }

    public void TheWalk()
    {
        _rigidBody2D.linearVelocity = new Vector2(_walkSpeed * _position.FaceDir, _rigidBody2D.linearVelocity.y);
    }
    public void TheRun()
    {
        _rigidBody2D.linearVelocity = new Vector2(_runSpeed * _position.FaceDir, _rigidBody2D.linearVelocity.y);
    }
    public void TheJump()
    {
        if(_position.isGrounded && _canJump)
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, _jumpPower);
    }
    public void TheDash()
    {
        if(!isDashing && _canDash)
            _rigidBody2D.linearVelocity = new Vector2(_dashPower * _position.FaceDir, _rigidBody2D.linearVelocity.y);
        isDashing = true;
    }

    public void TheSetDash(bool dash)
    {
        isDashing = dash;
    }
}
