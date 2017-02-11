using UnityEngine;
using System.Collections;


public class Controller2D : RaycastController {


    public Transform player;
    private Animator myAnimator;
    public bool jump;
    public bool rool;
    public bool isGrounded;
    public float maxSlopeAngle = 80; // максимальный градус подъема.
    public CollisionInfo collisions;
	[HideInInspector]
	public Vector2 playerInput;
    public bool flip;

    public Vector3 Direction = Vector3.zero;
    public AnimationCurve AnimCurve;

   
    RayCastClimb rayclimb;
    GameObject rayObj;

    public override void Start() {
        base.Start ();
        collisions.faceDir = 1;
        GetComponent<SpriteRenderer>().flipX = false;
        rayObj = GameObject.FindGameObjectWithTag("Triggers");
        rayclimb = (RayCastClimb)rayObj.GetComponent(typeof(RayCastClimb));
        myAnimator = GetComponent<Animator>();
    }
	public void Move(Vector2 moveAmount, bool standingOnPlatform) {
		Move (moveAmount, Vector2.zero, standingOnPlatform);
	}

    public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false) {
		UpdateRaycastOrigins ();
		collisions.Reset ();
		collisions.moveAmountOld = moveAmount;
		playerInput = input;

        if (moveAmount.y < 0)
        {
			DescendSlope(ref moveAmount); // ссылка (ref) изменяет само значение ссылки.    
		}

		if (moveAmount.x != 0)
        {
			collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
		}

		HorizontalCollisions (ref moveAmount);
		if (moveAmount.y != 0) 
        {
			VerticalCollisions (ref moveAmount);
		}

		transform.Translate (moveAmount);

		if (standingOnPlatform) {
			collisions.below = true;
		}      
}
    void Update()
    {
        Flipp();
    }

    void Flipp()
    {
            if (collisions.faceDir > 0 && flip)
            {
                rayclimb.rayHighStart.transform.localPosition = new Vector2(0.4f, 2.5f);
                rayclimb.rayHighEnd.transform.localPosition = new Vector2(1.3f, 2.5f);

                rayclimb.rayLowStart.transform.localPosition = new Vector2(0.3f, -0.43f);
                rayclimb.rayLowEnd.transform.localPosition = new Vector2(1.3f, -0.43f);

                rayclimb.lowStuckRayEnd.transform.localPosition = new Vector2(0.772f, -2.13f);
                rayclimb.medStuckRayEnd.transform.localPosition = new Vector2(0.772f, 0f);
                rayclimb.highStuckRayEnd.transform.localPosition = new Vector2(0.772f, 1.811f);
                GetComponent<SpriteRenderer>().flipX = false;
            }
                if (collisions.faceDir < 0 && flip)
            {
                rayclimb.rayHighStart.transform.localPosition = new Vector2(-0.4f, 2.5f);
                rayclimb.rayHighEnd.transform.localPosition = new Vector2(-1.3f, 2.5f);

                rayclimb.rayLowStart.transform.localPosition = new Vector2(-0.3f, -0.43f);
                rayclimb.rayLowEnd.transform.localPosition = new Vector2(-1.3f, -0.43f);

                rayclimb.lowStuckRayEnd.transform.localPosition = new Vector2(-0.772f, -2.13f);
                rayclimb.medStuckRayEnd.transform.localPosition = new Vector2(-0.772f, 0f);
                rayclimb.highStuckRayEnd.transform.localPosition = new Vector2(-0.772f, 1.811f);
                GetComponent<SpriteRenderer>().flipX = true;
        }           
    }

    void HorizontalCollisions(ref Vector2 moveAmount) // тоже самое что и VerticalCollision за исключением оси с Y на X.
    {
		float directionX = collisions.faceDir;
		float rayLength = Mathf.Abs (moveAmount.x) + skinWidth;
        // Если будут проблемы с остановкой у стены убрать комментарий.       
        if (rayclimb.stuckWall)
        {
            moveAmount.x = 0;
            directionX = 0;
        }
        if (Mathf.Abs(moveAmount.x) < skinWidth) {
			rayLength = 2*skinWidth;
		}
        for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX,Color.red);
            if (hit) {

				if (hit.distance == 0) {
					continue;
				}

				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up); // определяем угол подьема.

                if (i == 0 && slopeAngle <= maxSlopeAngle) {
					if (collisions.descendingSlope) {
						collisions.descendingSlope = false;
						moveAmount = collisions.moveAmountOld;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAngleOld) {
						distanceToSlopeStart = hit.distance-skinWidth;
						moveAmount.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
					moveAmount.x += distanceToSlopeStart * directionX;
				}

				if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle) {
					moveAmount.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					if (collisions.climbingSlope) {
						moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
					}

					collisions.left = directionX == -1; // Если столкновение происходит в левой стороне то directionX == -1;
					collisions.right = directionX == 1; // Если столкновение происходит в правой стороне то directionX == 1;
                }
			}
		}
	}

   /* void IsGrounded(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;
        for (int i = 0; i < verticalRayCount; i ++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            {
                if (hit)
                {
                    print("isGrounded");
                }
            }
            moveAmount.y = (hit.distance - skinWidth) * directionY; 
            rayLength = hit.distance;
            collisions.below = directionY == -1;
            collisions.above = directionY == 1;
        }

    }*/
	void VerticalCollisions(ref Vector2 moveAmount) {
		float directionY = Mathf.Sign (moveAmount.y); // Указывем направления по оси y.
		float rayLength = Mathf.Abs (moveAmount.y) + skinWidth; // Длина рэйкаста

        if (collisions.below)
        {
            isGrounded = true;
            myAnimator.ResetTrigger("jump");
            myAnimator.SetBool("fall", false);
        }

        if (directionY > 0 && isGrounded && jump)
        {
            isGrounded = false;
            //myAnimator.SetBool("fall", false);
            myAnimator.SetTrigger("jump");
            jump = false;
        }

        if (directionY < 0 && !isGrounded)
        {
            myAnimator.ResetTrigger("jump");
            myAnimator.SetBool("fall", true);
        }

        for (int i = 0; i < verticalRayCount; i++) {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft; // если мы подпрыгиваем рэйкаст снизу(bottom) переходит на верх (top).
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask); // в каких направлениях и что может касаться рэйкаст.

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red); // Дебаг, линии появляющиеся в Gizmos.
            
            if (hit && collisions.below) // Is Grounded;
            {
                jump = true;
                isGrounded = true;
                myAnimator.ResetTrigger("jump");
                myAnimator.SetBool("fall", false);

            }
			if (hit) {
				if (hit.collider.tag == "Through") {
					if (directionY == 1 || hit.distance == 0) {
						continue;
					}
					if (collisions.fallingThroughPlatform) {
						continue;
					}
					if (playerInput.y == -1) {
						collisions.fallingThroughPlatform = true;
						Invoke("ResetFallingThroughPlatform",.5f);
						continue;
					}
				}

				moveAmount.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				if (collisions.climbingSlope) // для дальнейшего взбирания вверх при появлении подъема с другим углом.
                {
					moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
				}

				collisions.below = directionY == -1; // если столкновение происходит ниже то направление по y == -1;
				collisions.above = directionY == 1; // если столкновение происходит выше то направление по y == 1;
            }
		}

		if (collisions.climbingSlope) {
			float directionX = Mathf.Sign(moveAmount.x);
			rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin,Vector2.right * directionX,rayLength,collisionMask);

			if (hit) {
				float slopeAngle = Vector2.Angle(hit.normal,Vector2.up);
				if (slopeAngle != collisions.slopeAngle) {
					moveAmount.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
					collisions.slopeNormal = hit.normal;
				}
			}
		}
	}


	void ClimbSlope(ref Vector2 moveAmount, float slopeAngle, Vector2 slopeNormal) // метод угола подьема персонажа.
    {
		float moveDistance = Mathf.Abs (moveAmount.x); // Mathf.abs - всегда положительное число. передвижение по оси x
		float climbmoveAmountY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (moveAmount.y <= climbmoveAmountY) // позволяет нам прыгать при подьеме.
        {
			moveAmount.y = climbmoveAmountY;
			moveAmount.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (moveAmount.x);  
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
			collisions.slopeNormal = slopeNormal;
		}
	}

	void DescendSlope(ref Vector2 moveAmount) {

		RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast (raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs (moveAmount.y) + skinWidth, collisionMask);
		RaycastHit2D maxSlopeHitRight = Physics2D.Raycast (raycastOrigins.bottomRight, Vector2.down, Mathf.Abs (moveAmount.y) + skinWidth, collisionMask);
        if (maxSlopeHitLeft ^ maxSlopeHitRight) {
			SlideDownMaxSlope (maxSlopeHitLeft, ref moveAmount);
			SlideDownMaxSlope (maxSlopeHitRight, ref moveAmount);
		}

		if (!collisions.slidingDownMaxSlope) {
			float directionX = Mathf.Sign (moveAmount.x);
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

			if (hit) {
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
				if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle) {
					if (Mathf.Sign (hit.normal.x) == directionX) {
						if (hit.distance - skinWidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (moveAmount.x)) {
							float moveDistance = Mathf.Abs (moveAmount.x);
							float descendmoveAmountY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
							moveAmount.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (moveAmount.x);
							moveAmount.y -= descendmoveAmountY;

							collisions.slopeAngle = slopeAngle;
							collisions.descendingSlope = true;
							collisions.below = true;
							collisions.slopeNormal = hit.normal;
						}
					}
				}
			}
		}
	}

	void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount) {

		if (hit) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle > maxSlopeAngle) {
				moveAmount.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs (moveAmount.y) - hit.distance) / Mathf.Tan (slopeAngle * Mathf.Deg2Rad);

				collisions.slopeAngle = slopeAngle;
				collisions.slidingDownMaxSlope = true;
				collisions.slopeNormal = hit.normal;
			}
		}

	}
    private void ResetValue()
    {
        jump = false;
        rool = false;
        flip = false;
    }

    void ResetFallingThroughPlatform() {
		collisions.fallingThroughPlatform = false;
	}

	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public bool climbingSlope;
		public bool descendingSlope;
		public bool slidingDownMaxSlope;

		public float slopeAngle, slopeAngleOld;
		public Vector2 slopeNormal;
		public Vector2 moveAmountOld;
		public int faceDir;
		public bool fallingThroughPlatform;
    

		public void Reset() {
			above = below = false;
			left = right = false;
			climbingSlope = false;
			descendingSlope = false;
			slidingDownMaxSlope = false;
			slopeNormal = Vector2.zero;
     
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}

}
