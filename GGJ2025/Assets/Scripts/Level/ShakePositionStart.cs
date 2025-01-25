using UnityEngine;
using UnityEngine.EventSystems;

public class ShakePositionStart : MonoBehaviour
{
    [SerializeField] private Transform _shakePosition;
    
    public void StartShake()
    {
        Debug.Log("Click");
        LevelShakeManager.Instance.PlayWorldShake();
    }
}
