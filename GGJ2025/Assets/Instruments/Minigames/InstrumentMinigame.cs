using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Instruments
{
    public class InstrumentMinigame : MonoBehaviour
    {
        public Timeline Timeline;
        public InstrumentType Type;

        public UnityEngine.UI.Image ToasterImage;
        public float ToasterTime;

        public float AccuracyRange;

        private bool recording;
        private Song.Interval currentPlayedInterval;

        private float nextFadeTime;

        private bool waitingForInterval;
        private float nextPlayTime, nextShutoffTime;


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
                    float toasterFill = (ToasterTime - toWait) / ToasterTime;
                    ToasterImage.fillAmount = toasterFill;
                }
            }
            else ToasterImage.fillAmount = 0;
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
            bool hit = false;
            Song.Interval.Note nextNote = null;
            foreach (Song.Interval.Note note in currentPlayedInterval.Notes)
            {
                nextNote = note;
                if (hit)
                    break;
                if (Mathf.Abs(Time.time - SongManager.Instance.SongTimeToTime(note.PlayTime)) < AccuracyRange)
                {
                    hit = true;
                    nextNote = null;
                }
            }
            if (hit)
            {
                if (null == nextNote) 
                {
                    nextFadeTime = Mathf.Infinity;
                }
                else
                {
                    nextFadeTime = SongManager.Instance.SongTimeToTime(nextNote.PlayTime) + AccuracyRange / 2;
                }
            }
            else
            {
                //play miss sound byte with random pitch
            }
        }
    }
}
