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
        Debug.Log(Grounds.Count);
        for (int i = 0; i < createNum; i++)
        {
            if (Random.Range(0, 10) <= 6)
            {
                GameObject human = Instantiate(Human);
                human.transform.position = Grounds[Random.Range(0, Grounds.Count)].transform.position;
                Debug.Log(Random.Range(0, Grounds.Count));
                //for (int retry = 0; retry < maxRetryCount; retry++)
                //{
                //    //�����ꏊ������������
                //    if (Physics.CheckSphere(human.transform.position, 0.1f, obstacleLayer))
                //    {
                //        Debug.Log("�݂����I�I�I�I");
                //        //human.transform.position = new Vector3(Random.Range(-15, 15), 0.0f, Random.Range(-15, 15));
                //        human.transform.position = Grounds[Random.Range(0, Grounds.Count)].transform.position;

                //    }
                //    else
                //    {
                //        break;
                //    }
                //}
                human.transform.parent = GameObject.Find("Fields Characters").transform;
            }
            else
            {
                GameObject car = Instantiate(Car);
                car.transform.position = Roads[Random.Range(0, Roads.Count)].transform.position + Vector3.up*0.1f;
                car.transform.parent = GameObject.Find("Fields Characters").transform;

            }
        }

       
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
    }
}
