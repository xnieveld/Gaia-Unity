using UnityEngine;
using System.Runtime.InteropServices;
using System;

class OctreeDLL : MonoBehaviour
{
    // The imported function
    [DllImport("octree", EntryPoint = "oct_octree_init")]
    public static extern IntPtr InitOctree([In, Out] float[] position, long size);

    [DllImport("octree", EntryPoint = "oct_octree_build")]
    public static extern void BuildOctree(IntPtr octree, float[,] object_positions, long object_count);

    [DllImport("octree", EntryPoint = "oct_octree_free")]
    public static extern void FreeOctree(IntPtr octree);

    void Start()
    {
        float[] position = { 100, 100, 100 };
        IntPtr octree = InitOctree(position, 100);

        float[,] objects = { {1,5,2}, {4,6,1}, {8,9,4}, {5,7,3} };

        /* BuildOctree(octree, objects, objects.Length); */

        FreeOctree(octree);
    }
}
