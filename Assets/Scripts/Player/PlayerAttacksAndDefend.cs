using UnityEngine;

public class PlayerAttackAndDefend : MonoBehaviour
{
    [SerializeField]
    private CollectablePlayerAbilities _collectedAbilities;
    private PlayerMovement _playerMovement;
    private PlayerPosition _playerPosition;
    public float AttackLevel;
    [SerializeField]
    private float _comboTimer;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerPosition = GetComponent<PlayerPosition>();
    }

    void Update()
    {
        if (_comboTimer > 0)
        {
            _comboTimer -= Time.deltaTime;
            if (_comboTimer <= 0)
                AttackLevel = 0;
        }
    }
    public void TheAttak()
    {
        if(_playerPosition.isOnAir)
        {
            AttackLevel = 4;
            _comboTimer = 1.1f;
            Debug.Log($"Attack Level : {AttackLevel}");
            return;
        }    
        if (AttackLevel >= 3)
            AttackLevel = 0;
        AttackLevel++;

        if (AttackLevel == 1)
            _comboTimer = 1.7f;
        else if (AttackLevel == 2)
            _comboTimer = 3f;
        else if (AttackLevel == 3)
            _comboTimer = 1.1f;
    }
    public void TheDefend()
    {
        if (_playerPosition.isGrounded)
        {
            //Play the air defense animation
        }
        else
        {
            //Play the ground defense aniation 
        }
    }
}
