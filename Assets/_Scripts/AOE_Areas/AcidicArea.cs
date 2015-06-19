using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AcidicArea : BaseArea {

    public float damagePerSecond;
    public float convertingDelayed = 5.0f;

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
                StartCoroutine(ConvertToAcidicCell(convertingDelayed, enterCell));

            }
        }
    }

    //void OnTriggerStay(Collider collider) {
    //    if (collider.gameObject.tag == "Unit") {
    //        BaseCell enterCell = collider.gameObject.GetComponent<BaseCell>();

    //        if (enterCell.celltype == CellType.ACIDIC_CELL) {
    //            StartCoroutine(ConvertToAcidicCell(convertingDelayed, enterCell));

    //        }
    //    }
    //}

    IEnumerator ConvertToAcidicCell(float delayed, BaseCell baseCell) {

        yield return new WaitForSeconds(delayed);

        baseCell.Mutation(CellType.ACIDIC_CELL);
    }
}