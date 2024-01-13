using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveConstants
{
    [Header("Constants")]
    public const float PH2O = 0.627f;
    public const float PCO2 = 0.5296f;
    public const float Rq = 0.9f;

    public static readonly float[] COMPARTMENT_HALF_TIME_N = { 4, 8, 12.5f, 18.5f, 27, 38.3f, 54.3f, 77, 109, 146, 187, 239, 305, 390, 498, 635 };
    public static readonly float[] COMPARTMENT_A_N = { 11.696f, 10.0f, 8.618f, 7.562f, 6.667f, 5.6f, 4.947f, 4.5f, 4.187f, 3.798f, 3.497f, 3.223f, 2.85f, 2.737f, 2.523f, 2.327f };
    public static readonly float[] COMPARTMENT_B_N = { 0.5578f, 0.6514f, 0.7222f, 0.7825f, 0.8126f, 0.8434f, 0.8693f, 0.8910f, 0.9092f, 0.9222f, 0.9319f, 0.9403f, 0.9477f, 0.9544f, 0.9602f, 0.9653f };

    public static readonly float[] COMPARTMENT_HALF_TIME_H = { 1.51f, 3.02f, 4.72f, 6.99f, 10.21f, 14.48f, 20.53f, 29.11f, 41.2f, 55.19f, 70.69f, 90.34f, 115.29f, 147.42f, 188.24f, 240.03f };
    public static readonly float[] COMPARTMENT_A_H = { 16.189f, 13.83f, 11.919f, 10.458f, 9.22f, 8.205f, 7.305f, 6.502f, 5.95f, 5.545f, 5.333f, 5.189f, 5.181f, 5.176f, 5.172f, 5.119f };
    public static readonly float[] COMPARTMENT_B_H = { 0.477f, 0.5747f, 0.6527f, 0.7223f, 0.7582f, 0.7957f, 0.8279f, 0.8553f, 0.8757f, 0.8903f, 0.8997f, 0.9073f, 0.9122f, 0.9171f, 0.9217f, 0.9267f };

    public static readonly float[] COMPARTMENT_MO_N = { 32.4f, 25.4f, 22.5f, 20.3f, 19.0f, 17.5f, 16.5f, 15.7f, 15.2f, 14.6f, 14.2f, 13.9f, 13.4f, 13.2f, 12.9f, 12.7f };
    public static readonly float[] COMPARTMENT_MO_H = { 41.0f, 31.2f, 27.2f, 24.3f, 22.4f, 20.8f, 19.4f, 18.2f, 17.4f, 16.8f, 16.4f, 16.2f, 16.1f, 16.1f, 16.0f, 15.9f };

}
