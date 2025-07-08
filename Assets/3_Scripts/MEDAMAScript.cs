using Unity.VisualScripting;
using UnityEngine;

public class MEDAMAScript : MonoBehaviour
{
    [SerializeField] Transform target;           // �ڂŒǂ������Ώہi�J�����Ȃǁj
    [SerializeField] float moveRange = 0.02f;    // ���ڂ������ő勗��
    [SerializeField] Transform eyeCenter;        // �ڂ̒��S�ʒu�i�e����_�j

    void Update()
    {
        if (target == null || eyeCenter == null) return;

        // �^�[�Q�b�g�����i���[���h�����[�J���j
        Vector3 localDir = eyeCenter.InverseTransformDirection((target.position - eyeCenter.position).normalized);

        // �͈͓��ɗ}���Ĉړ�
        transform.localPosition = localDir * moveRange;
    }
}
