//GameManager.cs - Handles all remaining game-management functions.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private GameObject playfield; //Playfield objects that elements attach to.
    private GameObject elementHolderParent;
    public List<GameObject> elementOrbits; //Orbits of elements that spawned elements parent to.
    public List<GameObject> elementsInPlay; //List of current elements in play.
    private float playfieldRadius = 5; //TODO - Make this a detectable value instead of a hard coded value.

    public const int MAX_ELEMENTS_IN_PLAY = 11; //Maximum number of elements that can exist on the playfield.

	// Use this for initialization
	void Start ()
    {
        playfield = GameObject.Find("Playfield");
        elementHolderParent = GameObject.Find("ElementHolders");
        elementOrbits = new List<GameObject>();
        elementsInPlay = new List<GameObject>(MAX_ELEMENTS_IN_PLAY); //Instantiate the elementsInPlay list with a maximum size equal to the max number of possible elements in play.
        InitElementHolders();
	}
	
	// Update is called once per frame
	void Update ()
    {
        RotateElementHolders(); //Rotate the element holders (and in turn the elements attached to them)
	}

    void InitElementHolders()
    {
        //Initialize the element holders: 1 center; 2 mid; 8 far
        Vector3 newPos = new Vector3(0, 0);

        //Center element holder
        GameObject holder = new GameObject("ElementHolder1");
        holder.transform.position = newPos;
        holder.transform.SetParent(elementHolderParent.transform);
        elementOrbits.Add(holder);
        
        //Two mid element holders - each at a 180 degree increment.
        for(int e = 0; e < 2; e++)
        {
            newPos.x = 0 + playfieldRadius / 2 * Mathf.Cos(Mathf.Deg2Rad * 180 * e);
            newPos.y = 0 + playfieldRadius / 2 * Mathf.Sin(Mathf.Deg2Rad * 180 * e);

            GameObject newHolder = new GameObject("ElementHolder" + (elementOrbits.Count + 1));
            newHolder.transform.position = newPos;
            newHolder.transform.SetParent(elementHolderParent.transform);
            elementOrbits.Add(newHolder);
        }

        //Eight exterior element holders - each at a 45 degree increment
        for(int e = 0; e < 8; e++)
        {
            newPos.x = 0 + playfieldRadius * Mathf.Cos(Mathf.Deg2Rad * 45 * e);
            newPos.y = 0 + playfieldRadius * Mathf.Sin(Mathf.Deg2Rad * 45 * e);

            GameObject newHolder = new GameObject("ElementHolder" + (elementOrbits.Count + 1));
            newHolder.transform.position = newPos;
            newHolder.transform.SetParent(elementHolderParent.transform);
            elementOrbits.Add(newHolder);
        }
    
        

    }

    void RotateElementHolders()
    {
        //Rotate each fo the element holders around the center of the playfield.
        foreach(GameObject e in elementOrbits)
        {
            e.transform.RotateAround(playfield.GetComponent<Renderer>().bounds.center, Vector3.forward, 10 * Time.deltaTime);
        }
    }
}
