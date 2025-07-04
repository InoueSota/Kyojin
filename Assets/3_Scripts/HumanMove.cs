using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class HumanMove : MonoBehaviour
{
    private NavMeshAgent agent;

    public List<Transform> waypoints = new();
    public float moveInterval = 0f; // 何秒ごとに移動するか
    private int currentIndex;
    Vector3 moveDirection = Vector3.zero;
    void Start()
    {
        //// NavMesh 上にスナップさせる
        //NavMeshHit hit;
        //if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
        //{
        //    transform.position = hit.position;
        //}
        //else
        //{
        //    Debug.LogError("NavMesh上に初期位置が見つかりませんでした！");
        //    return; // NavMesh外にいるので処理中断
        //}
        agent = GetComponent<NavMeshAgent>();

        // "ground"タグのオブジェクトを取得して追加
        foreach (GameObject ground in GameObject.FindGameObjectsWithTag("ground"))
        {
            waypoints.Add(ground.transform);
        }

        //// 最初の目的地へ
        MoveToNextPoint();

        // 一定間隔で移動する処理を開始
        InvokeRepeating(nameof(MoveToNextPoint), moveInterval, moveInterval);
        moveDirection  =new Vector3( Random.Range(-1,1),0, Random.Range(-1, 1));
    }

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, 0.2f, LayerMask.GetMask("road")))
        {
            Debug.Log("みちだa！！！！");
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
