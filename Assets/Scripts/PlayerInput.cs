using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Controller2D controller;
    Player player;
    GameObject GMPlayer;

    public RoolState roolState;
    public float roolTimer;
    public float roolRange = 3;

    void Start()
    {       
        GMPlayer = GameObject.FindGameObjectWithTag("Player");
        player = (Player)GMPlayer.GetComponent(typeof(Player));
        controller = (Controller2D)GMPlayer.GetComponent(typeof(Controller2D));
    }

    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // указываем направления при нажатии кнопок.
        player.SetDirectionalInput(directionalInput);

        if (Input.GetKeyDown(KeyCode.LeftControl)) // включаем отключаем crouch.
        {
            player.crouch = !player.crouch;
            if (player.crouch)
            {
                player.myAnimator.SetBool("Crouch", true);
                player.crouch = true;
                player.Speed = 2;
            }
            else if (!player.crouch)
            {
                player.myAnimator.SetBool("Crouch", false);
                player.crouch = false;
                player.Speed = 6;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !player.crouch && !controller.rool)
        {
            player.OnJumpInputDown();
        }

        if (Input.GetKey(KeyCode.LeftShift) && !player.crouch && controller.collisions.below)
        {
            player.Speed = 12;
        }
        else
        {
            player.Speed = 6;
        }
        switch (roolState)
        {
            case RoolState.Ready:
                var IsRoolingKeyDown = Input.GetKeyDown(KeyCode.C);
                if (IsRoolingKeyDown && !this.player.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rool") && controller.collisions.below && player.directionalInput.x != 0)
                {
                    roolState = RoolState.Rooling;
                    controller.rool = true;
                    controller.flip = false;
                    player.myAnimator.SetBool("rool", true);
                    player.accelerationTimeGrounded = 4f;
                    player.Speed = 0;
                }
                break;
            case RoolState.Rooling:
                roolTimer += Time.deltaTime * 3;
                if (roolTimer >= roolRange && !this.player.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rool"))
                {
                    roolTimer = roolRange;
                    roolState = RoolState.Cooldown;
                    controller.rool = false;
                    player.myAnimator.SetBool("rool", false);
                    player.accelerationTimeGrounded = 0.13f;
                    player.Speed = 6;
                }
                break;
            case RoolState.Cooldown:
                roolTimer -= Time.deltaTime;
                if (roolTimer <= 0)
                {
                    roolTimer = 0;
                    roolState = RoolState.Ready;
                }
                break;
        }
    }
}

public enum RoolState
{
    Ready,
    Rooling,
    Cooldown
}
/*bool DoInputs = true;
float AxisH;
IEnumerator Dash()
{
    if (DoInputs)
        AxisH = player.GetAxis("Horizontal");

    if (AxisH < 0)
    {
        thePlayer.GetComponent<HeroController>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        move = move * 2;
        thePlayer.GetComponent<HeroController>().enabled = true;
    }
}*/ // Возможное отключения контроллера на какоето время


