using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Instruments
{
    public class TimelineNote : MonoBehaviour
    {
        public Sprite[] AvailableSprites;
        public AnimationCurve AlphaCurve;
        [SerializeField] private float timeToForget = 0.8f;

        private Image im;
        private RectTransform re;
        private float unitsPerSecond;
        public float PlayTime { get; private set; }
        
        private IEnumerator fadeCoroutine;

        private Color originalColor;

        private bool isVisible = false;
        
        public void Init(Timeline Timeline, float PlayTime, float UnitsPerSecond)
        {
            im = GetComponent<Image>();
            re = GetComponent<RectTransform>();
            im.sprite = AvailableSprites[Random.Range(0, AvailableSprites.Length)];
            originalColor = Timeline.NoteColor;
            im.color = originalColor;
            //im.color = Timeline.isVisible ? originalColor : Color.clear;
            this.PlayTime = PlayTime;
            unitsPerSecond = UnitsPerSecond;
            new Utils.Timer<Timeline, TimelineNote>(PlayTime + timeToForget - Time.time, (Timeline T, TimelineNote N) => { T.Forget(N); }, Timeline, this);
            Destroy(gameObject, PlayTime + 1 - Time.time);
        }

        void LateUpdate()
        {
            float timeToPlay = PlayTime - Time.time;
            re.anchoredPosition = new Vector2(0, timeToPlay * unitsPerSecond);
            if (isVisible)
            {
                Color c = im.color;
                c.a = AlphaCurve.Evaluate(timeToPlay);
                im.color = c;
            }
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
            isVisible = false;
            yield return new Utils.DoForSeconds<float, Color, Image>(0.5f,
                (float StartTime, Color StartColor, Image Image) =>
                {
                    Image.color = new Color(StartColor.r, StartColor.g, StartColor.b, 1 + StartTime - Time.time);
                },
                Time.time, im.color, im
            );
            //im.color = Color.clear;
        }
        
        private IEnumerator UnFade()
        {
            isVisible = true;
            yield return new Utils.DoForSeconds<float, Color, Image>(0.5f,
                (float StartTime, Color StartColor, Image Image) =>
                {
                    Image.color = new Color(StartColor.r, StartColor.g, StartColor.b, 0 + StartTime + Time.time);
                },
                Time.time, im.color, im
            );
            im.color = originalColor;
        }
    }
}