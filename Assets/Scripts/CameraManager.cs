using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Other Objects")]
    [SerializeField] private GameObject gameManagerObj;
    private InputManager inputManager;

    [Header("Lerp Camera Position")]
    [SerializeField] private Vector3 stayPosition;
    [SerializeField] private Vector3 peekPosition;

    [Header("Lerp Camera Rotation")]
    [SerializeField] private Vector3 stayRotation;
    [SerializeField] private Vector3 peekRotation;

    void Start()
    {
        // Get Other Component
        inputManager = gameManagerObj.GetComponent<InputManager>();
    }

    void Update()
    {
        // ���͏󋵂��擾����
        inputManager.GetAllInput();

        // �`��
        Peek();
    }

    void Peek()
    {
        float inputVertical = inputManager.ReturnInputValue(inputManager.vertical);

        // �������ɓ��͂��󂯕t���Ă���Ƃ�
        if (inputVertical >= 0f)
        {
            transform.position = Vector3.Lerp(stayPosition, peekPosition, inputVertical);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(stayRotation), Quaternion.Euler(peekRotation), inputVertical);
        }
    }
}
