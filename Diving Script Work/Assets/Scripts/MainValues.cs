using UnityEngine;

public class MainValues : MonoBehaviour
{
    [Header("External Scripts")]
    public DiveTank diveTank;
    public DiveComputer diveComputer;


    private void Awake()
    {
        DiveTank diveTank = new DiveTank("TestMat", 0, 3000, 0.15f, 0.4f, 0.45f);

        diveComputer = gameObject.AddComponent<DiveComputer>();
        diveComputer.SetBreathingMixture(diveTank);
        diveComputer.VariableDepth(0, 36.518574086255f, 18.259287043127f, 2);
    }
}
