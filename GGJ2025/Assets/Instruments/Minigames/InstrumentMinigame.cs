using UnityEngine;
using System.Collections.Generic;

namespace Instruments
{
    public class InstrumentMinigame : MonoBehaviour
    {
        public InstrumentMinigameTimeline Timeline;

        private bool recording;
        private Song.Interval currentPlayedInterval;
        
        private class PlayedNote
        {
            public float Offset; //from section start
            public AudioClip Clip;
        }

        private List<PlayedNote> PlayedNotes;

        private void Awake()
        {
            PlayedNotes = new();
        }

        public void Activate(InstrumentType Type)
        {
            currentPlayedInterval = SongManager.Instance.GetCurrentInterval(Type);
            if (null != currentPlayedInterval)
            {
                recording = true;
                //intercept activator player input
                foreach (Song.Interval.Note note in currentPlayedInterval.Notes)
                {
                    Timeline.CreateNote(SongManager.Instance.SongTimeToTime(note.PlayTime), note.Duration);
                }
            }
        }

        private void OnPlayNote()
        {
            foreach (Song.Interval.Note note in currentPlayedInterval.Notes)
            {
                Timeline.CreateNote(note.PlayTime, note.Duration);
            }
        }
    }
}
