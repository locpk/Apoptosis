using UnityEngine;
using System.Collections;

public class FogOfWarHider : MonoBehaviour {
    Transform m_trans;
	Renderer[] m_hider;
	float m_nextUpdate = 0f;
	bool m_isVisible = true;
	bool m_update = true;

	public bool isVisible { get { return m_isVisible; } }

	public void Rebuild () { m_update = true; }

    void Awake() 
    {
        m_trans = transform;
        if (gameObject.tag != "Protein")
        {
            gameObject.transform.FindChild("MinimapIndicator").GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
	void LateUpdate() {
        if (m_nextUpdate < Time.time) {
            UpdateNow();
        }
    }

	void UpdateNow () {
		m_nextUpdate = Time.time + 0.075f + Random.value * 0.05f;

		if (FogOfWarController.instance == null) {
			enabled = false;
			return;
		}

		if (m_update) 
            m_hider = GetComponentsInChildren<Renderer>();

		bool visible = FogOfWarController.instance.IsVisible(m_trans.position);

		if (m_update || m_isVisible != visible) {
			m_update = false;
			m_isVisible = visible;

			for (int i = 0, imax = m_hider.Length; i < imax; ++i) {
				Renderer ren = m_hider[i];

				if (ren) {
					ren.enabled = m_isVisible;
				} else {
					m_update = true;
					m_nextUpdate = Time.time;
				}
			}
		}
	}
}
