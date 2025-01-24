using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class PolygonColliderGenerator : Editor
{
    [MenuItem("Tools/Generate Polygon Colliders")]
    static void GeneratePolygonColliders()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            Debug.LogError("No objects selected! Please select GameObjects to generate colliders.");
            return;
        }

        foreach (GameObject selectedObject in selectedObjects)
        {
            FreezeTransforms(selectedObject); // Call FreezeTransforms before generating collider
            GeneratePolygonCollider(selectedObject);
        }
    }

    static void GeneratePolygonCollider(GameObject targetObject)
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is null!");
            return;
        }

        MeshFilter meshFilter = targetObject.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("Selected object does not have a mesh!");
            return;
        }

        PolygonCollider2D collider = targetObject.GetComponent<PolygonCollider2D>();
        if (collider == null)
        {
            collider = targetObject.AddComponent<PolygonCollider2D>();
        }

        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        List<Vector2> points = ConvexHull(vertices);

        collider.points = points.ToArray();

        Debug.Log("Polygon Collider generated successfully for " + targetObject.name + "!");
    }

    static void FreezeTransforms(GameObject obj)
    {
        // Get the mesh filter and renderer
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogWarning("MeshFilter component not found on " + obj.name + ". Skipping...");
            return;
        }

        // Create a new mesh to avoid modifying the original mesh
        Mesh newMesh = Instantiate(meshFilter.sharedMesh);
        meshFilter.mesh = newMesh;

        // Get the mesh vertices
        Vector3[] vertices = newMesh.vertices;
        Vector3[] worldVertices = new Vector3[vertices.Length];

        // Get the object's transform
        Transform objTransform = obj.transform;

        // Convert vertices to world space and calculate the center
        Vector3 center = Vector3.zero;
        for (int i = 0; i < vertices.Length; i++)
        {
            worldVertices[i] = objTransform.TransformPoint(vertices[i]);
            center += worldVertices[i];
        }
        center /= vertices.Length;

        // Calculate the offset to move the pivot to the center
        Vector3 pivotOffset = center - objTransform.position;

        // Move the object to center the pivot
        objTransform.position += pivotOffset;

        // Reset transform rotation and scale
        objTransform.rotation = Quaternion.identity;
        objTransform.localScale = Vector3.one;

        // Convert vertices back to local space
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = objTransform.InverseTransformPoint(worldVertices[i]);
        }

        // Update the mesh
        newMesh.vertices = vertices;
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();

        // Mark the object as dirty to ensure the changes are saved
        EditorUtility.SetDirty(obj);
    }

    static List<Vector2> ConvexHull(Vector3[] vertices)
    {
        List<Vector2> convexHull = new List<Vector2>();

        List<Vector2> points = vertices.Select(v => new Vector2(v.x, v.y)).ToList();
        points = points.OrderBy(p => p.x).ToList();

        // Compute the lower hull
        for (int i = 0; i < points.Count; i++)
        {
            while (convexHull.Count >= 2 && CrossProduct(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], points[i]) <= 0)
            {
                convexHull.RemoveAt(convexHull.Count - 1);
            }
            convexHull.Add(points[i]);
        }

        // Compute the upper hull
        int upperHullStartIndex = convexHull.Count + 1;
        for (int i = points.Count - 2; i >= 0; i--)
        {
            while (convexHull.Count >= upperHullStartIndex && CrossProduct(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], points[i]) <= 0)
            {
                convexHull.RemoveAt(convexHull.Count - 1);
            }
            convexHull.Add(points[i]);
        }

        return convexHull.Distinct().ToList();
    }

    static float CrossProduct(Vector2 O, Vector2 A, Vector2 B)
    {
        return (A.x - O.x) * (B.y - O.y) - (A.y - O.y) * (B.x - O.x);
    }
}
