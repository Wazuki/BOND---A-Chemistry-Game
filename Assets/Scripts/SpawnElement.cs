using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpawnElement : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler {

    public GameManager gameManager;
    private RectTransform elementPanelRectTransform;
    private GameObject[] tiles;
    private GameObject[] elements;

    private bool newElementActive;
    public GameObject elementSpawned;

    private void Awake()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        elements = GameObject.FindGameObjectsWithTag("Element");
        elementPanelRectTransform = GameObject.Find("ElementPanel").transform as RectTransform;
        foreach(GameObject e in elements)
        {
            Debug.Log(e.name);
        }
        elementSpawned = null;
        newElementActive = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        Vector2 mousePos = data.position;

        if(RectTransformUtility.RectangleContainsScreenPoint(elementPanelRectTransform, mousePos) && !newElementActive)
        {
            for(int x = 0; x < tiles.Length; x++)
            {
                GameObject t = tiles[x];
                if(RectTransformUtility.RectangleContainsScreenPoint(t.transform as RectTransform, mousePos))
                {
                    string prefab = t.name.Remove(t.name.Length - 4);
                    Debug.Log("Spawning " + prefab + ".");

                    foreach(GameObject e in elements)
                    {
                        if(prefab == e.name)
                        {
                            elementSpawned = Instantiate(e);
                            elementSpawned.name = elementSpawned.name.Remove(elementSpawned.name.Length - 7);
                        }
                    }

                    elementSpawned.layer = LayerMask.NameToLayer("Ignore Raycast");
                    newElementActive = true;
                    break;
                }
            }
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (elementSpawned == null) return;

        if(newElementActive)
        {
            float distance = Vector3.Distance(elementSpawned.transform.position, Camera.main.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(data.position);
            Vector3 rayPoint = ray.GetPoint(distance);
            rayPoint.z = 0;
            elementSpawned.transform.position = rayPoint;
        }

    }

    public void OnEndDrag(PointerEventData data)
    {
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
        Debug.Log(hit.transform.name);

        bool elementValid = false;
        if(gameManager.elementsInPlay.Count < 11)
        {
            //Find the first open free holder
            foreach(GameObject h in gameManager.elementOrbits)
            {
                if(h.transform.childCount == 0)
                {
                    //elementSpawned.transform.SetParent(h.transform);
                    var script = elementSpawned.GetComponent<MoveToLocOverTime>();
                    script.moveToLoc = h.transform;
                    //script.MoveTo();
                    //elementSpawned.transform.position = h.transform.position;

                    elementValid = true;
                    break;
                }
            }
            gameManager.elementsInPlay.Add(elementSpawned);
        }
        if(!elementValid) Destroy(elementSpawned);
        elementSpawned = null;
        newElementActive = false;
        Debug.Log("Drag ended.");
    }
}
