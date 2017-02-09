using UnityEngine;
using System.Collections;

public class OnTriggerEnter : MonoBehaviour {

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("OnTriggerEnter");
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("OnTriggerStay");
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("OnTriggerExit");
        }
    }
}
