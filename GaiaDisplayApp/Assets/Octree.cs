using System.Collections.Generic;
using UnityEngine;
using Mono.Simd;

class SparseOctree 
{
    /* private SparseOctree[] children; */
    /* private GalacticObject galacticObject; */
    /* private Vector3 position; */
    /* private float size; */

    /* /// <summary> */
    /* /// Child position for based on binary format. */
    /* /// This gives the following structure: */
    /* /// Front:      Back: */
    /* ///  --- ---     --- --- */ 
    /* /// | 2 | 3 |   | 6 | 7 | */
    /* /// | - | - |   | - | - | */
    /* /// | 0 | 1 |   | 4 | 5 | */
    /* ///  --- ---     --- --- */
    /* /// </summary> */
    /* private enum */ 
    /* ChildPos */ 
    /* { */
    /*     X = 1, */
    /*     Y = 2, */
    /*     Z = 3 */
    /* } */

    /* SparseOctree(Vector3 position, float size) */
    /* { */
    /*     this.position = position; */
    /*     this.size = size; */
    /* } */

    /* /// <summary> */
    /* /// Inserts the new galactic object in the node, unless this node is */
    /* /// already filled, in which case we split the node. */
    /* /// </summary> */
    /* /// <param name="obj">Object.</param> */
    /* public void */ 
    /* InsertObject(GalacticObject obj) */
    /* { */
    /*     if (ReferenceEquals(galacticObject, null)) { */
    /*         galacticObject = obj; */
    /*         return; */
    /*     } */

    /*     Split(); */
    /*     GetChildFromPosition(obj.coordinates).InsertObject(obj); */
    /* } */

    /* /// <summary> */
    /* /// Split this node into smaller children and put the galacticObject of */
    /* /// this node in one of the children nodes. */
    /* /// </summary> */
    /* private void */ 
    /* Split() */
    /* { */
    /*     children = new SparseOctree[8]; */
    /*     float childSize = size * .5f; */

    /*     children[0] = new SparseOctree(new Vector3(position.x - childSize, position.y - childSize, position.z - childSize), childSize); */
    /*     children[1] = new SparseOctree(new Vector3(position.x + childSize, position.y - childSize, position.z - childSize), childSize); */
    /*     children[2] = new SparseOctree(new Vector3(position.x - childSize, position.y + childSize, position.z - childSize), childSize); */
    /*     children[3] = new SparseOctree(new Vector3(position.x + childSize, position.y + childSize, position.z - childSize), childSize); */
    /*     children[4] = new SparseOctree(new Vector3(position.x - childSize, position.y - childSize, position.z + childSize), childSize); */
    /*     children[5] = new SparseOctree(new Vector3(position.x + childSize, position.y - childSize, position.z + childSize), childSize); */
    /*     children[6] = new SparseOctree(new Vector3(position.x - childSize, position.y + childSize, position.z + childSize), childSize); */
    /*     children[7] = new SparseOctree(new Vector3(position.x + childSize, position.y + childSize, position.z + childSize), childSize); */

    /*     // When we create the child nodes the object held by the current node should be passed to one of the child nodes */
    /*     GetChildFromPosition(galacticObject.coordinates).InsertObject(galacticObject); */
    /*     galacticObject = null; */
    /* } */

    /* /// <summary> */
    /* /// Gets the child from position. */ 
    /* /// </summary> */
    /* /// <returns>The child from position.</returns> */
    /* /// <param name="objectPosition">Object position.</param> */
    /* private SparseOctree */ 
    /* GetChildFromPosition(Vector3 objectPosition) */
    /* { */
    /*     uint child = 0; */
    /*     child |= objectPosition.x < position.x ? 0 : (uint)ChildPos.X; */
    /*     child |= objectPosition.y < position.y ? 0 : (uint)ChildPos.Y; */
    /*     child |= objectPosition.z < position.z ? 0 : (uint)ChildPos.Z; */

    /*     return children[child]; */
    /* } */

    /* /// <summary> */
    /* /// Finds the child from position. But does this recursively through it's child nodes */
    /* /// </summary> */
    /* /// <returns>The child from position.</returns> */
    /* /// <param name="objectPosition">Object position.</param> */
    /* public SparseOctree */
    /* FindChildFromPosition(Vector3 objectPosition) */
    /* { */
    /*     SparseOctree child = null; */
    /*     while (ReferenceEquals(GetChildFromPosition(objectPosition).children, null)) { */
    /*         GetChildFromPosition(objectPosition); */
    /*     } */

    /*     return child; */
    /* } */

    /* /// <summary> */
    /* /// Recursively counts the children. This does include the empty nodes */
    /* /// </summary> */
    /* /// <returns>The number of children.</returns> */
    /* private uint */
    /* CountChildren() */
    /* { */
    /*     uint count = 0; */

