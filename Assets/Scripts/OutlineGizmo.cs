using UnityEngine;
// using UnityEditor;
// using System.Numerics;


// public class OutlineGizmo : MonoBehaviour
// {
// }

// The icon has to be stored in Assets/Gizmos
// outline the panel the cards are instantiated in
public class OutlineGizmo : MonoBehaviour
{
    Vector3[] points;

    void Start()
    {
        points = new Vector3[4]
        {
            new Vector3(-100, 0, 0),
            new Vector3(100, 0, 0),
            new Vector3(100, 100, 0),
            new Vector3(-100, 100, 0)
        };
    }

    void OnDrawGizmosSelected()
    {
        // Draws four lines making a square
        Gizmos.color = Color.blue;
        Gizmos.DrawLineStrip(points, true);
    }
    // public GameObject gamePanelRectTransform;
    // [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    // static void DrawGizmoForMyScript(RectTransform gamePanelRectTransform, GizmoType gizmoType)
    // {
    //     UnityEngine.Vector3 position = gamePanelRectTransform.transform.position;

    //     if (UnityEngine.Vector3.Distance(position, Camera.current.transform.position) > 10f)
    //     {

    //         Gizmos.DrawLineList(new System.ReadOnlySpan<UnityEngine.Vector3>(new UnityEngine.Vector3[] { position }));

    //     }
    // }
}
