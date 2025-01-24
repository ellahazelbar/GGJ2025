using UnityEngine;
using UnityEngine.Events;
using Extensions;
using Instruments;

namespace Player
{
	public class PlayerInstrumentPointer : MonoBehaviour
	{
		public Instrument Current { get; private set; }
		[SerializeField] private LayerMask _instrumentLayer;

		public event UnityAction<Instrument> CurrentPointingInstrumentChanged;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other != null && _instrumentLayer.Contains(other.gameObject.layer) && other.TryGetComponent<Instrument>(out var instrument) && instrument != Current)
			{
				Current = instrument;
				CurrentPointingInstrumentChanged?.Invoke(Current);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (Current == null)
				return;
			Current = null;
			CurrentPointingInstrumentChanged?.Invoke(Current);
		}
	}
}