using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopKeeper : MonoBehaviour
{
    public Slider slider; //The slider object itself
    public TextMeshProUGUI healthText; //Text displayed in the health bar
    private HealthBar healthBar;
    public BoxCollider2D shopArea, playerCollider;
    public GameObject prompt,shopUI;
    public TextMeshProUGUI coinsText;

    public TextMeshProUGUI TMname1, TMname2, TMname3,TMdescription1, TMdescription2, TMdescription3,TMprice1, TMprice2,TMprice3;
    public Image image1,image2,image3,imageCross1,imageCross2,imageCross3;

    public Sprite imageItem1, imageItem2, imageItem3,circle;
    public string itemName1, itemName2, itemName3, itemDescription1, itemDescription2, itemDescription3;
    public int itemPrice1, itemPrice2, itemPrice3;
    private static bool itemBought1=false, itemBought2 = false, itemBought3 = false;
    private KeyCode interactKey;

    void Start(){
        
        interactKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact"));
        
        healthBar = FindObjectOfType<HealthBar>();
        image1.sprite = imageItem1;
        image2.sprite = imageItem2;
        image3.sprite = imageItem3;

        TMname1.text = itemName1;
        TMname2.text = itemName2;
        TMname3.text = itemName3;

        TMdescription1.text = itemDescription1;
        TMdescription2.text = itemDescription2;
        TMdescription3.text = itemDescription3;

        TMprice1.text = itemPrice1.ToString();
        TMprice2.text = itemPrice2.ToString();
        TMprice3.text = itemPrice3.ToString();
        checkItems();
    }
    void Update()
    {
        if (playerCollider.IsTouching(shopArea)){
            if (!prompt.activeSelf){
                prompt.SetActive(true);
            }
            if (Input.GetKeyDown(interactKey))
            {
                PlayerMovement.canMove = !PlayerMovement.canMove;
                shopUI.SetActive(!PlayerMovement.canMove);
            }
        } else{
            if (prompt.activeSelf){
                prompt.SetActive(false);
            }
        }
    }
    public void buyItem1S1(){
        if(PlayerStatistics.coins >= itemPrice1 && !itemBought1){
            PlayerStatistics.coins -= itemPrice1;
            coinsText.text = PlayerStatistics.coins.ToString();
            itemBought1 = true;
            PlayerStatistics.meleeDamage = 4;
            imageCross1.sprite = circle;
            imageCross1.color = new Color(1, 1, 1, 1);
        }
    }
    public void buyItem2S1()
    {
        if (PlayerStatistics.coins >= itemPrice2 && !itemBought2)
        {
            PlayerStatistics.coins -= itemPrice2;
            coinsText.text = PlayerStatistics.coins.ToString();
            itemBought2 = true;
            PlayerStatistics.healthPoints++;
            PlayerPrefs.SetInt("MaxHealth", PlayerStatistics.healthPoints);
            PlayerPrefs.Save();
            PlayerStatistics.currentHP += 1;
            slider.maxValue = PlayerStatistics.healthPoints;
            slider.value = PlayerStatistics.currentHP;
            healthText.text = PlayerStatistics.currentHP + "/" + PlayerStatistics.healthPoints; //Setting the text displayed in the hp bar
            imageCross2.sprite = circle;
            imageCross2.color = new Color(1, 1, 1, 1);
        }
    }
    public void buyItem3S1()
    {
        if (PlayerStatistics.coins >= itemPrice3 && !itemBought3)
        {
            PlayerStatistics.coins -= itemPrice3;
            coinsText.text = PlayerStatistics.coins.ToString();
            itemBought3 = true;
            Coins.doubleChance = 10;
            imageCross3.sprite = circle;
            imageCross3.color = new Color(1, 1, 1, 1);
        }
    }
    public void checkItems(){
        if (itemBought1){
            imageCross1.sprite = circle;
            imageCross1.color = new Color(1, 1, 1, 1);
        }
        if (itemBought2)
        {
            imageCross2.sprite = circle;
            imageCross2.color = new Color(1, 1, 1, 1);
        }
        if (itemBought3)
        {
            imageCross3.sprite = circle;
            imageCross3.color = new Color(1, 1, 1, 1);
        }
    }
}
