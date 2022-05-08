using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBossTorch : MonoBehaviour{
    // Start is called before the first frame update
    public CircleCollider2D playerAttackHitBox;
    public BoxCollider2D torchCollider;
    public SpriteRenderer mammonSR;
    private bool isPlayerAttacking=false;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(playerMovement.attackKey))
        {
            StartCoroutine(HitTimer());
        }
        if (isPlayerAttacking){
            if (playerAttackHitBox.IsTouching(torchCollider)){
                Mommon.torchesDown++;
                Mommon.otherWorld = false;
                mammonSR.color = new Color(1f,1f,1f,1f);
                gameObject.SetActive(false);
            }
        }
    }
    IEnumerator HitTimer()
    {
        isPlayerAttacking = true;
        yield return new WaitForSecondsRealtime((float)0.5);
        isPlayerAttacking = false;
    }
}
