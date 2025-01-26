using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class CharacterStateController : MonoBehaviour
	{
		[SerializeField] private CharState _state = CharState.FreeMovement;
		[SerializeField] private List<ConditionalBehaviour> _behaviours;

		public CharState State {
			get => enabled ? _state : CharState.Inactive;
			set
			{
				if (_state == value)
					return;
				_state = value;
				SetBehavioursEnabled(_state);
			}
		}

		private void OnEnable() => SetBehavioursEnabled(_state);

		private void OnDisable() => SetBehavioursEnabled(CharState.Inactive);

		private void SetBehavioursEnabled(CharState state) => _behaviours.ForEach(eb => eb.behaviour.enabled = (eb.condition & state) != 0);

		[Serializable]
		private struct ConditionalBehaviour
		{
			public Behaviour behaviour;
			public CharState condition;
		}

		[Flags]
		public enum CharState : byte
		{
			Inactive = 0,
			FreeMovement = 1,
			Playing = 2
		}
	}
}