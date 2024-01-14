using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wetsuit_7mm : DiveSuit
{
    public override string GetName()
    {
        return "7mm Wetsuit";
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }
    
    public override Vector2 GetRatedTemp()
    {
        return new Vector2(15, 19);
    }
}
