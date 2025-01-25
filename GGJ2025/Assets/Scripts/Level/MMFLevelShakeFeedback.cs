using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

public class MMFLevelShakeFeedback : MMF_Player
{
    public float distanceMultiplier = 0.1f;
    protected override void Start()
    {
        base.Awake();
        LevelShakeManager.WorldShake += Play;
    }

    private void Play(Vector3 position)
    {
        Debug.Log("Shake");
        float distance = Vector3.Distance(transform.position, position);
        distance *= LevelShakeManager.distanceMultiplier;
        
        StartCoroutine(PlayWithDelay(distance));
    }
    
    private IEnumerator PlayWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Extensions.MMF.StopAndPlayFeedbacks(this);
    }
}
