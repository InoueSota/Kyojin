using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class HumanMove : MonoBehaviour
{
    private NavMeshAgent agent;

    public List<Transform> waypoints = new();
    public float moveInterval = 3f; // ���b���ƂɈړ����邩
    private int currentIndex;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // "ground"�^�O�̃I�u�W�F�N�g���擾���Ēǉ�
        foreach (GameObject ground in GameObject.FindGameObjectsWithTag("ground"))
        {
            waypoints.Add(ground.transform);
        }

        // �ŏ��̖ړI�n��
        MoveToNextPoint();

        // ���Ԋu�ňړ����鏈�����J�n
        InvokeRepeating(nameof(MoveToNextPoint), moveInterval, moveInterval);
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(-90, -90, transform.rotation.y);
    }

    void MoveToNextPoint()
    {
        if (waypoints.Count == 0) return;

        currentIndex = Random.Range(0, waypoints.Count);
        agent.SetDestination(waypoints[currentIndex].position);
    }
}
