using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class HumanMove : MonoBehaviour
{
    private NavMeshAgent agent;

    public List<Transform> waypoints = new();
    public float moveInterval = 0f; // ���b���ƂɈړ����邩
    private int currentIndex;
    Vector3 moveDirection = Vector3.zero;
    void Start()
    {
        //// NavMesh ��ɃX�i�b�v������
        //NavMeshHit hit;
        //if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
        //{
        //    transform.position = hit.position;
        //}
        //else
        //{
        //    Debug.LogError("NavMesh��ɏ����ʒu��������܂���ł����I");
        //    return; // NavMesh�O�ɂ���̂ŏ������f
        //}
        agent = GetComponent<NavMeshAgent>();

        // "ground"�^�O�̃I�u�W�F�N�g���擾���Ēǉ�
        foreach (GameObject ground in GameObject.FindGameObjectsWithTag("ground"))
        {
            waypoints.Add(ground.transform);
        }

        //// �ŏ��̖ړI�n��
        MoveToNextPoint();

        // ���Ԋu�ňړ����鏈�����J�n
        InvokeRepeating(nameof(MoveToNextPoint), moveInterval, moveInterval);
        moveDirection  =new Vector3( Random.Range(-1,1),0, Random.Range(-1, 1));
    }

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, 0.2f, LayerMask.GetMask("road")))
        {
            Debug.Log("�݂���a�I�I�I�I");
            MoveToNextPoint();
        }
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        transform.LookAt(Camera.main.transform);
        //transform.rotation = Quaternion.Euler(-90, -90, transform.rotation.z);
        //transform.position += 3 * moveDirection * Time.deltaTime;
    }

    void MoveToNextPoint()
    {
        if (waypoints.Count == 0) return;

        currentIndex = Random.Range(0, waypoints.Count);
        agent.SetDestination(waypoints[currentIndex].position);
    }
}
