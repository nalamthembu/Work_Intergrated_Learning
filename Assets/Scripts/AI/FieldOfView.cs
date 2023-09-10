using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)] public float m_ViewRadius;
    public float m_ViewAngle;

    public LayerMask m_TargetMask, m_ObstacleMask;

    private List<Transform> visibleTargets = new();

    public List<Transform> VisibleTargets { get { return visibleTargets; } }

    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.1F));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, m_ViewRadius, m_TargetMask);

        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform t = targetInViewRadius[i].transform;

            Vector3 dirToTarget = (t.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < m_ViewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, t.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, m_ObstacleMask))
                {
                    visibleTargets.Add(t);

                    print(t.name);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
