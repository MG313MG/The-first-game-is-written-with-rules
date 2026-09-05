using UnityEngine;

[CreateAssetMenu(menuName = "Player/Abilities/DubleJump")]
public class DubleJumpSO : AbilitiesSO
{
    public override void Activate(GameObject user)
    {
        Rigidbody2D _rigidBody2D = user.GetComponent<Rigidbody2D>();
        PlayerMovement _playerMovement = user.GetComponent<PlayerMovement>();

        if (_playerMovement != null && _rigidBody2D != null)
        {
            _rigidBody2D.linearVelocity = new Vector2(_rigidBody2D.linearVelocity.x, _playerMovement.JumpSpeed);
            _playerMovement.isCanDubleJumping = false;
            _playerMovement.isCanDashing = true;
        }
    }
}
