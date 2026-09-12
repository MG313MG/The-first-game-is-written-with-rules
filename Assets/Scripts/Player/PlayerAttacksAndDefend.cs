using UnityEngine;

public class PlayerAttackAndDefend : MonoBehaviour
{
    [SerializeField]
    private CollectablePlayerAbilities _collectedAbilities;
    private PlayerMovement _playerMovement;
    public float AttackLevel;
    [SerializeField]
    private float _comboTimer;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
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
        if(_playerMovement.isOnAir)
        {
            AttackLevel = 4;
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

        Debug.Log($"Attack Level : {AttackLevel}");
    }
}
