using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiveComputer : MonoBehaviour
{
    [Header("Constants")]
    private const float PH2O = 0.627f;
    private const float PCO2 = 0.5296f;
    private const float MSWtoATMConversion = 10.1325f;
    private const float MSWtoFSWConversion = 3.25684678f;
    private const float Rq = 0.9f;

    private static readonly float[] COMPARTMENT_HALF_TIME_N = { 4, 8, 12.5f, 18.5f, 27, 38.3f, 54.3f, 77, 109, 146, 187, 239, 305, 390, 498, 635 };
    private static readonly float[] COMPARTMENT_A_N = { 11.696f, 10.0f, 8.618f, 7.562f, 6.667f, 5.6f, 4.947f, 4.5f, 4.187f, 3.798f, 3.497f, 3.223f, 2.85f, 2.737f, 2.523f, 2.327f };
    private static readonly float[] COMPARTMENT_B_N = { 0.5578f, 0.6514f, 0.7222f, 0.7825f, 0.8126f, 0.8434f, 0.8693f, 0.8910f, 0.9092f, 0.9222f, 0.9319f, 0.9403f, 0.9477f, 0.9544f, 0.9602f, 0.9653f };

    private static readonly float[] COMPARTMENT_HALF_TIME_H = { 1.51f, 3.02f, 4.72f, 6.99f, 10.21f, 14.48f, 20.53f, 29.11f, 41.2f, 55.19f, 70.69f, 90.34f, 115.29f, 147.42f, 188.24f, 240.03f };
    private static readonly float[] COMPARTMENT_A_H = { 16.189f, 13.83f, 11.919f, 10.458f, 9.22f, 8.205f, 7.305f, 6.502f, 5.95f, 5.545f, 5.333f, 5.189f, 5.181f, 5.176f, 5.172f, 5.119f };
    private static readonly float[] COMPARTMENT_B_H = { 0.477f, 0.5747f, 0.6527f, 0.7223f, 0.7582f, 0.7957f, 0.8279f, 0.8553f, 0.8757f, 0.8903f, 0.8997f, 0.9073f, 0.9122f, 0.9171f, 0.9217f, 0.9267f };

    private static readonly float[] COMPARTMENT_MO_N = { 32.4f, 25.4f, 22.5f, 20.3f, 19.0f, 17.5f, 16.5f, 15.7f, 15.2f, 14.6f, 14.2f, 13.9f, 13.4f, 13.2f, 12.9f, 12.7f };
    private static readonly float[] COMPARTMENT_MO_H = { 41.0f, 31.2f, 27.2f, 24.3f, 22.4f, 20.8f, 19.4f, 18.2f, 17.4f, 16.8f, 16.4f, 16.2f, 16.1f, 16.1f, 16.0f, 15.9f };

    private const float ATMtoMSW = 10.0628f;

    [Header("Compartment Values")]
    private float[] KN2 = new float[16];
    private float[] KHE = new float[16];

    private float[] NDL_N = new float[16];
    private float[] NDL_H = new float[16];

    private float NO_STOP;
    private float PO;

    [Header("Gas Pressures")]
    private List<DiveSegment> diveSegments = new List<DiveSegment>();
    public float[] PN2 = new float[16];
    private float[] PHE = new float[16];
    private float[] PIG = new float[16];

    [Header("Environmental")]
    private float SeaLevelPressure = 10; // 10 msw

    [Header("Gas Values")]
    private DiveTank diveTank;
    private float O2 = 0.21f;
    private float N2 = 0.79f;
    private float H2;

    [Header("Adjustments")]
    public float frequency = 3.0f;
    public float DEPTH_DEAD_ZONE = 0.25f;

    [Header("Internal Values")]
    private float IDLE_TIME = 0.0f;
    private float DEPTH;
    private float SDEPTH = 0.0f;
    private float FDEPTH = 0.0f;

    [Header("External Scripts")]
    private DiveComputerDisplay diveComputerDisplay;

    private void Awake() 
    {
        diveComputerDisplay = gameObject.AddComponent<DiveComputerDisplay>();

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
            FDEPTH = DEPTH;

            if (SDEPTH <= FDEPTH + DEPTH_DEAD_ZONE && SDEPTH >= FDEPTH - DEPTH_DEAD_ZONE)
            {
                float AVGDPTH = (SDEPTH + FDEPTH) / 2;
                ConstantDepth(AVGDPTH, (IDLE_TIME / 60.0f));
            }
            else
            {
                float RATE = (FDEPTH - SDEPTH) / (IDLE_TIME / 60.0f);
                VariableDepth(SDEPTH, FDEPTH, RATE, (IDLE_TIME / 60.0f));
                SDEPTH = DEPTH;
            }
            IDLE_TIME = 0.0f;
        }
        NDLTime(DEPTH);
        WriteToDisplay();
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
        for (int i = 0; i < 16; i++)
        {
            KN2[i] = Mathf.Log(2) / COMPARTMENT_HALF_TIME_N[i];
            KHE[i] = Mathf.Log(2) / COMPARTMENT_HALF_TIME_H[i];
        }
    }

    private void InitializeStartInertSat()
    {
        for (int i = 0; i < 16; i++)
        {
            PN2[i] = InertSat(0.79f, SeaLevelPressure);
            PO = InertSat(0.79f, SeaLevelPressure);
            PHE[i] = InertSat(0.0f, SeaLevelPressure);
        }
    }

    private void WriteToDisplay()
    {
        diveComputerDisplay.NO_STOP = NO_STOP;
        diveComputerDisplay.O2 = O2;
        diveComputerDisplay.DEPTH_MSW = DEPTH;
        diveComputerDisplay.MAX_DEPTH_MSW = PO2_DEPTH_LIMIT();
    }

    private float PO2_DEPTH_LIMIT()
    {
        return (1.6f / O2) * ATMtoMSW;
    }

    private float InertSat(float inertGas, float PAMB)
    {
        return (PAMB - PH2O) * inertGas;
    }

    private float InspiredPressure(float inertGas, float pAMB)
    {
        return (pAMB - PH2O) * inertGas; 
    }

    private void VariableDepth(float sDepth, float fDepth, float rate, float time)
    {
        float N2RATE = N2 * rate;
        float HERATE = H2 * rate;

        float SAMBP = sDepth + SeaLevelPressure;
        float FAMBP = fDepth + SeaLevelPressure;

        float PIN2O = InspiredPressure(N2, SAMBP);
        float PIHEO = InspiredPressure(H2, SAMBP);

        for (int i = 0; i < 16; i++)
        {
            float PTHEO = PHE[i];
            float PTN2O = PN2[i];

            PHE[i] = (float)(PIHEO + HERATE * (time - 1.0 / KHE[i]) - (PIHEO - PTHEO - (HERATE / KHE[i])) * Mathf.Exp(-KHE[i] * time));
            PN2[i] = (float)(PIN2O + N2RATE * (time - 1.0 / KN2[i]) - (PIN2O - PTN2O - (N2RATE / KN2[i])) * Mathf.Exp(-KN2[i] * time));

            PIG[i] = PHE[i] + PN2[i];
        }
    }

    private void ConstantDepth(float depth, float time) 
    {
        float PIN2O = InspiredPressure(depth, N2);
        float PIHEO = InspiredPressure(depth, H2);

        for (int i = 0; i < 16; i++)
        {
            float PTHEO = PHE[i];
            float PTN2O = PN2[i];

            PHE[i] = PTHEO + (PIHEO - PTHEO) * (1.0f - Mathf.Exp(-KHE[i] * time));
            PN2[i] = PTN2O + (PIN2O - PTN2O) * (1.0f - Mathf.Exp(-KN2[i] * time));
        }
    }

    private void NDLTime(float depth)
    {
        float PAMB = depth + SeaLevelPressure;

        float PI = InspiredPressure(N2, PAMB);

        for (int i = 0; i < 16; i++) 
        {
            if ((COMPARTMENT_MO_N[i] > PI && PI < PO) || (COMPARTMENT_MO_N[i] < PI && PI > PO))
            {
                NDL_N[i] = (-1.0f / KN2[i]) * Mathf.Log((PI - COMPARTMENT_MO_N[i]) / (PI - PO));
            } 
            else
            {
                NDL_N[i] = float.MaxValue;
            }
        }
        NO_STOP = NDL_N.Min();
    }
}
