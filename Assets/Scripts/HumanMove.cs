using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class HumanMove : MonoBehaviour
{
    private NavMeshAgent agent;

    public List<Transform> waypoints = new();
    public float moveInterval = 3f; // 何秒ごとに移動するか
    private int currentIndex;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // "ground"タグのオブジェクトを取得して追加
        foreach (GameObject ground in GameObject.FindGameObjectsWithTag("ground"))
        {
            waypoints.Add(ground.transform);
        }

        // 最初の目的地へ
        MoveToNextPoint();

        // 一定間隔で移動する処理を開始
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
