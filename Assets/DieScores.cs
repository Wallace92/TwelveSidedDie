using System;
using UnityEngine;

public class DieScores : MonoBehaviour
{
    private TwelveSideDieController m_twelveSideDieController;

    private void Awake()
    {
        m_twelveSideDieController = GetComponent<TwelveSideDieController>();
    }

    public void StopMovement()
    {
        Debug.Log("Stop");
        
        RaycastHit[] hits = Physics.RaycastAll(m_twelveSideDieController.transform.position, Vector3.up, 4.0f);

        // Sort the raycast hits based on distance from the starting position of the raycast.
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        if (hits.Length > 0)
        {
            GameObject topFace = hits[0].collider.gameObject;
            
            if (topFace.GetComponent<OneSideDie>() == null)
                return;
            
            int topFaceNumber = topFace.GetComponent<OneSideDie>().Number;
            
            Debug.Log("The number on the top face is: " + topFaceNumber);
        }
    }
}