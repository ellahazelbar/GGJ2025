using System;
using MoreMountains.Feedbacks;
using Player;
using UnityEngine;

public class LevelShakeManager : MonoBehaviour
{
    public static LevelShakeManager Instance { get; private set; }
    public float distanceMultip = 1;
    public static float distanceMultiplier {get => Instance.distanceMultip;}
    public static event Action<Vector3> WorldShake;
    private void Awake()
    {
        Instance = this;
    }
    
    public void PlayWorldShake(Vector3 position)
    {
        WorldShake?.Invoke(position);
    }
    
    

    
}
