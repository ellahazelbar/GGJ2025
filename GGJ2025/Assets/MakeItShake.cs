using MoreMountains.Feedbacks;
using UnityEngine;

public class MakeItShake : MonoBehaviour
{
    public MMF_Player player;

    public uint amountOfBeatsToShakeLevel = 8;
    private float counter = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        BaseLineBeat.Beat += BaseLineBeatOnBeat;
    }

    private void BaseLineBeatOnBeat()
    {
        counter++;
        if (counter >= amountOfBeatsToShakeLevel)
        {
            counter = 0;
            player.PlayFeedbacks();
        }
    }
}
