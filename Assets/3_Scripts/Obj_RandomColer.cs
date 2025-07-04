using System.Collections.Generic;
using UnityEngine;

public class Obj_RandomColor : MonoBehaviour
{

    [SerializeField] List<Material> materials = new List<Material>();
    public Material material;
    public bool isRed;
    int num;
    void Start()
    {
        num = Random.Range(0, 100);

        //�ԐF�ɂ���
        if (0 <= num && num <=20)
        {
            Debug.Log("�ԐF");
            material = materials[0];
            isRed = true;
        }
        else
        {
            //�ԐF�ȊO�������_����
            material = materials[Random.Range(1, materials.Count)];
        }

        GetComponent<MeshRenderer>().material = material;
       
    }

    void Update()
    {

    }
}
