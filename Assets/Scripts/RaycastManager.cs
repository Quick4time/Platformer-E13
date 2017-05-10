using UnityEngine;
using System.Collections;

public class RaycastManager : MonoBehaviour {

    public static RaycastManager instance;

    private GameObject StartRayStuck;
    private GameObject[] stuckray;
    //public Transform StartDir, EndDirleft, EndDirright;
    private GameObject[] rayLow;
    private GameObject[] rayHigh;

    public bool climbHighHit = false;
    public bool climbLowHit = false;
    public bool stuckWall = false;
    /*public bool left = false;
    public bool right = false;*/

    LayerMask stuck;
    LayerMask climb;
    LayerMask crouch;

    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }   
    void Start ()
    {
        rayLow = new GameObject[2];
        rayLow[0] = GameObject.FindGameObjectWithTag("RayLowStart");
        rayLow[1] = GameObject.FindGameObjectWithTag("RayLowEnd");

        rayHigh = new GameObject[2];
        rayHigh[0] = GameObject.FindGameObjectWithTag("RayHighStart");
        rayHigh[1] = GameObject.FindGameObjectWithTag("RayHighEnd");
        
        stuckray = new GameObject[3];
        stuckray[0] = GameObject.FindGameObjectWithTag("LowStuckRayEnd");
        stuckray[1] = GameObject.FindGameObjectWithTag("MedStuckRayEnd");
        stuckray[2] = GameObject.FindGameObjectWithTag("HighStuckRayEnd");
        StartRayStuck = GameObject.FindGameObjectWithTag("StartRay");

        crouch = LayerMask.GetMask("Obstacle");
        stuck = LayerMask.GetMask("Default", "Obstacle");
        climb = LayerMask.GetMask("RayClimb");
    }
    
    void Update()
    {
        Raycasting();
        if (this.gameObject == null)
        {
            Debug.Log("HeIsMissing!!");
        }
    }
    void Raycasting()
    {
        Debug.DrawLine(rayHigh[0].transform.position, rayHigh[1].transform.position, Color.green);
        climbHighHit = Physics2D.Linecast(rayHigh[0].transform.position, rayHigh[1].transform.position, climb);
        
        Debug.DrawLine(rayLow[0].transform.position, rayLow[1].transform.position, Color.blue);
        climbLowHit = Physics2D.Linecast(rayLow[0].transform.position, rayLow[0].transform.position, climb);
        
        Debug.DrawLine(StartRayStuck.transform.position, stuckray[2].transform.position, Color.yellow);
        Debug.DrawLine(StartRayStuck.transform.position, stuckray[1].transform.position, Color.yellow);
        Debug.DrawLine(StartRayStuck.transform.position, stuckray[0].transform.position, Color.yellow);
        stuckWall = Physics2D.Linecast(StartRayStuck.transform.position, stuckray[2].transform.position, stuck) || Physics2D.Linecast(StartRayStuck.transform.position, stuckray[1].transform.position, stuck) || Physics2D.Linecast(StartRayStuck.transform.position, stuckray[0].transform.position, stuck);
        
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
