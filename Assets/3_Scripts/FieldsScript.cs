using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Material brightRed;
    [SerializeField] private float countIntervalTime;
    private float countIntervalTimer;
    private int countNum;
    private bool canCount;

  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        for (int i = 0; i < createNum && availableGrounds.Count > 0; i++)
        {
            rand = Random.Range(0, 12);

            if (rand <= 6)
            {
                int index = Random.Range(0, availableGrounds.Count);
                Vector3 spawnPos = availableGrounds[index].transform.position;
                availableGrounds.RemoveAt(index); // 重複防止

                GameObject human = Instantiate(Human, spawnPos, Quaternion.identity);
                human.transform.parent = GameObject.Find("Fields Characters").transform;
            }else if(rand>6&&rand <= 9)
            {
                int index = Random.Range(0, availableRoad.Count);
                Vector3 spwnPos = availableRoad[index].transform.position;
                availableRoad.RemoveAt(index);

                GameObject car = Instantiate(Car, spwnPos, Quaternion.identity);

                car.transform.parent = GameObject.Find("Fields Characters").transform;
            }
            else
            {               
                GameObject plane = Instantiate(Plane);

                plane.transform.parent = GameObject.Find("Fields Characters").transform;
            }
        }


        //米くする
        

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Obj_RandomColor obj in GameObject.FindObjectsOfType<Obj_RandomColor>())
        {
            if (obj.isRed && !RedList.Contains(obj.gameObject))
            {
                //Debug.Log("赤色でリストに未登録： " + obj.gameObject.name);
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
                // カウントに対応したGameObjectのMaterialを変更する
                RedList[countNum].GetComponent<MeshRenderer>().material = brightRed;

                // カウントを次に進める
                countNum++;

                // インターバルの再設定
                countIntervalTimer = countIntervalTime;
            }
        }
    }

    // Setter
    public void SetCanCount(bool _canCount) { canCount = _canCount; }
}
