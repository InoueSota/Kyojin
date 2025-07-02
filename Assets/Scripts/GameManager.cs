using UnityEngine;

public class GameManager : MonoBehaviour
{
    // My Component
    private InputManager inputManager;

    [Header("Game Parameter")]
    [SerializeField] private float maxTime;
    private float remainingTime;
    private int discoveryCount;

    void Start()
    {
        // Set Component
        inputManager = GetComponent<InputManager>();

        // Set Parameter
        remainingTime = maxTime;
    }

    void Update()
    {
        // “ü—Íó‹µ‚ğæ“¾‚·‚é
        inputManager.GetAllInput();
    }
    void RemainingTime()
    {
        remainingTime -= Time.deltaTime;
    }

    void LateUpdate()
    {
        inputManager.SetIsGetInput();
    }
}
