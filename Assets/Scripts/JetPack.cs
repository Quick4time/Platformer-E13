using UnityEngine;
using System.Collections;

public class JetPack : MonoBehaviour {

    public float JetPackForce;
    public float startTime = 3.5f;
    public GameObject player;
    public float fuel = 100f, maxFuel = 100f;
    bool isFlying;
    Rigidbody2D myRigidbody;

    Rect fuelRect;
    Texture2D fueltexture;

    void Start()
    {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        fuelRect = new Rect(Screen.width / 10, Screen.height / 10, Screen.width / 3, Screen.height / 50);
        fuelRect.y -= fuelRect.height;

        fueltexture = new Texture2D(1, 1);
        fueltexture.SetPixel(0, 0, Color.red);
        fueltexture.Apply();
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.C))
        {
            fuel -= 0.1f;
            isFlying = true;
            startTime -= Time.deltaTime;
            player.GetComponent<Rigidbody2D>().velocity = (new Vector3(myRigidbody.velocity.x, 10f, 0.0f) * JetPackForce);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            isFlying = false;
        }
        if (isFlying)
        {
            fuel -= Time.deltaTime;
            if (fuel <= 0)
            {
                fuel += 10f;
                fuel = 0;
                player.GetComponent<Rigidbody2D>().velocity = (new Vector3(0f, 0f, 0.0f));
                isFlying = false;
            }
        }
        else if (fuel < maxFuel)
        {
            fuel += Time.deltaTime;
        }

        if (startTime <= 0.0f)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector3.down * Time.smoothDeltaTime;
            startTime = 3.5f;
        }
    }
    void OnGUI()
    {
        float ratio = fuel / maxFuel;
        float rectWidth = ratio * Screen.width / 3;
        fuelRect.width = rectWidth;
        GUI.DrawTexture(fuelRect, fueltexture);
    }
}

