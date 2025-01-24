using System;
using System.Collections.Generic;
using UnityEngine;

namespace Instruments
{
    public enum InstrumentType
    {
        Microwave,
        Pots,
        Grandma,
        Blender,

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
                res[i.Instrument].Add(i);
            }
            foreach (List<Interval> inters in res.Values)
            {
                inters.Sort((Interval A, Interval B) => { return A.PlayTime.CompareTo(B.PlayTime);  });
            }
            return res;
        }
    }
}