using UnityEngine;

public class HumanManager : MonoBehaviour
{
    private enum Status
    {
        NORMAL, // í èÌèÛë‘
        DOUBT,  // ã^îO
        FIND    // î≠å©
    }
    private Status status = Status.NORMAL;

    [Header("Normal Parameter")]
    [SerializeField] private float normalRandomRange;
    [SerializeField] private int normalRandomMin;
    [SerializeField] private int normalRandomMax;
    private float normalTimer;

    [Header("Doubt Parameter")]
    [SerializeField] private float doubtRandomRange;
    [SerializeField] private int doubtRandomMin;
    [SerializeField] private int doubtRandomMax;

    [Header("Find Parameter")]
    [SerializeField] private float findTime;
    private float findTimer;

    // Other Components
    private GameManager gameManager;

    void Start()
    {
        // Variables Initialize
        normalTimer = Random.Range(normalRandomMin, normalRandomMax) * normalRandomRange;

        // Set Other Components
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.GetIsStart())
        {
            switch (status)
            {
                case Status.NORMAL:



                    break;
                case Status.DOUBT:
                    break;
                case Status.FIND:
                    break;
            }
        }
    }
}
