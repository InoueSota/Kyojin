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
        // ���͏󋵂��擾����
        inputManager.GetAllInput();

        // �Q�[�����݂̂̏���
        if (isStart)
        {
            // ��������
            TimeLimit();

            // �J�E���g
            Count();
        }

        // �Q�[���J�n����
        GameStart();
    }
    void GameStart()
    {
        if (!isStart && inputManager.IsTrgger(inputManager.a))
        {
            // UI�̕\���^��\����؂�ւ���
            toStartObj.SetActive(false);
            timeLimitObj.SetActive(true);

            isStart = true;
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
}
