using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {
    public GameObject particles;
    public GameObject player;
    public HealthBar healthBar;
    public BoxCollider2D respawnPointCollider, PlayerCollider;
    private Vector3 RespawnPointLocation;
    private void Start() {
        RespawnPointLocation = gameObject.transform.position;
    }
    private void Update() {
        if (respawnPointCollider.IsTouching(PlayerCollider) && Input.GetKeyDown(KeyCode.O)) {
            UpdateRespawnPoint();
            SetHpToMax();
            Instantiate(particles,gameObject.transform);
        }
    }
    private void UpdateRespawnPoint() {
        PlayerStatistics.PlayerRespawnPoint = RespawnPointLocation;
    }
    public void RespawnPlayer() {
        player.transform.position = PlayerStatistics.PlayerRespawnPoint;
        SetHpToMax();
        Instantiate(particles, gameObject.transform);
    }
    private void SetHpToMax()
    {
        PlayerStatistics.currentHP = PlayerStatistics.healthPoints;
        healthBar.healthText.text = PlayerStatistics.currentHP + "/" + PlayerStatistics.healthPoints;
        healthBar.slider.value = PlayerStatistics.currentHP;
    }
}
