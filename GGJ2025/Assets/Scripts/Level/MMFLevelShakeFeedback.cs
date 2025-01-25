using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

public class MMFLevelShakeFeedback : MMF_Player
{
    protected override void Awake()
    {
        base.Awake();
        LevelShakeManager.WorldShake += Play;
    }

    private void Play()
    {
        Debug.Log("Shake");
        Extensions.MMF.StopAndPlayFeedbacks(this);
    }
}