    /*    // Recursive count */
    /*     for (int i = 0; i < 8; ++i) { */
    /*         if (!ReferenceEquals(children, null)) { */
    /*             count += 1 + children[i].CountChildren(); */
    /*         } */
    /*     } */
    /*     return count; */
    /* } */

/* void sse_culling_aabb(SparseOctree[] octree_nodes, int num_objects, int[] culling_res) */
/* { */
    /* // To optimize calculations we gather xyzw elements in separate vectors */
    /* Vector4f frustum_planes_x = new Vector4f(); */
    /* Vector4f frustum_planes_y = new Vector4f(); */
    /* Vector4f frustum_planes_z = new Vector4f(); */

    /* Plane[] frustum_planes = GeometryUtility.CalculateFrustumPlanes(Camera.main); */

    /* int i, j; */
    /* for (i = 0; i < 4; i++) { */
    /*     frustum_planes_x[i] = frustum_planes[i].normal.x; */
    /*     frustum_planes_y[i] = frustum_planes[i].normal.y; */
    /*     frustum_planes_z[i] = frustum_planes[i].normal.z; */
    /* } */

    /* // We process 4 objects per step */
    /* for (i = 0; i < num_objects; i += 4) { */
    /*     aabb_min_x = _mm_load_ps(aabb_data_ptr); */
    /*     __m128 aabb_min_y = _mm_load_ps(aabb_data_ptr + 8); */
    /*     __m128 aabb_min_z = _mm_load_ps(aabb_data_ptr + 16); */
    /*     __m128 aabb_min_w = _mm_load_ps(aabb_data_ptr + 24); */
    /*     //load aabb max */
    /*     __m128 aabb_max_x = _mm_load_ps(aabb_data_ptr + 4); */
    /*     __m128 aabb_max_y = _mm_load_ps(aabb_data_ptr + 12); */
    /*     __m128 aabb_max_z = _mm_load_ps(aabb_data_ptr + 20); */
    /*     __m128 aabb_max_w = _mm_load_ps(aabb_data_ptr + 28); */
    /*     aabb_data_ptr += 32; */
    /*     //for now we have points in vectors aabb_min_x..w, but for calculations we need to xxxx yyyy zzzz vectors representation - just transpose data */
    /*     _MM_TRANSPOSE4_PS(aabb_min_x, aabb_min_y, aabb_min_z, aabb_min_w); */
    /*     _MM_TRANSPOSE4_PS(aabb_max_x, aabb_max_y, aabb_max_z, aabb_max_w); */
    /*     __m128 intersection_res = _mm_setzero_ps(); */

    /*     for (j = 0; j < 4; j++) { */
    /*         //this code is similar to what we make in simple culling */
    /*         //pick closest point to plane and check if it begind the plane. if yes - object outside frustum */
    /*         //dot product, separate for each coordinate, for min & max aabb points */
    /*         __m128 aabbMin_frustumPlane_x = _mm_mul_ps(aabb_min_x, frustum_planes_x[j]); */
    /*         __m128 aabbMin_frustumPlane_y = _mm_mul_ps(aabb_min_y, frustum_planes_y[j]); */
    /*         __m128 aabbMin_frustumPlane_z = _mm_mul_ps(aabb_min_z, frustum_planes_z[j]); */
    /*         __m128 aabbMax_frustumPlane_x = _mm_mul_ps(aabb_max_x, frustum_planes_x[j]); */
    /*         __m128 aabbMax_frustumPlane_y = _mm_mul_ps(aabb_max_y, frustum_planes_y[j]); */
    /*         __m128 aabbMax_frustumPlane_z = _mm_mul_ps(aabb_max_z, frustum_planes_z[j]); */
    /*         //we have 8 box points, but we need pick closest point to plane. Just take max */
    /*         __m128 res_x = _mm_max_ps(aabbMin_frustumPlane_x, aabbMax_frustumPlane_x); */
    /*         __m128 res_y = _mm_max_ps(aabbMin_frustumPlane_y, aabbMax_frustumPlane_y); */
    /*         __m128 res_z = _mm_max_ps(aabbMin_frustumPlane_z, aabbMax_frustumPlane_z); */
    /*         //dist to plane = dot(aabb_point.xyz, plane.xyz) + plane.w */
    /*         __m128 sum_xy = _mm_add_ps(res_x, res_y); */
    /*         __m128 sum_zw = _mm_add_ps(res_z, frustum_planes_d[j]); */
    /*         __m128 distance_to_plane = _mm_add_ps(sum_xy, sum_zw); */
    /*         __m128 plane_res = _mm_cmple_ps(distance_to_plane, zero); //dist from closest point to plane < 0 ? */
    /*         intersection_res = _mm_or_ps(intersection_res, plane_res); //if yes - aabb behind the plane & outside frustum */
    /*     } */

    /*     // Store result */
    /*     __m128i intersection_res_i = _mm_cvtps_epi32(intersection_res); */
    /*     _mm_store_si128((__m128i*)&culling_res_sse, intersection_res_i); */
    /* } */
/* } */
}
