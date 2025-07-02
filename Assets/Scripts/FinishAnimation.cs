using UnityEngine;

public class FinishAnimation : MonoBehaviour
{
    // Other Component
    [SerializeField] private GameManager gameManager;

    public void SetIsStartTrue() { gameManager.SetIsStart(true); }
}
