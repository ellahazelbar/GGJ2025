using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace Instruments
{
    public class InstrumentMinigame : MonoBehaviour
    {
        public const int NOTES_REQUIRED_FOR_AUTOPLAY = 8;
        public Timeline Timeline;

        public UnityEngine.UI.Image ToasterImage;
        public float ToasterTime;

        public float AccuracyRange;

        private Song.Interval currentPlayedInterval;

        public float NextFadeTime { get; private set; }

        private bool waitingForInterval;
        private float nextPlayTime;
        public float NextShutoffTime { get; private set; }

        public int Combo { get; private set; }
        [Tooltip("Called when note is played sucessfully.")] public event UnityAction NotePlayedEvent;

        private void Awake()
        {
            NextShutoffTime = nextPlayTime = Mathf.Infinity;
        }

        public void NotifyIncomingInterval(Song.Interval Interval)
        {
            //Timeline.Unfade();
            nextPlayTime = SongManager.Instance.SongTimeToTime(Interval.PlayTime);
            NextShutoffTime = nextPlayTime + Interval.Duration;
            waitingForInterval = true;
            currentPlayedInterval = Interval;
            foreach (Song.Interval.Note note in currentPlayedInterval.Notes)
            {
                Timeline.CreateNote(SongManager.Instance.SongTimeToTime(note.PlayTime), note.Duration);
            }
            new Utils.Timer<Song.Interval>(NextShutoffTime - Time.time, (Song.Interval inter) => { if (currentPlayedInterval == inter) currentPlayedInterval = null; }, Interval);
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

        public void Activate()
        {
            Combo = 0;
            Timeline.OnInstrumentEquipped();
        }
        
        public void Deactivate()
        {
            Combo = 0;
            Timeline.OnInstrumentUnEquipped();
        }

        public void NotePlayed()
        {
            if (null == currentPlayedInterval)
                return;
            Timeline.NotePlayed();
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
                if(GrandmaScore.Instance != null)
                    GrandmaScore.Instance.OnAddScore(1);
                Timeline.NoteHit(hit);
                ++Combo;
                if (NOTES_REQUIRED_FOR_AUTOPLAY == Combo)
                {
                    Timeline.OnAutomaticPlayStarted();
                    LevelShakeManager.Instance.PlayWorldShake();
                    NextFadeTime = Mathf.Infinity;
                }
                else if (Combo < NOTES_REQUIRED_FOR_AUTOPLAY)
                { 
                    if (null == nextNote) 
                    {
                        NextFadeTime = Mathf.Infinity;
                    }
                    else
                    {
                        NextFadeTime = SongManager.Instance.SongTimeToTime(nextNote.PlayTime) + AccuracyRange / 2;
                    }
                }
				NotePlayedEvent?.Invoke();

			}
            else
            {
                if(GrandmaScore.Instance != null)
                    GrandmaScore.Instance.OnAddScore(-1);
            }
        }
    }
}
