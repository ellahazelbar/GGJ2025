using UnityEngine;
using Instruments;
using System.Collections.Generic;
public class SongManager : Utils.SingletonMonoBehaviour<SongManager>
{
    public Song Song;
    public float LookAheadTime = 10;

    private Dictionary<InstrumentType, List<Song.Interval>> intervalsPerInstrument;

    private Dictionary<InstrumentType, Instrument> instruments;

    private Dictionary<InstrumentType, Song.Interval> currentInterval;

    public float SongStartTime;
    public float TimeSongTime(float Time) { return Time - SongStartTime; }
    public float SongTimeToTime(float SongTime) { return SongTime + SongStartTime; }
    public Song.Interval GetCurrentInterval(InstrumentType Type) { return currentInterval.TryGetValue(Type, out Song.Interval inter) ? inter : null; }


    override protected void Awake()
    {
        base.Awake();
        instruments = new();
        currentInterval = new();
    }

    private void Start()
    {
        intervalsPerInstrument = Song.SortedIntervals();
    }

    public void RegisterInstrument(InstrumentType Type, Instrument Instrument)
    {
        instruments.Add(Type, Instrument);
    }


    private void Update()
    {
        for (InstrumentType i = 0; i < InstrumentType.Max; i++)
        {

            List<Song.Interval> intervalsLeft = intervalsPerInstrument[i];
            if (0 < intervalsLeft.Count && TimeSongTime(Time.time) + LookAheadTime > intervalsLeft[0].PlayTime)
            {
                Song.Interval inter = intervalsLeft[0];
                instruments[i].Minigame.NotifyIncomingInterval(inter);
                intervalsLeft.RemoveAt(0);
            }
        }
    }
}

