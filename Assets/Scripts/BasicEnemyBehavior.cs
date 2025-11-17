using UnityEngine;

public class BasicEnemyBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private bool facingLeft;
    private float enemyDirection;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(enemyDirection * moveSpeed, rb.linearVelocity.y);
        if (facingLeft)
        {
            enemyDirection = 1;
        }
        else
        {
            enemyDirection = -1;
        }
    }
}
