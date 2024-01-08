using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Drawing;

public class MainValues : MonoBehaviour
{
    [Header("Constants")]
    const float PO2_LIMIT = 1.6f;
    readonly float [] COMPARTMENT_HALF_TIME_N = { 4, 8, 12.5f, 18.5f, 27, 38.3f, 54.3f, 77, 109, 146, 187, 239, 305, 390, 498, 635 };
    readonly float [] COMPARTMENT_A_N = { 1.2599f, 1.0f, 0.8618f, 0.7562f, 0.6667f, 0.5933f, 0.5282f, 0.4701f, 0.4187f, 0.3798f, 0.3497f, 0.3223f, 0.2971f, 0.2737f, 0.2523f, 0.2327f };
    readonly float [] COMPARTMENT_B_N = { 0.5050f, 0.6514f, 0.7222f, 0.7725f, 0.8125f, 0.8434f, 0.8693f, 0.8910f, 0.9092f, 0.9222f, 0.9319f, 0.9403f, 0.9477f, 0.9544f, 0.9602f, 0.9653f };

    [Header("Environmental")]
    public float depth;
    public float time;
    public float pressure;
    public float temp;
    public int vis;

    [Header("Diver")]
    public float PSI;
    public float O2;
    public float N2;
    public float PO2;
    public float PN2;
    public float boyancy;
    public float lungs;
    public float noStop;

    [Header("Deco Values")]
    public float[] CompartmentPressure = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public float[] CompartmentAmbTol = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public float DecoCeiling;
    public float DecoDepth;

    private void CalculatePressureAndGas()
    {
        pressure = 1 + (depth / 10);
        PO2 = O2 * pressure;
        PN2 = N2 * pressure;
    }

    private float CalculateInertPressureNitrogen(int i)
    {
        return N2 + (PN2 - N2) * (1 - Mathf.Pow(2, ( (-time) / COMPARTMENT_HALF_TIME_N[i])));
    }

    private float CalculateAmbTol(int i)
    {
        return (CompartmentPressure[i] - COMPARTMENT_A_N[i]) * COMPARTMENT_B_N[i];
    }

    private void CalculateCompartments()
    {
        Parallel.For(0, COMPARTMENT_HALF_TIME_N.Length, 
            index => { CompartmentPressure[index] = CalculateInertPressureNitrogen(index); });

        Parallel.For(0, COMPARTMENT_HALF_TIME_N.Length,
          index => { CompartmentAmbTol[index] = CalculateAmbTol(index); });

        DecoCeiling = CompartmentAmbTol.Max();
        DecoDepth = (DecoCeiling - 1) * 10;
    }
   
    void Update()
    {
        depth = -transform.position.y;

        CalculatePressureAndGas();
        CalculateCompartments();
    }
}
