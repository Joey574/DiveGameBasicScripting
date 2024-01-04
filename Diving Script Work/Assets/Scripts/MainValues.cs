using UnityEngine;

public class MainValues : MonoBehaviour
{
    const float PO2_LIMIT = 1.6f;

    [Header("Current Readouts")]
    public float depth;
    public float pressure;
    public float noStop;

    [Header("Gas")]
    public float PO2;
    public float PN;

    public float O2;
    public float N;

    public float airPressure;

    [Header("Gas Buildup")]
    public float absorbedNitrogen;

    public float nitrogenBuildupRate;
    public float nitrogenDecompressionRate;

    private void CalculatePO2()
    {
        PO2 = O2 * pressure;
    }

    private void CalculatePN()
    {
        PN = N * pressure;
    }

    private void CalculatePressure()
    {
        pressure = 1 + (depth / 10); 
    }

    private void CalculateAbsorbedNitrogen()
    {
        absorbedNitrogen += nitrogenBuildupRate - nitrogenDecompressionRate;
    }

    void Update()
    {
        CalculatePressure();

        CalculatePO2();
        CalculatePN();

        CalculateAbsorbedNitrogen();
    }
}
