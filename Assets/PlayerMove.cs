using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float speed = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(moveX, moveY);
        move.Normalize();

        rb.linearVelocity = move * speed;
    }
}
