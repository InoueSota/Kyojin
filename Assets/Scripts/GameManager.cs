using UnityEngine;

public class GameManager : MonoBehaviour
{
    // My Component
    private InputManager inputManager;

    void Start()
    {
        // Set Component
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        // “ü—Íó‹µ‚ğæ“¾‚·‚é
        inputManager.GetAllInput();
    }

    void LateUpdate()
    {
        inputManager.SetIsGetInput();
    }
}
