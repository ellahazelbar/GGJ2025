using TNRD.Autohook;
using UnityEngine;

namespace Player
{
	public class PlayerCharacter : MonoBehaviour
	{
		[field: SerializeField, AutoHook] public PlayerInstrumentPointer InstrumentPointer { get; private set; }
		[field: SerializeField, AutoHook] public PlayerInstrumentActivator InstrumentActivator { get; private set; }
		public InputSystemActions Input { get; private set; }

		private void Awake()
		{
			Input = new();
		}

		private void OnEnable()
		{
			Input.Enable();
		}

		private void OnDisable()
		{
			Input.Disable();
		}

		private void OnDestroy()
		{
			Input.Disable();
			Input.Dispose();
		}
	}
}