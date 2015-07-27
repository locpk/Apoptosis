using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Protein : MonoBehaviour {

    public float value;
    public float consumeRate = 30.0f;
    public bool beingConsumed = false;
    ParticleSystem emitter;
    public List<BaseCell> consumers;
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
        value -= consumeRate;
        return value >= 0 ? consumeRate : consumeRate + value;
    }

    void Awake()
    {
        GameObject.Find("PlayerControl").GetComponent<PlayerController>().allSelectableTargets.Add(gameObject);
    }
	// Use this for initialization
	void Start () {
        value = Random.Range(200, 300);
        emitter = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        
        
	}
    void FixedUpdate()
    {
        if (consumers.FindAll(item => (item != null)&&(item.GetComponent<BaseCell>().currentState == CellState.CONSUMING)).Count == 0)
	    {
            beingConsumed = false;
            consumers.Clear();
	    }
        else
        {
            consumers.RemoveAll(item => (item != null)&&(item.GetComponent<BaseCell>().currentState != CellState.CONSUMING));
            beingConsumed = true;
        }
        
        if (beingConsumed)
        {
            if (emitter.isStopped)
            {
                emitter.Play();
            }
        }
        else
        {
            if (emitter.isPlaying)
            {
                emitter.Stop();
            }
        }
    }
    void LateUpdate()
    {
        float scale = 0.3f * value / 300.0f + .2f;
        transform.localScale = new Vector3(scale, scale, 1.0f);
        if (value <= 0.0f)
        {
            if (PhotonNetwork.connected)
                PhotonNetwork.Destroy(gameObject);
            else
                Destroy(gameObject);
        }
    }
}
