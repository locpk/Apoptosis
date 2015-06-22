using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeatArea : BaseArea {

    public float damagePerSecond;

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
            if (!enterCell) return;

            switch (enterCell.celltype) {
                case CellType.STEM_CELL:
                    break;
                case CellType.HEAT_CELL:
                    break;
                case CellType.COLD_CELL:
                    break;
                case CellType.HEAT_CELL_TIRE2:
                    break;
                case CellType.COLD_CELL_TIRE2:
                    break;
                case CellType.ACIDIC_CELL:
                    break;
                case CellType.ALKALI_CELL:
                    break;
                case CellType.CANCER_CELL:
                    break;
                default:
                    break;
            }
        }
    }


    void OnTriggerStay(Collider collider) {

    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Unit") {
            BaseCell enterCell = collider.gameObject.GetComponent<BaseCell>();
            if (!enterCell) return;

            switch (enterCell.celltype) {
                case CellType.STEM_CELL:
                    break;
                case CellType.HEAT_CELL:
                    break;
                case CellType.COLD_CELL:
                    break;
                case CellType.HEAT_CELL_TIRE2:
                    break;
                case CellType.COLD_CELL_TIRE2:
                    break;
                case CellType.ACIDIC_CELL:
                    break;
                case CellType.ALKALI_CELL:
                    break;
                case CellType.CANCER_CELL:
                    break;
                default:
                    break;
            }
        }
    }
}