using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Item[] startingItems;
    private List<Item> items;
    private List<Item> restartItems;
    public Recipe[] recipes;

    private Item[] itemsInSlots = new Item[2];
    private MainCanvas canvas;
    private LevelManager levelManager;

    public bool levelGoing = false;

    void Awake () {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvas>();
        levelManager = gameObject.GetComponent<LevelManager>();
    }

    void Start () {
        RenewList();
    }

    public void RenewList () {
        items = new List<Item>();
        restartItems = new List<Item>();
        canvas.ClearBottomPanel();
        foreach (Item item in startingItems) {
            items.Add(item);
            restartItems.Add(item);
            canvas.AddItem(item);
        }
        
    }

    void Update () {
        if (levelGoing && Input.GetKeyDown(KeyCode.Space))
            ClearItemSlots();
    }

    public void ChangeItemInSlot (int slotId, Item item) {
        itemsInSlots[slotId] = item;
        canvas.UpdateResultSlot(null);
        CheckRecipes();
        CheckForUnusedItems();
    }

    void CheckRecipes () {
        for (int i = 0; i <= 1; i++) {
            if (itemsInSlots[0] == null) 
                return;
        }

        if (itemsInSlots[0] == null || itemsInSlots[1] == null)
            return;

        foreach (Recipe recipe in recipes) {
            if ((recipe.item1 == itemsInSlots[0] && recipe.item2 == itemsInSlots[1]) ||
                (recipe.item1 == itemsInSlots[1] && recipe.item2 == itemsInSlots[0])) {
                    CreateNewItem(recipe.resultingItem);
                    break;
                }
        }
    }

    public void CreateNewItem (Item resultingItem) {
        canvas.UpdateResultSlot(resultingItem);
        if (!items.Contains(resultingItem)) {
            items.Add(resultingItem);
            canvas.AddItem(resultingItem);

            levelManager.CheckLevelStatus (resultingItem);
        }
    }

    public void ClearItemSlots () {
        for (int i = 0; i < 2; i++) {
            ChangeItemInSlot(i, null);
        }
        canvas.ClearItemSlots();
    }

    public void ReloadLevel() {
        canvas.ClearBottomPanel();

        items = new List<Item>();
        foreach (Item item in restartItems) {
            items.Add(item);
            canvas.AddItem(item);
        }
    }

    public void SaveRestartItems () {
        restartItems = new List<Item> ();
        foreach (Item item in items) {
            restartItems.Add(item);
        }
    }

    public void CheckForUnusedItems () {
        foreach (Item item in items) {
            bool colorThis = true;
            foreach (Recipe recipe in recipes) {
                if ((recipe.item1 == item || recipe.item2 == item) && !items.Contains(recipe.resultingItem)) {
                    colorThis = false; 
                    //if (item.itemName == "BRICK") Debug.Log("haven't found " + recipe.resultingItem.itemName);
                }
            }
            if (colorThis) canvas.ColorUnusedItem(item);
            //if (colorThis) Debug.Log("coloring item " + item.itemName);
        }
    }
}
