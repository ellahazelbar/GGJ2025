using System;
using UnityEngine;
using TNRD.Autohook;
using Instruments;
using DG.Tweening;
using MoreMountains.Tools;

namespace Player
{
	public class PlayAnimationOnInteract : MonoBehaviour
	{
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private PlayerInstrumentActivator _instrumentActivator;
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private Rigidbody2D _rb;
		[SerializeField, AutoHook(AutoHookSearchArea.Children)] private Flip _flip;
		[SerializeField] private float _animationDuration = 0.4f;

		private void OnEnable() => _instrumentActivator.InstrumentActivated += OnInstrumentActivated;
		
		private void OnDisable() => _instrumentActivator.InstrumentActivated -= OnInstrumentActivated;

		private void OnInstrumentActivated(Instrument instrument)
		{
			_rb.DOMove(instrument.InteractionSpot.position, _animationDuration).SetEase(Ease.OutSine).OnComplete(() => _flip.TryAnimateFlip(instrument.transform.position.x - transform.position.x));
		}
	}
}