using UnityEngine;

public abstract class AbilitiesSO : ScriptableObject
{
    public string AbilityName;
    public float StaminaCost;
    public float CoolDownTime;
    public bool isActivated;
    public float ActivateTime;

    public abstract void Activate(GameObject user);
}
