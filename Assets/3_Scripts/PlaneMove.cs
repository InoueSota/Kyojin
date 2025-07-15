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
        Debug.Log("��s�@������������");

        fields = GameObject.Find("Fields Characters").GetComponent<FieldsScript>();

        // �O���̒��S�������_���Ɍ��߂�
        center = transform.position = new Vector3(Random.Range(-30, 30), Random.Range(25, 30), Random.Range(-15, 30));

        // ��]���������_���Ɍ��߂�i�㉺�E���E�E�΂߂̉�]�Ȃǁj
        axis = Random.onUnitSphere; // �P�ʃx�N�g����3D���������_��

        angle = Random.Range(0f, 360f); // �p�x������
        transform.position = center+Vector3.up*200;
    }

    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().GetIsStart())
        {
            if (fields.GetCanCount() == false)
            {

                angle += rotateSpeed * Time.deltaTime;

                // ��]�s����g���Ă��邮����
                Vector3 offset = Quaternion.AngleAxis(angle, axis) * Vector3.forward * radius;


                transform.position = Vector3.Lerp(transform.position, (center + offset), Time.deltaTime);

                // �O�������悤�ɉ�]������ioptional�j
                transform.rotation = Quaternion.LookRotation(Vector3.Cross(axis, offset).normalized, axis);
            }
        }

       
    }
}
