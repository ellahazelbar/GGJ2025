using UnityEngine;
using TNRD.Autohook;

namespace Player
{
	public class PlayerMove : MonoBehaviour
	{
		[SerializeField, AutoHook] private PlayerCharacter _player;
		[SerializeField, AutoHook] private Rigidbody2D _rb;
		[field: SerializeField] public float Speed { get; set; } = 5f;

		private void FixedUpdate()
		{
			_rb.linearVelocity = _player.Input.House.Move.ReadValue<Vector2>() * Speed;
		}
	}
}