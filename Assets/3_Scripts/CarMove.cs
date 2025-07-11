using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class CarMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private int currentIndex = 0;
    public List<Transform> waypoints = new();
    FieldsScript fields;

    void Start()
    {
        fields = GameObject.Find("Fields Characters").GetComponent<FieldsScript>();

        // NavMesh ��ɃX�i�b�v������
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogError("NavMesh��ɏ����ʒu��������܂���ł����I");
            return; // NavMesh�O�ɂ���̂ŏ������f
        }

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;

        foreach (GameObject road in GameObject.FindGameObjectsWithTag("road"))
        {
            waypoints.Add(road.transform);
        }

        if (waypoints.Count > 0)
            MoveToNext();
    }

    void Update()
    {
        if (fields.GetCanCount() == false)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f && waypoints.Count > 0)
            {
                MoveToNext();
            }
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

        }
        else
        {
            // ���ړ���~������ǉ���
            agent.ResetPath(); // NavMeshAgent�̖ړI�n���N���A
            agent.velocity = Vector3.zero; // ���x���[���ɂ��Ċ��S��~
        }

    }

    void MoveToNext()
    {
        currentIndex = Random.Range(0, waypoints.Count);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(waypoints[currentIndex].position, out hit, 15.0f, agent.areaMask))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning("NavMesh�O��Waypoint���I�΂�܂���");
        }
    }
}
