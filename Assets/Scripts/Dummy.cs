using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Dummy script to be used for testing with the UniversalTextTag and other related scripts.
/// Imitates a MonoBehaviour script that would be attached to some GameObject in a Unity Scene.
/// </summary>
public class Dummy : MonoBehaviour
{
    public const int MAX_HEALTH = 100;
    public int health = MAX_HEALTH;

    float delay = 1.0f;
    float time = 0.0f;
    private void Update()
    {
        // Heal by 1hp every second up to MAX_HEALTH
        time += Time.deltaTime;
        if (time >= delay && health < MAX_HEALTH)
        {
            health += 1;
            time = 0.0f;
        }
    }
}
