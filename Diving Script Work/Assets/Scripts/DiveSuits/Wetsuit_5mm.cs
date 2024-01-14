using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wetsuit_5mm : DiveSuit
{
    public override string GetName()
    {
        return "5mm Wetsuit";
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public override Vector2 GetRatedTemp()
    {
        return new Vector2(20, 24);
    }
}
