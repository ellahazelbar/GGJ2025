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
		public List<Behaviour> SinglePlayer;
		public List<Behaviour> LocalMultiplayer;

		private void OnEnable()
		{
			if (Gamepad.all.Count == 1)
			{
				foreach (Behaviour b in SinglePlayer)
					b.enabled = true;
			}
			else
			{
				foreach (Behaviour b in LocalMultiplayer)
					b.enabled = true;
			}
		}
	}
}