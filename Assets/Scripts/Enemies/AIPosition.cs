using UnityEngine;

public enum EnemyStates { Idle, Walk, Run, Jump, Fall, Attack, Hit, Die }

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

    private float _xScale;
    private float _faceDir;

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
        _faceDir = UnityEngine.Random.value < 0.5f ? -1 : 1;
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
        isWallInYourFront = Physics2D.Raycast(wallPos, Vector2.right * _faceDir, _wallDistance, _groundLayer);

        if (isPlayer)
            PlayerRaycast = Physics2D.Raycast(wallPos, Vector2.right * _faceDir, _playerDistance, _playerLayer | _groundLayer);
    }

    private void _theChangeFace()
    {
        _faceDir *= -1;
        transform.localScale = new Vector3(_xScale * _faceDir, transform.localScale.y, transform.localScale.z);
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
            Gizmos.DrawLine(wallPos, new Vector3(wallPos.x + (_playerDistance * _faceDir), wallPos.y, wallPos.z));
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(wallPos, new Vector3(wallPos.x + (_wallDistance * _faceDir), wallPos.y, wallPos.z));
    }
}