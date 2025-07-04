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
        // 入力状況を取得する
        inputManager.GetAllInput();

        // ゲーム開始処理
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
