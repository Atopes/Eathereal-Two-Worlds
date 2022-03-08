using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float playerMovementSpeed = 2f;
    public float JumpForce = 2f;
    public Rigidbody2D PlayerRigidBody; // Player's rigid body
    Vector2 movement; // Variable for declaring movement on X & Y axis
    public CircleCollider2D playerColision; // Player's physicall collider
    void Update()
    {
        //Getting X & Y axis input (WASD / arrow keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //Jump mechanics
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(PlayerRigidBody.velocity.y) < 0.001f) // Checking if space is pressed and if the player is NOT in the air
        {
            PlayerRigidBody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse); // Adding a force that makes the player jump
        }
        transform.position += new Vector3(movement.x, 0, 0) * Time.deltaTime * playerMovementSpeed; // Updating player's position
    }
}