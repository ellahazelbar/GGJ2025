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
        public Dictionary<InstrumentType, List<Interval>> SortedIntervals()
        {
            Dictionary<InstrumentType, List<Interval>> res = new();
            for (int i = 0; i < (int)InstrumentType.Max; i++)
            {
                res.Add((InstrumentType)i, new List<Interval>());
            }
            foreach (Interval i in Intervals)
            {
                if (!i.Skip)
                    res[i.Instrument].Add(i);
            }
            foreach (List<Interval> inters in res.Values)
            {
                inters.Sort((Interval A, Interval B) => { return A.PlayTime.CompareTo(B.PlayTime);  });
            }
            return res;
        }

        public TextAsset TextFile;

        public float ReadOffset;
        public float SpeedMultiplier;

        [ContextMenu("ReadTextAsset")]
        public void ReadTextFile()
        {
            Intervals.Clear();
            string text = TextFile.text;
            string[] lines = text.Split('\n');
            Interval current = null;
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                if (line[0] == '[')
                {
                    string min = line.Substring(1, 2), time = line.Substring(4, 5);
                    float playTime = ((60f * int.Parse(min)) + float.Parse(time) + ReadOffset) * SpeedMultiplier;
                    current.Notes.Add(new Interval.Note { Duration = 2, PlayTime = playTime });
                }
                else 
                {
                    string[] nums = line.Split(' ');
                    if (3 == nums.Length && int.TryParse(nums[0], out int instrument) && float.TryParse(nums[1], out float playTime) && float.TryParse(nums[2], out float duration))
                    {
                        if (null != current)
                            Intervals.Add(current);
                        current = new Interval
                        {
                            Duration = duration,
                            PlayTime = playTime,
                            Instrument = (InstrumentType)instrument,
                            Notes = new()
                        };
                    }
                }
            }
            Intervals.Add(current);
        }
    }
}