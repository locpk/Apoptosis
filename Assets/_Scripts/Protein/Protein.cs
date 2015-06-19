using UnityEngine;
using System.Collections;

public class Protein : MonoBehaviour {

    public float value;


    public float Harvest()
    {
        value -= 5.0f;
        return value >= 0 ? 5.0f : 5.0f + value;
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
        if (value >= 100.0f)
        {
            transform.localScale = new Vector3(0.3f * value / 110, 0.3f * value / 110, 0.3f * value / 110);
        }
        else
        {
            transform.transform.localScale = new Vector3( 30.0f / 110, 30.0f / 110,30.0f / 110);
        }
        if (value <= 0.0f)
        {
            DestroyImmediate(gameObject);
        }
    }
}
