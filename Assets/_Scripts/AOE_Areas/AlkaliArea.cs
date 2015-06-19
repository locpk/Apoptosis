using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlkaliArea : BaseArea {

    public float damagePerSecond;
    public float pendingConvertDelayed = 5.0f;

	public override void Awake() {
        base.Awake();

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
        if (collider.gameObject.tag == "Unit") {
            BaseCell enterCell = collider.gameObject.GetComponent<BaseCell>();

            if (enterCell.celltype == CellType.STEM_CELL) {
                StopCoroutine("ReadyToConvert");
                StartCoroutine(ReadyToConvert(pendingConvertDelayed, collider.gameObject.GetComponent<StemCell>()));

            }
        }
    }

    void OnTriggerStay(Collider collider) {

    }

    void OnTriggerExit(Collider collider) {
        collider.gameObject.GetComponent<StemCell>().isInAlkali = false;

    }

    IEnumerator ReadyToConvert(float delayed, StemCell stemCell) {
        yield return new WaitForSeconds(delayed);
        // to toggle on the pending converting
        stemCell.isInAlkali = true;

    }




}