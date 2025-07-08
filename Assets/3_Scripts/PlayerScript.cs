using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [Header("ワイプ")]
    [SerializeField] RawImage Wipe;

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
        Wipe = GameObject.Find("Wipe").GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        float stickInput = Input.GetAxis("Vertical"); // 上:+1、下:-1、ニュートラル:0

        switch (mode)
        {
            case Mode.Title:
                if (gameManager.GetIsStartAnimation())
                {
                    transform.DOMove(new Vector3(-1, 5.5f, -19), easeStartTime).SetEase(easeStart).OnComplete(() =>
                    {
                        mode = Mode.Game;
                        animator.SetTrigger("isStart");
                        transform.DOMoveY(7, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
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
               

                // -1 ~ +1 → 0 ~ 1 に変換（逆転させたいなら 1 - で反転）
                float normalizedTime = (stickInput);
                stickInput = Mathf.Clamp(stickInput, 0, 100);

                // アニメーションのその位置へジャンプ
                animator.Play("START", 0, normalizedTime);
                transform.position = OriginPos + new Vector3(0, stickInput * 5, 0);


                if (Input.GetAxis("Vertical") < 0)
                {
                    Debug.Log("上見てるにょ");
                    transform.rotation = Quaternion.Euler(Input.GetAxis("Vertical")*-20, 180f, 0f);
                    //ワイプを半透明に
                    Color color = Wipe.color;  
                    color.a = 0.5f;            
                    Wipe.color = color;        

                }
                else
                {
                    //ワイプを元に戻す
                    Color color = Wipe.color;
                    color.a = 1.0f;
                    Wipe.color = color;

                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }

               
                break;
            case Mode.Clear:

                break;
            case Mode.Over:

                break;
        }
    }
}
