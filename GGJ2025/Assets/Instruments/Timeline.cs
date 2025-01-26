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
        public Color ActiveInputHintColor;
        public Color AutomaticPlayInputHintColor;
        public bool isVisible { get; private set; }

        public ParticleSystem OnHit, Passive;

        private Utils.Timer<Image, Sprite> ResetSpriteTimer;


        private List<TimelineNote> notesAlive;

        private bool isInstrumentManned;
        
        private IEnumerator inputHintFadeCoroutine;

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
            
            DisplayInputHint(false);
        }

        public void OnInstrumentEquipped()
        {
            isInstrumentManned = true;
            UnFadeVisuals();
        }
        
        public void OnInstrumentUnEquipped()
        {
            isInstrumentManned = false;
            if (Passive.isPlaying)
            {
                Passive.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            else
                FadeVisuals();
        }

        public void DisplayInputHint(bool shouldDisplay)
        {
            if (inputHintFadeCoroutine != null)
            {
                StopCoroutine(inputHintFadeCoroutine);
            }
            InputHint.color = shouldDisplay ? ActiveInputHintColor : Color.clear;
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
            InputHint.color = AutomaticPlayInputHintColor;
            Passive.Play(true);
        }

        private void FadeVisuals()
        {
            isVisible = false;
            foreach (TimelineNote n in notesAlive)
            {
                n.FadeNote();
            }


            
            /*if (notesAlive.Count > 0)
            {
                if (inputHintFadeCoroutine != null)
                {
                    StopCoroutine(inputHintFadeCoroutine);
                }
                inputHintFadeCoroutine = FadeInputHint();
                StartCoroutine(inputHintFadeCoroutine);

                IEnumerator FadeInputHint()
                {
                    yield return new Utils.DoForSeconds<float, Color, Image>(1,
                        (float StartTime, Color StartColor, Image Image) =>
                        {
                            Image.color = new Color(StartColor.r, StartColor.g, StartColor.b, 1 + StartTime - Time.time);
                        },
                        Time.time, InputHint.color, InputHint
                    );
                    InputHint.color = Color.clear;
                }
            }*/
        }
        
        public void UnFadeVisuals()
        {
            isVisible = true;
            foreach (TimelineNote n in notesAlive)
            {
                n.UnFadeNote();
            }

            /*if (inputHintFadeCoroutine != null)
            {
                StopCoroutine(inputHintFadeCoroutine);
            }
            inputHintFadeCoroutine = UnFadeInputHint();
            StartCoroutine(inputHintFadeCoroutine);
            
            IEnumerator UnFadeInputHint()
            {
                yield return new Utils.DoForSeconds<float, Color, Image>(1,
                    (float StartTime, Color StartColor, Image Image) =>
                    {
                        Image.color = new Color(StartColor.r, StartColor.g, StartColor.b, 0 + StartTime + Time.time);
                    },
                    Time.time, InputHint.color, InputHint
                );
                InputHint.color = Color.white;
            }*/
        }
    }
}
