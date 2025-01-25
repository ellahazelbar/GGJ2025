using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

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

        public float NextFadeTime { get; private set; }

        private bool waitingForInterval;
        private float nextPlayTime;
        public float NextShutoffTime { get; private set; }

        [Tooltip("Called when note is played sucessfully.")] public event UnityAction NotePlayedEvent;


        public void NotifyIncomingInterval(float PlayTime, float Duration)
        {
            nextPlayTime = PlayTime;
            NextShutoffTime = PlayTime + Duration;
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

        public void NotePlayed()
        {
            if (null == currentPlayedInterval)
                return;
            Song.Interval.Note hit = null;
            Song.Interval.Note nextNote = null;
            foreach (Song.Interval.Note note in currentPlayedInterval.Notes)
            {
                nextNote = note;
                if (null != hit)
                    break;
                if (Mathf.Abs(Time.time - SongManager.Instance.SongTimeToTime(note.PlayTime)) < AccuracyRange)
                {
                    hit = note;
                    nextNote = null;
                }
            }
            if (null != hit)
            {
                Timeline.NoteHit(hit);
                if (null == nextNote) 
                {
                    NextFadeTime = Mathf.Infinity;
                }
                else
                {
                    NextFadeTime = SongManager.Instance.SongTimeToTime(nextNote.PlayTime) + AccuracyRange / 2;
                }
				NotePlayedEvent?.Invoke();

			}
            else
            {
                //play miss sound byte with random pitch
            }
        }
    }
}
