using MoreMountains.Feedbacks;
using UnityEngine;

public class MakeItShake : MonoBehaviour
{
    public MMF_Player player;

    private float timer = 3f;
    private float timerCounter = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timerCounter += Time.deltaTime;
        if (timerCounter >= timer)
        {
            player.PlayFeedbacks();
            timerCounter = 0;
        }
    }
}
