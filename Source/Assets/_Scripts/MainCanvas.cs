using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    
    public RectTransform dragIcon;
    //public ParticleSystem trailEffect;
    public GameObject resultingItemSlot;
    public ItemObject[] itemSlots;

    public GameObject itemPrefab;
    public GameObject bottomPanel;

    //private DragObject curDrag;
    private Item curItem;
    private GameController gameController;
    private SoundManager soundManager;
    
    void Awake () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        soundManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>();
    }

    void Start()
    {
        dragIcon.gameObject.SetActive(false);
        //trailEffect.enabled = false;
    }

    
    public void OnBeginDrag(DragObject target) {
        //curDrag = target;
        curItem = target.GetComponentInParent<ItemObject>().item;
        dragIcon.GetComponentInChildren<ItemObject>().ChangeItem(curItem);
        dragIcon.gameObject.SetActive(true);
        //trailEffect.enabled = true;
        
        soundManager.PlayPopSound();
    }

    public void OnDrag () {
        dragIcon.position = Input.mousePosition;
        //trailEffect.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnEndDrag () {
        //curDrag = null;
        dragIcon.gameObject.SetActive(false);
        //trailEffect.enabled = false;
        curItem = null;
    }

    public void OnDrop(DropObject target, int slotId) {
        if (curItem != null) { //in case of just dragging randomly
            ItemObject itemObject = target.GetComponentInParent<ItemObject> ();
            itemObject.ChangeItem(curItem);
        }
        gameController.ChangeItemInSlot(slotId, curItem);
        
        soundManager.PlayPopSound();
    }

    public void ClearBottomPanel () {
        int childs = bottomPanel.transform.childCount;
        for (int i = childs-1; i >= 0; i--) {
            Destroy(bottomPanel.transform.GetChild(i).gameObject);
        }
    }

    public void ColorUnusedItem (Item itemToColor) {
        int childs = bottomPanel.transform.childCount;
        for (int i = 0; i < childs; i++) {
           GameObject curItemGO = bottomPanel.transform.GetChild(i).gameObject;
           ItemObject curItem = curItemGO.GetComponent<ItemObject> ();
           if (curItem.item == itemToColor) {
               curItem.ColorItem();
               return;
           }
        }
    }

    public void ClearResultSlot () {
        UpdateResultSlot(null);
    }

    public void ClearItemSlots() {
        foreach (ItemObject itemObject in itemSlots) {
            itemObject.ChangeItem(null);
        }
    }

    public void UpdateResultSlot (Item item) {
        resultingItemSlot.GetComponentInChildren<ItemObject>().ChangeItem(item);
    }

    public void AddItem (Item item) {
        GameObject itemGO = Instantiate(itemPrefab, bottomPanel.transform);
        ItemObject itemObject = itemGO.gameObject.GetComponent<ItemObject> ();
        itemObject.ChangeItem(item);
    }
}
