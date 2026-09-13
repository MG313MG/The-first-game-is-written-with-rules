using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    [Header("")]
    public Vector3 LastPositionOnTheGround;
    public Vector3 RespawnPointSet;

    [Space(5)]
    [Header("Bool of Distances")]
    public bool isOnAir;
    public bool isWalled;
    public bool isGrounded;

    [Space(5)]
    [Header("Layers")]
    [SerializeField] private LayerMask _groundLayer;

    [Space(5)]
    [Header("Float of Distances")]
    [SerializeField] private float _checkAirDistance;
    [SerializeField] private float _checkWallDistance;
    [SerializeField] private float _checkGroundDistance;

    [Space(5)]
    [Header("Game objects for check distances")]
    [SerializeField] private GameObject _wallDistanceCheckerGameObject;
    private PlayerInputController _playerInputController;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInputController = GetComponent<PlayerInputController>();
    }

    void Update()
    {
        _theCheckDistance();
        if(_playerInputController.CurrentState == PlayerState.Jump && LastPositionOnTheGround == Vector3.zero)
        {
            LastPositionOnTheGround = transform.position;
        }
        if(isGrounded && LastPositionOnTheGround != Vector3.zero)
        {
            LastPositionOnTheGround = Vector3.zero;
        }
    }
    private void _theCheckDistance()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, _checkGroundDistance, _groundLayer);
        isWalled = Physics2D.Raycast(_wallDistanceCheckerGameObject.transform.position, Vector2.right * _playerMovement.FaceDir, _checkWallDistance, _groundLayer);
        isOnAir = Physics2D.Raycast(transform.position, Vector2.down, _checkAirDistance, _groundLayer);
    }
}
