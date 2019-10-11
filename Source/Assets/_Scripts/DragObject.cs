using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour
{
    
    private MainCanvas canvas;

    void Awake () {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvas>();
    }

    public void OnBeginDrag () {
        canvas.OnBeginDrag(this);
    }

    public void OnDrag() {
        canvas.OnDrag();
    }

    public void OnEndDrag() {
        canvas.OnEndDrag();
    }

}
