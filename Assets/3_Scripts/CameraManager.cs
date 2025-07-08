using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Result Camera")]
    [SerializeField] private Vector3 resultCameraPosition;
    [SerializeField] private Vector3 resultCameraRotation;
    [SerializeField] private Vector3 resultTargetCameraRotation;

    [Header("Chase Parameter")]
    [SerializeField] private float chasePower;
    private Vector3 targetPosition;
    private Vector3 targetRotation;
    private Vector3 nowPosition;
    private Vector3 nowRotation;

    [Header("Wipes")]
    [SerializeField] private Image backWipeFrame;
    [SerializeField] private Image wipeFrame;
    [SerializeField] private RawImage wipe;

    [Header("Leak Out")]
    [SerializeField] private RawImage saturatedLineImage;
    private HumanManager[] humanManagers;
    private int humanCount;
    private float minDoubtTimer;

    // Flag
    private bool isLeakingOut;
    private bool isFinishCount;

    void Awake()
    {
        humanManagers = new HumanManager[50];
    }
    void Start()
    {
        // Get Other Component
        gameManager = gameManagerObj.GetComponent<GameManager>();
        inputManager = gameManagerObj.GetComponent<InputManager>();

        // Set Parameter
        targetPosition = stayPosition;
        targetRotation = stayRotation;
        nowPosition = new(0.2519214f, 14.96579f, -28.43637f);
        nowRotation = new(16.265f, -0.307f, -1.042f);
    }

    void Update()
    {
        // 入力状況を取得する
        inputManager.GetAllInput();

        // ゲーム中のみの処理
        if (gameManager.GetIsStart() && !gameManager.GetIsFinish())
        {
            if (!isLeakingOut)
            {
                // 覗き
                CameraMove();
                // バレているか
                CheckLeakOut();
            }
            // バレた
            else { LeakOut(); }
        }

        // ゲーム終了後の処理
        if (gameManager.GetIsFinish() && gameManager.GetIsFinishDarkAnimation())
        {
            Result();
        }

        // Cameraの移動が終了したあとの処理
        if (gameManager.GetIsFinishAnimation())
        {
            // 目標座標／目標角度に向けて動く
            Chase();
        }
    }

    void CameraMove()
    {
        float inputVertical = inputManager.ReturnInputValue(inputManager.vertical);

        // 正方向に入力を受け付けているとき
        if (inputVertical >= 0f)
        {
            targetPosition = Vector3.Lerp(stayPosition, peekPosition, inputVertical);
            targetRotation = Vector3.Lerp(stayRotation, peekRotation, inputVertical);

            // Wipeの透明処理
            backWipeFrame.color = new(backWipeFrame.color.r, backWipeFrame.color.g, backWipeFrame.color.b, 1f);
            wipeFrame.color = new(wipeFrame.color.r, wipeFrame.color.g, wipeFrame.color.b, 1f);
            wipe.color = new(wipe.color.r, wipe.color.g, wipe.color.b, 1f);
        }
        // 負方向に入力を受け付けているとき
        else if (inputVertical < 0f)
        {
            targetPosition = Vector3.Lerp(stayPosition, crouchPosition, Mathf.Abs(inputVertical));
            targetRotation = Vector3.Lerp(stayRotation, crouchRotation, Mathf.Abs(inputVertical));

            // Wipeの透明処理
            backWipeFrame.color = new(backWipeFrame.color.r, backWipeFrame.color.g, backWipeFrame.color.b, inputVertical + 1f);
            wipeFrame.color = new(wipeFrame.color.r, wipeFrame.color.g, wipeFrame.color.b, inputVertical + 1f);
            wipe.color = new(wipe.color.r, wipe.color.g, wipe.color.b, inputVertical + 1f);
        }
    }
    void Chase()
    {
        // 目標パラメータに向けて追跡
        nowPosition += (targetPosition - nowPosition) * (chasePower * Time.deltaTime);
        nowRotation += (targetRotation - nowRotation) * (chasePower * Time.deltaTime);

        // 適用
        transform.position = nowPosition;
        transform.rotation = Quaternion.Euler(nowRotation);
    }
    void Result()
    {
        if (isFinishCount)
        {
            targetRotation = resultTargetCameraRotation;
        }
        else
        {
            targetPosition = resultCameraPosition;
            targetRotation = resultCameraRotation;
        }
    }

    void CheckLeakOut()
    {
        bool noDoubt = true;

        for (int i = 0; i < humanCount + 1; i++)
        {
            if (humanManagers[i] != null && humanManagers[i].GetStatus() == HumanManager.Status.DOUBT && humanManagers[i].GetIsRecognizedCamera())
            {
                // 未代入 || 現在の最小値よりも下回るタイマーの場合
                if (minDoubtTimer == 0f || humanManagers[i].GetDoubtTimer() < minDoubtTimer)
                {
                    // 現在の最小値を更新する
                    minDoubtTimer = humanManagers[i].GetDoubtTimer();
                }
                // Doubt発見
                noDoubt = false;
            }
            else if (humanManagers[i] != null && humanManagers[i].GetStatus() == HumanManager.Status.FIND)
            {
                isLeakingOut = true;
            }
        }

        if (!noDoubt)
        {
            saturatedLineImage.color = new(1f - minDoubtTimer / 3f, saturatedLineImage.color.g, saturatedLineImage.color.b, (1f - minDoubtTimer / 3f) / 2f + 0.5f);
        }
        else
        {
            saturatedLineImage.color = new(0f, saturatedLineImage.color.g, saturatedLineImage.color.b, 0.5f);
            minDoubtTimer = 0f;
        }
    }
    void LeakOut()
    {
        // GameManager
        if (!gameManager.GetIsGameOver()) { gameManager.SetIsGameOver(true); }

        // 最覗きにする
        targetPosition = Vector3.Lerp(stayPosition, peekPosition, 1f);
        targetRotation = Vector3.Lerp(stayRotation, peekRotation, 1f);
        // 集中線は真っ赤にする
        saturatedLineImage.color = new(1f, saturatedLineImage.color.g, saturatedLineImage.color.b, 1f);
        // Wipeの透明処理
        backWipeFrame.color = new(backWipeFrame.color.r, backWipeFrame.color.g, backWipeFrame.color.b, 1f);
        wipeFrame.color = new(wipeFrame.color.r, wipeFrame.color.g, wipeFrame.color.b, 1f);
        wipe.color = new(wipe.color.r, wipe.color.g, wipe.color.b, 1f);
    }

    // Setter
    public void ShakeStart()
    {
        transform.parent.DOShakePosition(0.3f, 0.3f, 15, 1, false, true);
    }
    public void SetHumanManagers(HumanManager _humanManager)
    {
        int index = 0;
        bool isFinishSetting = false;

        while (!isFinishSetting)
        {
            if (humanManagers[index] == null)
            {
                humanManagers[index] = _humanManager;
                humanCount = index;
                isFinishSetting = true;
            }
            index++;
        }
    }
    public void SetIsFinishCount(bool _isFinishCount) { isFinishCount = _isFinishCount; }
}
