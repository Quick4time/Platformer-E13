using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Flip : MonoBehaviour {

    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().flipX = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Flipp();
    }
    void Flipp()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}

