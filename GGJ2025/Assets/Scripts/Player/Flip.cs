using UnityEngine;
using TNRD.Autohook;
using DG.Tweening;

namespace Player
{
	public class Flip : MonoBehaviour
	{
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private Rigidbody2D _rb;
		[SerializeField] private float _animationDuration;

		private Vector3 _defaultSize = Vector3.one;

		private float _previousDirection = 1f;

		private void Awake()
		{
			_defaultSize = transform.localScale;
		}

		private void FixedUpdate()
		{
			if (Mathf.Abs(_rb.linearVelocity.sqrMagnitude) <= 0f)
				return;
			var newDirection = Mathf.Sign(_rb.linearVelocityX);
			if (newDirection * _previousDirection < 0f)
				AnimateFlip(newDirection);
			_previousDirection = newDirection;
		}

		private void AnimateFlip(float sign)
		{
			DOTween.Kill(this);
			transform.DOScaleX(_defaultSize.x * sign, _animationDuration).SetEase(Ease.InOutSine).SetTarget(this);
		}
	}
}