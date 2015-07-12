using UnityEngine;
using System.Collections;

public class FogOfWarViewer : MonoBehaviour {
	Transform m_trans;
	public Vector2 range = new Vector2(2.0f, 30.0f);
	public FogOfWarController.LOSChecks lineOfSightCheck = FogOfWarController.LOSChecks.None;
	public bool isActive = true;

    FogOfWarController.Viewer m_viewer;

	void Awake () {
		m_trans = transform;
		m_viewer = FogOfWarController.CreateViewer();
	}

	void OnDisable () {
		m_viewer.isActive = false;
	}

	void OnDestroy () {
		FogOfWarController.DeleteViewer(m_viewer);
		m_viewer = null;
	}
	
	void LateUpdate () {
		if (isActive) {
			if (lineOfSightCheck != FogOfWarController.LOSChecks.OnlyOnce) m_viewer.cachedBuffer = null;

			m_viewer.pos = m_trans.position;
			m_viewer.inner = range.x;
			m_viewer.outer = range.y;
			m_viewer.los = lineOfSightCheck;
			m_viewer.isActive = true;
		} else {
			m_viewer.isActive = false;
			m_viewer.cachedBuffer = null;
		}
	}

	void OnDrawGizmosSelected () {
		if (lineOfSightCheck != FogOfWarController.LOSChecks.None && range.x > 0f) {
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(transform.position, range.x);
		}
		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere(transform.position, range.y);
	}

	public void Rebuild () { m_viewer.cachedBuffer = null; }
}
