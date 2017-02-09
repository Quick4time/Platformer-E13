using UnityEngine;
using System.Collections;

public class DashAbility : MonoBehaviour
{
    public DashState dashState;
    public float dashTimer;
    public float dashRange = 20;

    public Vector2 savedVel;

    Player player;
    GameObject GMPlayer;

    void Start()
    {
        player = (Player)GMPlayer.GetComponent(typeof(Player));
        GMPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        switch (dashState)
        {
            case DashState.Ready:
                var isDashKeyDown = Input.GetKeyDown(KeyCode.X);
                if (isDashKeyDown)
                {
                    savedVel = player.velocity;
                    player.velocity = new Vector3(player.velocity.x * 3, player.velocity.y); //player.velocity.x = player.roolspeed;
                    dashState = DashState.Dashing;
                }
                break;
            case DashState.Dashing:
                dashTimer += Time.deltaTime * 3;
                if (dashTimer >= dashRange)
                {
                    dashTimer = dashRange;
                    player.velocity = savedVel;
                    dashState = DashState.Cooldown;
                }
                break;
            case DashState.Cooldown:
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }
                break;
        }
    }
}
public enum DashState
{
    Ready,
    Dashing,
    Cooldown
}

      /*public DashState dashState;
      public float dashTimer;
      public float maxDash = 20f;

      public Vector2 savedVelocity;

      void Update()
      {
          switch (dashState)
          {
              case DashState.Ready:
                  var isDashKeyDown = Input.GetKeyDown(KeyCode.LeftControl);
                  if (isDashKeyDown)
                  {
                      savedVelocity = myRigidbody.velocity;
                      myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * 3f, myRigidbody.velocity.y);
                      dashState = DashState.Dashing;
                  }
                  break;
              case DashState.Dashing:
                  dashTimer += Time.deltaTime * 3;
                  if (dashTimer >= maxDash)
                  {
                      dashTimer = maxDash;
                      GetComponent<Rigidbody>().velocity = savedVelocity;
                      dashState = DashState.Cooldown;
                  }
                  break;
              case DashState.Cooldown:
                  dashTimer -= Time.deltaTime;
                  if (dashTimer <= 0)
                  {
                      dashTimer = 0;
                      dashState = DashState.Ready;
                  }
                  break;
          }
      }
    }
}

public enum DashState
{
    Ready,
    Dashing,
    Cooldown
}*/
