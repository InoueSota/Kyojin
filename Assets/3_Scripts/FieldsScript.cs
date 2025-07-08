using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldsScript : MonoBehaviour
{
    [Header("�����f�[�^")]
    [SerializeField] int RandomMinNum;
    [SerializeField] int RandomMaxNum;
    [SerializeField] int rand;
    [SerializeField] LayerMask obstacleLayer; // �� Inspector�ŁuRoad�v�⌚���Ȃǂ�Layer���w��
    [SerializeField] int maxRetryCount = 10;  // �ő僊�g���C�񐔁i�������[�v�h�~�j

    [Header("�����I�u�W�F�N�g")]
    [SerializeField] GameObject Human;
    [SerializeField] GameObject Car;
    [SerializeField] GameObject Plane;

    [Header("�ԐF�I�u�W�F�N�g")]
    [SerializeField] public int RedCount;
    [SerializeField] List<GameObject> RedList = new List<GameObject>();

    [Header("�t�B�[���h�I�u�W�F�N�g")]
    [SerializeField] List<GameObject> Grounds = new List<GameObject>();
    [SerializeField] List<GameObject> Roads = new List<GameObject>();

    [Header("���U���g�֌W")]
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
                availableGrounds.RemoveAt(index); // �d���h�~

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


        //�Ă�����
        

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Obj_RandomColor obj in GameObject.FindObjectsOfType<Obj_RandomColor>())
        {
            if (obj.isRed && !RedList.Contains(obj.gameObject))
            {
                //Debug.Log("�ԐF�Ń��X�g�ɖ��o�^�F " + obj.gameObject.name);
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
            // �C���^�[�o���̍X�V
            countIntervalTimer -= Time.deltaTime;

            if (countIntervalTimer <= 0f && countNum < RedList.Count)
            {
                // �J�E���g�ɑΉ�����GameObject��Material��ύX����
                RedList[countNum].GetComponent<MeshRenderer>().material = brightRed;

                // �J�E���g�����ɐi�߂�
                countNum++;

                // �C���^�[�o���̍Đݒ�
                countIntervalTimer = countIntervalTime;
            }
        }
    }

    // Setter
    public void SetCanCount(bool _canCount) { canCount = _canCount; }
}
