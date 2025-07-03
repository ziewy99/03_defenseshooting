using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class E_OrcRider_Script : MonoBehaviour
{
    public int maxHP = 200;
    public int currentHP;

    public int attackPower = 40;
    public float moveSpeed = 2f;

    private int coinReward = 100;
    private int scoreReward = 500;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform attackCheckPoint;
    private GameObject attackTarget; // 攻撃対象（壁や塔）

    public Transform targetObject; // 目標オブジェクト
    public Vector2 attackCheckSize = new Vector2(1f, 1f); // 攻撃範囲

    private enum OrcRiderState { Walk, Attack, Damaged, Die }
    private OrcRiderState currentState;
    private int attackCount = 0;

    [SerializeField] private AudioClip damagedClip;
    private AudioSource audioSource;

    void Start()
    {
        currentHP = maxHP;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        GameObject checkPoint = new GameObject("AttackCheckPoint");
        checkPoint.transform.SetParent(transform);
        checkPoint.transform.localPosition = new Vector3(0.6f, 0f, 0f);
        attackCheckPoint = checkPoint.transform;

        audioSource = GetComponent<AudioSource>();

        SetState(OrcRiderState.Walk);
    }

    void Update()
    {
        if (currentState == OrcRiderState.Walk)
        {
            Vector2 direction = ((Vector2)targetObject.position - rb.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            if (IsTargetInAttackRange())
            {
                SetState(OrcRiderState.Attack);
                StartCoroutine(AttackAfterAnimation());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState == OrcRiderState.Die) return;

        if (collision.CompareTag("Arrow"))
        {
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                int arrowPower = gameManager.GetArrowPower();
                TakeDamage(arrowPower);
            }
        }
    }

    private bool IsTargetInAttackRange()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCheckPoint.position, attackCheckSize, 0f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Wall") || hit.CompareTag("Tower"))
            {
                attackTarget = hit.gameObject;
                return true;
            }
        }
        return false;
    }

    IEnumerator AttackAfterAnimation()
    {
        rb.linearVelocity = Vector2.zero;

        yield return null;

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_OrcRider_Attack1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_OrcRider_Attack2"));

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        if (attackTarget != null)
        {
            IDamageable target = attackTarget.GetComponent<IDamageable>();
            if (target != null)
            {
                int damage = (attackCount == 3) ? attackPower * 3 : attackPower;
                target.TakeDamage(damage);
            }
        }
        SetState(OrcRiderState.Walk);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // ダメージ音を再生
        if (damagedClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(damagedClip);
        }

        if (currentHP <= 0)
        {
            SetState(OrcRiderState.Die);
            StartCoroutine(DieAfterAnimation());
        }
        else
        {
            SetState(OrcRiderState.Damaged);
            StartCoroutine(RecoverFromDamage());
        }
    }

    IEnumerator RecoverFromDamage()
    {
        rb.linearVelocity = Vector2.zero;

        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        SetState(OrcRiderState.Walk);
    }

    IEnumerator DieAfterAnimation()
    {
        rb.linearVelocity = Vector2.zero;

        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.AddGold(coinReward);
            gameManager.AddScore(scoreReward);
        }
        Destroy(gameObject);
    }

    void SetState(OrcRiderState newState)
    {
        currentState = newState;
        rb.linearVelocity = Vector2.zero;

        switch (currentState)
        {
            case OrcRiderState.Walk:
                animator.Play("Enemy_OrcRider_Walk");
                break;
            case OrcRiderState.Attack:
                attackCount = (attackCount % 3) + 1; // 弱 → 弱 → 強
                if (attackCount == 3)
                {
                    animator.Play("Enemy_OrcRider_Attack2"); // 強攻撃
                }
                else
                {
                    animator.Play("Enemy_OrcRider_Attack1"); // 弱攻撃
                }
                break;
            case OrcRiderState.Damaged:
                animator.Play("Enemy_OrcRider_Damaged");
                break;
            case OrcRiderState.Die:
                animator.Play("Enemy_OrcRider_Die");
                break;
        }
    }

    void OnDrawGizmosSelected()//Debug用
    {
        if (attackCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackCheckPoint.position, attackCheckSize);
        }
    }
}
