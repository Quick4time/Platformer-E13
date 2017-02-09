using UnityEngine;
using System.Collections;

public class MoweLerp : MonoBehaviour {
    // Для более фиксированого движения, которое нельзя изменять во время проигрывания.
    public Transform target;
    float totalDistanceToDestination;
    float startTime;
    public float speed;

    void Start ()
    {
        startTime = Time.time;
        totalDistanceToDestination = Vector2.Distance(transform.position, target.position);
    }
	
	void Update ()
    {
        float currentDuration = (Time.time - startTime) * speed;
        float journeyfraction = currentDuration / totalDistanceToDestination;
        transform.position = Vector2.Lerp(transform.position, target.position, journeyfraction);
        if (transform.position != target.position)
        {
            Debug.Log(string.Format("Duration {0} -- Distance {1}", currentDuration, totalDistanceToDestination));
        }
        else
        {
            Debug.Log("Time");
        }
        
    }
}
