using UnityEngine;
using System;
using System.Collections.Generic;

namespace Instruments
{
    public enum Instrument
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
            public Instrument Instrument;
            public float PlayTime;
            public float Duration;
        }

        public List<Interval> Notes;

        /// <summary>
        /// Expensive. Call and cache result.
        /// </summary>
        /// <returns></returns>
        public Dictionary<Instrument, List<Interval>> SortedIntervals()
        {
            Dictionary<Instrument, List<Interval>> res = new Dictionary<Instrument, List<Interval>>();
            for (int i = 0; i < (int)Instrument.Max; i++)
            {
                res.Add((Instrument)i, new List<Interval>());
            }
            foreach (Interval i in Notes)
            {
                res[i.Instrument].Add(i);
            }
            foreach (List<Interval> inters in res.Values)
            {
                inters.Sort();
            }
            return res;
        }
    }
}