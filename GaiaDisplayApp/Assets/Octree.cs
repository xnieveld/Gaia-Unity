using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Octree
{
    private readonly Vector3 position;
    private readonly uint size;

    byte[] nodes;
    List<int> break_points;

    public Octree(uint size, Vector3 position)
    {
        nodes = new byte[int.MaxValue];
        break_points = new List<int>();

        this.size = size;
        this.position = position;
    }

    public void BuildOctree()
    {
        Debug.Log("Getting star from db");
        var ctx = GaiaDB.SetupContext();
        var cursor = GaiaDB.GetCursor(ctx.dbp);

        var star = GaiaDB.GetNextStar(cursor);
        Debug.Log(star);

        ////while (position != null) {
        ////    if (node_local_codes == 0) {
        ////        Node node = new Node { location_code = 0 };
        ////        nodes.Add(node);
        ////        continue;
        ////    }

            //FindEmpty(nodes[0], position);
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start_position"></param>
    /// <param name="node">OPTIONAL, If you don't want to start from the rootnode you can specify it here</param>
    /// <returns></returns>
    int FindEmpty(Vector3 start_position, int start_index = 0)
    {
    //    if (node == null) {
    //        node = nodes[0];
    //    }

    //    Vector3 node_position = NodeGetPosition(node);

    //    uint child_location = 0;
    //    child_location |= start_position.x > node_position.x ? 1u << 0 : 0;
    //    child_location |= start_position.y > node_position.y ? 1u << 1 : 0;
    //    child_location |= start_position.z > node_position.z ? 1u << 2 : 0;

    //    int index = (int)path_to_processing_node + (int)child_location;
    //    if (nodes[index] != null) {

    //    }
        return 0;
    }

    int SplitNode(int node_index)
    {
        //    uint object_index = node.object_index;
        //    ulong location_code = node.location_code;
        //    RemoveLeaf(location_code);

        //    Node branch_node = AddBranch(location_code);

        //    Node new_child = FindEmptyLeaf(branch_node, object_positions[object_index]);
        //    new_child.object_index = object_index;

        //    return branch_node;
        return 0;
    }

    Vector3 NodeGetPosition(int node_index)
    {
        //    Vector3 position = this.position;

        //    ushort tree_depth = NodeGetDepth(node);
        //    for (int i = 0; i < tree_depth; i++) {
        //        int offset = tree_depth * 3 - 3 * (i + 1);
        //        byte local_code = (byte)((node.location_code >> offset) & 7); // 7 = 0b111
        //        position.x = Convert.ToBoolean(local_code & 1u)
        //                          ? position.x + this.size / Mathf.Pow(2.0f, (i + 1))
        //                          : position.x - this.size / Mathf.Pow(2.0f, (i + 1));
        //        position.y = Convert.ToBoolean(local_code & 2u)
        //                          ? position.y + this.size / Mathf.Pow(2.0f, (i + 1))
        //                          : position.y - this.size / Mathf.Pow(2.0f, (i + 1));
        //        position.z = Convert.ToBoolean(local_code & 4u)
        //                          ? position.z + this.size / Mathf.Pow(2.0f, (i + 1))
        //                          : position.z - this.size / Mathf.Pow(2.0f, (i + 1));
        //    }

        //    return position;
        return Vector3.zero;
    }

    ushort NodeGetDepth(int node_index)
    {
        //    ushort depth = 0;
        //    for (ulong location_code = node.location_code; location_code != 1; location_code >>= 3) {
        //        depth++;
        //    }

        //    return depth;
        return 0;
    }

    float NodeGetSize(int node_index)
    {
        return NodeGetDepth(node_index) > 0 ? size / Mathf.Pow(2f, NodeGetDepth(node_index)) : size;
    }

    int NodeGetParent(int node_index)
    {
        //    ulong location_code_parent = node.location_code >> 3;
        //    return LookupNode(location_code_parent);
        return 0;
    }

    int NodeGetChild(int node_index, byte child_location)
    {
        //    ulong child_location_code = (location_code << 3) | child_location;
        //    return LookupNode(child_location_code);
        return nodes[node_index + child_location];
    }

    int LookupNode(ulong location_code)
    {
        return nodes[location_code];
    }

    //// TEST CASE
    //void VisitAll(BaseNode node)
    //{
    //    for (int i = 0; i < 8; i++) {
    //        if (node.type == 0) {
    //            Node branch_node = (Node)node;
    //            if (Convert.ToBoolean(branch_node.child_exists & (1 << i))) {
    //                Node child = (Node)NodeGetChild(node.location_code, (byte)i);
    //                VisitAll(child);
    //            }
    //        }
    //        else {
    //            Debug.Log(node);
    //        }
    //    }
    //}

    ///// <summary>
    ///// /////////////////////////////////////////
    ///// </summary>
    //public struct NodeDebug
    //{
    //    public Vector3 position;
    //    public float size;
    //}

    //public NodeDebug[] DebugGetNodes()
    //{
    //    NodeDebug[] debug_nodes = new NodeDebug[nodes.Count];

    //    uint i = 0;
    //    foreach (var node in nodes) {
    //        debug_nodes[i].position = NodeGetPosition((BaseNode)node.Value);
    //        debug_nodes[i].size = NodeGetSize((BaseNode)node.Value) * 2;
    //        ++i;
    //    }

    //    return debug_nodes;
    //}
}
