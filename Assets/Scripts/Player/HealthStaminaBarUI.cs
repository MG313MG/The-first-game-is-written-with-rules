using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthStaminaBarUI : MonoBehaviour
{
    private PlayerStaminaController _playerStaminaController;
    private PlayerHealthController _playerHealthController;

    [Header("Bras")]
    [SerializeField]
    private Slider _healthSlider, _staminaSlider;
    
    void Start()
    {
        _playerHealthController = GetComponent<PlayerHealthController>();
        _playerStaminaController = GetComponent<PlayerStaminaController>();

        _playerHealthController.HealthSender += TheSetHealthBarUI;
        _playerStaminaController.StaminaSender += TheSetStaminaBarUI;

        _healthSlider.maxValue = _playerHealthController.MaxHealth;
        _staminaSlider.maxValue = _playerStaminaController.MaxStamina;
    }

    
    void Update()
    {
        
    }

    public void TheSetHealthBarUI(float setHealth)
    {
        _healthSlider.value = setHealth;
    }
    public void TheSetStaminaBarUI(float setStamina)
    {
        _staminaSlider.value = setStamina;
    }
}
