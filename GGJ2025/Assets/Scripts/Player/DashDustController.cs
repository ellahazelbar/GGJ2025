using UnityEngine;
using TNRD.Autohook;

namespace Player
{
	public class DashDustController : MonoBehaviour
	{
		[SerializeField, AutoHook] private ParticleSystem _ps;
		[SerializeField, AutoHook(AutoHookSearchArea.Parent)] private PlayerDash _dash;

		private void OnEnable() => _dash.Dashed += OnDash;

		private void OnDisable() => _dash.Dashed -= OnDash;

		private void OnDash()
		{
			_ps.transform.localScale = new(Mathf.Sign(_dash.transform.localPosition.x), 1f, 1f);
			_ps.Play();
		}
	}
}