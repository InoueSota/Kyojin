using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // My Component
    private InputManager inputManager;

    // Flag
    private bool isStart;
    private bool isFinish;
    private bool isGameOver;
    private bool isStartAnimation;
    private bool isFinishAnimation;
    private bool isFinishEndAnimation;
    private bool isFinishDarkAnimation;
    private bool isStartsToLightenUp;
    private bool isStartResult;
    private bool isFinishCount;
    private bool isChangeScene;

    [Header("Game Parameter")]
    [SerializeField] private float maxTime;
    private float timeLimit;
    private int discoveryCount;

    [Header("Game Over paramter")]
    [SerializeField] private float gameOverDarkTime;
    private float gameOverDarkTimer;

    [Header("Finish Parameter")]
    [SerializeField] private float finishIntervalTime;
    [SerializeField] private float finishDarkIntervalTime;
    private float finishIntervalTimer;

    [Header("Result Parameter")]
    [SerializeField] private FieldsScript fieldsScript;
    [SerializeField] private float startResultIntervalTime;
    private float startResultIntervalTimer;
    private enum ResultPhase
    {
        DEFAULT,
        YOUR,
        TRUE,
        ERROR,
        RESULT
    }
    private ResultPhase resultPhase = ResultPhase.DEFAULT;
    [SerializeField] private float toYourPhaseTime;
    [SerializeField] private float toTruePhaseTime;
    [SerializeField] private float toErrorPhaseTime;
    [SerializeField] private float toResultPhaseTime;
    private float phaseTimer;
    private int redCount;
    private int errorNumber;

    [Header("Result BGM")]
    [SerializeField] private AudioSource childAudioSource;
    [SerializeField] private AudioClip resultBGM;
    [SerializeField] private float volumeDownTime;
    private float volumeDownTimer;
    [SerializeField] private float volumeDownStartTime;
    [SerializeField] private float changeBGMTime;
    private float resultBGMTimer;
    private bool isStartChangeBGM;
    private bool isStartVolumeDown;
    private bool isFinishVolumeDown;
    private bool isFinishChangeBGM;

    [Header("Result UI")]
    [SerializeField] private Text fadeYourCount;
    [SerializeField] private Text backYourCount;
    [SerializeField] private Text yourCount;
    [SerializeField] private Text fadeTrueCount;
    [SerializeField] private Text backTrueCount;
    [SerializeField] private Text trueCount;
    [SerializeField] private Text fadeErrorCount;
    [SerializeField] private Text backErrorCount;
    [SerializeField] private Text errorCount;
    [SerializeField] private GameObject groupErrorObj;
    [SerializeField] private GameObject groupPerfectObj;
    [SerializeField] private GameObject groupTooManyObj;
    [SerializeField] private GameObject groupNotEnoughObj;

    [Header("Game UI")]
    [SerializeField] private GameObject explanationObj;
    [SerializeField] private GameObject saturatedLineObj;
    [SerializeField] private GameObject beforeStartObj;
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private GameObject finishObj;
    [SerializeField] private GameObject timeLimitObj;
    private Text timeLimitText;
    [SerializeField] private Text countText;
    [SerializeField] private GameObject afterFinishTextsObj;
    [SerializeField] private GameObject resultObj;
    [SerializeField] private GameObject resultBackGroundObj;

    [Header("Animators")]
    [SerializeField] private Animator purposeAnimator;
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Animator readyAnimator;
    [SerializeField] private Animator goAnimator;
    [SerializeField] private Animator finishesAnimator;
    [SerializeField] private Animator darkBackgroundAnimator;
    [SerializeField] private Animator finishFramesAnimator;
    [SerializeField] private Animator gameOverDarkAnimator;
    [SerializeField] private Animator darkAnimator;

    [Header("Sound")]
    [SerializeField] private AudioClip goClip;
    [SerializeField] private AudioClip countUpClip;
    [SerializeField] private AudioClip countDownClip;
    [SerializeField] private AudioClip finishClip;
    private AudioSource audioSource;

    void Start()
    {
        // Set Component
        inputManager = GetComponent<InputManager>();
        audioSource = GetComponent<AudioSource>();

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
        // ゲーム失敗処理
        GameOver();
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

            // Purposeを消す
            purposeAnimator.SetTrigger("Start");
            // CameraのAnimationを動かす
            cameraAnimator.SetTrigger("Start");

            isStartAnimation = true;
        }
    }
    void GameOver()
    {
        if (isGameOver)
        {
            gameOverDarkTimer -= Time.deltaTime;

            if (gameOverDarkTimer <= 0f)
            {
                gameOverDarkAnimator.SetTrigger("Start");
            }
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
    void Finish()
    {
        if (!isFinish && timeLimit <= 0f)
        {
            // UIの表示／非表示を切り替える
            timeLimitObj.SetActive(false);
            finishObj.SetActive(true);

            // Sound
            audioSource.PlayOneShot(finishClip);

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
                explanationObj.SetActive(false);
                saturatedLineObj.SetActive(false);
                resultObj.SetActive(true);

                // YourCountTextに適用
                fadeYourCount.text = string.Format("{0:00}", discoveryCount);
                backYourCount.text = string.Format("{0:00}", discoveryCount);
                yourCount.text = string.Format("{0:00}", discoveryCount);

                // BGMを変えようとする処理開始
                isStartChangeBGM = true;
                resultBGMTimer = 0f;

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

            // カウント終了後
            if (isFinishCount)
            {
                phaseTimer += Time.deltaTime;

                switch (resultPhase)
                {
                    case ResultPhase.DEFAULT:

                        if (phaseTimer >= toYourPhaseTime)
                        {
                            fadeYourCount.gameObject.SetActive(true);
                            resultPhase = ResultPhase.YOUR;
                        }

                        break;
                    case ResultPhase.YOUR:

                        if (phaseTimer >= toTruePhaseTime)
                        {
                            fadeTrueCount.gameObject.SetActive(true);
                            resultPhase = ResultPhase.TRUE;
                        }

                        break;
                    case ResultPhase.TRUE:

                        if (phaseTimer >= toErrorPhaseTime)
                        {
                            // 表示
                            groupErrorObj.SetActive(true);

                            errorNumber = discoveryCount - redCount;

                            // 誤差の適用
                            fadeErrorCount.text = string.Format("{0:00}", errorNumber);
                            backErrorCount.text = string.Format("{0:00}", errorNumber);
                            errorCount.text = string.Format("{0:00}", errorNumber);

                            resultPhase = ResultPhase.ERROR;
                        }

                        break;
                    case ResultPhase.ERROR:

                        if (phaseTimer >= toResultPhaseTime)
                        {
                            // Perfect
                            if (errorNumber == 0)
                            {
                                groupPerfectObj.SetActive(true);
                            }
                            // Too Many
                            else if (errorNumber > 0)
                            {
                                groupTooManyObj.SetActive(true);
                            }
                            // Not Enough
                            else if (errorNumber < 0)
                            {
                                groupNotEnoughObj.SetActive(true);
                            }

                            resultPhase = ResultPhase.RESULT;
                        }

                        break;
                    case ResultPhase.RESULT:

                        if (!isChangeScene && inputManager.IsTrgger(inputManager.a))
                        {
                            if (errorNumber == 0) { darkAnimator.gameObject.GetComponent<FinishAnimation>().SetNextSceneName("GameClearScene"); }
                            else { darkAnimator.gameObject.GetComponent<FinishAnimation>().SetNextSceneName("TitleScene"); }
                            darkAnimator.SetTrigger("StartFadeIn");

                            isChangeScene = true;
                        }

                        break;
                }
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

        // Resultの音関係
        if (isStartChangeBGM)
        {
            resultBGMTimer += Time.deltaTime;

            if (!isStartVolumeDown && resultBGMTimer >= volumeDownStartTime)
            {
                volumeDownTimer = volumeDownTime;

                // フラグをtrueにする
                isStartVolumeDown = true;
            }

            if (!isFinishVolumeDown && isStartVolumeDown)
            {
                volumeDownTimer -= Time.deltaTime;
                volumeDownTimer = Mathf.Clamp(volumeDownTimer, 0f, volumeDownTime);
                childAudioSource.volume = (volumeDownTimer / volumeDownTime) * 0.2f;
                if (volumeDownTimer <= 0f) { childAudioSource.Stop(); isFinishVolumeDown = true; }
            }

            if (!isFinishChangeBGM && resultBGMTimer >= changeBGMTime)
            {
                childAudioSource.volume = 0.2f;
                childAudioSource.PlayOneShot(resultBGM);
                isFinishChangeBGM = true;
            }
        }
    }
    void Count()
    {
        // Countの増減
        if (inputManager.IsTrgger(inputManager.a)) { discoveryCount++; audioSource.PlayOneShot(countUpClip); }
        else if (inputManager.IsTrgger(inputManager.b)) { discoveryCount--; audioSource.PlayOneShot(countDownClip); }

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
    public bool GetIsGameOver() { return isGameOver; }
    public bool GetIsFinish() { return isFinish; }
    public bool GetIsFinishAnimation() { return isFinishAnimation; }
    public bool GetIsFinishDarkAnimation() { return isFinishDarkAnimation; }

    // Setter
    public void SetStartReadyAnimation() { isFinishAnimation = true; readyAnimator.gameObject.SetActive(true); readyAnimator.SetTrigger("Start"); }
    public void SetStartGoAnimation() { audioSource.PlayOneShot(goClip); saturatedLineObj.SetActive(true); goAnimator.gameObject.SetActive(true); goAnimator.SetTrigger("Start"); }
    public void SetIsStart(bool _isStart)
    {
        // UIの表示／非表示を切り替える
        timeLimitObj.SetActive(true);

        audioSource.volume = 0.2f;

        isStart = _isStart;
    }
    public void SetIsGameOver(bool _isGameOver)
    {
        if (_isGameOver)
        {
            // UIの表示／非表示を切り替える
            gameOverObj.SetActive(true);
            // インターバルの設定
            gameOverDarkTimer = gameOverDarkTime;

            isGameOver = _isGameOver;
        }
    }
    public void SetTrueCount(int _redCount) { redCount = _redCount; fadeTrueCount.text = string.Format("{0:00}", _redCount); backTrueCount.text = string.Format("{0:00}", _redCount); trueCount.text = string.Format("{0:00}", _redCount); }
    public void SetIsFinishCount(bool _isFinishCount)
    {
        if (_isFinishCount)
        {
            resultBackGroundObj.SetActive(true);
        }

        isFinishCount = _isFinishCount;
    }
}
