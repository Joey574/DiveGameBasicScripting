using UnityEngine;

public class MainValues : MonoBehaviour
{
    [Header("External Scripts")]
    public DiveTank diveTank;
    public DiveComputer diveComputer;


    private void Awake()
    {
        diveComputer = gameObject.AddComponent<DiveComputer>();

        //DiveTank diveTank = new DiveTank("TestMat", 0, 3000, 0.15f, 0.4f, 0.45f);
        //diveComputer.SetBreathingMixture(diveTank);

        diveComputer.SetBreathingMixture(0.21f, 0.79f, 0.0f);

        //diveComputer.VariableDepth(0, 36.518574086255f, 18.259287043127f, 2);
    }
}
