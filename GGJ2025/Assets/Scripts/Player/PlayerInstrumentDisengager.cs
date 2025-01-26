using Instruments;
using System;
using TNRD.Autohook;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
	/// <summary>
	/// Holds the currently pointed-to instrument gameobject.
	/// </summary>
	[RequireComponent(typeof(PlayerCharacter), typeof(PlayerInstrumentActivator))]
	public class PlayerInstrumentDisengager : MonoBehaviour
	{
		[SerializeField, AutoHook] private PlayerCharacter _character;
		[SerializeField, AutoHook] private PlayerInstrumentActivator _activator;

		private void OnEnable() => _character.Input.House.Dash.started += OnDisengage;

		private void OnDisable() => _character.Input.House.Dash.started -= OnDisengage;

		private void OnDisengage(InputAction.CallbackContext context) => _activator.Disengage();
	}
}