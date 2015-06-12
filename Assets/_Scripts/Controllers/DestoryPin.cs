using UnityEngine;
using System.Collections;

public class DestoryPin : MonoBehaviour {

    void DestoryPinObject()
    {
        GameObject.Destroy(this.gameObject.transform.parent.gameObject);
    }
}
