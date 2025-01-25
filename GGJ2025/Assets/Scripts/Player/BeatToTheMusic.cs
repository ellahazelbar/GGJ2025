using Spine.Unity;
using System;
using TNRD.Autohook;
using UnityEngine;

namespace Player
{
	public class BeatToTheMusic : MonoBehaviour
	{
		[SerializeField, AutoHook] private SkeletonAnimation _skeleton;
		[SerializeField, AutoHook] private MovementAnimation _movement;

		private void OnEnable()
		{
			BaseLineBeat.Beat += OnBeat;
		}

		private void OnDisable()
		{
			BaseLineBeat.Beat -= OnBeat;
		}

		private void OnBeat()
		{
			if (_skeleton.AnimationName == _movement.IdleAnimation)
				_movement.PlayIdleAnimation();
		}
	}
}