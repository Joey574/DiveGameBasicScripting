using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveTank
{
    [Header("Dive Tank")]
    public string material;
    public float weight;
    public float PSI;
    public float O2;
    public float N2;
    public float H2;

    public DiveTank(string material, float weight, float PSI, float o2, float n2, float h2)
    {
        this.material = material;
        this.weight = weight;
        this.PSI = PSI;
        O2 = o2;
        N2 = n2;
        H2 = h2;
    }
}
