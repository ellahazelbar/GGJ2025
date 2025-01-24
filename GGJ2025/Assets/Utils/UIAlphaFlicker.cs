#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ===================================================================================
 * UIFlicker -
 * DESCRIPTION - Flicker UI Elements.
 * Modes:
 * - Smooth: Sine Wave. Stays longer near MaxAlpha and MinAlpha. 
 * - Ping Pong: Lineraly goes up to MaxAlpha then back down to MinAlpha etc. 
 * - Instant: Waits a set amount of time, then immediately alternates between MinAlpha and MaxAlpha
 * =================================================================================== */

namespace Utils
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIFlicker : MonoBehaviour
    {
        [System.Serializable]
        private enum FlickerMode { Smooth, PingPong, Instant }

        [SerializeField]
        private FlickerMode Mode;

        public float Speed = 1;
        [Range(0, 1)]
        public float MinAlpha = 0, MaxAlpha = 1;

        private CanvasGroup canvasGroup;
        private float startTime;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void OnEnable()
        {
            startTime = Time.time;
        }
        void Update()
        {
            switch (Mode)
            {
                case FlickerMode.PingPong:
                {
                    canvasGroup.alpha = MaxAlpha - Mathf.PingPong((Time.time - startTime) * Speed, MaxAlpha - MinAlpha);
                    break;
                }
                case FlickerMode.Smooth:
                {
                    canvasGroup.alpha = (MaxAlpha - MinAlpha) * (0.5f * (Mathf.Cos((Time.time - startTime) * Speed * Mathf.PI) + 1)) + MinAlpha;
                    break;
                }
                case FlickerMode.Instant:
                {
                    canvasGroup.alpha = Mathf.PingPong((Time.time - startTime) * Speed, 1) > 0.5f ? MaxAlpha : MinAlpha;
                    break;
                }
            }
        }
    }
}