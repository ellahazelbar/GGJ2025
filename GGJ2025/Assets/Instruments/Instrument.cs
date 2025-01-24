using UnityEngine;

namespace Instruments
{
    public class Instrument : MonoBehaviour
    {
        public InstrumentMinigame Minigame;
        public InstrumentType Type;

        private bool active;

        private void Start()
        {
            SongManager.Instance.RegisterInstrument(Type, this);
        }

        public void Activate()
        {
            if (active) return;
            active = true;
            Minigame.Activate(Type);
        }

        public void Deactivate()
        {
            active = false;
        }
    }
}
