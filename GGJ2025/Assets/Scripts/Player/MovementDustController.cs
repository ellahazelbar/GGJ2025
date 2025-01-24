using UnityEngine;
using TNRD.Autohook;

namespace Player
{
	public class MovementDustController : MonoBehaviour
	{
		[SerializeField, AutoHook] private ParticleSystem _ps;
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private Rigidbody2D _rb;

		private void Update()
		{
			var isMoving = _rb.linearVelocity.sqrMagnitude > 0.1f;
            if (isMoving && !_ps.isPlaying)
				_ps.Play();
			else if (!isMoving && _ps.isPlaying)
				_ps.Stop();
		}
	}
}