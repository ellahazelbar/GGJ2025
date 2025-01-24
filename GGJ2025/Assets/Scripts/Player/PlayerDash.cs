using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using TNRD.Autohook;
using UnityEngine.Events;

namespace Player
{
	public class PlayerDash : MonoBehaviour
	{
		[SerializeField, AutoHook] private PlayerCharacter _character;
		[SerializeField, AutoHook] private PlayerMove _playerMove;
		[SerializeField] private float _speedMult = 2f;
		[SerializeField] private float _duration = 0.25f;
		[SerializeField] private float _cooldown = 1f;
		[SerializeField] private AnimationCurve _easeType;
		private float _lastDashTime;
		private float _defaultSpeed;

		public event UnityAction Dashed;

		private bool CanDash => Time.time >= _lastDashTime + _cooldown;

		private void Awake()
		{
			_defaultSpeed = _playerMove.Speed;
			_lastDashTime = Mathf.NegativeInfinity;
		}

		private void OnEnable()
		{
			_character.Input.House.Dash.performed += OnDash;
		}

		private void OnDisable()
		{
			_character.Input.House.Move.performed -= OnDash;
		}

		private void OnDash(InputAction.CallbackContext context)
		{
			if (!CanDash)
				return;
			_lastDashTime = Time.time;
			DOTween.To(startValue: _defaultSpeed, endValue: _defaultSpeed * _speedMult, setter: speed => _playerMove.Speed = speed, duration: _duration).SetEase(_easeType).OnComplete(OnDashComplete);
			Dashed?.Invoke();
		}

		private void OnDashComplete()
		{
			_playerMove.Speed = _defaultSpeed;
		}
	}
}