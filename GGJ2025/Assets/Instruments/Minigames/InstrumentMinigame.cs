using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Instruments
{
    public class InstrumentMinigame : MonoBehaviour
    {
        public RectTransform TimelineParent;
        public InstrumentMinigameTimeline Timeline;
        public AnimationCurve CanvasScale;
        public float OpenTime;

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
            TimelineParent.gameObject.SetActive(true);
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

            StartCoroutine(ScaleCanvas());

            IEnumerator ScaleCanvas()
            {
                yield return new Utils.DoForSeconds<float, RectTransform>(OpenTime,
                    (float StartTime, RectTransform Transform) =>
                    {
                        float x = CanvasScale.Evaluate((Time.time - StartTime) / OpenTime);
                        Transform.localScale = new Vector3(x, x, x);
                    }
                    , Time.time, TimelineParent
                );
                TimelineParent.localScale = Vector3.one;
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
