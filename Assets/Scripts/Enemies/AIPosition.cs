using System;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

public enum EnemyStates {Idle, Walk, Run, Jump, Fall, Attack, Hit, Die}

public class AIPosition : MonoBehaviour
{
    [Header("Need To Check")]
    [SerializeField] private bool isPlayer;
    [Space(5)]
    [Header("Position bools")]
    public bool isGrounded;
    public bool isGroundInYourFront; 
    public bool isWallInYourFront;
    public bool isPlayerInYourFront;

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
    [Header("Position distances")]
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private LayerMask _playerLayer;

    private float _xScale;
    private float _faceDir;

    private RaycastHit2D PlayerRaycast;
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
        if (PlayerRaycast != isPlayerInYourFront)
            isPlayerInYourFront = PlayerRaycast;
    }

    private void _theCheckPosition()
    {
        isGrounded = Physics2D.Raycast(new Vector3(transform.position.x, _gameObjectIsGroundInYourFront.transform.position.y, transform.position.z), Vector2.down, _groundDistance, _groundLayer);
        isGroundInYourFront = Physics2D.Raycast(new Vector3(_gameObjectIsGroundInYourFront.transform.position.x, _gameObjectIsGroundInYourFront.transform.position.y, _gameObjectIsGroundInYourFront.transform.position.z), Vector2.down, _groundDistance, _groundLayer);
        isWallInYourFront = Physics2D.Raycast(_gameObjectIsPlayerOrWallInYourFront.transform.position, Vector2.right * _faceDir, _wallDistance, _groundLayer);
        if(isPlayer)
            PlayerRaycast = Physics2D.Raycast(_gameObjectIsPlayerOrWallInYourFront.transform.position, Vector2.right * -_faceDir, -_playerDistance, _playerLayer);
    }
    private void _theChangeFace()
    {
        _faceDir *= -1;
        transform.localScale = new Vector3(_xScale * _faceDir, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x, _gameObjectIsGroundInYourFront.transform.position.y, transform.position.z), new Vector3(transform.position.x, _gameObjectIsGroundInYourFront.transform.position.y - _groundDistance, transform.position.z));
        Gizmos.color = Color.orange;
        Gizmos.DrawLine(new Vector3(_gameObjectIsGroundInYourFront.transform.position.x, _gameObjectIsGroundInYourFront.transform.position.y, _gameObjectIsGroundInYourFront.transform.position.z), new Vector3(_gameObjectIsGroundInYourFront.transform.position.x, _gameObjectIsGroundInYourFront.transform.position.y - _groundDistance, _gameObjectIsGroundInYourFront.transform.position.z));
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(new Vector3(_gameObjectIsPlayerOrWallInYourFront.transform.position.x, _gameObjectIsPlayerOrWallInYourFront.transform.position.y, _gameObjectIsPlayerOrWallInYourFront.transform.position.z), new Vector3(_gameObjectIsPlayerOrWallInYourFront.transform.position.x + (_playerDistance * _faceDir), _gameObjectIsPlayerOrWallInYourFront.transform.position.y, _gameObjectIsPlayerOrWallInYourFront.transform.position.z));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(_gameObjectIsPlayerOrWallInYourFront.transform.position.x, _gameObjectIsPlayerOrWallInYourFront.transform.position.y, _gameObjectIsPlayerOrWallInYourFront.transform.position.z), new Vector3(_gameObjectIsPlayerOrWallInYourFront.transform.position.x + (_wallDistance * _faceDir), _gameObjectIsPlayerOrWallInYourFront.transform.position.y, _gameObjectIsPlayerOrWallInYourFront.transform.position.z));
    }
}
