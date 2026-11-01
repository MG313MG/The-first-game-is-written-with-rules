using Unity.VisualScripting;
using UnityEngine;



public class AIMovement : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D;
    private AIPosition _position;

    
    [Header("Move Variables")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _dashPower;

    public bool isDashing;


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
        if(_position.isGrounded)
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, _jumpPower);
    }
    public void TheDash()
    {
        if(!isDashing)
            _rigidBody2D.linearVelocity = new Vector2(_dashPower * _position.FaceDir, _rigidBody2D.linearVelocity.y);
        isDashing = true;
    }

    public void TheSetDash(bool dash)
    {
        isDashing = dash;
    }
}
