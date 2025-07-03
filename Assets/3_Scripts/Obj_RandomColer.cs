using UnityEngine;

public class Obj_RandomColor : MonoBehaviour
{
    private Material material;

    void Start()
    {
        // Renderer����}�e���A�����擾���ăC���X�^���X���i���I�u�W�F�N�g�Ƌ��L���Ȃ��悤�Ɂj
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material; // ����ŐV�����C���X�^���X�����������
        }

        if (material != null)
        {
            material.color = Random.ColorHSV();
        }

    }

    void Update()
    {
       
    }
}
