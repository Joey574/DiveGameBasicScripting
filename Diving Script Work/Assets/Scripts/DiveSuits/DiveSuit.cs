using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiveSuit
{
    [Header("Description")]
    protected string name;
    protected string description;

    [Header("Thermal Protection")]
    protected float RatedTemp;

    public abstract string GetName();

    public abstract string GetDescription();

    public abstract Vector2 GetRatedTemp();
}
