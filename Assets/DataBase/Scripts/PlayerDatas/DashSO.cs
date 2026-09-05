using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Abilities/Dash")]

public class DashSO : AbilitiesSO
{
    public float DashForce;
    public override void Activate(GameObject user)
    {
        Rigidbody2D _rigidBody2D = user.GetComponent<Rigidbody2D>();
        PlayerMovement _playerMovement = user.GetComponent<PlayerMovement>();
        if (_rigidBody2D != null && isActivated)
        {
            _rigidBody2D.linearVelocity = new Vector2(DashForce * _playerMovement.FaceDir, 0);
            isActivated = false;
        }
    }
}
