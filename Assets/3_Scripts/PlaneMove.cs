using UnityEngine;

public class PlaneMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float radius = 5f;
    Vector3 center;
    Vector3 axis;
    float angle;
    FieldsScript fields;

    void Start()
    {
        Debug.Log("飛行機だだだだだｄ");

        fields = GameObject.Find("Fields Characters").GetComponent<FieldsScript>();

        // 軌道の中心をランダムに決める
        center = transform.position = new Vector3(Random.Range(-30, 30), Random.Range(25, 30), Random.Range(-15, 30));

        // 回転軸をランダムに決める（上下・左右・斜めの回転など）
        axis = Random.onUnitSphere; // 単位ベクトルで3D方向ランダム

        angle = Random.Range(0f, 360f); // 角度初期化
        transform.position = center+Vector3.up*200;
    }

    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().GetIsStart())
        {
            if (fields.GetCanCount() == false)
            {

                angle += rotateSpeed * Time.deltaTime;

                // 回転行列を使ってぐるぐる回る
                Vector3 offset = Quaternion.AngleAxis(angle, axis) * Vector3.forward * radius;


                transform.position = Vector3.Lerp(transform.position, (center + offset), Time.deltaTime);

                // 前を向くように回転させる（optional）
                transform.rotation = Quaternion.LookRotation(Vector3.Cross(axis, offset).normalized, axis);
            }
        }

       
    }
}
