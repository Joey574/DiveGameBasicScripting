using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class PlayerCalculator : MonoBehaviour
{
    [Header("Conversion Constants")]
    private const float ATMtoMSWConversion = 10.0628f;
    private const float ATMtoFSWConversion = 33.066f;
    private const float MSWtoFSWConversion = 3.286f;

    [Header("Compartment Values")]
    private float[] PN2 = new float[16];
    private float[] PHE = new float[16];
    private float[] PIG = new float[16];

    private float[] KN2 = new float[16];
    private float[] KHE = new float[16];

    private float[] NDL_N = new float[16];
    private float[] NDL_H = new float[16];

    private float[] N2_AMBTOL = new float[16];
    private float[] HE_AMBTOL = new float[16];

    private float[] PO = new float[16];

    [Header("Limits")]
    private float N2_AMBTOL_LIMIT;
    private float HE_AMBTOL_LIMIT;

    private float NO_STOP;

    [Header("Dive Profile")]
    private List<DiveSegment> diveSegments = new List<DiveSegment>();

    [Header("Environmental")]
    private float SeaLevelPressure = 10; // 10 msw

    [Header("Gas Values")]
    private DiveTank diveTank;
    private float O2 = 0.21f;
    private float N2 = 0.79f;
    private float H2 = 0.0f;

    [Header("Adjustments")]
    public float frequency = 3.0f;
    public float PO2_WARNING = 1.4f;

    [Header("Internal Values")]
    private float IDLE_TIME = 0.0f;
    private float DEPTH;
    private float SDEPTH = 0.0f;

    [Header("External Scripts")]
    private DiveComputerDisplay diveComputerDisplay;
    private SymptomCalculator symptomCalculator;

    private void Awake()
    {
        symptomCalculator = new SymptomCalculator();

        InitializeKValues();
        InitializeStartInertSat();

        SDEPTH = -transform.position.y;
    }

    private void Update()
    {
        IDLE_TIME += Time.deltaTime;
        DEPTH = -transform.position.y;

        if (IDLE_TIME > frequency)
        {
            float TIME_MIN = IDLE_TIME;// / 60.0f;
            float RATE = (DEPTH - SDEPTH) / TIME_MIN;

            VariableDepth(SDEPTH, DEPTH, RATE, TIME_MIN);
            SDEPTH = DEPTH;

            IDLE_TIME = 0.0f;

            Parallel.For(0, 16, i =>
            {
                N2_AMBTOL[i] = PAMBTOL_N(i);
            });
            N2_AMBTOL_LIMIT = N2_AMBTOL.Max();

            Parallel.For(0, 16, i =>
            {
                HE_AMBTOL[i] = PAMBTOL_H(i);
            });
            HE_AMBTOL_LIMIT = HE_AMBTOL.Max();

            if (symptomCalculator.SufferingCNSToxicity(O2, DEPTH / ATMtoMSWConversion))
            {
                Debug.Log("CNS");
            }
        }
    }

    public void SetBreathingMixture(DiveTank diveTank)
    {
        this.diveTank = diveTank;

        N2 = diveTank.N2;
        O2 = diveTank.O2;
        H2 = diveTank.H2;
    }

    public void SetBreathingMixture(float O2, float N2, float H2)
    {
        diveTank = null;
        this.O2 = O2;
        this.N2 = N2;
        this.H2 = H2;
    }

    private void InitializeKValues()
    {
        Parallel.For(0, 16, i =>
        {
            KN2[i] = Mathf.Log(2) / DiveConstants.COMPARTMENT_HALF_TIME_N[i];
            KHE[i] = Mathf.Log(2) / DiveConstants.COMPARTMENT_HALF_TIME_H[i];
        });
    }

    private void InitializeStartInertSat()
    {
        Parallel.For(0, 16, i =>
        {
            PO[i] = InertSat(0.79f, 10f); // 10 -> normal air pressure  0.79 -> normal fn2
            PN2[i] = InertSat(0.79f, 10); // 10 -> normal air pressure  0.79 -> normal fn2
            PHE[i] = InertSat(0.0f, 10); // 10 -> normal air pressure  0.0 -> normal fhe
        });
    }

    private float InertSat(float inertGas, float PAMB)
    {
        return (PAMB - DiveConstants.PH2O) * inertGas;
    }

    private float InspiredPressure(float inertGas, float pAMB)
    {
        return (pAMB - DiveConstants.PH2O) * inertGas;
    }

    private void VariableDepth(float sDepth, float fDepth, float rate, float time)
    {
        float N2RATE = N2 * rate;
        float HERATE = H2 * rate;

        float SAMBP = sDepth + SeaLevelPressure;
        float FAMBP = fDepth + SeaLevelPressure;

        float PIN2O = InspiredPressure(N2, SAMBP);
        float PIHEO = InspiredPressure(H2, SAMBP);

        Parallel.For(0, 16, i =>
        {
            PHE[i] = (float)(PIHEO + HERATE * (time - (1.0f / KHE[i])) - (PIHEO - PHE[i] - (HERATE / KHE[i])) * Mathf.Exp(-KHE[i] * time));
            PN2[i] = (float)(PIN2O + N2RATE * (time - (1.0f / KN2[i])) - (PIN2O - PN2[i] - (N2RATE / KN2[i])) * Mathf.Exp(-KN2[i] * time));

            PIG[i] = PHE[i] + PN2[i];
        });
    }

    private float PAMBTOL_N(int i)
    {
        return (PN2[i] - DiveConstants.COMPARTMENT_A_N[i]) * DiveConstants.COMPARTMENT_B_N[i];
    }

    private float PAMBTOL_H(int i)
    {
        return (PHE[i] - DiveConstants.COMPARTMENT_A_H[i]) * DiveConstants.COMPARTMENT_B_H[i];
    }
}
