using System.Collections.Generic;
using UnityEngine;

public class MEDAMAScript : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveRange = 0.02f;
    [SerializeField] Vector3 eyeCenter;

    [SerializeField] List<GameObject> gameObjects = new List<GameObject>();
    [SerializeField] float coolTime;

    private void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("obj"))
        {
            gameObjects.Add(obj);
        }
    }

    void Update()
    {
        if (target == null || gameObjects.Count == 0) return;

        coolTime += Time.deltaTime;
        if (coolTime >= 3.0f)
        {
            target = gameObjects[Random.Range(0, gameObjects.Count)].transform;
            coolTime = 0;
        }

        // 目の中心からターゲット方向を計算
        Vector3 dir = (target.position - eyeCenter).normalized;

        // XZ平面に限定（またはXZでなくXYでもOK）
        Vector3 localOffset = new Vector3(dir.x, dir.y, 0) * moveRange;

        // 黒目をローカル座標で移動
        transform.localPosition = Vector3.Lerp(transform.localPosition, eyeCenter+ localOffset, Time.deltaTime ); // 速度調整したければ5fなどを調整
    }
}
