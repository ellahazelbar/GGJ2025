using UnityEngine;
using TNRD.Autohook;
using DG.Tweening;
using Instruments;

namespace Player
{
	public class PlayAnimationOnInteract : MonoBehaviour
	{
		[SerializeField] private InstrumentAnimationDefinition _animationDefinition;
		[SerializeField] private InstrumentAnimator _instrumentAnimator;
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private CharacterStateController _stateController;
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private PlayerInstrumentActivator _instrumentActivator;
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private Rigidbody2D _rb;
		[SerializeField, AutoHook(AutoHookSearchArea.Children)] private Flip _flip;
		[SerializeField] private float _animationDuration = 0.4f;

		private void OnEnable()
		{
			_instrumentActivator.InstrumentActivated += OnInstrumentActivated;
			_instrumentActivator.InstrumentDisengaged += OnInstrumentDisengaged;
		}

		private void OnDisable()
		{
			_instrumentActivator.InstrumentActivated -= OnInstrumentActivated;
			_instrumentActivator.InstrumentDisengaged -= OnInstrumentDisengaged;
		}

		private void OnInstrumentActivated(Instrument instrument)
		{
			var sequence = DOTween.Sequence();
			sequence.Append(_rb.DOMove(instrument.InteractionPosition, _animationDuration).SetEase(Ease.OutSine));
			var flipAnimation = _flip.TryAnimateFlip(instrument.transform.position.x - instrument.InteractionPosition.x);
			if (flipAnimation != null)
				sequence.Append(flipAnimation);
			sequence.OnComplete(() => ShowStartAnimation(instrument));
		}

		private void ShowStartAnimation(Instrument instrument)
		{
			instrument.SetVisible(false);
			_instrumentAnimator.InstrumentMinigame = instrument.Minigame;
			var definition = _animationDefinition[(int)instrument.Type];
			_instrumentAnimator.PlaySetupAnimation(definition.SetupAnimation);
			_instrumentAnimator.PlayAnimations = definition;
			_stateController.State = CharacterStateController.CharState.Playing;
		}

		private void OnInstrumentDisengaged()
		{
			_instrumentAnimator.StopInstrumentAnimation();
		}
	}
}