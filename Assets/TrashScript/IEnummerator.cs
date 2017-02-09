using UnityEngine;
using System.Collections;

public class IEnummerator : MonoBehaviour {

    float lifetime = 10f;

    void Start ()
    {
        Destroy(gameObject, lifetime);

    }
    void Update ()
    {
        print(" " + lifetime);
    }
}

//
// Скрипт для решения проблемы заикания оптимизировать под мой скрипт.
//

/*
/// Walk down slopes safely. Prevents Player from "hopping" down hills.
/// Apply gravity before running this. Should only be used if Player
/// was touching ground on the previous frame.
void SafeMove(Vector2 velocity)
{
    // X and Z first. We don't want the sloped ground to prevent
    // Player from falling enough to touch the ground.
    Vector3 displacement;
    displacement.x = velocity.x * Time.deltaTime;
    displacement.y = 0;
    displacement.z = -characterController.transform.position.z;
    characterController.Move(displacement);
    // Now Y
    displacement.y = velocity.y * Time.deltaTime;
    // Our steepest down slope is 45 degrees. Force Player to fall at least
    // that much so he stays in contact with the ground.
    if (-Mathf.Abs(displacement.x) < displacement.y && displacement.y < 0)
    {
        displacement.y = -Mathf.Abs(displacement.x) - 0.001f;
    }
    displacement.z = 0;
    displacement.x = 0;
    characterController.Move(displacement);
}*/

