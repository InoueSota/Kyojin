using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class HumanMove : MonoBehaviour
{
    private NavMeshAgent agent;
    FieldsScript fields;
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
        fields = GameObject.Find("Fields Characters").GetComponent<FieldsScript>();
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
        if (fields.GetCanCount()== false)
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
        else if (agent.isOnNavMesh)
        {
            // ���ړ���~������ǉ���
            agent.ResetPath(); // NavMeshAgent�̖ړI�n���N���A
            agent.velocity = Vector3.zero; // ���x���[���ɂ��Ċ��S��~
        }
    }
    void MoveToNextPoint()
    {
        if (waypoints.Count == 0) return;

        if (agent.isOnNavMesh)
        {
            currentIndex = Random.Range(0, waypoints.Count);
            agent.SetDestination(waypoints[currentIndex].position);
        }
    }
}
