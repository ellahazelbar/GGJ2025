using UnityEngine;
using TNRD.Autohook;

namespace Instruments
{
    public class Instrument : MonoBehaviour
    {
        [AutoHook] public AudioSource AudioSource;
        public InstrumentMinigame Minigame;
        public InstrumentType Type;
        [field: SerializeField] public Transform InteractionSpot { get; private set; }

        public AnimationCurve FadeCurve;

        private bool active;

        private void Start()
        {
            SongManager.Instance.RegisterInstrument(Type, this);
            Deactivate();
        }

        public void Activate()
        {
            if (active)
            {
                Minigame.NotePlayed();
            }
            else
            {
                active = true;
                Minigame.Activate(Type);
            }
        }

        private void Update()
        {
            if (active)
            {
                if (Minigame.NextFadeTime < Time.time)
                {
                    AudioSource.volume = FadeCurve.Evaluate(Time.time - Minigame.NextFadeTime);
                }
                else AudioSource.volume = Mathf.MoveTowards(AudioSource.volume, 1, Time.deltaTime / Minigame.AccuracyRange);
                if (Minigame.NextShutoffTime < Time.time)
                {
                    active = false;
                }
            }
        }

        public void Deactivate()
        {
            AudioSource.volume = 0;
            active = false;
        }
    }
}
