//SpawnElement.cs -- Handles the spawning of new elements off of the base ones hidden in the holders.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpawnElement : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler {

    //Stored definitions for important variables.
    public GameManager gameManager;
    private RectTransform elementPanelRectTransform;
    private GameObject[] tiles;
    private GameObject[] elements;

    private bool newElementActive;
    public GameObject elementSpawned;

    private void Awake()
    {
        //Find each element tile as wel as each element to be cloned in the spawning with FindGameObjectsWithTag
        tiles = GameObject.FindGameObjectsWithTag("Tile"); //Finds each tile and stores it in the array tiles
        elements = GameObject.FindGameObjectsWithTag("Element"); //Finds each element and stores it in the arrays.
        elementPanelRectTransform = GameObject.Find("ElementPanel").transform as RectTransform; //Find the transform of the element panel where the player will grab the elements from

        //Reset created variables.
        elementSpawned = null; //Serves as the location for new elements. Null prevents OnDrag from repeatedly calling or throwing an exception.
        newElementActive = false; //If there is a new element active and currently on the mouse.
    }

    //Spawning elements based on the player clicking in the element panel.
    public void OnPointerDown(PointerEventData data)
    {
        Vector2 mousePos = data.position; //Stores the position of the mouse pointer

        //If the player clicks inside the element panel AND an element is new spawned element isn't currently active, spawn a new one if the player clicked on a tile.
        if(RectTransformUtility.RectangleContainsScreenPoint(elementPanelRectTransform, mousePos) && !newElementActive)
        {
          //Find the tile clicked on by the player.
            for(int x = 0; x < tiles.Length; x++)
            {
                GameObject t = tiles[x]; //Retrieve the tile from the array for ease of comparison.
                if(RectTransformUtility.RectangleContainsScreenPoint(t.transform as RectTransform, mousePos)) //Check to see if the player has clicked on the tile currently being iterated. RectTransformUtility.RectangleContainsScreenPoint is used for the conversion.
                {
                    string newEleName = t.name.Remove(t.name.Length - 4); //Remove the word suffix from the suffix from the tile and use it to find the proper element name.

                    //Iterate through the elements and find hte element with the matching name.
                    foreach(GameObject e in elements)
                    {
                        if(newEleName == e.name) //If the element's name matches the one called up above, spawn it.
                        {
                            elementSpawned = Instantiate(e); //Instantiates the new element.
                            elementSpawned.name = elementSpawned.name.Remove(elementSpawned.name.Length - 7); //Trims the element's name to remove the "(Clone)" suffix.
                            break; //End the loop - found the correct element.
                        }
                    }

                    elementSpawned.layer = LayerMask.NameToLayer("Ignore Raycast"); //Prevent the element from throwing false collision when checking for the playfield collision later.
                    newElementActive = true; //Declare that a new element to prevent further elements from spawning.
                    break; //End the loop - found the correct tile/element pair.
                }
            }
        }
    }

    //Responsible for snapping the currently spawned element to the mouse cursor.
    public void OnDrag(PointerEventData data)
    {
        if (elementSpawned == null) return; //Prevents OnDrag from continuing if there's no element currently spawned.

        if(newElementActive) //If there is a new element currently active, snap it to the mouse cursor.
        {
            //Based on other examples. Uses rays and the distance between the camera and the elementSpawned position to find the proper location. Avoid editing.
            float distance = Vector3.Distance(elementSpawned.transform.position, Camera.main.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(data.position);
            Vector3 rayPoint = ray.GetPoint(distance);
            rayPoint.z = 0;
            elementSpawned.transform.position = rayPoint;
        }

    }

    //Handles what happens when the player releases the mouse cursor and drops the element onto the playfield.
    public void OnEndDrag(PointerEventData data)
    {
        //Find where the new element currently is, based on the mouse cursor position and using Camera.main.ScreenToWorldPoint to translate it to the actual world position.
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f); //Find the object hit by the ray.

        bool elementValid = false;

        //Only allow the element to be added if there are less than the max number of elements in play.
        if(gameManager.elementsInPlay.Count < 11) //TODO - Remove the hardcoding from this line.
        {
            //Find the first open free holder
            foreach(GameObject h in gameManager.elementOrbits)
            {
                if(h.transform.childCount == 0) //If the holder has no children (ie, is "free")
                {
                    //Call the "MoveToLocOverTime()" function from the element to allow it to transition to its new location via LERP
                    var script = elementSpawned.GetComponent<MoveToLocOverTime>();
                    script.moveToLoc = h.transform;

                    elementValid = true; //Set to true to destroy the element in case it is invalid (too many elements, for instance)
                    break;
                }
            }
            gameManager.elementsInPlay.Add(elementSpawned); //Add the new element to the total elements in play.
        }
        if(!elementValid) Destroy(elementSpawned); //Destroy the spawned element if it isn't valid.
        elementSpawned = null; //Reset the elemetnSpawned to null for OnDrag
        newElementActive = false; //Reset that no element is active.
    }
}
