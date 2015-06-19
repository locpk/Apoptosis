using UnityEngine;
using System.Collections;

public class MidForm : MonoBehaviour {

    public float hp;
	// Use this for initialization
	void Start () {
        hp = GetComponent<CellSplitAnimation>().currentProtein;
	}
	
	// Update is called once per frame
	void Update () {
        if (hp <= 0.0f)
        {
            DestroyImmediate(gameObject);
        }
	}
}
