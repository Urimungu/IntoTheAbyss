using UnityEngine.UI;
using UnityEngine;

public class PlayerUIFunctions : PlayerUIStats{

    //Initialization
    private void Start(){
        HideCrossHairText();

        //Checks to make sure there is a game manager and sets itself inside of it
        if (GameManager.Manager == null) return;

        //Destroys itself if there the reference is already set
        if (GameManager.Manager.PlayerUI != null) {
            Destroy(gameObject);
            return;
        }

        //Sets itself in the Game Manager
        GameManager.Manager.PlayerUI = this;
    }

    //Cross Hair
    public Image CrossHairIcon{
        get => _crossHairIcon != null ? _crossHairIcon : _crossHairIcon = transform.Find("CrossHair/Image").GetComponent<Image>();
    }
    public GameObject ItemDisplay {
        get => _itemDisplay != null ? _itemDisplay : _itemDisplay = transform.Find("ItemDisplay").gameObject;
    }
    public Text ItemDisplayText {
        get => _itemDisplayText != null ? _itemDisplayText : _itemDisplayText = transform.Find("ItemDisplay/Text").GetComponent<Text>();
    }

    //Functions
    public void UpdateCrossHairText(string text) {
        //Shows the indicator if it was set to off
        ItemDisplay.SetActive(true);

        //Sets the text on the item
        ItemDisplayText.text = text;
    }
    public void HideCrossHairText() {
        //Turns off the display of the image
        ItemDisplay.SetActive(false);
    }
    public void UpdateCrossHairImage() {
        //Updates how the crosshair looks
    }

}
