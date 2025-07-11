using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FieldsScript : MonoBehaviour
{
    [Header("生成データ")]
    [SerializeField] int RandomMinNum;
    [SerializeField] int RandomMaxNum;
    [SerializeField] int rand;
    [SerializeField] LayerMask obstacleLayer; // ← Inspectorで「Road」や建物などのLayerを指定
    [SerializeField] int maxRetryCount = 10;  // 最大リトライ回数（無限ループ防止）

    [Header("生成オブジェクト")]
    [SerializeField] GameObject Human;
    [SerializeField] GameObject Car;
    [SerializeField] GameObject Plane;

    [Header("赤色オブジェクト")]
    [SerializeField] public int RedCount;
    [SerializeField] List<GameObject> RedList = new List<GameObject>();

    [Header("フィールドオブジェクト")]
    [SerializeField] List<GameObject> Grounds = new List<GameObject>();
    [SerializeField] List<GameObject> Roads = new List<GameObject>();

    [Header("リザルト関係")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Material brightRed;
    [SerializeField] private float countIntervalTime;
    [SerializeField] private AudioClip resultCountClip;
    private float countIntervalTimer;
    private int countNum;
    [SerializeField] private bool canCount;
    [Header("音関係")]
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] float beatSpeed = 2f; // 拍の速さ（例：2なら1秒に2回ふくらむ）
    [SerializeField] float waveScale = 0.3f;
    float[] spectrum = new float[64];
    [SerializeField] float threshold = 0.01f; // 反応する最小音量（しきい値）
    [SerializeField] float scaleFactor = 0.3f;
    [SerializeField] float decaySpeed = 2f;
    float currentScale = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicAudioSource = GameObject.Find("GameManager").GetComponent<AudioSource>();

        foreach (GameObject ground in GameObject.FindGameObjectsWithTag("ground"))
        {
            Grounds.Add(ground);
            ground.transform.parent = GameObject.Find("FieldsGround").transform;

        }
        foreach (GameObject road in GameObject.FindGameObjectsWithTag("road"))
        {
            Roads.Add(road);
        }

        int createNum = Random.Range(RandomMinNum, RandomMaxNum);
        Debug.Log(Grounds.Count);
        List<GameObject> availableGrounds = new List<GameObject>(Grounds);
        List<GameObject> availableRoad = new List<GameObject>(Roads);

        for (int i = 0; i < createNum; i++)
        {




            //rand = Random.Range(0, 12);

            //if (rand <= 6 && availableGrounds.Count > 0)
            //{
            //    int index = Random.Range(0, availableGrounds.Count);
            //    Vector3 spawnPos =/* availableGrounds[index].transform.position*/new Vector3(0,15,0);
            //    availableGrounds.RemoveAt(index);


            //    GameObject human = Instantiate(Human, new Vector3(0, 15, 0), Quaternion.identity);
            //    human.transform.localScale = Vector3.one*2;
            //    human.SetActive(true);

            //    foreach (var renderer in human.GetComponentsInChildren<Renderer>())
            //    {
            //        renderer.enabled = true;
            //        if (renderer.material != null)
            //        {
            //            Color c = renderer.material.color;
            //            c.a = 1f; // Alphaを強制
            //            renderer.material.color = c;
            //        }
            //    }
            //}
            //else if (rand > 6 && rand <= 9)
            //{
            //    if (availableRoad.Count > 0)
            //    {
            //        int index = Random.Range(0, availableRoad.Count);
            //        Vector3 spwnPos = availableRoad[index].transform.position;
            //        availableRoad.RemoveAt(index);

            //        GameObject car = Instantiate(Car, spwnPos, Quaternion.identity);
            //        //car.transform.parent = GameObject.Find("Fields Characters").transform;
            //    }
            //    else
            //    {
            //        Debug.LogWarning("availableRoadが空なのでCarを生成できませんでした");
            //        continue;
            //    }
            //}
            //else
            //{
            //    if (Plane != null)
            //    {
            //        GameObject plane = Instantiate(Plane);
            //        //plane.transform.parent = GameObject.Find("Fields Characters").transform;
            //    }
            //    else
            //    {
            //        Debug.LogWarning("Planeプレハブが設定されていません");
            //    }
            //}
        }

        //米くする


    }

    // Update is called once per frame
    void Update()
    {
        if (canCount == false)
        {
            musicAudioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
            float volume = spectrum[1] + spectrum[2] + spectrum[3]; // 低音帯域（調整可）

            foreach (Obj_RandomColor obj in GameObject.FindObjectsOfType<Obj_RandomColor>())
            {
                if (volume > obj.threshold && obj.currentScale <= 1.01f)
                {
                    obj.currentScale = 1f + volume * obj.scaleFactor;
                }
                else
                {
                    obj.currentScale = Mathf.Lerp(obj.currentScale, 1f, Time.deltaTime * 3f); // 元に戻る
                }

                obj.transform.localScale = obj.origineScale * obj.currentScale;
            }

        }
        //musicAudioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        //// 低音成分から音量を取得（index 1〜4くらいが低音）
        //float volume = spectrum[1] + spectrum[2] + spectrum[3];

        //if (volume > threshold)
        //{
        //    currentScale = 1f + volume * scaleFactor;
        //}
        //else
        //{
        //    // 徐々に元の大きさに戻す
        //    currentScale = Mathf.Lerp(currentScale, 1f, Time.deltaTime * decaySpeed);
        //}

        //foreach (Obj_RandomColor obj in GameObject.FindObjectsOfType<Obj_RandomColor>())
        //{
        //    obj.transform.localScale = obj.origineScale * currentScale;
        //}

        // 赤色検知
        foreach (Obj_RandomColor obj in GameObject.FindObjectsOfType<Obj_RandomColor>())
        {
            if (obj.isRed && !RedList.Contains(obj.gameObject))
            {
                RedList.Add(obj.gameObject);
            }
        }

        RedCount = RedList.Count;
        Result();
    }

    void Result()
    {
        if (canCount)
        {
            // インターバルの更新
            countIntervalTimer -= Time.deltaTime;

            if (countIntervalTimer <= 0f && countNum < RedList.Count)
            {
                RedList[countNum].GetComponent<MeshRenderer>().material = brightRed;
                Transform target = RedList[countNum].transform;
                target.GetComponent<MeshRenderer>().material = brightRed;

                
                var agent = RedList[countNum].GetComponent<NavMeshAgent>();
                if (agent != null) agent.enabled = false;

                // ★グリッド表示のためのパラメータ
                int columns = 5; // 1行に5つ
                float spacingX = 130f; // 横の間隔（ピクセル）
                float spacingY = 130f; // 縦の間隔（ピクセル）
                float startX = 450f; // 左からの位置
                float startY = Screen.height - 300f; // 上からの位置（Top揃え）

                int row = countNum / columns;
                int col = countNum % columns;

                float posX = startX + col * spacingX;
                float posY = startY - row * spacingY;

                Vector3 screenPos = new Vector3(posX, posY, 10f);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

                target.DOMove(worldPos, 0.5f)
                    .SetEase(Ease.InOutQuint)
                .OnComplete(() => {
                        target.parent = Camera.main.transform;
                });

                target.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuint);

                // このあとでカウント++
                countNum++;
                musicAudioSource.PlayOneShot(resultCountClip);
                gameManager.SetTrueCount(countNum);
                countIntervalTimer = countIntervalTime;
            }
            else if (countIntervalTimer <= 0f && countNum == RedList.Count)
            {
                // 最後のGameObjectのMaterialを変更したら
                cameraManager.SetIsFinishCount(true);
            }
        }
    }

    // Setter
    public void SetCanCount(bool _canCount) { canCount = _canCount; }
    public bool GetCanCount() { return canCount; }
}
