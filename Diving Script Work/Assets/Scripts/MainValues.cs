using UnityEngine;

public class MainValues : MonoBehaviour
{
    [Header("External Scripts")]
    public DiveTank diveTank;
    public DiveComputer deco;


    private void Awake()
    {
        DiveTank diveTank = new DiveTank("TestMat", 0, 3000, 0.21f, 0.79f, 0.0f);

        deco = gameObject.AddComponent<DiveComputer>();
        deco.SetBreathingMixture(diveTank);

        //deco.VariableDepth(0, (120 / 3.25684678f), (60 / 3.25684678f));
        deco.ConstantDepth(30, 5);
        deco.NDLTime(30);

        deco.WriteToDisplay();
    }
}
