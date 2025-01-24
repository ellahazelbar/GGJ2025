using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Extensions;

namespace Player
{
	/// <summary>
	/// Abstract decorator for a single player.
	/// Can control multiple characters.
	/// </summary>
	public class PlayerManager : MonoBehaviour
	{
		[SerializeField] private List<PlayerCharacter> _characters;
		public InputSystemActions Input { get; private set; }
		private int _activeCharacterIndex;
		public PlayerCharacter ActiveCharacter => _characters[_activeCharacterIndex];

		public IReadOnlyList<PlayerCharacter> Characters => _characters;

		private void OnValidate()
		{
			_characters = GetComponentsInChildren<PlayerCharacter>().ToList();
		}

		private void Awake()
		{
			Input = new();
			SetCharacterEnabledStatus();
		}

		private void OnEnable()
		{
			Input.Enable();
			Input.House.ChangeActiveCharacter.started += OnActiveCharacterChanged;
		}

		private void OnDisable()
		{
			Input.House.ChangeActiveCharacter.started -= OnActiveCharacterChanged;
			Input.Disable();
		}

		private void OnDestroy()
		{
			Input.Disable();
			Input.Dispose();
		}

		private void OnActiveCharacterChanged(InputAction.CallbackContext context)
		{
			_activeCharacterIndex = (_activeCharacterIndex+1) % _characters.Count;
			SetCharacterEnabledStatus();
		}

		private void SetCharacterEnabledStatus()
		{
			foreach (var character in _characters.Except(ActiveCharacter))
				character.enabled = false;
			ActiveCharacter.enabled = true;
		}
	}
}