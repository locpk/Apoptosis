using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AcidicArea : BaseArea {

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
            StemCell stemCell = collider.gameObject.GetComponent<StemCell>();

            if (enterCell.celltype == CellType.STEM_CELL) {
                StopCoroutine("ReadyToConvert");
                StartCoroutine(ReadyToConvert(pendingConvertDelayed, stemCell));

            }
        }
    }


    void OnTriggerStay(Collider collider) {

    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Unit") {
            StemCell stemCell = collider.gameObject.GetComponent<StemCell>();
            if (stemCell) {
                stemCell.isInAcidic = false;

            }
        }
    }

    IEnumerator ReadyToConvert(float delayed, StemCell stemCell) {
        yield return new WaitForSeconds(delayed);
        // to toggle on the pending converting
        if (stemCell)
            stemCell.isInAcidic = true;
    }
}