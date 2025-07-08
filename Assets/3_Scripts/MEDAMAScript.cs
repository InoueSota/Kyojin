using Unity.VisualScripting;
using UnityEngine;

public class MEDAMAScript : MonoBehaviour
{
    [SerializeField] Transform target;           // 目で追いたい対象（カメラなど）
    [SerializeField] float moveRange = 0.02f;    // 黒目が動く最大距離
    [SerializeField] Transform eyeCenter;        // 目の中心位置（親か基準点）

    void Update()
    {
        if (target == null || eyeCenter == null) return;

        // ターゲット方向（ワールド→ローカル）
        Vector3 localDir = eyeCenter.InverseTransformDirection((target.position - eyeCenter.position).normalized);

        // 範囲内に抑えて移動
        transform.localPosition = localDir * moveRange;
    }
}
