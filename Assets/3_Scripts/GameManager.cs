using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // My Component
    private InputManager inputManager;

    // Flag
    private bool isStart;
    private bool isFinish;
    private bool isStartAnimation;
    private bool isFinishAnimation;
    private bool isFinishEndAnimation;
    private bool isFinishDarkAnimation;
    private bool isStartsToLightenUp;
    private bool isStartResult;

    [Header("Game Parameter")]
    [SerializeField] private float maxTime;
    private float timeLimit;
    private int discoveryCount;

    [Header("Finish Parameter")]
    [SerializeField] private float finishIntervalTime;
    [SerializeField] private float finishDarkIntervalTime;
    private float finishIntervalTimer;

    [Header("Result Parameter")]
    [SerializeField] private FieldsScript fieldsScript;
    [SerializeField] private float startResultIntervalTime;
    private float startResultIntervalTimer;

    [Header("Game UI")]
    [SerializeField] private GameObject saturatedLineObj;
    [SerializeField] private GameObject beforeStartObj;
    [SerializeField] private GameObject finishObj;
    [SerializeField] private GameObject timeLimitObj;
    private Text timeLimitText;
    [SerializeField] private Text countText;
    [SerializeField] private GameObject afterFinishTextsObj;
    [SerializeField] private GameObject resultObj;

    [Header("Animators")]
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Animator readyAnimator;
    [SerializeField] private Animator goAnimator;
    [SerializeField] private Animator finishesAnimator;
    [SerializeField] private Animator darkBackgroundAnimator;
    [SerializeField] private Animator finishFramesAnimator;

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
        if (isStart && !isFinish)
        {
            // 制限時間
            TimeLimit();
            // カウント
            Count();
        }

        // ゲーム開始処理
        GameStart();
        // ゲーム終了処理
        Finish();
        // リザルト処理
        Result();
    }
    void GameStart()
    {
        if (!isStartAnimation && inputManager.IsTrgger(inputManager.a))
        {
            // UIの表示／非表示を切り替える
            beforeStartObj.SetActive(false);

            // CameraのAnimationを動かす
            cameraAnimator.SetTrigger("Start");

            isStartAnimation = true;
        }
    }
    void GameOver()
    {

    }
    void TimeLimit()
    {
        // 時間経過
        timeLimit -= Time.deltaTime;
        timeLimit = Mathf.Clamp(timeLimit, 0, 99);

        // Textに適用
        timeLimitText.text = Mathf.Ceil(timeLimit).ToString();
    }
    void Finish()
    {
        if (!isFinish && timeLimit <= 0f)
        {
            // UIの表示／非表示を切り替える
            timeLimitObj.SetActive(false);
            finishObj.SetActive(true);

            // 遷移するまでのインターバル計測開始
            finishIntervalTimer = finishIntervalTime;

            isFinishEndAnimation = false;
            isFinish = true;
        }

        if (isFinish)
        {
            // インターバルの更新
            finishIntervalTimer -= Time.deltaTime;

            // インターバルを満たしたら
            if (finishIntervalTimer <= 0f && !isFinishEndAnimation)
            {
                // Animation開始
                finishesAnimator.SetTrigger("Start");
                darkBackgroundAnimator.SetTrigger("Start");

                // UIの表示／非表示を切り替える
                afterFinishTextsObj.SetActive(true);

                // TimerをResetする
                finishIntervalTimer = finishDarkIntervalTime;

                // Animationを一度でも開始したらフラグをtrueにする
                isFinishEndAnimation = true;
            }

            // 暗闇のインターバルを満たしたら
            if (finishIntervalTimer <= 0f && isFinishEndAnimation && !isFinishDarkAnimation)
            {
                // UIの表示／非表示を切り替える
                resultObj.SetActive(true);

                // Animation開始
                finishFramesAnimator.SetTrigger("Start");

                // Animationを一度でも開始したらフラグをtrueにする
                isFinishDarkAnimation = true;
            }

            // 明転
            if (finishIntervalTimer <= -2f && isFinishDarkAnimation && !isStartsToLightenUp)
            {
                // Animation開始
                darkBackgroundAnimator.SetTrigger("StartFadeOut");

                // インターバルの設定
                startResultIntervalTimer = startResultIntervalTime;

                // Animationを一度でも開始したらフラグをtrueにする
                isStartsToLightenUp = true;
            }
        }
    }
    void Result()
    {
        if (isStartsToLightenUp && !isStartResult)
        {
            // インターバルの更新
            startResultIntervalTimer -= Time.deltaTime;

            if (startResultIntervalTimer <= 0f)
            {
                // FieldsScriptのカウントを開始する
                fieldsScript.SetCanCount(true);

                isStartResult = true;
            }
        }
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
    public bool GetIsStartAnimation() { return isStartAnimation; }
    public bool GetIsFinish() { return isFinish; }
    public bool GetIsFinishAnimation() {  return isFinishAnimation; }
    public bool GetIsFinishDarkAnimation() {  return isFinishDarkAnimation; }

    // Setter
    public void SetStartReadyAnimation() { isFinishAnimation = true; readyAnimator.gameObject.SetActive(true); readyAnimator.SetTrigger("Start"); }
    public void SetStartGoAnimation() { saturatedLineObj.SetActive(true); goAnimator.gameObject.SetActive(true); goAnimator.SetTrigger("Start"); }
    public void SetIsStart(bool _isStart)
    {
        // UIの表示／非表示を切り替える
        timeLimitObj.SetActive(true);

        isStart = _isStart;
    }
}
