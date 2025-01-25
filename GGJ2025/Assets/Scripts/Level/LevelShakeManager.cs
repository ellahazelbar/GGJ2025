using System;
using UnityEngine;

public class LevelShakeManager : MonoBehaviour
{
    public static LevelShakeManager Instance { get; private set; }
    public static event Action WorldShake;
    private void Awake()
    {
        Instance = this;
    }
    
    public void PlayWorldShake()
    {
        WorldShake?.Invoke();
    }
    
    

    
}
