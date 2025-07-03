using UnityEngine;

public class ArrowUltra_Script : MonoBehaviour
{
    public float speed = 15f;

    public void MoveTo(Vector3 start, Vector3 target)
    {
        transform.position = start;

        Vector3 direction = (target - start).normalized;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        CheckOutOfScreenAndDestroy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void CheckOutOfScreenAndDestroy()
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < -0.1f || screenPos.x > 1.1f || screenPos.y < -0.1f || screenPos.y > 1.1f)
        {
            Destroy(gameObject);
        }
    }
}
