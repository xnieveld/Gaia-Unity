using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    Octree octree;
    [SerializeField]
    uint size;
    [SerializeField]
    Vector3 position;

    List<Vector3> object_positions;

    [SerializeField]
    GameObject cube_prefab;

    bool debug;

    // Use this for initialization
    void Start()
    {
        octree = new Octree(size, position);
        object_positions = new List<Vector3>();

        for (int i = 0; i < 100; i++) {
           // AddObject();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) {
           // AddObject();
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            octree.BuildOctree();
            //Thread thread = new Thread(OctreeThread);
            //thread.Start();
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            debug = !debug;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            octree = null;
            octree = new Octree(size, position);
        }
    }

    private void OctreeThread()
    {
        octree.BuildOctree(/*object_positions.ToArray()*/);
        Debug.Log("Done building");
    }

    private void OnDrawGizmos()
    {
        if (debug) {
            //var nodes = octree.DebugGetNodes();

           // for (int i = 0; i < nodes.Length; i++) {
        //        Gizmos.DrawWireCube(nodes[i].position, new Vector3(nodes[i].size, nodes[i].size, nodes[i].size));
        //    }
        }
    }

    //public void AddObject()
    //{
    //    float x = Random.Range(-size, size);
    //    float y = Random.Range(-size, size);
    //    float z = Random.Range(-size, size);
    //    Vector3 position = new Vector3(x, y, z);
    //    Instantiate(cube_prefab, position, Quaternion.identity);

    //    object_positions.Add(position);
    //}
}
