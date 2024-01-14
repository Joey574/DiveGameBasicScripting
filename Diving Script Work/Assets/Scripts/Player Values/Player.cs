using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("External Scripts")]
    PlayerCalculator playerCalculator;

    [Header("Equipment")]
    DiveSuit diveSuit;

    void Awake()
    {
        playerCalculator = gameObject.AddComponent<PlayerCalculator>();
    }

    void Update()
    {
        
    }
}
