using UnityEngine;

public class HumanManager : MonoBehaviour
{
    private enum Status
    {
        NORMAL, // �ʏ���
        DOUBT,  // �^�O
        FIND    // ����
    }
    private Status status = Status.NORMAL;

    [Header("Normal Parameter")]
    [SerializeField] private float normalRandomRange;
    [SerializeField] private int normalRandomMin;
    [SerializeField] private int normalRandomMax;
    private float normalTimer;

    [Header("Doubt Parameter")]
    [SerializeField] private float doubtTime;
    private float doubtTimer;

    [Header("Doubt Effect")]
    [SerializeField] private GameObject effectObj;
    [SerializeField] private Sprite doubtSprite;

    [Header("Find Effect")]
    [SerializeField] private Sprite findSprite;

    [Header("Sounds")]
    [SerializeField] private AudioClip doubtSE;
    [SerializeField] private AudioClip findSE;
    private AudioSource audioSource;

    // Flag
    private bool isRecognizedCamera;

    // Other Transforms
    private Transform mainCameraTransform;

    // Other Components
    private GameManager gameManager;

    void Start()
    {
        // Variables Initialize
        normalTimer = Random.Range(normalRandomMin, normalRandomMax) * normalRandomRange;

        // Set My Components
        audioSource = GetComponent<AudioSource>();

        // Set Other Transforms
        mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        // Set Other Components
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.GetIsStart())
        {
            // �J�����Ɍ����Ă��邩����
            CheckCamera();

            switch (status)
            {
                case Status.NORMAL:

                    // �J�����F��
                    if (isRecognizedCamera)
                    {
                        // �J�E���g�_�E��
                        normalTimer -= Time.deltaTime;

                        // �^�C�}�[��0�ɂȂ�����
                        if (normalTimer <= 0f)
                        {
                            ChangeStatus(Status.DOUBT);
                        }
                    }

                    break;
                case Status.DOUBT:

                    // �J�����F��
                    if (isRecognizedCamera)
                    {
                        // �J�E���g�_�E��
                        doubtTimer -= Time.deltaTime;

                        // �^�C�}�[��0�ɂȂ�����
                        if (doubtTimer <= 0f)
                        {
                            ChangeStatus(Status.FIND);
                        }
                    }
                    // �J������F��
                    else
                    {
                        ChangeStatus(Status.NORMAL);
                    }

                    break;
                case Status.FIND:
                    break;
            }
        }
    }
    void CheckCamera()
    {
        // Ray�̕���
        Vector3 direction = mainCameraTransform.position - transform.position;
        // Ray���쐬
        Ray ray = new Ray(transform.position, direction);

        // Raycast�𐶐�
        RaycastHit hit;
        // ���炩�̃R���C�_�[�ɏՓ˂�����
        if (Physics.Raycast(ray, out hit))
        {
            // �ՓˑΏۂ�MainCamera�Ȃ�J�����F���t���O�ɂ�true������
            isRecognizedCamera = hit.collider.CompareTag("MainCamera");
        }
    }
    void ChangeStatus(Status _status)
    {
        switch (_status)
        {
            case Status.NORMAL:

                // Sprite���\���ɂ���
                effectObj.GetComponent<SpriteRenderer>().sprite = null;
                // �^�C�}�[�̐ݒ�
                normalTimer = Random.Range(normalRandomMin, normalRandomMax) * normalRandomRange;

                break;
            case Status.DOUBT:

                // �\������Sprite���N�G�X�`�����}�[�N�ɕύX
                effectObj.GetComponent<SpriteRenderer>().sprite = doubtSprite;
                // �^�C�}�[�̐ݒ�
                doubtTimer = doubtTime;
                // �^�O����炷
                audioSource.PlayOneShot(doubtSE);

                break;
            case Status.FIND:

                // �\������Sprite���G�N�X�N�����[�V�����}�[�N�ɕύX
                effectObj.GetComponent<SpriteRenderer>().sprite = findSprite;
                // ��������炷
                audioSource.PlayOneShot(findSE);

                break;
        }

        // Status��ύX
        status = _status;
    }
}
