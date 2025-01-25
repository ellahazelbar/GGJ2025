using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player.InstrumentAnimationDefinition;

namespace Player
{
	[CreateAssetMenu(menuName = "ScriptableObject/InstrumentAnimationDefinition", fileName = "InstrumentAnimationDefinition", order = 0)]
	public class InstrumentAnimationDefinition : ScriptableObject, IReadOnlyList<AnimationDefinition>
	{
		[SerializeField] private List<AnimationDefinition> _definitions;

		public AnimationDefinition this[int index] => ((IReadOnlyList<AnimationDefinition>)_definitions)[index];

		public int Count => ((IReadOnlyCollection<AnimationDefinition>)_definitions).Count;

		public IEnumerator<AnimationDefinition> GetEnumerator() => ((IEnumerable<AnimationDefinition>)_definitions).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_definitions).GetEnumerator();

		[Serializable]
		public struct AnimationDefinition : IReadOnlyList<string>
		{
			public string SetupAnimation;
			[SerializeField] private List<string> _playAnimations;

			public string this[int index] => ((IReadOnlyList<string>)_playAnimations)[index];

			public int Count => ((IReadOnlyCollection<string>)_playAnimations).Count;

			public IEnumerator<string> GetEnumerator()
			{
				return ((IEnumerable<string>)_playAnimations).GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)_playAnimations).GetEnumerator();
			}
		}
	}
}