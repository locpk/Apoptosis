using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DenseArea : BaseArea {

    //public float speedDecreaseRate;
    public float speedCoefficient = 0.2f;
    private float enterCellSpeed;

    private Sound_Manager sound_manager;

	public override void Awake() {
        base.Awake();
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();

    }

	public override void Start () {
        base.Start();

	}
	
	public override void Update () {
        base.Update();

	}

	public override void FixedUpdate() {
        base.FixedUpdate();

    }

	public override void LateUpdate() {
        base.LateUpdate();

    }

    
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Unit" || collider.gameObject.tag == "EnemyCell") {
            BaseCell enterCell = collider.gameObject.GetComponent<BaseCell>();
            if (!enterCell) return;
            enterCellSpeed = enterCell.navAgent.speed;
            enterCell.navAgent.speed *= speedCoefficient;
            if (!sound_manager.sounds_miscellaneous[4].isPlaying)
            {
                sound_manager.sounds_miscellaneous[4].Play();

            }
        }
    }


    void OnTriggerStay(Collider collider) {

    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Unit" || collider.gameObject.tag == "EnemyCell") {
            BaseCell enterCell = collider.gameObject.GetComponent<BaseCell>();
            if (!enterCell) return;
            enterCell.navAgent.speed = enterCellSpeed;
            if (!sound_manager.sounds_miscellaneous[5].isPlaying)
            {
                sound_manager.sounds_miscellaneous[5].Play();

            }
        }
    }
}