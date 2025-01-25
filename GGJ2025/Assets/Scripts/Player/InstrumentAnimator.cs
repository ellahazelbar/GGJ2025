using Spine.Unity;
using System.Collections.Generic;
using TNRD.Autohook;
using UnityEngine;

namespace Player
{
	public class InstrumentAnimator : MonoBehaviour
	{
		[SerializeField, AutoHook] private SkeletonAnimation _animator;
		private int _animationIndex;

		public string StartAnimation { get; set; }
		public IReadOnlyList<string> PlayAnimations { get; set; }

		private void OnEnable()
		{
			_animationIndex = 0;
			_animator.state.SetAnimation(1, StartAnimation, false);
			// TODO Register for on instrument play event
		}

		private void OnDisable()
		{
			// TODO Unregister for on instrument play event
		}

		private void OnPlay()
		{
			_animator.state.SetAnimation(1, PlayAnimations[_animationIndex % PlayAnimations.Count], false);
			_animationIndex++;
		}
	}
}