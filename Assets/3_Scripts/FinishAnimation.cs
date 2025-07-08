using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishAnimation : MonoBehaviour
{
    // Other Component
    [SerializeField] private GameManager gameManager;

    // Other Scene
    [SerializeField] private string nextSceneName;

    // GameManager
    public void SetIsStartTrue() { gameManager.SetIsStart(true); }
    public void SetStartReadyAnimation() { gameManager.SetStartReadyAnimation(); }
    public void SetStartGoAnimation() { gameManager.SetStartGoAnimation(); }

    public void SetIsActiveFalse() { gameObject.SetActive(false); }

    public void SetNextSceneName(string _nextSceneName) { nextSceneName = _nextSceneName; }
    public void ChangeScene() { SceneManager.LoadScene(nextSceneName); }
}
