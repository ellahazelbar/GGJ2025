using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

public class MMFLevelShakeFeedback : MMF_Player
{
    protected override void Start()
    {
        base.Awake();
        LevelShakeManager.WorldShake += Play;
    }

    private void Play()
    {
        Extensions.MMF.StopAndPlayFeedbacks(this);
    }
    
   
}
