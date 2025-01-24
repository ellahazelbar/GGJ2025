using UnityEngine;

namespace Instruments
{
    public class Instrument : MonoBehaviour
    {
        public InstrumentMinigame Minigame;
        public InstrumentType Type;

        public void Activate()
        {
            Minigame.Activate();
        }

        public void Deactivate()
        {

        }
    }
}
