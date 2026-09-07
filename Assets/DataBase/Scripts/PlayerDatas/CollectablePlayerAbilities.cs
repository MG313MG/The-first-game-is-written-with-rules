using UnityEngine;

[CreateAssetMenu(menuName = "Player/Abilities/CollectableAbilities")]
public class CollectablePlayerAbilities : ScriptableObject
{
    public bool isCanDashing;
    public bool isCanDubleJumping;
    public bool isCanTornadoAttacking;

    public void The_Set_Movement_To_True()
    {
        isCanDashing = true;
        isCanDubleJumping = true;
    }
    public void The_Set_Attack_To_True()
    {
        isCanDubleJumping = true;
    }
}
