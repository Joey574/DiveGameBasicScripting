using UnityEngine;

public class MainValues : MonoBehaviour
{
    [Header("External Scripts")]
    public DiveTank diveTank;
    public DiveComputer diveComputer;


    private void Awake()
    {
        DiveTank diveTank = new DiveTank("TestMat", 0, 3000, 0.21f, 0.79f, 0.0f);

        diveComputer = gameObject.AddComponent<DiveComputer>();
        diveComputer.SetBreathingMixture(diveTank);
    }
}
