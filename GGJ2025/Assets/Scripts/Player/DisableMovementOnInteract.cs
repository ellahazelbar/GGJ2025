using System;
using System.Collections.Generic;
using UnityEngine;
using Instruments;
using TNRD.Autohook;

namespace Player
{
	public class DisableMovementOnInteract : MonoBehaviour
	{
		[SerializeField, AutoHook] private PlayerInstrumentActivator _instrumentActivator;
		[SerializeField] private List<Behaviour> _toDisable;

		private void OnEnable() => _instrumentActivator.InstrumentActivated += OnInstrumentActivated;
		
		private void OnDisable() => _instrumentActivator.InstrumentActivated -= OnInstrumentActivated;

		private void OnInstrumentActivated(Instrument instrument)
		{
			_toDisable.ForEach(b => b.enabled = false);
		}
	}
}