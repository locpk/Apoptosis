using UnityEngine;
using System.Collections;

public class Protein : MonoBehaviour {

    public float value;


    public float Harvest()
    {
        value -= 5.0f;
        return 5.0f;
    }
	// Use this for initialization
	void Start () {
        value = Random.Range(70, 110);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void LateUpdate()
    {
        if (value <= 0.0f)
        {
            DestroyImmediate(gameObject);
        }
    }
}
