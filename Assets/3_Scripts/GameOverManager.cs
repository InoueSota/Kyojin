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
        // “ü—Íó‹µ‚ğæ“¾‚·‚é
        inputManager.GetAllInput();

        // ƒV[ƒ“‘JˆÚˆ—
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
