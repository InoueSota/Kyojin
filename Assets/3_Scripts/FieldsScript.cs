using System.Collections.Generic;
using UnityEngine;

public class FieldsScript : MonoBehaviour
{
    [Header("�����f�[�^")]
    [SerializeField] int RandomMinNum;
    [SerializeField] int RandomMaxNum;
    [SerializeField] LayerMask obstacleLayer; // �� Inspector�ŁuRoad�v�⌚���Ȃǂ�Layer���w��
    [SerializeField] int maxRetryCount = 10;  // �ő僊�g���C�񐔁i�������[�v�h�~�j

    [Header("�����I�u�W�F�N�g")]
    [SerializeField] GameObject Human;
    [SerializeField] GameObject Car;

    [Header("�ԐF�I�u�W�F�N�g")]
    [SerializeField] public int RedCount;
    [SerializeField] List<GameObject> RedList = new List<GameObject>();

    [Header("�t�B�[���h�I�u�W�F�N�g")]
    [SerializeField] List<GameObject> Grounds = new List<GameObject>();
    [SerializeField] List<GameObject> Roads = new List<GameObject>();

  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject ground in GameObject.FindGameObjectsWithTag("ground"))
        {
            Grounds.Add(ground);
        }
        foreach (GameObject road in GameObject.FindGameObjectsWithTag("road"))
        {
            Roads.Add(road);
        }

        int createNum = Random.Range(RandomMinNum, RandomMaxNum);

        for (int i = 0; i < createNum; i++)
        {
            if (Random.Range(0, 10) <= 6)
            {
                GameObject human = Instantiate(Human);
                human.transform.position = new Vector3(Random.Range(-15,15),1, Random.Range(-15, 15));
                for (int retry = 0; retry < maxRetryCount; retry++)
                {
                    //�����ꏊ������������
                    if (Physics.CheckSphere(human.transform.position, 2.5f, obstacleLayer))
                    {
                        Debug.Log("�݂����I�I�I�I");
                        human.transform.position = new Vector3(Random.Range(-15, 15), 0.0f, Random.Range(-15, 15));

                    }
                    else
                    {
                        break;
                    }
                }
                human.transform.parent = GameObject.Find("Fields Characters").transform;
            }
            else
            {
                GameObject car = Instantiate(Car);
                car.transform.position = Roads[Random.Range(0, Roads.Count)].transform.position + Vector3.up*0.1f;
                car.transform.parent = GameObject.Find("Fields Characters").transform;

            }
        }

        foreach (Obj_RandomColor obj in GameObject.FindObjectsOfType<Obj_RandomColor>())
        {
            // obj.gameObject �ɃA�N�Z�X�\
            
            if (obj.isRed)
            {
                //�ԐF�������烊�X�g�ɒǉ�
                RedCount++;
                RedList.Add(obj.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
