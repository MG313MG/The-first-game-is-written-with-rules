using UnityEngine;

public class PlayerAttackAndDefend : MonoBehaviour
{
    [SerializeField]
    private CollectablePlayerAbilities _collectedAbilities;
    private PlayerMovement _playerMovement;
    public float AttackLevel;
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        
    }
    public void TheAttak()
    {
        if (!_playerMovement.isGrounded && _playerMovement.isOnAir)
        {
            Debug.Log($"Attack Level : {AttackLevel}");
        }
        else if (!_playerMovement.isGrounded && !_playerMovement.isOnAir)
        {
            Debug.Log($"Attack Level : {AttackLevel}");
        }
        else if (_playerMovement.isGrounded)
        {
            if (AttackLevel == 1)
            { 
                Debug.Log($"Attack Level : {AttackLevel}");
            }
            else if (AttackLevel == 2)
            {
                Debug.Log($"Attack Level : {AttackLevel}");
            }
            else if (AttackLevel == 3 && _collectedAbilities.isCanTornadoAttacking)
            {   
                Debug.Log($"Attack Level : {AttackLevel}");
            }
            else
            { 
                Debug.Log($"Attack Level : {AttackLevel}");
            }
        }
    }
}
