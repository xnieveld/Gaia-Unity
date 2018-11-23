using UnityEngine;
using System.Runtime.InteropServices;
using System;

class OctreeWrapper : MonoBehaviour
{
    // The imported function
    [DllImport("octree", EntryPoint = "oct_octree_init", CharSet = CharSet.Unicode)]
    public static extern IntPtr InitOctree(Position position, uint size);

    [DllImport("octree", EntryPoint = "oct_octree_build", CharSet = CharSet.Unicode)]
    public static extern void BuildOctree(IntPtr octree, Position[] object_positions, uint object_count);

    [DllImport("octree", EntryPoint = "oct_octree_free", CharSet = CharSet.Unicode)]
    public static extern void FreeOctree(IntPtr octree);

    [DllImport("octree", EntryPoint = "oct_octree_get_size", CharSet = CharSet.Unicode)]
    public static extern uint GetSize(IntPtr octree);

    [DllImport("octree", EntryPoint = "oct_octree_get_inner_count", CharSet = CharSet.Unicode)]
    public static extern uint GetInnerCount(IntPtr octree);

    [DllImport("octree", EntryPoint = "oct_octree_get_leaf_count", CharSet = CharSet.Unicode)]
    public static extern uint GetLeafCount(IntPtr octree);

    [StructLayout(LayoutKind.Sequential)]
    public struct Position
    {
        public float x;
        public float y;
        public float z;

        public Position(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    IntPtr octree;

    void Start()
    {
        Position position = new Position(100, 100, 100);
        octree = InitOctree(position, 100);

        Position[] objects = new Position[5];
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].x = UnityEngine.Random.Range(0, 10f);
            objects[i].y = UnityEngine.Random.Range(0, 10f);
            objects[i].z = UnityEngine.Random.Range(0, 10f);
        }

        BuildOctree(octree, objects, (uint)objects.Length);

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(GetSize(octree));
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(GetInnerCount(octree));
            Debug.Log(GetLeafCount(octree));
        }
    }

    void OnDestroy()
    {
        FreeOctree(octree);
    }
}
