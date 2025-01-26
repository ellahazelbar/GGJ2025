using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Extensions;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Events;

namespace Player
{
	/// <summary>
	/// Abstract decorator for a single player.
	/// Can control multiple characters.
	/// </summary>
	public class PlayerManager : MonoBehaviour
	{
		[SerializeField] private List<PlayerCharacter> _characters;
		[SerializeField] private List<UnityEngine.GameObject> _toActivate;
		[field: SerializeField] public Color Color { get; private set; } = Color.white;
		[SerializeField] private int _playerIndex;
		public InputSystemActions Input { get; private set; }
		private int _activeCharacterIndex;
		public PlayerCharacter ActiveCharacter => _characters[_activeCharacterIndex];

		public event UnityAction<PlayerCharacter> ActiveCharacterChanged;

		public IReadOnlyList<PlayerCharacter> Characters => _characters;

		private void Awake()
		{
			Input = new();
		}

		private void OnEnable()
		{
			Input.Enable();
			Input.House.ChangeActiveCharacter.started += OnActiveCharacterChanged;
			_characters.ForEach(c => c.Owner = this);
			_toActivate.ForEach(go => go.SetActive(true));
		}

		private void OnDisable()
		{
			Input.House.ChangeActiveCharacter.started -= OnActiveCharacterChanged;
			Input.Disable();
			_toActivate.ForEach(go => go.SetActive(false));
		}

		private void OnDestroy()
		{
			Input.Disable();
			Input.Dispose();
		}

		private void Start()
		{
			Input.devices = new(new InputDevice[] { Gamepad.all[_playerIndex] });
			UpdateCharacterEnabledStatus();
		}

		private void OnActiveCharacterChanged(InputAction.CallbackContext context)
		{
			_activeCharacterIndex = (_activeCharacterIndex+1) % _characters.Count;
			UpdateCharacterEnabledStatus();
			ActiveCharacterChanged?.Invoke(ActiveCharacter);
		}

		private void UpdateCharacterEnabledStatus()
		{
			foreach (var character in _characters)
				character.enabled = character == ActiveCharacter;
		}
	}
}