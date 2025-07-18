using UnityEngine;

public class LookCamera : MonoBehaviour
{
    // Other Transforms
    private Transform mainCameraTransform;

    void Start()
    {
        mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        transform.LookAt(mainCameraTransform);
    }
}
