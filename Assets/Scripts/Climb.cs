using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Controller2D))]
public class Climb : MonoBehaviour {

    GameObject[] StartClimb;
    string tagStartClimb = "ClimbDestStart";
    GameObject[] EndClimb;
    string tagEndClimb = "ClimbDestEnd";
    float speedMoveTorLow = 1.8f;//0.84//1.8//различная скорость перемещения персонажа
    float speedMoveTorHigh = 0.84f;
    [HideInInspector]
    public Animator myAnimator;
    Controller2D controller;
    RayCastClimb ray;
    GameObject rayObj;
    Player playScr;
    GameObject PlayObj;

    bool GetKeyClimb = false;
    bool isMovingLow = false;
    bool isMovingHigh = false;
    bool isTransform = false;

    void Start () {
        GetKeyClimb = Input.GetKeyDown(KeyCode.Space);
        myAnimator = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        rayObj = GameObject.FindGameObjectWithTag("Triggers");
        ray = (RayCastClimb)rayObj.GetComponent(typeof(RayCastClimb));
        PlayObj = GameObject.FindGameObjectWithTag("Player");
        playScr = (Player)PlayObj.GetComponent(typeof(Player));
        StartClimb = GameObject.FindGameObjectsWithTag(tagStartClimb);
        EndClimb = GameObject.FindGameObjectsWithTag(tagEndClimb);
    }
		
	void Update () {
        ClimbPos();
    }
    IEnumerator DelayLow (float sec)
    { 
        playScr.SetAllVelStatus(true);
        isMovingLow = true;
        yield return new WaitForSeconds(sec);
        isMovingLow = false;
        playScr.SetAllVelStatus(false);
    }
    IEnumerator DelayHigh (float sec)
    {
        playScr.SetAllVelStatus(true);
        isMovingHigh = true;
        yield return new WaitForSeconds(sec);
        isMovingHigh = false;
        playScr.SetAllVelStatus(false);
    }

    public void ClimbPos()
    {
        GameObject NearstEndClimbPos = EndClimb[0];
        float shortClimbEnd = Vector2.Distance(transform.position, EndClimb[0].transform.position);
        float stepEndLow = speedMoveTorLow * Time.deltaTime;
        float stepEndHigh = speedMoveTorHigh * Time.deltaTime;

        foreach (GameObject obj in EndClimb)
        {
            float Distance = Vector2.Distance(transform.position, obj.transform.position);
            if (Distance < shortClimbEnd)
            {
                NearstEndClimbPos = obj;
                shortClimbEnd = Distance;
            }
        }
        GameObject NearstStartClimbPos = StartClimb[0];
        float shortClimbStart = Vector2.Distance(transform.position, StartClimb[0].transform.position);
        foreach (GameObject obj in StartClimb)
        {
            float Distance = Vector2.Distance(transform.position, obj.transform.position);
            if (Distance < shortClimbStart)
            {
                NearstStartClimbPos = obj;
                shortClimbStart = Distance;
            }
        }

        if (ray.climbHighHit && !controller.isGrounded) // !!!!!!!StuckWall mb убрать !!!!!// в конце кода возможно сделать return null.
        {
            transform.position = new Vector2(NearstStartClimbPos.transform.position.x, NearstStartClimbPos.transform.position.y);
            StartCoroutine(DelayHigh(2.5f));
        }
        if (isMovingHigh)
        {
            myAnimator.SetBool("Climb", true);
            transform.position = Vector2.MoveTowards(transform.position, NearstEndClimbPos.transform.position, stepEndHigh);
        }
        if (transform.position == NearstEndClimbPos.transform.position)
        {
            myAnimator.SetBool("Climb", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && playScr.isGrounded && ray.climbLowHit)
            {
                transform.position = new Vector2(NearstStartClimbPos.transform.position.x, NearstStartClimbPos.transform.position.y);
                StartCoroutine(DelayLow(1.2f));// 2.5f // 1.2f// различный дилей персонажа//
            }
        if (isMovingLow)
            {
                myAnimator.SetBool("ClimbLowGround", true);
                transform.position = Vector2.MoveTowards(transform.position, NearstEndClimbPos.transform.position, stepEndLow);
            }
        if (transform.position == NearstEndClimbPos.transform.position)

            {   
                myAnimator.SetBool("ClimbLowGround", false);
            }
        /*
        if (Input.GetKeyDown(KeyCode.Space) && playScr.isGrounded && ray.climbLowHit)
            {
                StartCoroutine(Delay(1.2f));
                if (isMoving)
                {
                    myAnimator.SetBool("ClimbLowGround", true);
                    transform.position = Vector2.MoveTowards(transform.position, NearstEndClimbPos.transform.position, stepEnd);  
                }
                if (transform.position == NearstEndClimbPos.transform.position)
                    myAnimator.SetBool("ClimbLowGround", false);

            }
        /*
        if (!playScr.isGrounded && ray.climbLowHit)
            {
                StartCoroutine(Delay(1.2f));
                if (isMoving)
                {
                    myAnimator.SetBool("ClimbLowAir", true);
                    transform.position = Vector2.MoveTowards(transform.position, NearstEndClimbPos.transform.position, stepEnd);    
                }
                if (transform.position == NearstEndClimbPos.transform.position)
                    myAnimator.SetBool("ClimbLowAir", false);
            }*/
    }
}

