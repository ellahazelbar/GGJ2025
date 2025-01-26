using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Instruments
{
    public class Timeline : MonoBehaviour
    {
        public float UnitsPerSecond;
        public GameObject NotePrefab;
        public GameObject NoteContainer;
        public Image InputHint;
        public Sprite Base, Input;
        public Color NoteColor = Color.white;
        public bool isVisible { get; private set; }

        public ParticleSystem OnHit, Passive;

        private Utils.Timer<Image, Sprite> ResetSpriteTimer;


        private List<TimelineNote> notesAlive;

        private bool isInstrumentManned;
        
        private IEnumerator fadeCoroutine;

        private void Awake()
        {
            notesAlive = new List<TimelineNote>();
            isVisible = false;
        }

        private void Start()
        {
            ParticleSystem.MainModule main = OnHit.main;
            main.startColor = NoteColor;
            //OnHit.main = main;
            main = Passive.main;
            main.startColor = NoteColor;
        }

        public void OnInstrumentEquipped()
        {
            isInstrumentManned = true;
            isVisible = true;
            UnFadeVisuals();
        }
        
        public void OnInstrumentUnEquipped()
        {
            isVisible = false;
            isInstrumentManned = false;
            if (Passive.isPlaying)
            {
                Passive.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            else
                FadeVisuals();
        }

        public void CreateNote(float PlayTime, float Duration)
        {
            TimelineNote note = Instantiate(NotePrefab, transform).GetComponent<TimelineNote>();
            note.Init(this, PlayTime, UnitsPerSecond);
            notesAlive.Add(note);
            ResetSpriteTimer = new Utils.Timer<Image, Sprite>(0.2f, (Im, Sp) => { Im.sprite = Sp; }, InputHint, Base);
            ResetSpriteTimer.Abort();
        }

        public void Forget(TimelineNote Note)
        {
            notesAlive.Remove(Note);
        }

        public void NotePlayed()
        {
            InputHint.sprite = Input;
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

        [ContextMenu("OnAutomaticPlayStarted")]
        public void OnAutomaticPlayStarted()
        {
            FadeVisuals();
            Passive.Play(true);
        }

        private void FadeVisuals()
        {
            foreach (TimelineNote n in notesAlive)
            {
                n.FadeNote();
            }

            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = FadeInputHint();
            StartCoroutine(fadeCoroutine);
            
            IEnumerator FadeInputHint()
            {
                while (InputHint.color.a > 0)
                {
                    InputHint.color = Color.Lerp(Color.clear, Color.white, Mathf.PingPong(Time.time, 1));
                    yield return null;
                }
            }
        }
        
        public void UnFadeVisuals()
        {
            foreach (TimelineNote n in notesAlive)
            {
                n.UnFadeNote();
            }

            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = UnFadeInputHint();
            StartCoroutine(fadeCoroutine);
            
            IEnumerator UnFadeInputHint()
            {
                while (InputHint.color.a < 1)
                {
                    Color ogColor = InputHint.color;
                    InputHint.color = Color.Lerp(ogColor, Color.clear, Mathf.PingPong(Time.time, 1));
                    yield return null;
                }
                /*yield return new Utils.DoForSeconds<float, Color, Image>(1,
                    (float StartTime, Color StartColor, Image Image) =>
                    {
                        Image.color = new Color(StartColor.r, StartColor.g, StartColor.b, 1 + StartTime - Time.time);
                    },
                    Time.time, InputHint.color, InputHint
                );*/
                //InputHint.color = Color.clear;
            }
        }
    }
}
