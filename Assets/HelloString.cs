using UnityEngine;
using System.Collections;

public class HelloString : MonoBehaviour {

    public string _string;

    // Use this for initialization
     IEnumerator fuck (float t)
    {
        yield return new WaitForSeconds(t);
        Debug.LogError(_string);
    }

    void Start () {
        StartCoroutine(fuck(10f));
	}
}
