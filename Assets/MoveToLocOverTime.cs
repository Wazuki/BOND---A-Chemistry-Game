using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocOverTime : MonoBehaviour {

    public Transform moveToLoc;
    
    public float timeToTake = 1f;
    private Transform _transform;

    private bool hasReachedTarget;

    private void Awake()
    {
        hasReachedTarget = false;
        moveToLoc = null;
        _transform = transform;
    }

    private void Update()
    {
        if(moveToLoc != null && !hasReachedTarget)
        {
            _transform.SetParent(moveToLoc);
            MoveTo();
        }
    }

    public void MoveTo()
    {
       _transform.position = Vector3.Lerp(_transform.position, moveToLoc.position, Time.deltaTime);
        
        if(_transform.position == moveToLoc.position)
        {
            hasReachedTarget = true;
        }

    }

    /*
    public IEnumerator MoveTo()
    {
        float t = 0;
        Vector3 originalPosition = _transform.position;
        hasReachedTarget = false;

        while(t < 1)
        {
            Debug.Log("Moving " + this.name);
            t += Time.deltaTime / timeToTake;
            //_transform.position = Vector3.Lerp(originalPosition, moveToLoc.position, t);
            _transform.position = Vector3.MoveTowards(originalPosition, moveToLoc.position, t);
            yield return null;
        }

        hasReachedTarget = true;
    }*/
}
