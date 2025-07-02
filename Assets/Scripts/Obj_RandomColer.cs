using UnityEngine;

public class Obj_RandomColor : MonoBehaviour
{
    private Material material;

    void Start()
    {
        // Rendererからマテリアルを取得してインスタンス化（他オブジェクトと共有しないように）
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material; // これで新しいインスタンスが生成される
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
