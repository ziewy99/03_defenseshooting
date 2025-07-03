using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;

public class Archer_Script : MonoBehaviour
{
    private Animator animator;
    private bool isAttacking = false;

    public GameObject arrowPrefab;  // 矢のプレハブ
    public GameObject arrowUltraPrefab;  // Ultra矢のプレハブ
    public Transform arrowSpawnPoint; // 矢の発射位置

    public float longPressThreshold = 2f; //長押し判定時間（秒）
    public float pressTime = 0f;
    public bool isPressing = false;

    [SerializeField] private Coin_Controller coinController;

    private float animationSpeed = 1f; // animationの再生速度

    Vector3 shootTarget;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!isAttacking)
        {
            if (Input.GetMouseButtonDown(0))// マウスボタン押下開始
            {
                isPressing = true;
                pressTime = 0f;
            }

            if (Input.GetMouseButton(0) && isPressing)// 押してる間は時間を計測
            {
                pressTime += Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(0) && isPressing)// マウスボタンを離した時に判定
            {
                isPressing = false;

                Vector3 mouseScreenPosition = Input.mousePosition;
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
                mouseWorldPosition.z = 0f;

                if (pressTime >= longPressThreshold)
                {
                    // 長押し → Ultra攻撃
                    StartCoroutine(PlayUltraAttack("Archer_Attack2", mouseWorldPosition));
                }
            
                else
                {
                    // 短押し → 通常攻撃
                    shootTarget = mouseWorldPosition;
                    PlayAttack("Archer_Attack1");
                }
            }

            // 右クリックで壁設置
            if (Input.GetMouseButtonDown(1))
            {
                if (coinController != null)
                {
                    coinController.PlaceWall();
                }
            }

            if (!isPressing)
            {
                animator.Play("Archer_Idle");
            }
        }
    }

    public void IncreaseAttackSpeed(float amount)
    {
        animationSpeed += amount;
        animator.speed = animationSpeed;
    }

    public void OnAttack()
    {
        // 通常攻撃の場合のみ矢を発射
        GameObject arrowInstance = Instantiate(arrowPrefab);
        Arrow_Script arrow = arrowInstance.GetComponent<Arrow_Script>();
        arrow.MoveTo(arrowSpawnPoint.position, shootTarget);
    }

    public void OnAttackFinish()
    {
        animator.Play("Archer_Idle");
        isAttacking = false;
    }

    void PlayAttack(string attackName)
    {
        isAttacking = true;
        animator.Play(attackName);
    }
    IEnumerator PlayUltraAttack(string attackName, Vector3 target)
    {
        isAttacking = true;
        animator.Play(attackName);

        yield return null;

        float animLength = animator.GetCurrentAnimatorStateInfo(0).length / animator.speed;
        yield return new WaitForSeconds(animLength);

        // Ultra矢を発射
        if (arrowUltraPrefab != null)
        {
            GameObject arrowInstance = Instantiate(arrowUltraPrefab);
            ArrowUltra_Script arrow = arrowInstance.GetComponent<ArrowUltra_Script>();
            arrow.MoveTo(arrowSpawnPoint.position, target);
        }

        animator.Play("Archer_Idle");
        isAttacking = false;
    }
}
