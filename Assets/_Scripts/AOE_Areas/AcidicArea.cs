using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AcidicArea : BaseArea {

    public float damagePerSecond;

    private BaseCell enterCell;

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
        //Debug.Log(enterCell.name +"'s curProtein: "+ enterCell.currentProtein);
    }

	public override void LateUpdate() {
        base.LateUpdate();

    }

    void DamageOverSecond() {
        if (enterCell) {
            enterCell.currentProtein -= damagePerSecond;
        }
    }

    void OnTriggerEnter(Collider other) {
        enterCell = other.GetComponent<BaseCell>();

        switch (enterCell.celltype) {
            case CellType.ALKALI_CELL: {

                break;
            }
            case CellType.COLD_CELL: {
                if (!IsInvoking("DamageOverSecond")) {
                    InvokeRepeating("DamageOverSecond", 1.0f, 1.0f);
                }
                break;
            }
            case CellType.COLD_CELL_TIRE2: {
                if (!IsInvoking("DamageOverSecond")) {
                    InvokeRepeating("DamageOverSecond", 1.0f, 1.0f);
                }
                break;
            }
            case CellType.HEAT_CELL: {
                InvokeRepeating("DamageOverSecond", 1.0f, 1.0f);
                break;
            }
            case CellType.HEAT_CELL_TIRE2: {
                if (!IsInvoking("DamageOverSecond")) {
                    InvokeRepeating("DamageOverSecond", 1.0f, 1.0f);
                }
                break;
            }
            default: {
                break;
            }
        }
    }

    void OnTriggerStay(Collider other) {

    }

    void OnTriggerExit(Collider other) {
        CancelInvoke("DamageOverSecond");
    }
}