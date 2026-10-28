using UnityEngine;

public class AIPosition : MonoBehaviour
{
    [Header("Need To Check")]
    [SerializeField] private bool isPlayer;
    [Space(5)]
    [Header("Position bools")]
    public bool isGrounded;
    public bool isGroundInYourFront;
    public bool isWallInYourFront;

    [Space(5)]
    [Header("Position game objects")]
    [SerializeField]
    private GameObject _gameObjectIsGroundInYourFront;
    [SerializeField]
    private GameObject _gameObjectIsPlayerOrWallInYourFront;

    [Space(5)]
    [Header("Position distances")]
    [SerializeField]
    private float _groundDistance;
    [SerializeField]
    private float _wallDistance;
    [SerializeField]
    private float _playerDistance;

    [Space(5)]
    [Header("Position layers")]
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private LayerMask _playerLayer;

    [Space(5)]
    [Header("Face")]
    public float FaceDir;
    private float _xScale;

    public RaycastHit2D PlayerRaycast;

    public bool IsPlayerVisible()
    {
        return isPlayer
            && PlayerRaycast.collider != null
            && ((1 << PlayerRaycast.collider.gameObject.layer) & _playerLayer) != 0;
    }

    public float PlayerDistance => PlayerRaycast.distance;

    void Start()
    {
        _xScale = transform.localScale.x;
        FaceDir = UnityEngine.Random.value < 0.5f ? -1 : 1;
        _theChangeFace();
    }

    void Update()
    {
        _theCheckPosition();
        if ((!isGroundInYourFront && isGrounded) || isWallInYourFront)
            _theChangeFace();
        if (Input.GetKey(KeyCode.E) && IsPlayerVisible())
            print("Is player");
    }

    private void _theCheckPosition()
    {
        Vector3 groundFrontPos = _gameObjectIsGroundInYourFront.transform.position;
        Vector3 wallPos = _gameObjectIsPlayerOrWallInYourFront.transform.position;

        isGrounded = Physics2D.Raycast(new Vector3(transform.position.x, groundFrontPos.y, transform.position.z), Vector2.down, _groundDistance, _groundLayer);
        isGroundInYourFront = Physics2D.Raycast(groundFrontPos, Vector2.down, _groundDistance, _groundLayer);
        isWallInYourFront = Physics2D.Raycast(wallPos, Vector2.right * FaceDir, _wallDistance, _groundLayer);

        if (isPlayer)
            PlayerRaycast = Physics2D.Raycast(wallPos, Vector2.right * FaceDir, _playerDistance, _playerLayer | _groundLayer);
    }

    private void _theChangeFace()
    {
        FaceDir *= -1;
        transform.localScale = new Vector3(_xScale * FaceDir, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        if (_gameObjectIsGroundInYourFront == null || _gameObjectIsPlayerOrWallInYourFront == null) return;

        Vector3 groundFrontPos = _gameObjectIsGroundInYourFront.transform.position;
        Vector3 wallPos = _gameObjectIsPlayerOrWallInYourFront.transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x, groundFrontPos.y, transform.position.z), new Vector3(transform.position.x, groundFrontPos.y - _groundDistance, transform.position.z));
        Gizmos.color = new Color(1f, 0.5f, 0f);
        Gizmos.DrawLine(groundFrontPos, new Vector3(groundFrontPos.x, groundFrontPos.y - _groundDistance, groundFrontPos.z));
        if (isPlayer)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(wallPos, new Vector3(wallPos.x + (_playerDistance * FaceDir), wallPos.y, wallPos.z));
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(wallPos, new Vector3(wallPos.x + (_wallDistance * FaceDir), wallPos.y, wallPos.z));
    }
}