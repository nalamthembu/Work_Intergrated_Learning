using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;

        Handles.color = Color.white;

        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.m_ViewRadius);

        Vector3 viewAngleA = fov.DirFromAngle(-fov.m_ViewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.m_ViewAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.m_ViewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.m_ViewRadius);


        Handles.color = Color.red;

        foreach (Transform visibleTarget in fov.VisibleTargets)
            Handles.DrawLine(fov.transform.position, visibleTarget.transform.position);
    }
}
