using UnityEngine;
using TNRD.Autohook;

namespace Player
{
	public class PlayerCharacter : MonoBehaviour
	{
		[field: SerializeField, AutoHook] public CharacterStateController StateController { get; private set; }
		[field: SerializeField, AutoHook] public PlayerInstrumentPointer InstrumentPointer { get; private set; }
		[field: SerializeField, AutoHook] public PlayerInstrumentActivator InstrumentActivator { get; private set; }
		[field: SerializeField, AutoHook(AutoHookSearchArea.Parent)] public PlayerManager Owner { get; set; }

		public InputSystemActions Input => Owner.Input;

		private void OnValidate()
		{
			OnDisable();
		}

		private void OnEnable()
		{
			StateController.enabled = true;
		}

		private void OnDisable()
		{
			StateController.enabled = false;
		}
	}
}