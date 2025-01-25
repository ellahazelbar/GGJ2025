using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GrandmaScore : MonoBehaviour
{
    
    [SerializeField] private int madScore = -50;
    [SerializeField] private int almostMadScore = -25;
    [SerializeField] private int almostHappyScore = 25;
    [SerializeField] private int happyScore = 50;

    [SerializeField] private Sprite madSprite;
    [SerializeField] private Sprite almostMadSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite almostHappySprite;
    [SerializeField] private Sprite happySprite;

    [SerializeField] private Image grandmaSpriteRenderer;
    
    [SerializeField] private MMF_Player positiveFeedbacks, negativeFeedbacks;
    

    private int currentScore = 0;
    private int currentGrandmaState = 0; // 0: Neutral, 1: Almost Mad, 2: Mad, 3: Almost Happy, 4: Happy

    private void OnAddScore(int score)
    {
        currentScore += score;
        CheckIfScoreThresholdReached(currentScore);
    }
    
    [ContextMenu("Add 25 Score")]
    public void Add25Score()
    {
        OnAddScore(25);
    }
    [ContextMenu("Remove 25 Score")]
    public void Remove25Score()
    {
        OnAddScore(-25);
    }

    private void CheckIfScoreThresholdReached( int newScore)
    {
        if (newScore >= almostMadScore && newScore <= almostHappyScore && currentGrandmaState != 0)
        {
            UpdateGrandmaState(0, normalSprite);
        }
        else if (newScore <= madScore && currentGrandmaState != -2)
        {
            UpdateGrandmaState(-2, madSprite);
        }
        else if (newScore <= almostMadScore && newScore > madScore && currentGrandmaState != -1)
        {
            UpdateGrandmaState(-1, almostMadSprite);
        }
        else if (newScore >= almostHappyScore && newScore < happyScore && currentGrandmaState != 1)
        {
            UpdateGrandmaState(1, almostHappySprite);
        }
        else if (newScore >= happyScore && currentGrandmaState != 2)
        {
            UpdateGrandmaState(2, happySprite);
        }
    }

    private void UpdateGrandmaState(int newState, Sprite newSprite)
    {
        int changeDirection;
        if(newState > currentGrandmaState)
            changeDirection = 1;
        else
            changeDirection = -1;
        
        Debug.Log(changeDirection);
        currentGrandmaState = newState;

        if (grandmaSpriteRenderer != null)
        {
            grandmaSpriteRenderer.sprite = newSprite;
        }

        if (changeDirection == 1)
        {
            TriggerPositiveEffects();
        }
        else
        {
            TriggerNegativeEffects();
        }
    }
    
    private void TriggerPositiveEffects()
    {
        positiveFeedbacks.PlayFeedbacks();
    }
    private void TriggerNegativeEffects()
    {
        negativeFeedbacks.PlayFeedbacks();
    }
    
    
}
