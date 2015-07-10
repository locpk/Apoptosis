using UnityEngine;
using System.Collections;

public class Protein : MonoBehaviour {

    public float value;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(value);
        }
        else
        {
            // Network player, receive data
            this.value = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
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
        //if (value >= 20.0f)
        //{
        //    transform.localScale = new Vector3(0.3f * value / 110, 0.3f * value / 110, 0.3f * value / 110);
        //}
        //else
        //{
        //    transform.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //}

        transform.localScale = new Vector3(0.3f * value / 110 + .2f, 0.3f * value / 110 + .2f, 0.3f * value / 110 + .2f);
        if (value <= 0.0f)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
