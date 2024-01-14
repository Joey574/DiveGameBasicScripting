using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wetsuit_3mm : DiveSuit
{
    public override string GetName()
    {
        return "3mm Wetsuit";
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public override Vector2 GetRatedTemp()
    {
        return new Vector2(25, 29);
    }
}
