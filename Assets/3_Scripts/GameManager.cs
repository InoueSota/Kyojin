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

            // Camera��Animation�𓮂���
            cameraAnimator.SetTrigger("Start");

            isStartAnimation = true;
        }
    }
    void GameOver()
    {

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
                resultObj.SetActive(true);

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
    }
    void Count()
    {
        // Count�̑���
        if (inputManager.IsTrgger(inputManager.a)) { discoveryCount++; }
        else if (inputManager.IsTrgger(inputManager.b)) { discoveryCount--; }

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
    public bool GetIsFinish() { return isFinish; }
    public bool GetIsFinishAnimation() {  return isFinishAnimation; }
    public bool GetIsFinishDarkAnimation() {  return isFinishDarkAnimation; }

    // Setter
    public void SetStartReadyAnimation() { isFinishAnimation = true; readyAnimator.gameObject.SetActive(true); readyAnimator.SetTrigger("Start"); }
    public void SetStartGoAnimation() { saturatedLineObj.SetActive(true); goAnimator.gameObject.SetActive(true); goAnimator.SetTrigger("Start"); }
    public void SetIsStart(bool _isStart)
    {
        // UI�̕\���^��\����؂�ւ���
        timeLimitObj.SetActive(true);

        isStart = _isStart;
    }
}
