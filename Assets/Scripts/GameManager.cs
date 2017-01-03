using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private GameObject playfield;
    private GameObject elementHolderParent;
    public List<GameObject> elementOrbits;
    public List<GameObject> elementsInPlay;
    private float playfieldRadius = 5;

    public const int MAX_ELEMENTS_IN_PLAY = 11;

	// Use this for initialization
	void Start ()
    {
        playfield = GameObject.Find("Playfield");
        elementHolderParent = GameObject.Find("ElementHolders");
        elementOrbits = new List<GameObject>();
        elementsInPlay = new List<GameObject>(MAX_ELEMENTS_IN_PLAY);
        InitElementHolders();
	}
	
	// Update is called once per frame
	void Update ()
    {
        RotateElementHolders();
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
        
        //Two mid element holders
        for(int e = 0; e < 2; e++)
        {
            newPos.x = 0 + playfieldRadius / 2 * Mathf.Cos(Mathf.Deg2Rad * 180 * e);
            newPos.y = 0 + playfieldRadius / 2 * Mathf.Sin(Mathf.Deg2Rad * 180 * e);

            GameObject newHolder = new GameObject("ElementHolder" + (elementOrbits.Count + 1));
            newHolder.transform.position = newPos;
            newHolder.transform.SetParent(elementHolderParent.transform);
            elementOrbits.Add(newHolder);
        }

        //Eight exterior element holders
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
        foreach(GameObject e in elementOrbits)
        {
            e.transform.RotateAround(playfield.GetComponent<Renderer>().bounds.center, Vector3.forward, 10 * Time.deltaTime);
        }
    }
}
