using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderFix : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public BoxCollider2D colliderArea;
    private Respawn respawn;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        respawn = FindObjectOfType<Respawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.playerColision.IsTouching(colliderArea))
        {
            respawn.SliderFix();
        }   
    }
}
