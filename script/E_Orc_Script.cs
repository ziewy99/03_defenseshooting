using UnityEngine;
using System.Collections;

public class E_Orc_Script : MonoBehaviour, IDamageable
{
    public int maxHP = 50;
    public int currentHP;

    public int attackPower = 10;
    public float moveSpeed = 1.5f;

    private int coinReward = 10;
    private int scoreReward = 50;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform attackCheckPoint;
    private GameObject attackTarget; // 攻撃対象（壁や塔）

    public Transform targetObject; // 目標オブジェクト
    public Vector2 attackCheckSize = new Vector2(1f, 1f); // 攻撃範囲

    private enum OrcState { Walk, Attack, Damaged, Die }
    private OrcState currentState;

    [SerializeField] private AudioClip damagedClip;
    private AudioSource audioSource;

    void Start()
    {
        currentHP = maxHP;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        GameObject checkPoint = new GameObject("AttackCheckPoint");
        checkPoint.transform.SetParent(transform);
        checkPoint.transform.localPosition = new Vector3(0.5f, 0f, 0f);
        attackCheckPoint = checkPoint.transform;

        audioSource = GetComponent<AudioSource>();

        SetState(OrcState.Walk);
    }

    void Update()
    {
        if (currentState == OrcState.Walk)
        {
            Vector2 direction = ((Vector2)targetObject.position - rb.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            if (IsTargetInAttackRange())
            {
                SetState(OrcState.Attack);
                StartCoroutine(AttackAfterAnimation());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState == OrcState.Die) return;

        if (collision.CompareTag("Arrow"))
        {
            if (currentState != OrcState.Die)
            {
                GameManager gameManager = FindFirstObjectByType<GameManager>();
                if (gameManager != null)
                {
                    int arrowPower = gameManager.GetArrowPower();
                    TakeDamage(arrowPower);
                }
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
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        if (attackTarget != null)
        {
            IDamageable target = attackTarget.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(attackPower);
            }
        }

        SetState(OrcState.Walk);
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
            SetState(OrcState.Die);
            StartCoroutine(DieAfterAnimation());
        }
        else
        {
            SetState(OrcState.Damaged);
            StartCoroutine(RecoverFromDamage());
        }
    }

    IEnumerator RecoverFromDamage()
    {
        rb.linearVelocity = Vector2.zero;

        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        SetState(OrcState.Walk);
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

    void SetState(OrcState newState)
    {
        currentState = newState;
        rb.linearVelocity = Vector2.zero;

        switch (currentState)
        {
            case OrcState.Walk:
                animator.Play("Enemy_Orc_Walk");
                break;
            case OrcState.Attack:
                animator.Play("Enemy_Orc_Attack");
                break;
            case OrcState.Damaged:
                animator.Play("Enemy_Orc_Damaged");
                break;
            case OrcState.Die:
                animator.Play("Enemy_Orc_Die");
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
