using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wetsuit_12mm : DiveSuit
{
    public override string GetName()
    {
        return "12mm Wetsuit";
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public override Vector2 GetRatedTemp()
    {
        return new Vector2(10, 15);
    }
}
