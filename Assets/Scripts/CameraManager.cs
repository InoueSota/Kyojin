using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Other Objects")]
    [SerializeField] private GameObject gameManagerObj;
    private GameManager gameManager;
    private InputManager inputManager;

    [Header("Lerp Camera Position")]
    [SerializeField] private Vector3 crouchPosition;
    [SerializeField] private Vector3 stayPosition;
    [SerializeField] private Vector3 peekPosition;

    [Header("Lerp Camera Rotation")]
    [SerializeField] private Vector3 crouchRotation;
    [SerializeField] private Vector3 stayRotation;
    [SerializeField] private Vector3 peekRotation;

    [Header("Chase Parameter")]
    [SerializeField] private float chasePower;
    private Vector3 targetPosition;
    private Vector3 targetRotation;
    private Vector3 nowPosition;
    private Vector3 nowRotation;

    void Start()
    {
        // Get Other Component
        gameManager = gameManagerObj.GetComponent<GameManager>();
        inputManager = gameManagerObj.GetComponent<InputManager>();

        // Set Parameter
        targetPosition = stayPosition;
        targetRotation = stayRotation;
        nowPosition = stayPosition;
        nowRotation = stayRotation;
    }

    void Update()
    {
        // ���͏󋵂��擾����
        inputManager.GetAllInput();

        // �Q�[�����݂̂̏���
        if (gameManager.GetIsStart())
        {
            // �`��
            Peek();
        }
    }

    void Peek()
    {
        float inputVertical = inputManager.ReturnInputValue(inputManager.vertical);

        // �������ɓ��͂��󂯕t���Ă���Ƃ�
        if (inputVertical >= 0f)
        {
            targetPosition = Vector3.Lerp(stayPosition, peekPosition, inputVertical);
            targetRotation = Vector3.Lerp(stayRotation, peekRotation, inputVertical);
        }
        // �������ɓ��͂��󂯕t���Ă���Ƃ�
        else if (inputVertical < 0f)
        {
            targetPosition = Vector3.Lerp(stayPosition, crouchPosition, Mathf.Abs(inputVertical));
            targetRotation = Vector3.Lerp(stayRotation, crouchRotation, Mathf.Abs(inputVertical));
        }

        // �ڕW�p�����[�^�Ɍ����Ēǐ�
        nowPosition += (targetPosition - nowPosition) * (chasePower * Time.deltaTime);
        nowRotation += (targetRotation - nowRotation) * (chasePower * Time.deltaTime);

        // �K�p
        transform.position = nowPosition;
        transform.rotation = Quaternion.Euler(nowRotation);
    }
}
