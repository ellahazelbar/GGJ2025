using UnityEngine;

namespace Instruments
{
    public class InstrumentTimeline : Timeline
    {
        private void Start()
        {
            SongManager.Instance.RegisterInstrumentTimeline(Type, this);    
        }
    }
}
