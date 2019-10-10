using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Item[] startingItems;
    private List<Item> items;
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
        canvas.ClearBottomPanel();
        foreach (Item item in startingItems) {
            items.Add(item);
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
}
