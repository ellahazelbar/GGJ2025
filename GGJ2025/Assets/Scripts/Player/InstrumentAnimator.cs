using Instruments;
using Spine.Unity;
using System;
using System.Collections.Generic;
using TNRD.Autohook;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
	public class InstrumentAnimator : MonoBehaviour
	{
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private PlayerCharacter _character;
		[SerializeField, AutoHook] private SkeletonAnimation _animator;
		private int _animationIndex;

		public IReadOnlyList<string> PlayAnimations { get; set; }
		public InstrumentMinigame InstrumentMinigame { get; set; }

		private void OnEnable()
		{
			_animationIndex = 0;
			_character.Input.House.Interact.started += OnNotePlayed;
		}

		private void OnDisable()
		{
			_character.Input.House.Interact.started -= OnNotePlayed;
		}

		public void PlaySetupAnimation(string startAnimation)
		{
			_animator.state.SetAnimation(1, startAnimation, false);
		}

		public void StopInstrumentAnimation()
		{
			_animator.state.SetEmptyAnimation(1, 0.3f);
		}

		private void OnNotePlayed(InputAction.CallbackContext context)
		{
			_animator.state.SetAnimation(1, PlayAnimations[_animationIndex % PlayAnimations.Count], false);
			_animationIndex++;
		}
	}
}