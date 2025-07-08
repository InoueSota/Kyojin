using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    // My Component
    private InputManager inputManager;

    [Header("Animator")]
    [SerializeField] private Animator darkAnimator;

    [Header("Interval Parameter")]
    [SerializeField] private float pushAIntervalTime;
    private float pushAIntervalTimer;

    // Flag
    private bool isChangeScene;

    void Start()
    {
        // Set Component
        inputManager = GetComponent<InputManager>();

        // Set Variables
        pushAIntervalTimer = pushAIntervalTime;
    }

    void Update()
    {
        // ���͏󋵂��擾����
        inputManager.GetAllInput();

        // �V�[���J�ڏ���
        ChangeScene();
    }
    void ChangeScene()
    {
        pushAIntervalTimer -= Time.deltaTime;

        if (!isChangeScene && pushAIntervalTimer <= 0f && inputManager.IsTrgger(inputManager.a))
        {
            darkAnimator.SetTrigger("StartFadeIn");

            isChangeScene = true;
        }
    }

    void LateUpdate()
    {
        inputManager.SetIsGetInput();
    }
}
