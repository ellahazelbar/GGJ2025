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
		[SerializeField, AutoHook] private PlayerCharacter _character;
		[SerializeField, AutoHook] private PlayerInstrumentPointer _instrumentPointer;
		private Instrument _currentCache;

		public event UnityAction<Instrument> InstrumentActivated;
		public event UnityAction InstrumentDisengaged;

		private void OnEnable()
		{
			_character.Input.House.Interact.started += OnInteract;
		}

		private void OnDisable()
		{
			_character.Input.House.Interact.started -= OnInteract;
		}

		private void OnInteract(InputAction.CallbackContext context)
		{
			_currentCache = _instrumentPointer.Current;
			if (null == _currentCache)
				return;
			InstrumentActivated?.Invoke(_currentCache);
			_currentCache.Activate(this);
		}

		public void Disengage()
		{
			if (_instrumentPointer.Current != null)
				_instrumentPointer.Current.SetVisible(true);
			else
				_currentCache.SetVisible(true);
			_character.StateController.State = CharacterStateController.CharState.FreeMovement;
			InstrumentDisengaged?.Invoke();
		}
	}
}