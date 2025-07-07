using DG.Tweening;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] Ease easeStart;
    [SerializeField] float easeStartTime;

    [SerializeField]
    enum Mode
    {
        Title,
        Start,
        Game,
        Clear,
        Over
    };

    [SerializeField] Mode mode;
    [SerializeField] Animator animator;
    [SerializeField] GameManager gameManager;
    Vector3 OriginPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case Mode.Title:
                if (gameManager.GetIsStartAnimation())
                {
                    transform.DOMove(new Vector3(-1, 5.5f, -19), easeStartTime).SetEase(easeStart).OnComplete(() =>
                    {
                        mode = Mode.Game;
                        animator.SetTrigger("isStart");
                        transform.DOMoveY(7, 1.0f).SetEase(Ease.OutQuad).OnComplete(() =>
                        {
                            OriginPos = new Vector3(transform.position.x, 7, transform.position.z);
                        });
                    });
                    //animator.set
                    
                }
                break;
            case Mode.Start:

                if (gameManager.GetIsStart())
                {
                    mode = Mode.Game;

                }
                break;
            case Mode.Game:
                float stickInput = Input.GetAxis("Vertical"); // 上:+1、下:-1、ニュートラル:0

                // -1 ~ +1 → 0 ~ 1 に変換（逆転させたいなら 1 - で反転）
                float normalizedTime = (stickInput);
                stickInput = Mathf.Clamp(stickInput, 0, 100);

                if (Input.GetAxis("Vertical") < 0)
                {
                    Debug.Log("上見てるにょ");
                    transform.rotation = Quaternion.Euler(Input.GetAxis("Vertical")*-20, 180f, 0f);

                }
                else
                {
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }

                // アニメーションのその位置へジャンプ
                animator.Play("START", 0, normalizedTime);
                transform.position = OriginPos + new Vector3(0, stickInput * 5, 0);
                break;
            case Mode.Clear:

                break;
            case Mode.Over:

                break;
        }
    }
}
