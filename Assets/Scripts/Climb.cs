using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RaycastManager))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Controller2D))]
public class Climb : MonoBehaviour {

    GameObject[] StartClimb;
    string tagStartClimb = "ClimbDestStart";
    float speedMoveTorLow = 1.8f;//0.84//1.8//различная скорость перемещения персонажа
    float speedMoveTorHigh = 0.84f;
    [HideInInspector]
    public Animator myAnimator;
    private Controller2D controller;
    private RaycastManager ray;
    private GameObject rayObj;
    private Player playScr;
    private GameObject PlayObj;

    bool isMovingLow = false;
    bool isMovingHigh = false;
    bool isTransform = false;

    void Start () {
        ray = GetComponent<RaycastManager>();   
        myAnimator = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        StartClimb = GameObject.FindGameObjectsWithTag(tagStartClimb);
        PlayObj = GameObject.FindGameObjectWithTag("Player");
        playScr = (Player)PlayObj.GetComponent(typeof(Player));
        rayObj = GameObject.FindGameObjectWithTag("Triggers");
        ray = (RaycastManager)rayObj.GetComponent(typeof(RaycastManager));
    }
		
	void Update () {
        ClimbPos();
    }
    IEnumerator moveTo(float posX, float posY, float speed)
    {
        while ((transform.position.x != posX) && (transform.position.y != posY))
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(posX, posY), speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Delay (float sec)
    { 
        playScr.SetAllVelStatus(true);
        yield return new WaitForSeconds(sec);
        myAnimator.SetBool("ClimbLowGround", false);
        myAnimator.SetBool("ClimbHigh", false);
        playScr.SetAllVelStatus(false);
    }
    /*
    IEnumerator DelayHigh (float sec)
    {
        playScr.SetAllVelStatus(true);
        isMovingHigh = true;
        yield return new WaitForSeconds(sec);
        isMovingHigh = false;
        playScr.SetAllVelStatus(false);
    }*/

    public void ClimbPos()
    {
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
        if (Input.GetKeyDown(KeyCode.Space) && ray.climbLowHit && controller.isGrounded)
        {
            if(controller.collisions.faceDir > 0 )
            {
                transform.position = new Vector2(NearstStartClimbPos.transform.position.x, NearstStartClimbPos.transform.position.y);
                myAnimator.SetBool("ClimbLowGround", true);
                StartCoroutine(Delay(1.2f));
                StartCoroutine(moveTo(transform.position.x + 0.61042f, transform.position.y + 1.985699f, 1.8f));
            }
            if (controller.collisions.faceDir < 0)
            {
                transform.position = new Vector2(NearstStartClimbPos.transform.position.x, NearstStartClimbPos.transform.position.y);
                myAnimator.SetBool("ClimbLowGround", true);
                StartCoroutine(Delay(1.2f));
                StartCoroutine(moveTo(transform.position.x - 0.61042f, transform.position.y + 1.985699f, 1.8f));
            }
        }
        if (ray.climbHighHit && !controller.isGrounded)
        {
            if (controller.collisions.faceDir > 0)
            {
                transform.position = new Vector2(NearstStartClimbPos.transform.position.x, NearstStartClimbPos.transform.position.y);
                myAnimator.SetBool("ClimbHigh", true);
                StartCoroutine(Delay(1.8f));
                StartCoroutine(moveTo(transform.position.x + 0.61042f, transform.position.y + 1.985699f, 1.8f));
            }
            if (controller.collisions.faceDir < 0)
            {
                transform.position = new Vector2(NearstStartClimbPos.transform.position.x, NearstStartClimbPos.transform.position.y);
                myAnimator.SetBool("ClimbHigh", true);
                StartCoroutine(Delay(1.8f));
                StartCoroutine(moveTo(transform.position.x - 0.61042f, transform.position.y + 1.985699f, 1.8f));
            }
        }
        /*
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

