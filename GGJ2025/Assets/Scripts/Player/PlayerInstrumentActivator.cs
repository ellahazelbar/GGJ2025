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
	[RequireComponent(typeof(PlayerInstrumentPointer))]
	public class PlayerInstrumentActivator : MonoBehaviour
	{
		[SerializeField, AutoHook] private PlayerCharacter _player;
		[SerializeField, AutoHook] private PlayerInstrumentPointer _instrumentPointer;

		public event UnityAction<Instrument> InstrumentActivated;

		private bool attachedToMinigame;

		private void OnEnable()
		{
			_player.Input.House.Interact.started += OnInteract;
			_player.Input.House.Dash.started += OnDisengage;
		}

		private void OnDisable()
		{
			_player.Input.House.Interact.started -= OnInteract;
			_player.Input.House.Dash.started -= OnDisengage;
		}

		private void OnInteract(InputAction.CallbackContext context)
		{
			Instrument current = _instrumentPointer.Current;
			if (null == current)
				return;
			InstrumentActivated?.Invoke(current);
			attachedToMinigame = true;
			current.Activate(this);
		}

		private void OnDisengage(InputAction.CallbackContext context)
        {
			Disengage();
        }

		public void Disengage()
        {
			Instrument current = _instrumentPointer.Current;
			if (attachedToMinigame && null != current)
			{
				current.Deactivate();
				attachedToMinigame = false;
			}
		}
	}
}