using UnityEngine;
using System.Collections;
//void\s*Update\s*?\(\s*\)\s*?\n*?\{\n*?\s*?\}
public class GameManager : MonoBehaviour {
    void Awake()
    {
        Application.targetFrameRate = 60; // Достижения определенного кадра в секунду.
    }
}
