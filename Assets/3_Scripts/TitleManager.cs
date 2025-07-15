using UnityEngine;

public class TitleManager : MonoBehaviour
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

        // ƒQ[ƒ€ŠJnˆ—
        GameStart();

        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
    }
    void GameStart()
    {
        pushAIntervalTimer -= Time.deltaTime;

        if (!isChangeScene && pushAIntervalTimer <= 0f && inputManager.IsTrgger(inputManager.a))
        {
            int randomNum = Random.Range(0, 99);

            if (randomNum % 2 == 0) { darkAnimator.gameObject.GetComponent<FinishAnimation>().SetNextSceneName("Stage1"); }
            else { darkAnimator.gameObject.GetComponent<FinishAnimation>().SetNextSceneName("Stage2"); }

            darkAnimator.SetTrigger("StartFadeIn");

            isChangeScene = true;
        }
    }

    void LateUpdate()
    {
        inputManager.SetIsGetInput();
    }

}
