using UnityEngine;
using System;
using System.Collections;

public class PlayerStaminaController : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField]
    private float _currentStamina;
    public float MaxStamina;

    public Action<float> StaminaSender;

    private bool isRechargeMode;

    void Start()
    {
        if (_currentStamina < MaxStamina)
            StartCoroutine(_theStaminaRechargerTimer());
    }

    public void ThePlayerStaminaConsumption(float consumptionValue)
    {
        if (_currentStamina >= consumptionValue)
        {
            _currentStamina -= consumptionValue;
            StaminaSender?.Invoke(_currentStamina);
        }
        else
            print("You dont have enough stamina");
        StopAllCoroutines();
        isRechargeMode = false;
        StartCoroutine(_theStaminaRechargerTimer());
    }

    public void TheStaminaRecharger(float rechargeValue)
    {

        _currentStamina += rechargeValue;
        if(_currentStamina > MaxStamina)
            _currentStamina = MaxStamina;
        StaminaSender?.Invoke(_currentStamina);
        if (_currentStamina < MaxStamina)
        {
            isRechargeMode = true;
            StartCoroutine(_theStaminaRechargerTimer());
        }
        else if (_currentStamina == MaxStamina)
            StopAllCoroutines();
    }

    private IEnumerator _theStaminaRechargerTimer()
    {
        if (!isRechargeMode)
            yield return new WaitForSeconds(5);
        else
            yield return new WaitForSeconds(3);
        TheStaminaRecharger(5);
    }
}
