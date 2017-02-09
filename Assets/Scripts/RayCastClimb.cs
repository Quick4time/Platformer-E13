using UnityEngine;
using System.Collections;

public class RayCastClimb : MonoBehaviour {

    public Transform StartRayStuck;

    public Transform highStuckRayEnd, medStuckRayEnd, lowStuckRayEnd;
    //public Transform StartDir, EndDirleft, EndDirright;

    public Transform rayLowStart, rayLowEnd;
    public Transform rayHighStart, rayHighEnd;

    public bool climbHighHit = false;
    public bool climbLowHit = false;
    public bool stuckWall = false;
    /*public bool left = false;
    public bool right = false;*/

    LayerMask stuck;
    LayerMask climb;

    void Start ()
    {
        stuck = LayerMask.GetMask("Default", "Obstacle");
        climb = LayerMask.GetMask("RayClimb");
    }
    
    void Update()
    {
        Raycasting();
    }
    void Raycasting()
    {
        Debug.DrawLine(rayHighStart.position, rayHighEnd.position, Color.green);
        climbHighHit = Physics2D.Linecast(rayHighStart.position, rayHighEnd.position, climb);

        Debug.DrawLine(rayLowStart.position, rayLowEnd.position, Color.blue);
        climbLowHit = Physics2D.Linecast(rayLowStart.position, rayLowEnd.position, climb);

        Debug.DrawLine(StartRayStuck.position, highStuckRayEnd.position, Color.yellow);
        Debug.DrawLine(StartRayStuck.position, medStuckRayEnd.position, Color.yellow);
        Debug.DrawLine(StartRayStuck.position, lowStuckRayEnd.position, Color.yellow);
        stuckWall = Physics2D.Linecast(StartRayStuck.position, highStuckRayEnd.position, stuck) || Physics2D.Linecast(StartRayStuck.position, medStuckRayEnd.position, stuck) || Physics2D.Linecast(StartRayStuck.position, lowStuckRayEnd.position, stuck);

        /*Debug.DrawLine(StartDir.position, EndDirleft.position, Color.red);
        Debug.DrawLine(StartDir.position, EndDirright.position, Color.red);
        left = Physics2D.Linecast(StartDir.position, EndDirleft.position, stuck);
        right = Physics2D.Linecast(StartDir.position, EndDirright.position, stuck);*/

    }
	/*void Update () {
        RaycastHit2D hit;
        float theDistance;

        Vector3 forward = transform.TransformDirection(Vector2.right) * 10;
        Debug.DrawRay(transform.position, forward, Color.blue);

        if (Physics2D.Raycast(transform.position, (forward)))
        {
            theDistance = hit.distance;
            print(theDistance + "" + hit.collider.gameObject.name);
        }
	}*/
}
