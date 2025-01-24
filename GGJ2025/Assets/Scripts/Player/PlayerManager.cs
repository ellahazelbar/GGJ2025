using TNRD.Autohook;
using UnityEngine;
using Utils;

namespace Player
{
	public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
	{
		[field: SerializeField, AutoHook] public PlayerInstrumentPointer InstrumentPointer { get; private set; }
		[field: SerializeField, AutoHook] public PlayerInstrumentActivator InstrumentActivator { get; private set; }
		public InputSystemActions Input { get; private set; }

		protected override void Awake()
		{
			base.Awake();
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