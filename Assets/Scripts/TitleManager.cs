using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // My Component
    private InputManager inputManager;

    [Header("Scene String")]
    [SerializeField] private string changeSceneName;

    void Start()
    {
        // Set Component
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        // “ü—Íó‹µ‚ğæ“¾‚·‚é
        inputManager.GetAllInput();

        // ƒQ[ƒ€ŠJnˆ—
        GameStart();
    }
    void GameStart()
    {
        if (inputManager.IsTrgger(inputManager.a))
        {
            SceneManager.LoadScene(changeSceneName);
        }
    }

    void LateUpdate()
    {
        inputManager.SetIsGetInput();
    }

}
