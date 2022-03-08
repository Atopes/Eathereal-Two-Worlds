using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerMovementSpeed = 2f;
    public float JumpForce = 2f;
    public Rigidbody2D rb;
    Vector2 movement;
    public CircleCollider2D playerColision;

    private void Start()
    {
    }
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y)< 0.001f)
        {
            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }
        transform.position += new Vector3(movement.x, 0, 0) * Time.deltaTime * playerMovementSpeed;
    }
}
