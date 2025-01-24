using Utils;

namespace Player
{
	public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
	{
		public InputSystemActions Input { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			Input = new();
		}

		private void OnEnable()
		{
			Input.Enable();
		}

		private void OnDisable()
		{
			Input.Disable();
		}

		private void OnDestroy()
		{
			Input.Disable();
			Input.Dispose();
		}
	}
}