using System.Collections;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public AbilitiesSO[] abilities;

    public void The_Check_Not_Activated_Abilities()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            print("For loop did it work");
            if (!abilities[i].isActivated)
            {
                print("Check activated did it");
                StartCoroutine(_theAbilityActivator(i));
            }
        }
    }
    private IEnumerator _theAbilityActivator(int _number_Of_Assigned_Array)
    {
        yield return new WaitForSeconds(abilities[_number_Of_Assigned_Array].ActivateTime);
        abilities[_number_Of_Assigned_Array].isActivated = true;
    }
}
