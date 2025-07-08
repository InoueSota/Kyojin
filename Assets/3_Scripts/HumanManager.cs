using UnityEngine;

public class HumanManager : MonoBehaviour
{
    private enum Status
    {
        NORMAL, // 通常状態
        DOUBT,  // 疑念
        FIND    // 発見
    }
    private Status status = Status.NORMAL;

    [Header("Normal Parameter")]
    [SerializeField] private float normalRandomRange;
    [SerializeField] private int normalRandomMin;
    [SerializeField] private int normalRandomMax;
    private float normalTimer;

    [Header("Doubt Parameter")]
    [SerializeField] private float doubtTime;
    private float doubtTimer;

    [Header("Doubt Effect")]
    [SerializeField] private GameObject effectObj;
    [SerializeField] private Sprite doubtSprite;

    [Header("Find Effect")]
    [SerializeField] private Sprite findSprite;

    // Flag
    private bool isRecognizedCamera;

    // Other Transforms
    private Transform mainCameraTransform;

    // Other Components
    private GameManager gameManager;

    void Start()
    {
        // Variables Initialize
        normalTimer = Random.Range(normalRandomMin, normalRandomMax) * normalRandomRange;

        // Set Other Transforms
        mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        // Set Other Components
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.GetIsStart())
        {
            // カメラに見られているか判定
            CheckCamera();

            switch (status)
            {
                case Status.NORMAL:

                    // カメラ認識
                    if (isRecognizedCamera)
                    {
                        // カウントダウン
                        normalTimer -= Time.deltaTime;

                        // タイマーが0になったら
                        if (normalTimer <= 0f)
                        {
                            // 表示するSpriteをクエスチョンマークに変更
                            effectObj.GetComponent<SpriteRenderer>().sprite = doubtSprite;

                            // タイマーの設定
                            doubtTimer = doubtTime;
                            // Statusを変更
                            status = Status.DOUBT;
                        }
                    }

                    break;
                case Status.DOUBT:

                    // カメラ認識
                    if (isRecognizedCamera)
                    {
                        // カウントダウン
                        doubtTimer -= Time.deltaTime;

                        // タイマーが0になったら
                        if (doubtTimer <= 0f)
                        {
                            // 表示するSpriteをエクスクラメーションマークに変更
                            effectObj.GetComponent<SpriteRenderer>().sprite = findSprite;

                            // Statusを変更
                            status = Status.FIND;
                        }
                    }
                    // カメラ非認識
                    else
                    {
                        // Spriteを非表示にする
                        effectObj.GetComponent<SpriteRenderer>().sprite = null;

                        // タイマーの設定
                        normalTimer = Random.Range(normalRandomMin, normalRandomMax) * normalRandomRange;
                        // Statusを変更
                        status = Status.NORMAL;
                    }

                    break;
                case Status.FIND:
                    break;
            }
            Debug.Log(status);
        }
    }
    void CheckCamera()
    {
        // Rayの方向
        Vector3 direction = mainCameraTransform.position - transform.position;
        // Rayを作成
        Ray ray = new Ray(transform.position, direction);

        // Raycastを生成
        RaycastHit hit;
        // 何らかのコライダーに衝突したら
        if (Physics.Raycast(ray, out hit))
        {
            // 衝突対象がMainCameraならカメラ認識フラグにはtrueが入る
            isRecognizedCamera = hit.collider.CompareTag("MainCamera");
        }
    }
}
