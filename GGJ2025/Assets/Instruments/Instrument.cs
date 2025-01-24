using UnityEngine;

namespace Instruments
{
    public class Instrument : MonoBehaviour
    {
        public InstrumentMinigame Minigame;
        public InstrumentType Type;

        public bool IsMinigameActive { get { return Minigame.Timeline.gameObject.activeInHierarchy; } }

        public void Activate()
        {
            Minigame.Activate(Type);
        }

        public void Deactivate()
        {

        }
    }
}
