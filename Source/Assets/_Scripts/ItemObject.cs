using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemObject : MonoBehaviour
{
    public Item item;

    void Awake () {
        UpdateDisplay();
    }

    public void ChangeItem (Item newItem) {
        item = newItem;
        UpdateDisplay();
    }

    public void ColorItem () {
        Image imageComponent;
        TextMeshProUGUI textComponent;
        
        imageComponent = gameObject.GetComponentInChildren<Image>();
        textComponent = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        Color newColor = imageComponent.color;
        newColor.a = 0.2f;
        imageComponent.color = newColor;

        newColor = textComponent.color;
        newColor.a = 0.2f;
        textComponent.color = newColor;
    }

    void UpdateDisplay () {
        Image imageComponent;
        TextMeshProUGUI textComponent;
        
        imageComponent = gameObject.GetComponentInChildren<Image>();
        textComponent = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if (item != null) {
            //Debug.Log ("item not null");
            textComponent.text = item.itemName;
            Color newColor = imageComponent.color;
            newColor.a = 1;
            imageComponent.color = newColor;
            imageComponent.sprite = item.image;
        } else {
            //Debug.Log("item null");
            textComponent.text = "";
            Color newColor = imageComponent.color;
            newColor.a = 0;
            imageComponent.color = newColor;
        }
    }
 
}
