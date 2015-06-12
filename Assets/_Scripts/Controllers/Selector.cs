using UnityEngine;
using System.Collections;

/// <summary>
/// Draw a marker above the terrain when the object is selected
/// </summary>
public class Selector : MonoBehaviour {

    // The entity to display this marker for
    public Transform parent;

    void Start() {
        parent = transform.parent;
        GetComponent<Renderer>().material.color = Color.green;
        GetComponent<Renderer>().enabled = false;
    }

    public void LateUpdate() {
        // Update the position to be slightly above the terrain
        // at the entities position
        var pos = parent.position;
        pos.y = 0.1f;
        transform.position = pos;
    }

}
