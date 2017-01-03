using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelCollapseTest : MonoBehaviour, IPointerDownHandler {

    private Vector2 pointerOffset;
    private RectTransform canvasRectTransform;
    private RectTransform elementPanelRectTransform;

    private bool panelToggled;
    private GameObject[] tiles;
    private void Awake()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if(canvas != null)
        {
            canvasRectTransform = canvas.transform as RectTransform;
            elementPanelRectTransform = transform.parent as RectTransform; //Parent because the script is on the drag zone
        }

        tiles = GameObject.FindGameObjectsWithTag("Tile");
        panelToggled = true;
    }

    public void OnPointerDown(PointerEventData data)
    {
        elementPanelRectTransform.SetAsLastSibling(); //Forces object as last item on canvas so it will be rendered on top
        RectTransformUtility.ScreenPointToLocalPointInRectangle(elementPanelRectTransform, data.position, data.pressEventCamera, out pointerOffset);
    }

    public void TogglePanel()
    {
        Vector2 minSize = new Vector2(20, 327);
        Vector2 maxSize = new Vector2(103.25f, 327);

  

        if(panelToggled)
        {
            panelToggled = false;

            foreach(GameObject t in tiles)
            {
                t.SetActive(false);
            }

            elementPanelRectTransform.sizeDelta = minSize;
        }
        else
        {
            panelToggled = true;

            foreach (GameObject t in tiles)
            {
                t.SetActive(true);
            }

            elementPanelRectTransform.sizeDelta = maxSize;
        }
    }

    //Event handlers for actual dragging
    /* NOT USED
    public void OnDrag(PointerEventData data)
    {
        if (elementPanelRectTransform == null) return;

        Vector2 pointerPosition = ClampToWindow(data);
        Vector2 localPointerPosition;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, pointerPosition, data.pressEventCamera, out localPointerPosition))
        {
            elementPanelRectTransform.localPosition = localPointerPosition - pointerOffset;
        }
    }

    Vector2 ClampToWindow(PointerEventData data)
    {
        Vector2 rawPointerPosition = data.position;
        Vector3[] canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);

        float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
        float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

        Vector2 newPointerPosition = new Vector2(clampedX, clampedY);

        return newPointerPosition;
    } */
}
