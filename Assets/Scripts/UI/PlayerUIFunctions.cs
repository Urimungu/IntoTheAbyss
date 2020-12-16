using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerUIFunctions : PlayerUIStats{

    //Initialization
    private void Start(){
        //Initializes the UI
        HideCrossHairText();
        InitializeInventory();
        HideInventory();
        CloseOptions();
        InitializeStorage();
        HideStorage();

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
    private void FixedUpdate(){
        //Exits if there is nothing being dragged
        if (!drag) return;

        //Sets the position of the cursor icon on the cursor
        MouseIcon.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition + new Vector3(-25, 20, 0);
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

    //Inventory
    public List<GameObject> InventorySlots {
        get => _inventorySlots;
        set => _inventorySlots = value;
    }
    public Transform InventoryHolder {
        get => _inventoryHolder != null ? _inventoryHolder : _inventoryHolder = transform.Find("Inventory");
    }
    public Transform ItemHolder {
        get => _itemHolder != null ? _itemHolder : _itemHolder = transform.Find("Inventory/Items");
    }
    public Text CarryWeight {
        get => _carryWeight != null ? _carryWeight :_carryWeight = transform.Find("Inventory/CarryWeight").GetComponent<Text>();
    }
    public GameObject OptionsMenu {
        get => _optionsMenu != null ? _optionsMenu : _optionsMenu = transform.Find("Inventory/Options").gameObject;
    }
    public GameObject MouseIcon {
        get =>_mouseIcon != null ? _mouseIcon : _mouseIcon = transform.Find("MouseItem").gameObject;
    }

    //Storage
    public Text StorageName {
        get => _storageName != null ? _storageName : _storageName = transform.Find("StorageUI/Text").GetComponent<Text>();
    }
    public GameObject StorageUI {
        get => _storageUI != null ? _storageUI : _storageUI = transform.Find("StorageUI").gameObject;
    }
    public Transform StoredItemHolder {
        get => _storedItemHolder != null ? _storedItemHolder : _storedItemHolder = transform.Find("StorageUI/Items");
    }
    public List<GameObject> StoredItemSlots{
        get => _storedItemSlots;
        set => _storedItemSlots = value;

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

    //Variables
    float weightCarrying = 0;
    int clicked = 404;
    bool drag = false;
    bool checkInv = true;
    private List<Item> playerInventory;
    private List<Item> storageInventory;

    //Inventory Functions
    public void HideInventory(){
        InventoryHolder.gameObject.SetActive(false);
        CloseOptions();
    }
    public void OpenInventory(List<Item> newInventory){
        playerInventory = newInventory;
        UpdateInventory(newInventory);
        InventoryHolder.gameObject.SetActive(true);
    }
    private void OpenOptions(int id, bool inv = true) {
        //Sets the position before opening
        var newPos = InventorySlots[id].GetComponent<RectTransform>().anchoredPosition;

        OptionsMenu.transform.parent = inv ? InventoryHolder.transform : StorageUI.transform;
        var offset = inv ? new Vector2(80, -200) : new Vector2(90, -100);

        OptionsMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPos.x + offset.x, newPos.y + offset.y);

        //Shows the options menu
        OptionsMenu.SetActive(true);
    }
    private void CloseOptions() {
        OptionsMenu.SetActive(false);
    }
    private void InitializeInventory (){
        //Initializes variables
        var slot = Resources.Load<GameObject>("UI/Inventory/Prefabs/Slot");

        //Creates the inventory slots
        for (int y = 0; y < 7; y++){
            //Places the slots
            for (int x = 0; x < 5; x++){
                var obj = Instantiate(slot, ItemHolder);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(10 + (110 * x), -10 - (110 * y));
                InventorySlots.Add(obj);

                //Turns on the button and sets the reference
                AddButtons(InventorySlots.Count - 1);
            }
        }
    }
    private void UpdateInventory(List<Item> newInventory){

        //Sets the carry weight to 0
        weightCarrying = 0;

        for (int i = 0; i < InventorySlots.Count; i++) {
            //Updates the slots 
            if (newInventory.Count > i && newInventory[i].ID != 0) {
                //Updates the Slot
                UpdateSlot(i, newInventory[i].Name, newInventory[i].Quantity);

                //Counts the amount of weight that is being carried
                weightCarrying += (newInventory[i].Weight * newInventory[i].Quantity);

                continue;
            }

            //Empties the slot
            ClearSlot(i);
        }

        //Fixes the weight carrying display
        CarryWeight.text = "Weight: " + weightCarrying.ToString("0.00");
    }

    //Buttons
    public void ClickItem(int id, bool inv = true){
        //Closes the options if the same button is pressed twice
        if (id == clicked && inv == checkInv)
        {
            clicked = 404;
            checkInv = inv;
            CloseOptions();
            return;
        }

        //Opens the options on the right UI surface
        clicked = id;
        checkInv = inv;
        OpenOptions(id, inv);
        return;

    }
    public void AddButtons(int id, bool inv = true) {
        //Initializes the slot
        var slot = inv ? InventorySlots[id] : StoredItemSlots[id];

        //Turns on the button and sets the reference
        slot.GetComponent<Button>().onClick.AddListener(delegate { ClickItem(id, inv); });

        //Initialization
        EventTrigger trigger = slot.GetComponent<EventTrigger>();

        //Drag
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.BeginDrag;
        pointerDown.callback.AddListener((e) => DragItem(id, inv));

        //End Drag
        var endDrag = new EventTrigger.Entry();
        endDrag.eventID = EventTriggerType.EndDrag;
        endDrag.callback.AddListener((e) => EndDrag(id, inv));

        //Release
        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.Drop;
        pointerUp.callback.AddListener((e) => ReleaseItem(id, inv));

        //Adds the triggers
        trigger.triggers.Add(pointerUp);
        trigger.triggers.Add(pointerDown);
        trigger.triggers.Add(endDrag);

    }
    public void DragItem(int id, bool inv = true){
        //Prevents dragging empty slots
        if ((inv ? playerInventory[id].ID : storageInventory[id].ID) == 0) return;

        //Selects the item
        clicked = id;
        checkInv = inv;
        drag = true;

        //Fixes the mouse Icon
        MouseIcon.SetActive(true);
        MouseIcon.GetComponent<Image>().sprite = FindImage(inv ? playerInventory[id].Name : storageInventory[id].Name);
        MouseIcon.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition + new Vector3(-25, 20, 0);

        //Changes the color
        var color = inv ? InventorySlots[id].transform.Find("Icon").GetComponent<Image>() : StoredItemSlots[id].transform.Find("Icon").GetComponent<Image>();
        color.color = new Color(1, 1, 1, 0.2f);
    }
    public void EndDrag(int id, bool inv = true) {
        //Unselects the item
        drag = false;

        //Hides the Mouse Icon
        MouseIcon.SetActive(false);

        //Changes the apperance once it's dropped
        var color = inv ? InventorySlots[id].transform.Find("Icon").GetComponent<Image>() : StoredItemSlots[id].transform.Find("Icon").GetComponent<Image>();
        color.color = new Color(1, 1, 1, 1);
    }
    public void ReleaseItem(int id, bool inv = true) {
        //Stop if it's not anywhere
        var tempItem = checkInv ? playerInventory[clicked] : storageInventory[clicked];
        if (tempItem.ID == 0) return; 
        if (clicked == id && checkInv == inv) return;

        //Transfers the item
        var temp = inv ? playerInventory[id] : storageInventory[id];
        if(inv)
            playerInventory[id] = checkInv ? playerInventory[clicked] : storageInventory[clicked];
        else
            storageInventory[id] = checkInv ? playerInventory[clicked] : storageInventory[clicked];

        if (checkInv)
            playerInventory[clicked] = temp;
        else
            storageInventory[clicked] = temp;

        //Updates the slots
        AutoUpdateSlot(id, inv);
        AutoUpdateSlot(clicked, checkInv);
        
    }

    //Slots
    private void AutoUpdateSlot(int id, bool inv = true) {
        //Initializes the item that needs to be updated
        var temp = inv ? playerInventory[id] : storageInventory[id];

        //Clears the item if there is nothing there
        if (temp.ID == 0) {
            ClearSlot(id, inv);
            return;
        }

        //Updates the item with new information
        UpdateSlot(id, temp.Name, temp.Quantity, inv);
    }
    private void UpdateSlot(int id, string name, int quantity, bool inv = true) {
        //Initializes the slot
        var slot = inv ? InventorySlots[id] : StoredItemSlots[id];

        //Turns on the button
        slot.GetComponent<Button>().interactable = true;

        //Fixes image
        slot.transform.Find("Icon").GetComponent<Image>().enabled = true;
        slot.transform.Find("Icon").GetComponent<Image>().sprite = FindImage(name);

        //Fixes Quantity
        slot.transform.Find("Quantity").GetComponent<Text>().text = quantity.ToString();
    }
    private void ClearSlot(int id, bool inv = true) {
        //Initializes the slots
        var slot = inv ? InventorySlots[id] : StoredItemSlots[id];

        //Changes the apperance
        slot.transform.Find("Icon").GetComponent<Image>().enabled = false;
        slot.transform.Find("Quantity").GetComponent<Text>().text = "";
        slot.GetComponent<Button>().interactable = false;
    }
    private Sprite FindImage(string name) {
        //Checks to see if the sprite exists in the resource folder
        var newSprite = Resources.Load<Sprite>("UI/Inventory/Icons/" + name);
        if (newSprite != null)
            return newSprite;

        //Returns the blank one if the icon doesn't exist
        return Resources.Load<Sprite>("UI/Inventory/Icons/_NotFound");
    }

    //Storage Functions
    private void InitializeStorage() {
        //Initializes variables
        var slot = Resources.Load<GameObject>("UI/Inventory/Prefabs/Slot");

        //Creates the inventory slots
        for (int y = 0; y < 5; y++){
            //Places the slots
            for (int x = 0; x < 3; x++){
                var obj = Instantiate(slot, StoredItemHolder);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(10 + (110 * x), -10 - (110 * y));
                StoredItemSlots.Add(obj);

                //Turns on the button and sets the references
                AddButtons(StoredItemSlots.Count - 1, false);
            }
        }
    }
    public void OpenStorage(ref List<Item> containedItems, string name){
        //Sets the storage information
        StorageUI.SetActive(true);
        StorageName.text = name;
        storageInventory = containedItems;

        for (int i = 0; i < StoredItemSlots.Count; i++){
            //Updates the slots 
            if (containedItems.Count > i && containedItems[i].ID != 0){
                //Updates the Slot
                UpdateSlot(i, containedItems[i].Name, containedItems[i].Quantity, false);

                continue;
            }

            //Creates an empty slot
            ClearSlot(i, false);
        }
    }
    public void HideStorage() {
        StorageUI.SetActive(false);
    }
}
