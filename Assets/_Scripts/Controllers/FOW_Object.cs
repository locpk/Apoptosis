using UnityEngine;
using System.Collections;

public class FOW_Object : MonoBehaviour {
    FogOfWar fow;

    // Use this for initialization
    void Start () {
        fow = GameObject.FindObjectOfType<FogOfWar> ().GetComponent<FogOfWar> ();
    }
    
    // Update is called once per frame
    void Update () {
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;
        if (fow.GetMaskTexture().GetPixel(x, y).a > 0.25f) {
            //GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            if (GetComponent<ParticleSystem>().isPlaying) {
                GetComponent<ParticleSystem>().Stop();
            }
            GetComponent<ParticleSystem>().Stop();
        } else {
            //GetComponent<MeshRenderer>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            if (GetComponent<ParticleSystem>().isStopped) {
                GetComponent<ParticleSystem>().Play();
            }

        }

    }
}
