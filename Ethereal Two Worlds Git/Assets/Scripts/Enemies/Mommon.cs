using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mommon : MonoBehaviour{
    public int healthPoints = 50, attackPower = 1;
    public GameObject barObject,burningHands;
    public BoxCollider2D enemyCollider, playerColision, barTrigger, wallDetection;
    public CircleCollider2D playerAttackHitBox;
    public Rigidbody2D enemyRB;
    public Slider healthBar;
    public int movementSpeed =5,maximumVel =4;
    public GameObject eye,ringPrefab,ringTip,fireBallPrefab;
    public GameObject torch1, torch2, torch3, torch4;
    public Animator animator;
    public Button resetButton;
    private PlayerMovement playerMovement;
    private int layerWalls; // Reference to the different layers
    public static bool isAlive = true,otherWorld = false;
    public static int torchesDown = 0;
    private bool started = false, canMove = true, isMoveTimerRunning = false, seesPlayer = false,canTakeDamage=true,phase2 =false;
    private Vector3 seekDistance = new Vector3(20f, 0),enemyScale;
    private RaycastHit2D hit;
    private Vector2 movement;
    public AudioSource doomSound, soundtrack;

    private bool isPlayerAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        layerWalls = LayerMask.NameToLayer("Walls");// Defines the objects on the Walls layer 
        enemyScale = gameObject.transform.localScale; // Reference to the starting local scale of enemy
        healthBar.value = healthPoints;
        movement.x = -1;
        Flip();
    }

    // Update is called once per frame
    void Update() {
        if (playerColision.IsTouching(barTrigger) && isAlive) {
            started = true;
            resetButton.interactable = false;
            barObject.SetActive(true);
            soundtrack.Pause();
            doomSound.Play();
        }if(PlayerStatistics.currentHP == 0){
            started = false;
            otherWorld = false;
            barObject.SetActive(false);
            healthPoints = 50;
            healthBar.value = healthPoints;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            torch1.SetActive(false);
            torch2.SetActive(false);
            torch3.SetActive(false);
            torch4.SetActive(false);
            torchesDown = 0;
            phase2 = false;
            movementSpeed = 5;
            maximumVel = 4;
            doomSound.Stop();
            soundtrack.UnPause();
        }
        ///////////////////////MOVEMENT//////////////////////////
        if (started && canMove) {
            animator.SetBool("isMoving", true);
            if (wallDetection.IsTouchingLayers(1 << layerWalls)) { //Checks if the enemy is touching a wall 
                movement.x = -movement.x; // Changes the direction enemy is moving in
                enemyRB.velocity = new Vector2(0, enemyRB.velocity.y); // Nulifies the enemies momentum when changing directions
                Flip(); // Changes the way the enemy is looking 
            }
            if (enemyRB.velocity.x > maximumVel && movement.x > 0) {
                enemyRB.velocity = new Vector2(maximumVel, enemyRB.velocity.y);
            }
            else if (enemyRB.velocity.x < -maximumVel && movement.x < 0) {
                enemyRB.velocity = new Vector2(-maximumVel, enemyRB.velocity.y);
            }
            enemyRB.AddForce(new Vector2(movement.x * movementSpeed, 0), ForceMode2D.Force); // Moving with enemies rigid body component in a certain direction
        }
        if (started && !isMoveTimerRunning) {
            StartCoroutine(MoveTimer1());
        }
        if(enemyRB.velocity.x == 0){
            animator.SetBool("isMoving", false);
        }
        /////////////PLAYER DETECTION/////////////
        hit = Physics2D.Linecast(eye.transform.position, eye.transform.position + (seekDistance * movement.x), 1 << LayerMask.NameToLayer("Player"));
        if (hit.collider != null) {
            seesPlayer = true;
        }else{
            StartCoroutine(ForgetTimer());
        }
        ////////////BASIC ATTACK/////////
        if (seesPlayer && canMove){
            animator.SetBool("Attacking", true);
            burningHands.SetActive(true);
        }else{
            animator.SetBool("Attacking", false);
            burningHands.SetActive(false);
        }
        ///////////HIT DETECTION///////////
        if (Input.GetKeyDown(playerMovement.attackKey)){
            StartCoroutine(HitTimer());
        }
        if (isPlayerAttacking){
            if (playerAttackHitBox.IsTouching(enemyCollider) && canTakeDamage){
                TakeDamage(PlayerStatistics.meleeDamage);
            }
        }
    }



    IEnumerator MoveTimer1(){
        isMoveTimerRunning = true;
        if(!seesPlayer){
            movement.x = -movement.x; // Changes the direction enemy is moving in
            Flip(); // Changes the way the enemy is looking 
        }
        yield return new WaitForSecondsRealtime(5);
        canMove = !canMove;
        if (!seesPlayer || phase2){
            Instantiate(fireBallPrefab, new Vector3(gameObject.transform.position.x -3, gameObject.transform.position.y + 5, 1), Quaternion.identity);
        }
        if (phase2){
            Instantiate(fireBallPrefab, new Vector3(gameObject.transform.position.x + 3, gameObject.transform.position.y + 5, 1), Quaternion.identity);
        }
        isMoveTimerRunning = false;
    }
    IEnumerator ForgetTimer(){
        yield return new WaitForSecondsRealtime(1);
        if (hit.collider == null){
            seesPlayer = false;
        }
    }
    IEnumerator HitTimer()
    {
        isPlayerAttacking = true;
        yield return new WaitForSecondsRealtime((float) 0.5);
        isPlayerAttacking = false;
    }
    IEnumerator IFrameAfterHit()
    {
        canTakeDamage = false;
        yield return new WaitForSecondsRealtime((float)0.5);
        canTakeDamage = true;
    }
    IEnumerator WaitForDeath(){
        yield return new WaitForSecondsRealtime((float)0.3);
        barObject.SetActive(false);
        doomSound.Stop();
        soundtrack.UnPause();
        isAlive = false;
        resetButton.interactable = true;
        Instantiate(ringPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1), Quaternion.identity);
        ringTip.SetActive(true);
        Destroy(gameObject);
    }
    private void Flip()
    {
        enemyScale.x = -enemyScale.x; // Flips the local scale
        gameObject.transform.localScale = enemyScale; // Changes the local scale to the new one
    }
    public void TakeDamage(int damage){
        if (!otherWorld){
            healthPoints -= damage; // Damages enemy
            healthBar.value = healthPoints;
            animator.SetTrigger("hit");
            StartCoroutine(IFrameAfterHit());
        }

        if(healthPoints <= 40 && torchesDown ==0){
            otherWorld = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.4f);
            torch1.SetActive(true);
        }
        if (healthPoints <= 30 && torchesDown == 1)
        {
            otherWorld = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
            torch2.SetActive(true);
        }
        if(healthPoints <= 25){
            phase2 = true;
            movementSpeed = 8;
            maximumVel = 7;
        }
        if (healthPoints <= 20 && torchesDown == 2)
        {
            otherWorld = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
            torch3.SetActive(true);
        }
        if (healthPoints <= 10 && torchesDown == 3)
        {
            otherWorld = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
            torch4.SetActive(true);
        }

        isPlayerAttacking = false;
        if (healthPoints <= 0){
            animator.SetBool("isDead",true);
            canMove = false;
            StartCoroutine(WaitForDeath());
        }
    }
}
