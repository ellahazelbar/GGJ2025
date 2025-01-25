using UnityEngine;
using TNRD.Autohook;
using Spine.Unity;
using Spine;
using System;

namespace Player
{
	public class MovementDustController : MonoBehaviour
	{
		[SerializeField, AutoHook] private ParticleSystem _ps;
		[SerializeField] private SkeletonAnimation _animator;
		private EventData _stepEvent;

		private void OnEnable()
		{
			_stepEvent = _animator.Skeleton.Data.FindEvent("Run Event");
			_animator.state.Event += HandleEvents;
		}

		private void OnDisable()
		{
			_animator.state.Event -= HandleEvents;
		}

		private void HandleEvents(TrackEntry trackEntry, Spine.Event e)
		{
			if (e.Data != _stepEvent)
				return;
			_ps.Play();
		}
	}
}