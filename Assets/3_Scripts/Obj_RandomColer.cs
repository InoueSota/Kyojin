using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Obj_RandomColor : MonoBehaviour
{

    [SerializeField] List<Material> materials = new List<Material>();
    public Material material;
    public bool isRed;
    int num;

    [SerializeField] public Vector3 origineScale;

    public bool isOne;
    float coolTime;

   
    [HideInInspector] public float threshold;      // �ʂ������l
    [HideInInspector] public float scaleFactor;    // �ʊg��{��
    public float currentScale = 1f;  // �g���ԁi�ێ��j


    [SerializeField] int randomNum;
    void Start()
    {
        origineScale = transform.localScale;
        num = Random.Range(0, 100);

        //�ԐF�ɂ���
        if (0 <= num && num <= 20)
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
        // �����_���������i�͈͂͒����\�j
        threshold = Random.Range(0.04f, 0.15f);
        scaleFactor = Random.Range(1.2f, 7.0f);
    }

    void Update()
    {
        //if (randomNum <= 10 && coolTime <= 0)
        //{
        //    coolTime = 2.0f;
        //    if (isOne == false)
        //    {
        //        isOne = true;
        //        transform.DOScaleY(origineScale.y + 1.5f, 1.0f).SetEase(Ease.InElastic);
        //    }

        //}


        //if (coolTime <= 0)
        //{
        //    if (isOne == true)
        //    {
        //        Debug.Log("�����O");
        //        transform.DOScaleY(origineScale.y, 1.0f).SetEase(Ease.InElastic);
        //        isOne = false;

        //    }
        //    else
        //    {
        //        transform.localScale = origineScale;

        //    }
        //    coolTime = 1.0f;

        //}
        //randomNum = Random.Range(0, 100);
        //coolTime -= Time.deltaTime;

    }
}
