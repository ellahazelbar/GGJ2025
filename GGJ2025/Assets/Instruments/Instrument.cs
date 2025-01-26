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

        private Player.PlayerInstrumentActivator attached;

        private void Start()
        {
            SongManager.Instance.RegisterInstrument(Type, this);
            Deactivate();
        }

        public void Activate(Player.PlayerInstrumentActivator Attached)
        {
            if (attached)
            {
                Minigame.NotePlayed();
            }
            else
            {
                attached = Attached;
                Minigame.Activate();
            }
        }

        private void Update()
        {
            if (attached)
            {
                if (Minigame.NextFadeTime < Time.time)
                {
                    AudioSource.volume = FadeCurve.Evaluate(Time.time - Minigame.NextFadeTime);
                }
                else AudioSource.volume = Mathf.MoveTowards(AudioSource.volume, 1, Time.deltaTime / Minigame.AccuracyRange);
                if (Minigame.NextShutoffTime < Time.time)
                { 
                    if (null != attached)
                        attached.Disengage();
                }
            }
        }

        public void Deactivate()
        {
            AudioSource.volume = 0;
            attached = null;
            Minigame.Deactivate();
        }
    }
}
