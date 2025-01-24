using TNRD.Autohook;
using UnityEngine;
using Spine;
using Spine.Unity;

namespace Player
{
	public class AnimatorMediator : MonoBehaviour
	{
		[SerializeField, SpineAnimation] private string _runAnimation;
		[SerializeField, SpineAnimation] private string _idleAnimation;
		[SerializeField, AutoHook] private SkeletonAnimation _animator;
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private Rigidbody2D _rb;
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private PlayerInstrumentActivator _instrument;

		private Vector3 _defaultSize = Vector3.one;
		private Spine.AnimationState _state;

		private void Awake()
		{
			_defaultSize = transform.localScale;
		}

		private void Start()
		{
			_state = _animator.state;
		}

		private void FixedUpdate()
		{
			if (_rb.linearVelocityX > 0f)
				transform.localScale = new (Mathf.Sign(_rb.linearVelocityX) * _defaultSize.x, _defaultSize.y, _defaultSize.z);
			if (_rb.linearVelocity.sqrMagnitude > 1f)
				SetAnimation(0, _runAnimation, true);
			else
				SetAnimation(0, _idleAnimation, true);
			_animator.ApplyAnimation();
		}

		private void SetAnimation(int trackIndex, string animation, bool looping)
		{
			if (_animator.AnimationName == animation)
				return;
			if (animation == _idleAnimation)
				_state.SetEmptyAnimation(trackIndex, 0.25f);
			else
				_animator.AnimationState.SetAnimation(trackIndex, animation, looping);
		}
	}
}