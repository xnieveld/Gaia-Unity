using UnityEngine;
using System.Runtime.InteropServices;
using System;

class OctreeDLL : MonoBehaviour
{
    // The imported function
    [DllImport("liboctree", EntryPoint = "oct_octree_init")]
    public static extern IntPtr InitOctree(float[] position, long size);

    [DllImport("liboctree", EntryPoint = "oct_octree_build")]
    public static extern void BuildOctree(IntPtr octree, float[][] object_positions, long object_count);

    public int[] a;

    void Start()
    {
        float[] position = { 100, 100, 100 };
        IntPtr octree = InitOctree(position, 100);

        float[] pos1 = new float[3] { 1, 2, 3 };
        float[] pos2 = new float[3] { 5, 6, 7 };
        float[] pos3 = new float[3] { 1, 3, 2 };
        float[] pos4 = new float[3] { 5, 4, 3 };
        float[][] objects = { pos1, pos2, pos3, pos4 };

        BuildOctree(octree, objects, objects.Length);
    }
}