using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Instruments
{
    public class TimelineNote : MonoBehaviour
    {
        public Sprite[] AvailableSprites;
        public AnimationCurve AlphaCurve;

        private Image im;
        private RectTransform re;
        private float unitsPerSecond;
        public float PlayTime { get; private set; }
        
        private IEnumerator fadeCoroutine;

        private Color originalColor;
        
        public void Init(Timeline Timeline, float PlayTime, float UnitsPerSecond)
        {
            im = GetComponent<Image>();
            re = GetComponent<RectTransform>();
            im.sprite = AvailableSprites[Random.Range(0, AvailableSprites.Length)];
            originalColor = Timeline.NoteColor;
            im.color = Timeline.isVisible ? originalColor : Color.clear;
            this.PlayTime = PlayTime;
            unitsPerSecond = UnitsPerSecond;
            new Utils.Timer<Timeline, TimelineNote>(PlayTime + 0.8f - Time.time, (Timeline T, TimelineNote N) => { T.Forget(N); }, Timeline, this);
            Destroy(gameObject, PlayTime + 1 - Time.time);
        }

        void LateUpdate()
        {
            float timeToPlay = PlayTime - Time.time;
            re.anchoredPosition = new Vector2(0, timeToPlay * unitsPerSecond);
            Color c = im.color;
            c.a = AlphaCurve.Evaluate(timeToPlay);
            im.color = c;
        }

        public void FadeNote()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = Fade();
            StartCoroutine(fadeCoroutine);
        }
        
        public void UnFadeNote()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = UnFade();
            StartCoroutine(fadeCoroutine);
        }

        private IEnumerator Fade()
        {
            Color colorAtStartOfFade = im.color;
            while (im.color.a > 0)
            {
                im.color = Color.Lerp(colorAtStartOfFade, Color.clear, Mathf.PingPong(Time.time, 1));
                yield return null;
            }
            //im.color = Color.clear;
        }
        
        private IEnumerator UnFade()
        {
            Color colorAtStartOfFade = im.color;
            while (im.color.a < 1)
            {
                im.color = Color.Lerp(colorAtStartOfFade, originalColor, Mathf.PingPong(Time.time, 1));
                yield return null;
            }
        }
    }
}