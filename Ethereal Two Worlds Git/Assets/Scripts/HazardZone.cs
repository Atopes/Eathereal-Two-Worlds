using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardZone : MonoBehaviour
{
    public BoxCollider2D zoneCollision, playerCollision;
    public PlayerStatistics playerStatistics;
    bool isIn = false;
    void Update()
    {
        if (playerCollision.IsTouching(zoneCollision))
        {
            if (!isIn)
            {
                isIn = true;
                StartCoroutine(DamageOverTIme());
            }
        }
        else
        {
            if (isIn)
            {
                isIn = false;
                StopCoroutine(DamageOverTIme());
            }
        }
    }
    IEnumerator DamageOverTIme()
    {
        while (isIn)
        {
            playerStatistics.takeDamage(1);
            yield return new WaitForSeconds(1f);
        }
    }
}
