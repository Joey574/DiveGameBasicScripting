using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymptomCalculator
{
    public bool SufferingCNSToxicity(float O2, float ATM)
    {
        float PO2 = O2 * ATM;
        float chance = Mathf.Pow(PO2 - 1.4f, 3f) * 25;
        chance = Mathf.Clamp(chance, 0.0f, 1.0f);

        if (Random.Range(0.0f, 1.0f) > chance)
        {
            return false;
        }
        return true;
    }
}