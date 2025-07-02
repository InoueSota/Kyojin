using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // My Component
    private InputManager inputManager;

    // Flag
    private bool isStart;
    private bool isFinish;
    private bool isStartAnimation;

    [Header("Game Parameter")]
    [SerializeField] private float maxTime;
    private float timeLimit;
    private int discoveryCount;

    [Header("Game UI")]
    [SerializeField] private GameObject beforeStartObj;
    [SerializeField] private GameObject finishObj;
    [SerializeField] private GameObject timeLimitObj;
    private Text timeLimitText;
    [SerializeField] private Text countText;

    [Header("Camera")]
    [SerializeField] private Animator cameraAnimator;

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

            isFinish = true;
        }

        if (isFinish && inputManager.IsTrgger(inputManager.a))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
    public bool GetIsFinish() { return isFinish; }

    // Setter
    public void SetIsStart(bool _isStart)
    {
        // UI�̕\���^��\����؂�ւ���
        timeLimitObj.SetActive(true);

        isStart = _isStart;
    }
}
