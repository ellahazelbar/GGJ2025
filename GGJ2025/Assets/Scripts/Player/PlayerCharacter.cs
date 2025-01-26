using NUnit.Framework;
using System.Collections.Generic;
using TNRD.Autohook;
using UnityEngine;

namespace Player
{
	public class PlayerCharacter : MonoBehaviour
	{
		[SerializeField, Tooltip("Behaviours to enable/disable when the player is enabled disbaled.")] private List<Behaviour> _characterBehaviours;
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
			_characterBehaviours.ForEach(b => b.enabled = true);
		}

		private void OnDisable()
		{
			_characterBehaviours.ForEach(b => b.enabled = false);
		}
	}
}