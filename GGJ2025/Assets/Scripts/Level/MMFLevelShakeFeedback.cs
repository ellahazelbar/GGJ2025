using MoreMountains.Feedbacks;
using UnityEngine;

public class MMFLevelShakeFeedback : MMF_Player
{
    
    void Start()
    {
        LevelShakeManager.instance.WorldShake += PlayFeedbacks;
    }

    
}
