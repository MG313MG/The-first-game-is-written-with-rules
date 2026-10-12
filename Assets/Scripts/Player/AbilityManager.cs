using System.Collections;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField]
    private CollectablePlayerAbilities _collectedAbilities;
    public AbilitiesSO[] abilities;

    public void The_Check_Not_Activated_Abilities()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (!abilities[i].isActivated)
            {
                StartCoroutine(_theAbilityActivator(i));
            }
        }
    }
    private IEnumerator _theAbilityActivator(int _number_Of_Assigned_Array)
    {
        PlayerPosition _playerPosition = GetComponent<PlayerPosition>();
        yield return new WaitUntil(() => _playerPosition.isGrounded);
        yield return new WaitForSeconds(abilities[_number_Of_Assigned_Array].ActivateTime);
            abilities[_number_Of_Assigned_Array].isActivated = true;    
    }
    public void TheSetAbilitiesToTrue()
    {
        _collectedAbilities.isCanDubleJumping = true;
        _collectedAbilities.isCanDashing = true;
    }
}
