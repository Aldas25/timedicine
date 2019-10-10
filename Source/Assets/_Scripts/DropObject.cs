using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropObject : MonoBehaviour
{
    public int slotId = 0;
    private MainCanvas canvas;

    void Awake () {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvas>();
    }

    public void OnDrop() {
        canvas.OnDrop(this, slotId);
    }

    public void OnClick () {
        ItemObject itemObject = gameObject.GetComponentInParent<ItemObject> ();
        itemObject.ChangeItem(null);
        if (slotId < 2) {
            GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            gameController.ChangeItemInSlot(slotId, null);
        }
    }

}
