using UnityEngine;
using Instruments;
using System.Collections.Generic;
public class SongManager : Utils.SingletonMonoBehaviour<SongManager>
{
    public Song Song;
    public float LookAheadTime = 10;

    private Dictionary<InstrumentType, List<Song.Interval>> intervalsPerInstrument;

    private Dictionary<InstrumentType, Timeline> instrumentTimelines;

    public float SongStartTime;
    public float TimeSongTime(float Time) { return Time - SongStartTime; }
    public float SongTimeToTime(float SongTime) { return SongTime + SongStartTime; }

    override protected void Awake()
    {
        base.Awake();
        instrumentTimelines = new();
    }

    private void Start()
    {
        intervalsPerInstrument = Song.SortedIntervals();
    }

    public void RegisterInstrumentTimeline(InstrumentType Type, Timeline Timeline)
    {
        instrumentTimelines.Add(Type, Timeline);
    }


    private void Update()
    {
        for (int i = 0; i < (int)InstrumentType.Max; i++)
        {
            List<Song.Interval> intervalsLeft = intervalsPerInstrument[(InstrumentType)i];
            if (0 < intervalsLeft.Count && TimeSongTime(Time.time) + LookAheadTime > intervalsLeft[0].PlayTime)
            {
                Song.Interval inter = intervalsLeft[0];
                instrumentTimelines[(InstrumentType)i].CreateNote(SongTimeToTime(inter.PlayTime), inter.Duration);
                intervalsLeft.RemoveAt(0);
            }
        }
    }
}

