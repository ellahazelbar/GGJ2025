using UnityEngine;

namespace Instruments
{
    public class InstrumentTimeline : Timeline
    {
        public InstrumentType Type;

        private void Start()
        {
            SongManager.Instance.RegisterInstrumentTimeline(Type, this);    
        }
    }
}
