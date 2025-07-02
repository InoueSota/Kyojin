using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class CarMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private int currentIndex = 0;

    public List<Transform> waypoints = new(); // ← List型に変更

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // "road"タグの付いたオブジェクトのTransformをwaypointsに追加
        foreach (GameObject road in GameObject.FindGameObjectsWithTag("road"))
        {
            waypoints.Add(road.transform);
        }

        // 最初の目的地を設定
        if (waypoints.Count > 0)
        {
            agent.SetDestination(waypoints[0].position);
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && waypoints.Count > 0)
        {
            //currentIndex = (currentIndex + 1) % waypoints.Count;
            currentIndex = Random.Range(0, waypoints.Count);

            agent.SetDestination(waypoints[currentIndex].position);
        }
    }
}
