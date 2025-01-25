using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Instruments
{
    public class Timeline : MonoBehaviour
    {
        public float UnitsPerSecond;
        public GameObject NodePrefab;
        public Image Hint;
        public Sprite Base, Input;
        public Color NoteColor = Color.white;

        public ParticleSystem OnHit, Passive;

        private Utils.Timer<Image, Sprite> ResetSpriteTimer;


        private List<TimelineNote> notesAlive;

        private void Awake()
        {
            notesAlive = new List<TimelineNote>();
        }

        public void CreateNote(float PlayTime, float Duration)
        {
            TimelineNote note = Instantiate(NodePrefab, transform).GetComponent<TimelineNote>();
            note.Init(this, PlayTime, UnitsPerSecond);
            notesAlive.Add(note);
            ResetSpriteTimer = new Utils.Timer<Image, Sprite>(0.2f, (Im, Sp) => { Im.sprite = Sp; }, Hint, Base);
            ResetSpriteTimer.Abort();
        }

        public void Forget(TimelineNote Note)
        {
            notesAlive.Remove(Note);
        }

        public void NotePlayed()
        {
            Hint.sprite = Input;
            ResetSpriteTimer.ResetAndResume();
        }

        public void NoteHit(Song.Interval.Note Note)
        {
            foreach (TimelineNote n in notesAlive)
            {
                if (Mathf.Approximately(n.PlayTime, Note.PlayTime))
                {
                    notesAlive.Remove(n);
                    Destroy(n.gameObject);
                    OnHit.Play(true);
                    break;
                }
            }
        }

        public void Unfade()
        {
            Passive.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        public void Fade()
        {
            Passive.Play(true);
            foreach (TimelineNote n in notesAlive)
            {
                n.StartCoroutine(n.Fade());
            }
            StartCoroutine(FadeHint());
        }

        private IEnumerator FadeHint()
        {
            yield return new Utils.DoForSeconds<float, Color, Image>(1,
                (float StartTime, Color StartColor, Image Image) =>
                {
                    Image.color = new Color(StartColor.r, StartColor.g, StartColor.b, 1 + StartTime - Time.time);
                },
                Time.time, Hint.color, Hint
            );
            Hint.color = Color.clear;
        }
    }
}
