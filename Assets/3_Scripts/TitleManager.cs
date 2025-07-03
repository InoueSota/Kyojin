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
        // ���͏󋵂��擾����
        inputManager.GetAllInput();

        // �Q�[���J�n����
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
