using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TNRD.Autohook;

namespace Player
{
	public class PlayerMove : MonoBehaviour
	{
		[SerializeField, AutoHook] private Rigidbody2D _rb;
		[SerializeField] private float _speed = 5f;
		private Vector2 _movementInput;

		private void OnEnable()
		{
			PlayerManager.Instance.Input.House.Move.performed += OnMove;
			PlayerManager.Instance.Input.House.Move.canceled += OnMove;
		}

		private void OnDisable()
		{
			PlayerManager.Instance.Input.House.Move.performed -= OnMove;
			PlayerManager.Instance.Input.House.Move.canceled -= OnMove;
		}

		private void OnMove(InputAction.CallbackContext context)
		{
			_movementInput = context.ReadValue<Vector2>() * _speed;
		}

		private void FixedUpdate()
		{
			_rb.linearVelocity = _movementInput;
		}
	}
}