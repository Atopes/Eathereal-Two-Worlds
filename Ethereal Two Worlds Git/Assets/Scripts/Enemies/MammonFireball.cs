using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MammonFireball : MonoBehaviour
{
    public float moveSpeed = 8f;
    public CircleCollider2D fireballColision;
    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerStatistics playerStatistics; //Reference to the playerStatistics script
    private PlayerMovement playerMovement;
    private bool initiated = false;
    private int layerEnemies;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerStatistics =FindObjectOfType<PlayerStatistics>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        layerEnemies = LayerMask.NameToLayer("Enemies");
        Physics2D.IgnoreLayerCollision(layerEnemies, layerEnemies, true);
        StartCoroutine(LifeCycle());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = playerMovement.gameObject.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        direction.Normalize();
        movement = direction;
       if (playerMovement.playerColision.IsTouching(fireballColision)){ //Checking for collision 
            FindObjectOfType<PlayerMovement>().KnockBack(gameObject.transform.position);
            StartCoroutine(DamageOverTIme()); // Starting Coroutine for taking dmg over time
            Destroy(gameObject);
       }   
    }
    private void FixedUpdate(){
        if (initiated){
            move(movement);
        }
    }
    public void move(Vector2 direction)
    {
        Vector2 newPosition = new Vector2(transform.position.x + movement.x*moveSpeed*Time.deltaTime, transform.position.y + movement.y * moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);
    }
    IEnumerator DamageOverTIme(){
            playerStatistics.takeDamage(1);
            yield return new WaitForSeconds(1f);
    }
    IEnumerator LifeCycle()
    {
        yield return new WaitForSecondsRealtime(2);
        initiated = true;
        yield return new WaitForSecondsRealtime(5);
        Destroy(gameObject);
    }
}
