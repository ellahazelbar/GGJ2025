using System;
using UnityEngine;

public class LevelShakeManager : MonoBehaviour
{
    public static LevelShakeManager instance;

    public event Action WorldShake;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }
    }
    [ContextMenu("Shake the world")]
    public void PlayWorldShake()
    {
        WorldShake.Invoke();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            PlayWorldShake();
    }
}
