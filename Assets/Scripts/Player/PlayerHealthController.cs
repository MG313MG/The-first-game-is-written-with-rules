using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour,IDamageble
{
    private PlayerInputController _playerInputController;

    [Header("Health")]
    [SerializeField]
    private float _currentHealth;
    public float MaxHealth;

    public Action<float> HealthSender;

    void Start()
    {
        _playerInputController = GetComponent<PlayerInputController>();
    }

    public void TheHealPlayer(float healValue)
    {
        _currentHealth += healValue;
        HealthSender?.Invoke(_currentHealth);
        _playerInputController.CurrentState = PlayerState.Heal;
        if(_currentHealth > MaxHealth)
            _currentHealth = MaxHealth;
    }

    public void TheTakeDamage(float damage)
    {
        _currentHealth -= damage;
        HealthSender?.Invoke(_currentHealth);
        if (_currentHealth <= 0)
        {
            TheDead();
            return;
        }
        _playerInputController.CurrentState = PlayerState.Hurt;
    }

    public void TheDead()
    {
        _playerInputController.CurrentState = PlayerState.Dead;
        print("Player is dead");
    }

}
