using TNRD.Autohook;
using UnityEngine;
using Spine.Unity;

namespace Player
{
	public class MovementAnimation : MonoBehaviour
	{
		[SerializeField, SpineAnimation] private string _runAnimation;
		[SerializeField, SpineAnimation] private string _idleAnimation;
		[SerializeField, AutoHook] private SkeletonAnimation _animator;
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private Rigidbody2D _rb;
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private PlayerInstrumentActivator _instrument;

		public string IdleAnimation => _idleAnimation;

		private Spine.AnimationState _state;

		private void Start()
		{
			_state = _animator.state;
		}

		private void OnDisable()
		{
			PlayIdleAnimation();
		}

		private void FixedUpdate()
		{
			if (_rb.linearVelocity.sqrMagnitude > 0.1f)
				SetAnimation(0, _runAnimation, true);
			else
				SetAnimation(0, _idleAnimation, false);
		}

		private void SetAnimation(int trackIndex, string animation, bool looping)
		{
			if (_animator.AnimationName == animation)
				return;
			ForcePlayAnimation(trackIndex, animation, looping);
		}


		private void ForcePlayAnimation(int trackIndex, string animation, bool looping)
		{
			_state.SetAnimation(trackIndex, animation, looping);
		}

		public void PlayIdleAnimation() => ForcePlayAnimation(0, _idleAnimation, false);
	}
}