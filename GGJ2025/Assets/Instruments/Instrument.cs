using System.Collections.Generic;
using UnityEngine;
using TNRD.Autohook;

namespace Instruments
{
    public class Instrument : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _visuals;
        [AutoHook] public AudioSource AudioSource;
        public InstrumentMinigame Minigame;
        public InstrumentType Type;
        public Vector3 InteractionPosition => GetWorldPositionOnPlane(transform.position);

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

        public void SetVisible(bool visible) => _visuals.ForEach(go => go.SetActive(visible));


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
                    {
                        attached.Disengage();
                        attached = null;
                        //band aid
                        Minigame.Timeline.DisplayInputHint(false);
                    }
                }
            }
        }

        public void Deactivate()
        {
            AudioSource.volume = 0;
            attached = null;
            Minigame.Deactivate();
        }

		private Vector3 GetWorldPositionOnPlane(Vector3 worldPosition, float z = 0)
		{
            var camera = Camera.main;
			Ray ray = camera.ScreenPointToRay(camera.WorldToScreenPoint(worldPosition));
			Plane xy = new(Vector3.forward, new Vector3(0, 0, z));
			xy.Raycast(ray, out float distance);
			return ray.GetPoint(distance);
		}
	}
}
