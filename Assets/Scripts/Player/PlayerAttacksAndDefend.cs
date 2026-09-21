using UnityEngine;

public class PlayerAttackAndDefend : MonoBehaviour
{
    [SerializeField]
    private CollectablePlayerAbilities _collectedAbilities;
    private PlayerMovement _playerMovement;
    private PlayerPosition _playerPosition;
    public float AttackLevel { get; private set; }
    [SerializeField]
    private float _comboTimer;

    [SerializeField]
    private bool isPlayingSomeAttack;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerPosition = GetComponent<PlayerPosition>();
        AttackLevel = 0;
    }

    void Update()
    {
        if (_comboTimer > 0)
        {
            _comboTimer -= Time.deltaTime;
            if (_comboTimer <= 0)
                AttackLevel = 0;
        } 

        if (_playerPosition.isGrounded && AttackLevel != 0)
            AttackLevel = 0;
    }
    public void TheAttak()
    {
        if (!isPlayingSomeAttack)
        {
            isPlayingSomeAttack = true;
            if (!_playerPosition.isGrounded)
            {
                Debug.Log("Worked");
                AttackLevel = 4;
                _comboTimer = 1.1f;
                return;
            }
            else if (_playerPosition.isGrounded && AttackLevel < 4)
            {
                if (AttackLevel > 5)
                    AttackLevel = 0;
                AttackLevel++;

                if (AttackLevel == 1)
                    _comboTimer = 1.7f;
                else if (AttackLevel == 2)
                    _comboTimer = 3f;
                else if (AttackLevel == 3)
                    _comboTimer = 1.1f;
            }
        }
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
    public void TheSetIsPlayingAttackToFalse()
    {
        isPlayingSomeAttack = false;
    }
}
