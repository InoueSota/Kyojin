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
        // ���͏󋵂��擾����
        inputManager.GetAllInput();

        // �Q�[�����݂̂̏���
        if (isStart && !isFinish)
        {
            // ��������
            TimeLimit();
            // �J�E���g
            Count();
        }

        // �Q�[���J�n����
        GameStart();
        // �Q�[�����s����
        GameOver();
        // �Q�[���I������
        Finish();
        // ���U���g����
        Result();
    }
    void GameStart()
    {
        if (!isStartAnimation && inputManager.IsTrgger(inputManager.a))
        {
            // UI�̕\���^��\����؂�ւ���
            beforeStartObj.SetActive(false);

            // Purpose������
            purposeAnimator.SetTrigger("Start");
            // Camera��Animation�𓮂���
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
        // ���Ԍo��
        timeLimit -= Time.deltaTime;
        timeLimit = Mathf.Clamp(timeLimit, 0, 99);

        // Text�ɓK�p
        timeLimitText.text = Mathf.Ceil(timeLimit).ToString();
    }
    void Finish()
    {
        if (!isFinish && timeLimit <= 0f)
        {
            // UI�̕\���^��\����؂�ւ���
            timeLimitObj.SetActive(false);
            finishObj.SetActive(true);

            // Sound
            audioSource.PlayOneShot(finishClip);

            // �J�ڂ���܂ł̃C���^�[�o���v���J�n
            finishIntervalTimer = finishIntervalTime;

            isFinishEndAnimation = false;
            isFinish = true;
        }

        if (isFinish)
        {
            // �C���^�[�o���̍X�V
            finishIntervalTimer -= Time.deltaTime;

            // �C���^�[�o���𖞂�������
            if (finishIntervalTimer <= 0f && !isFinishEndAnimation)
            {
                // Animation�J�n
                finishesAnimator.SetTrigger("Start");
                darkBackgroundAnimator.SetTrigger("Start");

                // UI�̕\���^��\����؂�ւ���
                afterFinishTextsObj.SetActive(true);

                // Timer��Reset����
                finishIntervalTimer = finishDarkIntervalTime;

                // Animation����x�ł��J�n������t���O��true�ɂ���
                isFinishEndAnimation = true;
            }

            // �Èł̃C���^�[�o���𖞂�������
            if (finishIntervalTimer <= 0f && isFinishEndAnimation && !isFinishDarkAnimation)
            {
                // UI�̕\���^��\����؂�ւ���
                explanationObj.SetActive(false);
                saturatedLineObj.SetActive(false);
                resultObj.SetActive(true);

                // YourCountText�ɓK�p
                fadeYourCount.text = string.Format("{0:00}", discoveryCount);
                backYourCount.text = string.Format("{0:00}", discoveryCount);
                yourCount.text = string.Format("{0:00}", discoveryCount);

                // BGM��ς��悤�Ƃ��鏈���J�n
                isStartChangeBGM = true;
                resultBGMTimer = 0f;

                // Animation�J�n
                finishFramesAnimator.SetTrigger("Start");

                // Animation����x�ł��J�n������t���O��true�ɂ���
                isFinishDarkAnimation = true;
            }

            // ���]
            if (finishIntervalTimer <= -2f && isFinishDarkAnimation && !isStartsToLightenUp)
            {
                // Animation�J�n
                darkBackgroundAnimator.SetTrigger("StartFadeOut");

                // �C���^�[�o���̐ݒ�
                startResultIntervalTimer = startResultIntervalTime;

                // Animation����x�ł��J�n������t���O��true�ɂ���
                isStartsToLightenUp = true;
            }

            // �J�E���g�I����
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
                            // �\��
                            groupErrorObj.SetActive(true);

                            errorNumber = discoveryCount - redCount;

                            // �덷�̓K�p
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
            // �C���^�[�o���̍X�V
            startResultIntervalTimer -= Time.deltaTime;

            if (startResultIntervalTimer <= 0f)
            {
                // FieldsScript�̃J�E���g���J�n����
                fieldsScript.SetCanCount(true);

                isStartResult = true;
            }
        }

        // Result�̉��֌W
        if (isStartChangeBGM)
        {
            resultBGMTimer += Time.deltaTime;

            if (!isStartVolumeDown && resultBGMTimer >= volumeDownStartTime)
            {
                volumeDownTimer = volumeDownTime;

                // �t���O��true�ɂ���
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
        // Count�̑���
        if (inputManager.IsTrgger(inputManager.a)) { discoveryCount++; audioSource.PlayOneShot(countUpClip); }
        else if (inputManager.IsTrgger(inputManager.b)) { discoveryCount--; audioSource.PlayOneShot(countDownClip); }

        // Clamp
        discoveryCount = Mathf.Clamp(discoveryCount, 0, 99);

        // Text�ɓK�p
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
        // UI�̕\���^��\����؂�ւ���
        timeLimitObj.SetActive(true);

        audioSource.volume = 0.2f;

        isStart = _isStart;
    }
    public void SetIsGameOver(bool _isGameOver)
    {
        if (_isGameOver)
        {
            // UI�̕\���^��\����؂�ւ���
            gameOverObj.SetActive(true);
            // �C���^�[�o���̐ݒ�
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
