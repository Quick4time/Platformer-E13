using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour ,IListener
{
    //public Transform target;
    //public Transform climbStartR, climbEndR, ClimbStartL, ClimbEndL;

    private Rigidbody2D myRidgidbody;
    [HideInInspector]
    public Animator myAnimator;
    [SerializeField]
    private Transform[] GroundPoint;
    [SerializeField]
    public float groundRadius;

    public bool isGrounded;
    [SerializeField]
    private LayerMask WhatIsGround;
    //public bool cantmove;

    public int FallBoundary = -20;
    public float maxJumpHeight = 6; // максимальная высота прыжка.
    public float minJumpHeight = 6; // минимальная высота прыжка.
    public float timeToJumpApex = .2f; // за сколько времени игрок достигнет высшей точки прыжка.
    float accelerationTimeAirborne = 4f; // ускорение в воздухе при изменении направления по оси x. (Важно!!!) 
    public float accelerationTimeGrounded = 0.13f; // Ускорение на земле при изменении направления по оси x. (Важно!!!) заторможеннось придвежении.
    public bool crouch = false;
    
    public int Speed
    {
        get { return _speed; } 
        set
        {
            _speed = Mathf.Clamp(value, 0, 12); // границы значения скорости.
            //EventManager.Instance.PostNotification(EVENT_TYPE.SPEED_CHANGE, this, _speed);         
        }
    }
    private int _speed = 6;
    public int roolspeed = 1;

    
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    //float minJumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;
    [HideInInspector]
    public Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;
    public bool canMove = true;

    RayCastClimb rayclimb;
    GameObject rayObj;

    //public BoxCollider2D b;
    void Start()
    {
        //b = this.GetComponent<BoxCollider2D>();
        myRidgidbody = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller2D>();
        myAnimator = GetComponent<Animator>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2); // расчет гравитации по отношению к maxJumpHeight и timeTojumpApex. 
        print("Gravity " + gravity + "MaxjumpHeight " + maxJumpHeight + "time To Jump Apex " + timeToJumpApex);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight); 
        rayObj = GameObject.FindGameObjectWithTag("Triggers");
        rayclimb = (RayCastClimb)rayObj.GetComponent(typeof(RayCastClimb));
        EventManager.Instance.AddListener(EVENT_TYPE.SPEED_CHANGE, this);
    }

    void Update()
    {
        //ClimbPos();
        CalculateVelocity();

        controller.Move(velocity * Time.deltaTime, directionalInput);

            if (controller.collisions.above || controller.collisions.below) // если происходит столкновение с нижней или верхней поверхностью.
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
        if (transform.position.y <= FallBoundary)
        {
            Debug.Log("you are dead");
        }
        if (velocity.y < 0)
        {
            myAnimator.SetBool("fall", true);
            myAnimator.ResetTrigger("jump");
        }
        if (rayclimb.stuckWall)
        {
            velocity.x = 0;
        }
        myAnimator.SetFloat("speed", Mathf.Abs(velocity.x));
    }

    public void OnEvent (EVENT_TYPE Event_type, Component Sender, object Param = null)
    {
        switch(Event_type)
        {
            case EVENT_TYPE.SPEED_CHANGE:
                OnSpeedChange(Sender, (int)Param);
                break;
        }
    }

    void OnSpeedChange (Component Player, int NewSpeed)
    {
        if (this.GetInstanceID() != Player.GetInstanceID()) return;
        
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }
    public void OnJumpInputDown()
    {
        #region Говнокод
        /*
        if (directionalInput.x > 0 && velocity.y > 0)
        {
            directionalInput.x = velocity.x * jumpRange;
        }
        else if (directionalInput.x < 0 && velocity.y > 0)
        {
            directionalInput.x = velocity.x *  -jumpRange;
        }
        */
        #endregion
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                { // not jumping against max slope // создать таг коллайдера при котором у персонажа будет возможность прыгать с descending slope (крутой наклонности) 
                    // Возможно Удаление
                    /*velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;*/
                    // ВАЖНО ДЛЯ СЪЕЗДА ПО НАКЛОННОЙ ПОВЕРХНОСТИ ИСПОЛЬЗОВАТЬ РИГИБОДИ, А ИМЕННО MASS AND GRAVITY SCALE/
                    velocity.y = maxJumpVelocity;
                }
            }
            else 
            {
                velocity.y = maxJumpVelocity;
            }
            /*if (directionalInput.x != 0)
            {
                _speed = 10;
                accelerationTimeAirborne = 10f;
                timeToJumpApex = 1f;
            }
            else
            {
                _speed = 10;
                accelerationTimeAirborne = 4f;
                timeToJumpApex = .2f;
            }*/
        }
    }

   
    #region Велосити статус
    public void SetAllVelStatus(bool disable) // отключение коллайдеров.
    {
            if (disable)
            {
                velocity = Vector2.zero;
                myRidgidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                gravity = gravity - gravity;
                canMove = false;
                print("true");
                rayclimb.stuckWall = false;
            }
            if (!disable)
            {
                myRidgidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                gravity = -43.7f;
                canMove = true;
                print("false");  
            }        
    }
    #endregion
    /*void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }*/
    void FixedUpdate ()
    {
        HandleLayers();
        //HandleWallSliding();
        isGrounded = IsGrounded();
        HandleLayers();
    }
 public bool IsGrounded()
    {
        if (velocity.y <= 0)
        {
            foreach (Transform point in GroundPoint)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, WhatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("fall", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private void HandleLayers()
    {
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
            controller.flip = false;
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
            controller.flip = true;
        }
    }
     void CalculateVelocity() // отключить метод когда коллайд с grapple.
    {
        if (canMove)
        {
            float targetVelocityX = directionalInput.x * _speed; // указывем скорость премещения по оси х.
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne); // указывем характер передвижения по оси х.
            velocity.y += -43.7f * Time.deltaTime; // указываем гравитацию по оси y.
        }
        if (!canMove)
        {
            Debug.Log("Dont Move!");
        }       
    }

}
