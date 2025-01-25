using System;
using System.Collections.Generic;
using UnityEngine;

namespace Instruments
{
    public enum InstrumentType
    {
        Microwave,
        Pots,
        String,
        Blender,
        Chair,
        Glass,
        Radiator,
        Sandpaper,

        //Keep last
        Max
    }
    [CreateAssetMenu(fileName = "Song", menuName = "Scriptable Objects/Song")]
    public class Song : ScriptableObject
    {
        [Serializable]
        public class Interval
        {
            public InstrumentType Instrument;
            public float PlayTime;
            public float Duration;
            public bool Skip;

            [Serializable]
            public class Note
            {
                public float PlayTime;
                public float Duration;
            }
            public List<Note> Notes;
        }

        public List<Interval> Intervals;

        /// <summary>
        /// Expensive. Call and cache result.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<InstrumentType, List<Interval>> SortedIntervals(List<Song> Songs)
        {
            Dictionary<InstrumentType, List<Interval>> res = new();
            for (int i = 0; i < (int)InstrumentType.Max; i++)
            {
                res.Add((InstrumentType)i, new List<Interval>());
            }
            foreach (Song song in Songs) {
                foreach (Interval i in song.Intervals)
                {
                    if (!i.Skip)
                        res[i.Instrument].Add(i);
                }
            }
            foreach (List<Interval> inters in res.Values)
            {
                inters.Sort((Interval A, Interval B) => { return A.PlayTime.CompareTo(B.PlayTime);  });
            }
            return res;
        }

        public TextAsset TextFile;

        public float ReadOffset = -0.25f;
        public float SpeedMultiplier = 1f;
        public InstrumentType Type;

        [ContextMenu("ReadTextAsset")]
        public void ReadTextFile()
        {
            Intervals.Clear();
            string text = TextFile.text;
            string[] lines = text.Split('\n');
            Interval current = new()
            {
                Instrument = Type,
                Notes = new()
            };
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (null != current && 0 < current.Notes.Count)
                    {
                        current.PlayTime = current.Notes[0].PlayTime - 5;
                        current.Duration = current.Notes[^1].PlayTime - current.PlayTime + 0.05f;
                        Intervals.Add(current);
                    }
                    current = new()
                    {
                        Instrument = Type,
                        Notes = new()
                    };
                }
                else if (line[0] == '[')
                {
                    string min = line.Substring(1, 2), time = line.Substring(4, 5);
                    float playTime = ((60f * int.Parse(min)) + float.Parse(time) + ReadOffset) * SpeedMultiplier;
                    current.Notes.Add(new Interval.Note { Duration = 2, PlayTime = playTime });
                }
            }
            if (null != current && 0 < current.Notes.Count)
            {
                current.PlayTime = current.Notes[0].PlayTime - 5;
                current.Duration = current.Notes[^1].PlayTime - current.PlayTime + 0.05f;
                Intervals.Add(current);
            }
        }
    }
}