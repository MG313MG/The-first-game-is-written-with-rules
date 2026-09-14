using Unity.VectorGraphics;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField] 
    private AbilityManager _abilityManager;

    [Header("Positions")]
    public Vector3 LastPositionOnTheGround { get; private set; }
    public Vector3 RespawnPointSet { get; private set; }

    [Header("Bool of Distances")]
    public bool isOnAir { get; private set; }
    public bool isWalled { get; private set; }
    public bool isGrounded { get; private set; }

    [Space(5)]
    [Header("Layers")]
    [SerializeField] private LayerMask _groundLayer;

    [Space(5)]
    [Header("Float of Distances")]
    [SerializeField] private float _checkWallDistance;
    [SerializeField] private float _checkGroundDistance;

    [Space(5)]
    [Header("Game objects for check distances")]
    [SerializeField] private GameObject _wallDistanceCheckerGameObject;
    private PlayerInputController _playerInputController;

    void Start()
    {
        _playerInputController = GetComponent<PlayerInputController>();
    }

    void Update()
    {
        _theCheckDistance();
        if (!isGrounded)
            isOnAir = true;
        else 
            isOnAir = false;
        if(!isGrounded && LastPositionOnTheGround == Vector3.zero)
        {
            LastPositionOnTheGround = transform.position;
        }
        if(isGrounded && LastPositionOnTheGround != Vector3.zero)
        {
            LastPositionOnTheGround = Vector3.zero;
        }
    }
    public void SetRespawnPoint(Vector3 point, Scene sceneOfRespawnPointSet)
    {
        RespawnPointSet = point;
    }
    private void _theCheckDistance()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, _checkGroundDistance, _groundLayer);
        isWalled = Physics2D.Raycast(_wallDistanceCheckerGameObject.transform.position, Vector2.right * _playerMovement.FaceDir, _checkWallDistance, _groundLayer);    }
    //Draw the raycast line
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - _checkGroundDistance, transform.position.z));
        Gizmos.color = Color.gray;
        //Gizmos.DrawLine(_wallDistanceCheckerGameObject.transform.position, new Vector3(_wallDistanceCheckerGameObject.transform.position.x + (_checkWallDistance * _playerMovement.FaceDir), _wallDistanceCheckerGameObject.transform.position.y, _wallDistanceCheckerGameObject.transform.position.z));
        Gizmos.DrawLine(_wallDistanceCheckerGameObject.transform.position, new Vector3(_wallDistanceCheckerGameObject.transform.position.x + (_checkWallDistance * _playerMovement.FaceDir), _wallDistanceCheckerGameObject.transform.position.y, _wallDistanceCheckerGameObject.transform.position.z));
    }
}
