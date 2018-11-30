using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    private readonly Vector3 position;
    private readonly uint size;
    private uint leaf_count;
    private uint branch_count;
    private Vector3[] object_positions;
    private Dictionary<ulong, object> nodes;
    private BaseNode root_node;

    class BaseNode
    {
        public ulong location_code;
        public byte type;
    };

    /**
     * @brief The leaf node. This holds the object index of the object.
     *        The object index can be used by the user to find the right
     *        object in his array.
     */
    class LeafNode : BaseNode
    {
        public uint object_index;

        public LeafNode(ulong location_code, uint object_index)
        {
            this.location_code = location_code;
            this.object_index = object_index;
        }
    };

    /**
     * @brief The inner node is a node that doesn't contain an object but
     * branches into smaller nodes. It has a child exists flag that where every
     * bit is set for the correlating child. This can be used t quickly check if
     * child nodes already exist.
     */
    class BranchNode : BaseNode
    {
        public byte child_exists;

        public BranchNode(ulong location_code)
        {
            this.location_code = location_code;
            type = 0;
        }
    };

    public Octree(uint size, Vector3 position)
    {
        nodes = new Dictionary<ulong, object>();

        this.size = size;
        this.position = position;
        root_node = AddLeaf(0, 1, uint.MaxValue);
    }

    public void BuildOctree(Vector3[] object_positions)
    {
        this.object_positions = object_positions;

        for (uint i = 0; i < object_positions.Length; i++) {
            LeafNode leaf_node = FindEmptyLeaf(root_node, object_positions[i]);
            if (leaf_node.object_index != uint.MaxValue) {
                BranchNode new_branch = SplitLeaf(leaf_node);
                leaf_node = FindEmptyLeaf(new_branch, object_positions[i]);
                leaf_node.object_index = i;
            }
            else {
                leaf_node.object_index = i;
            }
        }
    }

    BranchNode AddBranch(ulong location_code)
    {
        BranchNode node = new BranchNode(location_code);
        nodes.Add(node.location_code, node);
        branch_count++;

        // If we just removed the root node set it to the new inner node
        if (location_code == 1u) {
            root_node = node;
        }

        return node;
    }

    void RemoveBranch(ulong location_code)
    {
        nodes.Remove(location_code);
        branch_count--;
    }

    LeafNode AddLeaf(ulong parent_location, byte child_location, uint object_index)
    {
        ulong new_location = (parent_location << 3) | child_location;
        LeafNode node = new LeafNode(new_location, object_index);

        nodes.Add(node.location_code, node);
        leaf_count++;

        if (parent_location > 0) {
            BranchNode parent_node = (BranchNode)LookupNode(parent_location);
            parent_node.child_exists |= (byte)(1u << child_location);
        }

        // If we just removed the root node set it to the new inner node
        if (new_location == 1u) {
            root_node = node;
        }

        return node;
    }

    void RemoveLeaf(ulong location_code)
    {
        nodes.Remove(location_code);
        leaf_count--;
    }

    LeafNode FindEmptyLeaf(object node, Vector3 object_position)
    {
        if (node is LeafNode) { return (LeafNode)node; }

        if (node is BranchNode) {
            BranchNode branch_node = (BranchNode)node;
            Vector3 node_position = NodeGetPosition(branch_node);

            byte child_location = 0;
            child_location |= object_position.x > node_position.x ? (byte)(1u << 0) : (byte)0;
            child_location |= object_position.y > node_position.y ? (byte)(1u << 1) : (byte)0;
            child_location |= object_position.z > node_position.z ? (byte)(1u << 2) : (byte)0;

            bool child_exists = Convert.ToBoolean(branch_node.child_exists & (byte)(1u << child_location));
            if (child_exists) {
                var child_node = NodeGetChild(branch_node.location_code, child_location);
                if (child_node is LeafNode) {
                    branch_node = SplitLeaf((LeafNode)child_node);
                }
                else if (child_node is BranchNode) {
                    branch_node = (BranchNode)child_node;
                }

                return FindEmptyLeaf(branch_node, object_position);
            }

            return AddLeaf(branch_node.location_code, child_location, uint.MaxValue);
        }

        return null;
    }

    BranchNode SplitLeaf(LeafNode node)
    {
        uint object_index = node.object_index;
        ulong location_code = node.location_code;
        RemoveLeaf(location_code);

        BranchNode branch_node = AddBranch(location_code);

        LeafNode new_child = FindEmptyLeaf(branch_node, object_positions[object_index]);
        new_child.object_index = object_index;

        return branch_node;
    }

    Vector3 NodeGetPosition(BaseNode node)
    {
        Vector3 position = this.position;

        ushort tree_depth = NodeGetDepth(node);
        for (int i = 0; i < tree_depth; i++) {
            int offset = tree_depth * 3 - 3 * (i + 1);
            byte local_code = (byte)((node.location_code >> offset) & 7); // 7 = 0b111
            position.x = Convert.ToBoolean(local_code & 1u)
                              ? position.x + this.size / Mathf.Pow(2.0f, (i + 1))
                              : position.x - this.size / Mathf.Pow(2.0f, (i + 1));
            position.y = Convert.ToBoolean(local_code & 2u)
                              ? position.y + this.size / Mathf.Pow(2.0f, (i + 1))
                              : position.y - this.size / Mathf.Pow(2.0f, (i + 1));
            position.z = Convert.ToBoolean(local_code & 4u)
                              ? position.z + this.size / Mathf.Pow(2.0f, (i + 1))
                              : position.z - this.size / Mathf.Pow(2.0f, (i + 1));
        }

        return position;
    }

    ushort NodeGetDepth(BaseNode node)
    {
        ushort depth = 0;
        for (ulong location_code = node.location_code; location_code != 1; location_code >>= 3) {
            depth++;
        }

        return depth;
    }

    float NodeGetSize(BaseNode node)
    {
        return NodeGetDepth(node) > 0 ? size / Mathf.Pow(2f, NodeGetDepth(node)) : size;
    }

    object NodeGetParent(BaseNode node)
    {
        ulong location_code_parent = node.location_code >> 3;
        return LookupNode(location_code_parent);
    }

    object NodeGetChild(ulong location_code, byte child_location)
    {
        ulong child_location_code = (location_code << 3) | child_location;
        return LookupNode(child_location_code);
    }

    object LookupNode(ulong location_code)
    {
        return nodes[location_code];
    }

    // TEST CASE
    void VisitAll(BaseNode node)
    {
        for (int i = 0; i < 8; i++) {
            if (node.type == 0) {
                BranchNode branch_node = (BranchNode)node;
                if (Convert.ToBoolean(branch_node.child_exists & (1 << i))) {
                    BaseNode child = (BaseNode)NodeGetChild(node.location_code, (byte)i);
                    VisitAll(child);
                }
            }
            else {
                Debug.Log(node);
            }
        }
    }

    /// <summary>
    /// /////////////////////////////////////////
    /// </summary>
    public struct NodeDebug
    {
        public Vector3 position;
        public float size;
    }

    public NodeDebug[] DebugGetNodes()
    {
        NodeDebug[] debug_nodes = new NodeDebug[nodes.Count];

        uint i = 0;
        foreach (var node in nodes) {
            debug_nodes[i].position = NodeGetPosition((BaseNode)node.Value);
            debug_nodes[i].size = NodeGetSize((BaseNode)node.Value) * 2;
            ++i;
        }

        return debug_nodes;
    }
}
