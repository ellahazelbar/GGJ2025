using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
	public class HaltUntillMultipleGamepads : SingletonMonoBehaviour<HaltUntillMultipleGamepads>
	{
		[SerializeField] private List<Behaviour> _enableWhenGamepadsConnected;
		[SerializeField] private int _requiredControllers = 2;

		private void OnEnable()
		{
			StartCoroutine(WaitFor2GamePads());
		}

		private IEnumerator WaitFor2GamePads()
		{
			SetEnabled(false);
			yield return new WaitForControllerCount(_requiredControllers);
			SetEnabled(true);

			void SetEnabled(bool enabled)
			{
				_enableWhenGamepadsConnected.ForEach(b => b.enabled = enabled);
			}
		}

		private class WaitForControllerCount : CustomYieldInstruction
		{
			private readonly int _controllerCount;

			public WaitForControllerCount(int controllerCount = 1)
			{
				_controllerCount = controllerCount;
			}

			public override bool keepWaiting => Gamepad.all.Count < _controllerCount;
		}
	}
}