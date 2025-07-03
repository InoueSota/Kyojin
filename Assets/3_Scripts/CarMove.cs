using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class CarMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private int currentIndex = 0;
    public List<Transform> waypoints = new();

    void Start()
    {
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
        if (!agent.pathPending && agent.remainingDistance < 0.5f && waypoints.Count > 0)
        {
            MoveToNext();
        }
        transform.rotation = Quaternion.Euler(-90, transform.rotation.y, transform.rotation.z);

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
            Debug.LogWarning("NavMeshŠO‚ÌWaypoint‚ª‘I‚Î‚ê‚Ü‚µ‚½");
        }
    }
}
