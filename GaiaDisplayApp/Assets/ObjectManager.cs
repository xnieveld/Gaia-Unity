using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    Octree octree;

    [SerializeField]
    uint size;

    [SerializeField]
    Vector3 position;

    List<Vector3> object_positions;

    bool debug;

	// Use this for initialization
	void Start () {
        octree = new Octree(size, position);
        object_positions = new List<Vector3>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.N)) {
            AddObject();
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            octree.BuildOctree(object_positions.ToArray());
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            debug = !debug;
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            var nodes = octree.DebugGetNodes();

            for (int i = 0; i < nodes.Length; i++)
            {
                Gizmos.DrawWireCube(nodes[i].position, new Vector3(size, size, size));
            }
        }
    }

    public void AddObject()
    {
        Vector3 position = new Vector3(Random.Range(-size, size), Random.Range(-size, size), Random.Range(-size, size));
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;

        object_positions.Add(position);
    }
}
