using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Instruments
{
    public class InstrumentMinigame : MonoBehaviour
    {
        public RectTransform TimelineParent;
        public InstrumentMinigameTimeline Timeline;
        public AnimationCurve CanvasScale;
        public float OpenTime;

        public InstrumentType Type;

        public float ToasterTime;


        private bool recording;
        private Song.Interval currentPlayedInterval;

        private class PlayedNote
        {
            public float Offset; //from section start
        }

        private List<PlayedNote> PlayedNotes;

        private bool waitingForInterval;
        private float nextPlayTime, nextShutoffTime;

        private void Awake()
        {
            PlayedNotes = new();
        }

        private void Start()
        {
            SongManager.Instance.RegisterInstrumentTimeline(Type, this);
        }

        public void NotifyIncomingInterval(float PlayTime, float Duration)
        {
            nextPlayTime = PlayTime;
            nextShutoffTime = PlayTime + Duration;
            waitingForInterval = true;
        }

        private void Update()
        {
            if (waitingForInterval)
            {
                float toWait = nextPlayTime - Time.time;
                if (toWait < 0)
                {
                    waitingForInterval = false;
                }
                else
                {
                    //set toaster accordingly
                    float toasterFill = (ToasterTime - toWait) / ToasterTime;
                    Debug.Log($"Toasting {toasterFill}!", this);
                }
            }
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

            /*
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
            */
        }

        private void OnPlayNote()
        {
            foreach (Song.Interval.Note note in currentPlayedInterval.Notes)
            {
                //Timeline.CreateNote(note.PlayTime, note.Duration);
            }
        }
    }
}
