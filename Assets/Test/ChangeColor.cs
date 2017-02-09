using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
           // print("CollidENTER");
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
           // print("CollidEXIT");
        }
    }
}
