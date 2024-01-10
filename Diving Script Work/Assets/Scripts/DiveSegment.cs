using UnityEngine;

public class DiveSegment
{
    [Header("Gas Pressures")]
    public float[] PN2 = new float[16];
    public float[] PHE = new float[16];
    public float[] PIG = new float[16];

    [Header("Depths")]
    public float SDEPTH;
    public float FDEPTH;
}
