using UnityEngine;
using System;
using System.Collections;

public class PlayerStaminaController : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField]
    public float CurrentStamina;
    public float MaxStamina;

    [SerializeField]
    private float _timeOfStaminaRechargerTimerMode;

    public Action<float> StaminaSender;

    private bool isRechargeMode;

    void Start()
    {
        if (CurrentStamina < MaxStamina)
            StartCoroutine(_theStaminaRechargerTimer());
    }

    public void ThePlayerStaminaConsumption(float consumptionValue)
    {
        if (CurrentStamina >= consumptionValue)
        {
            CurrentStamina -= consumptionValue;
            StaminaSender?.Invoke(CurrentStamina);
        }
        else
            print("You dont have enough stamina");
        StopAllCoroutines();
        isRechargeMode = false;
        StartCoroutine(_theStaminaRechargerTimer());
    }

    public void TheStaminaRecharger(float rechargeValue)
    {

        CurrentStamina += rechargeValue;
        if(CurrentStamina > MaxStamina)
            CurrentStamina = MaxStamina;
        StaminaSender?.Invoke(CurrentStamina);
        if (CurrentStamina < MaxStamina)
        {
            isRechargeMode = true;
            StartCoroutine(_theStaminaRechargerTimer());
        }
        else if (CurrentStamina == MaxStamina)
            StopAllCoroutines();
    }

    private IEnumerator _theStaminaRechargerTimer()
    {
        if (!isRechargeMode)
            yield return new WaitForSeconds(5);
        else
            yield return new WaitForSeconds(_timeOfStaminaRechargerTimerMode);
        TheStaminaRecharger(5);
    }
}
