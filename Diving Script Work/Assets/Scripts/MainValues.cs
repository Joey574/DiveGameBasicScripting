using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Drawing;
using System;

public class MainValues : MonoBehaviour
{
    [Header("External Scripts")]
    public DiveTank diveTank;
    public DecoCalculations deco;


    private void Update()
    {
        DiveTank diveTank = new DiveTank("TestMat", 0, 3000, 0.15f, 0.45f, 0.40f);

        if (deco == null )
        {
            deco = gameObject.AddComponent<DecoCalculations>();
            deco.SetValues(diveTank);
        }

        deco.VariableDepth(0, (120 / 3.25684678f), 60);
    }
}
