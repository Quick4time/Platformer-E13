using UnityEngine;
using System.Collections;

public class MoveTow : MonoBehaviour {

    public Transform target;
    public float speed = 2.06f;    
    bool stoptimer;

    void Start()
    {
        stoptimer = false;
    }

    void Update()
    {
        Fuck();
    }

	// Update is called once per frame
	void Fuck () {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        if (!stoptimer)
        {
            Debug.Log(Time.time);
        }
        if (transform.position == target.position)
        {
            stoptimer = true;
        }   
	}
}
