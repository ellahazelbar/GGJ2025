using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Instruments
{
    public class TimelineNote : MonoBehaviour
    {
        public Sprite[] AvailableSprites;

        private Image im;
        private RectTransform re;
        private float unitsPerSecond;
        public float PlayTime { get; private set; }
        
        public void Init(Timeline Timeline, float PlayTime, float UnitsPerSecond)
        {
            im = GetComponent<Image>();
            re = GetComponent<RectTransform>();
            im.sprite = AvailableSprites[Random.Range(0, AvailableSprites.Length)];
            im.color = Timeline.NoteColor;
            this.PlayTime = PlayTime;
            unitsPerSecond = UnitsPerSecond;
            new Utils.Timer<Timeline, TimelineNote>(PlayTime + 0.8f - Time.time, (Timeline T, TimelineNote N) => { T.Forget(N); }, Timeline, this);
            Destroy(gameObject, PlayTime + 1 - Time.time);
        }

        void LateUpdate()
        {
            re.anchoredPosition = new Vector2(0, (PlayTime - Time.time) * unitsPerSecond);
        }

        public IEnumerator Fade()
        {
            yield return new Utils.DoForSeconds<float, Color, Image>(1,
                (float StartTime, Color StartColor, Image Image) =>
                {
                    Image.color = new Color(StartColor.r, StartColor.g, StartColor.b, 1 + StartTime - Time.time);
                },
                Time.time, im.color, im
            );
            im.color = Color.clear;
        }
    }
}