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

        //赤色にする
        if (0 <= num && num <=20)
        {
            Debug.Log("赤色");
            material = materials[0];
            isRed = true;
        }
        else
        {
            //赤色以外をランダムで
            material = materials[Random.Range(1, materials.Count)];
        }

        GetComponent<MeshRenderer>().material = material;
       
    }

    void Update()
    {

    }
}
