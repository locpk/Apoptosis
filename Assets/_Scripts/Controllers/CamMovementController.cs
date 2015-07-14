using UnityEngine;
using System.Collections;

public class CamMovementController : MonoBehaviour {

	Transform m_trans;
	Vector3 m_mouse;
	Vector3 m_targetPos;
	Vector3 m_targetEuler;

	void Start () {
		m_trans = transform;
		m_mouse = Input.mousePosition;
		m_targetPos = m_trans.position;
		m_targetEuler = m_trans.rotation.eulerAngles;
	}

	void Update () {
		Vector3 delta = Input.mousePosition - m_mouse;
		m_mouse = Input.mousePosition;

		if (Input.GetMouseButton(2)) {
			Vector3 dir = transform.rotation * Vector3.forward;
			dir.y = 0f;
			dir.Normalize();
			Quaternion rot = Quaternion.LookRotation(dir);
			m_targetPos += rot * new Vector3(delta.x * 0.1f, 0f, delta.y * 0.1f);

		}

		float deltaTime = Time.deltaTime * 8f;
		m_trans.position = Vector3.Lerp(m_trans.position, m_targetPos, deltaTime);
		m_trans.rotation = Quaternion.Slerp(m_trans.rotation, Quaternion.Euler(m_targetEuler), deltaTime);
	}
}
