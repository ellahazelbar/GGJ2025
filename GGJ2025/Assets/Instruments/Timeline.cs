using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Instruments
{
    public class Timeline : MonoBehaviour
    {
        public float UnitsPerSecond;
        public GameObject NodePrefab;


        public class Note
        {
            public float PlayTime;
            public float Duration;
            public float Lifetime;
            public Transform Node;

            public Note SetHeight(float UnitsPerSecond)
            {
                RectTransform r = Node.GetChild(0).GetComponent<RectTransform>();
                r.sizeDelta = new Vector2(r.sizeDelta.x, Duration * UnitsPerSecond);
                return this;
            }
        }

        private List<Note> notesAlive;
        private List<Note> toDestroy;

        private void Awake()
        {
            notesAlive = new List<Note>();
            toDestroy = new List<Note>();
        }

        public void CreateNote(float PlayTime, float Duration)
        {
            notesAlive.Add(new Note()
                {
                    Node = Instantiate(NodePrefab, transform).transform,
                    PlayTime = PlayTime,
                    Duration = Duration,
                    Lifetime = Duration + 1
                }.SetHeight(UnitsPerSecond)
            );                
        }

        private void LateUpdate()
        {
            foreach (Note n in notesAlive)
            {
                if (n.PlayTime + n.Duration < Time.time)
                {
                    toDestroy.Add(n);
                }
                n.Node.localPosition = new Vector3(0, (n.PlayTime - Time.time) * UnitsPerSecond);
            }
            foreach (Note n in toDestroy)
            {
                Destroy(n.Node.gameObject);
                notesAlive.Remove(n);
            }
            toDestroy.Clear();
        }
    }
}
