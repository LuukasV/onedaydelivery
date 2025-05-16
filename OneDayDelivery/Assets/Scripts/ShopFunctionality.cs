using UnityEngine.UI;
using UnityEngine;

//Management script to the item/upgrade shop, its UI elements, and functions
public class ShopFunctionality : MonoBehaviour
{
    public GameObject soldIcon1;
    public GameObject soldButton1;
    [SerializeField]
    private Text item1soldText;

    public GameObject soldIcon2;
    public GameObject soldButton2;
    [SerializeField]
    private Text item2soldText;

    [SerializeField]
    private Text postmarksLeft;

    private int availableStars;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        availableStars = GameData.starsEarned - GameData.starsSpent;
        postmarksLeft.text = availableStars.ToString();

        //We set the shops items to purchased mode, if they already have been bought
        if(GameData.item1Active)
        {
            soldIcon1.SetActive(true);
            soldButton1.SetActive(false);
            item1soldText.text = "SOLD!";
        }
        if (GameData.item2Active)
        {
            soldIcon2.SetActive(true);
            soldButton2.SetActive(false);
            item2soldText.text = "SOLD!";
        }
    }

    //Initiates the purchase of Item 1
    public void BuyItem1()
    {
        if (availableStars >= 2)
        {
            // Debug.Log("Item 1 has been bought");
            GameData.item1Active = true;
            GameData.starsSpent += 2;

            soldIcon1.SetActive(true);
            soldButton1.SetActive(false);

            availableStars -= 2;
            postmarksLeft.text = availableStars.ToString();
            item1soldText.text = "SOLD!";

        }
        else
        {
            // Debug.Log("The Player Could not afford Item 1");
            return;
        }
    }

    //Initiates the purchase of Item 2
    public void BuyItem2()
    {
        if (availableStars >= 2)
        {
            // Debug.Log("Item 2 has been bought");
            GameData.item2Active = true;
            GameData.starsSpent += 2;

            soldIcon2.SetActive(true);
            soldButton2.SetActive(false);

            availableStars -= 2;
            postmarksLeft.text = availableStars.ToString();
            item2soldText.text = "SOLD!";

        }
        else
        {
            // Debug.Log("The Player Could not afford Item 2");
            return;
        }
    }

}
