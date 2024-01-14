using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drysuit : DiveSuit
{
    public override string GetName()
    {
        return "Drysuit";
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }
   
    public override Vector2 GetRatedTemp()
    {
        return new Vector2(0, 15);
    }
}
