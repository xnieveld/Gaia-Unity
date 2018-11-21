using System.Collections.Generic;
using UnityEngine;

class SparseOctree 
{
    private SparseOctree[] children;
    private GalacticObject galacticObject;
    private Vector3 position;
    private float size;

    /// <summary>
    /// Child position for based on binary format.
    /// This gives the following structure:
    /// Front:      Back:
    ///  --- ---     --- --- 
    /// | 2 | 3 |   | 6 | 7 |
    /// | - | - |   | - | - |
    /// | 0 | 1 |   | 4 | 5 |
    ///  --- ---     --- ---
    /// </summary>
    private enum 
    ChildPos 
    {
        X = 1,
        Y = 2,
        Z = 3
    }

    SparseOctree(Vector3 position, float size)
    {
        this.position = position;
        this.size = size;
    }

    /// <summary>
    /// Inserts the new galactic object in the node, unless this node is
    /// already filled, in which case we split the node.
    /// </summary>
    /// <param name="obj">Object.</param>
    public void 
    InsertObject(GalacticObject obj)
    {
        if (ReferenceEquals(galacticObject, null)) {
            galacticObject = obj;
            return;
        }

        Split();
        GetChildFromPosition(obj.coordinates).InsertObject(obj);
    }

    /// <summary>
    /// Split this node into smaller children and put the galacticObject of
    /// this node in one of the children nodes.
    /// </summary>
    private void 
    Split()
    {
        children = new SparseOctree[8];
        float childSize = size * .5f;

        children[0] = new SparseOctree(new Vector3(position.x - childSize, position.y - childSize, position.z - childSize), childSize);
        children[1] = new SparseOctree(new Vector3(position.x + childSize, position.y - childSize, position.z - childSize), childSize);
        children[2] = new SparseOctree(new Vector3(position.x - childSize, position.y + childSize, position.z - childSize), childSize);
        children[3] = new SparseOctree(new Vector3(position.x + childSize, position.y + childSize, position.z - childSize), childSize);
        children[4] = new SparseOctree(new Vector3(position.x - childSize, position.y - childSize, position.z + childSize), childSize);
        children[5] = new SparseOctree(new Vector3(position.x + childSize, position.y - childSize, position.z + childSize), childSize);
        children[6] = new SparseOctree(new Vector3(position.x - childSize, position.y + childSize, position.z + childSize), childSize);
        children[7] = new SparseOctree(new Vector3(position.x + childSize, position.y + childSize, position.z + childSize), childSize);

        // When we create the child nodes the object held by the current node should be passed to one of the child nodes
        GetChildFromPosition(galacticObject.coordinates).InsertObject(galacticObject);
        galacticObject = null;
    }

    /// <summary>
    /// Gets the child from position. 
    /// </summary>
    /// <returns>The child from position.</returns>
    /// <param name="objectPosition">Object position.</param>
    private SparseOctree 
    GetChildFromPosition(Vector3 objectPosition)
    {
        uint child = 0;
        child |= objectPosition.x < position.x ? 0 : (uint)ChildPos.X;
        child |= objectPosition.y < position.y ? 0 : (uint)ChildPos.Y;
        child |= objectPosition.z < position.z ? 0 : (uint)ChildPos.Z;

        return children[child];
    }

    /// <summary>
    /// Finds the child from position. But does this recursively through it's child nodes
    /// </summary>
    /// <returns>The child from position.</returns>
    /// <param name="objectPosition">Object position.</param>
    public SparseOctree
    FindChildFromPosition(Vector3 objectPosition)
    {
        SparseOctree child = null;
        while (ReferenceEquals(GetChildFromPosition(objectPosition).children, null)) {
            GetChildFromPosition(objectPosition);
        }

        return child;
    }

    /// <summary>
    /// Recursively counts the children. This does include the empty nodes
    /// </summary>
    /// <returns>The number of children.</returns>
    private uint
    CountChildren()
    {
        uint count = 0;

        // Recursive count
        for (int i = 0; i < 8; ++i) {
            if (!ReferenceEquals(children, null)) {
                count += 1 + children[i].CountChildren();
            }
        }

        return count;
    }
}
