using UnityEngine;

public class TornadoScript : MonoBehaviour
{
    private Transform _transform;
    [SerializeField]
    private PlayerMovement _playerMovement;
    private SpriteRenderer _spriteRenderer;
    [Space(5)]
    [Header("Tornado")]
    [SerializeField] private GameObject _tornadoSpawnPoint;
    [SerializeField] private float _tornadoCostStamina;

    [SerializeField]
    private float _speed, _damage;
    void Start()
    {
        gameObject.transform.position = _tornadoSpawnPoint.transform.position;
        _transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        _transform.position = new Vector3(_transform.position.x + (_speed * _playerMovement.FaceDir), _transform.position.y, _transform.position.z);
    }

    public void TheDisabler()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = _tornadoSpawnPoint.transform.position;
    }
    public void TheEnabler()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.GetComponent<IDamageble>() != null && collision.GetComponent<Player>() == null)
        {
            collision.GetComponent<IDamageble>().TakeDamage(_damage);
        }*/
    }
}
