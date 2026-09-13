using UnityEngine;

public class TornadoScript : MonoBehaviour
{
    private Transform _transform;
    private PlayerMovement _playerMovement;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _speed, _damage, _faceDir;
    void Start()
    {
        _transform = GetComponent<Transform>();
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _faceDir = _playerMovement.FaceDir;
        if (_faceDir != 0)
            _spriteRenderer.flipX = _faceDir == -1;
    }

    void FixedUpdate()
    {
        _transform.position = new Vector3(_transform.position.x + (_speed * _faceDir), _transform.position.y, _transform.position.z);
    }

    public void TheDestoryer()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.GetComponent<IDamageble>() != null && collision.GetComponent<Player>() == null)
        {
            collision.GetComponent<IDamageble>().TakeDamage(_damage);
        }*/
    }
}
