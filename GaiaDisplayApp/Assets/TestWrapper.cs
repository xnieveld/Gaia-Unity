using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestWrapper : MonoBehaviour {

    [DllImport("libtest", EntryPoint = "init_node", CharSet = CharSet.Unicode)]
    public static extern IntPtr init_node();

    [DllImport("libtest", EntryPoint = "free_node", CharSet = CharSet.Unicode)]
    public static extern void free_node([In, Out] IntPtr node);

    [DllImport("libtest", EntryPoint = "node_get_a", CharSet = CharSet.Unicode)]
    public static extern int node_get_a([In, Out] IntPtr node);

    [DllImport("libtest", EntryPoint = "node_get_b", CharSet = CharSet.Unicode)]
    public static extern float node_get_b([In, Out] IntPtr node);

    [DllImport("libtest", EntryPoint = "test_matrix", CharSet = CharSet.Unicode)]
    public static extern int test_matrix([In, Out] int[,] matrix, int dimensions);

    [DllImport("libtest", EntryPoint = "test_get_array", CharSet = CharSet.Unicode)]
    public static extern IntPtr test_get_array();

    [DllImport("libtest", EntryPoint = "test_delete_array", CharSet = CharSet.Unicode)]
    public static extern void test_delete_array(IntPtr arr);

    [DllImport("libtest", EntryPoint = "array_of_structs", CharSet = CharSet.Unicode)]
    public static extern void array_of_structs(Position[] position, int length);

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

    IntPtr node;

    // Use this for initialization
    void Start () {
        node = init_node();
	}

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log(node_get_a(node));
            Debug.Log(node_get_b(node));
        }

        if (Input.GetMouseButtonDown(1)) {
            //float[,] arr = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };
            //Debug.Log(test_double_pointer(arr));

            Position[] positions = new Position[5];
            for (int i = 0; i < positions.Length; i++) {
                positions[i].x = UnityEngine.Random.Range(0, 10f);
                positions[i].y = UnityEngine.Random.Range(0, 10f);
                positions[i].z = UnityEngine.Random.Range(0, 10f);
            }

            array_of_structs(positions, positions.Length);
        }
    }

    private void OnDestroy()
    {
        free_node(node);
    }
}
