using UnityEngine;

public class FinishAnimation : MonoBehaviour
{
    // Other Component
    [SerializeField] private GameManager gameManager;

    // GameManager
    public void SetIsStartTrue() { gameManager.SetIsStart(true); }
    public void SetStartReadyAnimation() { gameManager.SetStartReadyAnimation(); }
    public void SetStartGoAnimation() { gameManager.SetStartGoAnimation(); }

    public void SetIsActiveFalse() { gameObject.SetActive(false); }
}
