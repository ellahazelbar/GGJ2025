using UnityEngine;
using UnityEngine.Events;

public class BaseLineBeat : MonoBehaviour
{
	[SerializeField] private float _delay;
	[SerializeField] private float _beatTempo = 0.375f;

	public static event UnityAction Beat;

	private void OnEnable()
	{
		InvokeRepeating(nameof(InvokeBeat), _delay, _beatTempo);
	}

	private void OnDisable()
	{
		CancelInvoke(nameof(InvokeBeat));
	}

	private void InvokeBeat()
	{
		Beat?.Invoke();
	}
}