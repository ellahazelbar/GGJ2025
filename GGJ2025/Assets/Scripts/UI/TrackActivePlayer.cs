using DG.Tweening;
using UnityEngine;
using TNRD.Autohook;
using Player;

namespace UI
{
	public class TrackActivePlayer : MonoBehaviour
	{
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private PlayerManager _player;
		[SerializeField, AutoHook(AutoHookSearchArea.Children)] private SpriteRenderer _sprite;
		[SerializeField] private float _duration = 0.25f;
		[SerializeField] private Ease _ease = Ease.InOutSine;

		private void Start()
		{
			OnCharacterChanged(_player.ActiveCharacter);
			_sprite.color = _player.Color;
		}

		private void OnEnable()
		{
			_player.ActiveCharacterChanged += OnCharacterChanged;
		}

		private void OnDisable()
		{
			_player.ActiveCharacterChanged -= OnCharacterChanged;
		}

		private void OnCharacterChanged(PlayerCharacter character)
		{
			transform.parent = character.transform;
			DOTween.Kill(this);
			transform.DOLocalMove(Vector3.zero, _duration).SetEase(_ease).SetTarget(this);
		}
	}
}