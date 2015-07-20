using UnityEngine;
using System.Collections;

public class FogOfWarViewer : MonoBehaviour
{
    Transform m_trans;
    public Vector2 viewOfRange = new Vector2(2.0f, 6.0f);
    public FogOfWarController.LOSChecks lineOfSightCheck = FogOfWarController.LOSChecks.EveryUpdate;
    public bool isActive = true;

    FogOfWarController.Viewer m_viewer;

    void Awake()
    {
        m_trans = transform;
        m_viewer = FogOfWarController.CreateViewer();

        if (GetComponent<BaseCell>())
        {
            viewOfRange.x = GetComponent<BaseCell>().fovRadius;
            viewOfRange.y = viewOfRange.x * 3.0f;
        }

    }

    void OnDisable()
    {
        m_viewer.isActive = false;
    }

    void OnDestroy()
    {
        FogOfWarController.DeleteViewer(m_viewer);
        m_viewer = null;
    }

    void LateUpdate()
    {
        if (isActive)
        {
            if (lineOfSightCheck != FogOfWarController.LOSChecks.OnlyOnce) m_viewer.cachedBuffer = null;

            m_viewer.pos = m_trans.position;
            m_viewer.inner = viewOfRange.x;
            m_viewer.outer = viewOfRange.y;
            m_viewer.los = lineOfSightCheck;
            m_viewer.isActive = true;
        }
        else
        {
            m_viewer.isActive = false;
            m_viewer.cachedBuffer = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (lineOfSightCheck != FogOfWarController.LOSChecks.None && viewOfRange.x > 0f)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, viewOfRange.x);
        }
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, viewOfRange.y);
    }

    public void Rebuild() { m_viewer.cachedBuffer = null; }
}
