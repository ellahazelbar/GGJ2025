#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public class UIColorFlicker : MonoBehaviour
    {
        public Graphic Graphic;
        public Color ColorOne = Color.white, ColorTwo = Color.black;

        [System.Serializable]
        private enum FlickerMode { Smooth, PingPong, Instant }
        [SerializeField]
        private FlickerMode Mode;

        public float Speed = 1;
        private double startTime;

        void OnEnable()
        {
            startTime = Time.realtimeSinceStartupAsDouble;
        }

        void OnDisable()
        {
            Graphic.color = ColorOne;
        }
        void Update()
        {
            switch (Mode)
            {
                case FlickerMode.PingPong:
                {
                    Graphic.color = Color.Lerp(ColorOne, ColorTwo, Mathf.PingPong((float)(Time.realtimeSinceStartupAsDouble - startTime) * Speed, 1));
                    break;
                }
                case FlickerMode.Smooth:
                {
                    Graphic.color = Color.Lerp(ColorOne, ColorTwo, (0.5f * (Mathf.Cos((float)(Time.realtimeSinceStartupAsDouble - startTime) * Speed * Mathf.PI) + 1)));
                    break;
                }
                case FlickerMode.Instant:
                {
                    Graphic.color = Mathf.PingPong((float)(Time.realtimeSinceStartupAsDouble - startTime) * Speed, 1) > 0.5f ? ColorOne : ColorTwo;
                    break;
                }
            }
        }

        void Reset()
        {
            Graphic = GetComponent<Graphic>();
            if (null != Graphic)
                ColorOne = Graphic.color;
        }
    }
}