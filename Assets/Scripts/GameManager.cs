using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // My Component
    private InputManager inputManager;

    // Flag
    private bool isStart;

    [Header("Game Parameter")]
    [SerializeField] private float maxTime;
    private float timeLimit;
    private int discoveryCount;

    [Header("Game UI")]
    [SerializeField] private GameObject toStartObj;
    [SerializeField] private GameObject timeLimitObj;
    private Text timeLimitText;
    [SerializeField] private Text countText;

    void Start()
    {
        // Set Component
        inputManager = GetComponent<InputManager>();

        // Set Parameter
        timeLimit = maxTime;

        // Set Component - UI
        timeLimitText = timeLimitObj.GetComponent<Text>();
    }

    void Update()
    {
        // 入力状況を取得する
        inputManager.GetAllInput();

        // ゲーム中のみの処理
        if (isStart)
        {
            // 制限時間
            TimeLimit();

            // カウント
            Count();
        }

        // ゲーム開始処理
        GameStart();
    }
    void GameStart()
    {
        if (!isStart && inputManager.IsTrgger(inputManager.a))
        {
            // UIの表示／非表示を切り替える
            toStartObj.SetActive(false);
            timeLimitObj.SetActive(true);

            isStart = true;
        }
    }
    void TimeLimit()
    {
        // 時間経過
        timeLimit -= Time.deltaTime;
        timeLimit = Mathf.Clamp(timeLimit, 0, 99);

        // Textに適用
        timeLimitText.text = Mathf.Ceil(timeLimit).ToString();
    }
    void Count()
    {
        // Countの増減
        if (inputManager.IsTrgger(inputManager.a)) { discoveryCount++; }
        else if (inputManager.IsTrgger(inputManager.b)) { discoveryCount--; }

        // Clamp
        discoveryCount = Mathf.Clamp(discoveryCount, 0, 99);
        
        // Textに適用
        countText.text = string.Format("{0:00}", discoveryCount);
    }

    void LateUpdate()
    {
        inputManager.SetIsGetInput();
    }

    // Getter
    public bool GetIsStart() { return isStart; }
}
