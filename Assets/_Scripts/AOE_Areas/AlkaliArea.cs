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
        if (collider.gameObject.tag == "Unit" && collider.gameObject.tag == "EnemyCell") {
            BaseCell enterCell = collider.gameObject.GetComponent<BaseCell>();
            if (!enterCell) return;

            switch (enterCell.celltype) {
                case CellType.STEM_CELL: {
                    StemCell stemCell = enterCell.GetComponent<StemCell>();
                    StopCoroutine("ReadyToConvert");
                    StartCoroutine(ReadyToConvert(pendingConvertDelayed, stemCell));
                    break;
                }
                    
                case CellType.HEAT_CELL:
                    HeatCell heatCell = enterCell.GetComponent<HeatCell>();
                    heatCell.multidamagesources += heatCell.AreaDamage;

                    break;
                case CellType.COLD_CELL:
                    ColdCell coldCell = enterCell.GetComponent<ColdCell>();
                    coldCell.multidamagesources += coldCell.AreaDamage;

                    break;
                case CellType.HEAT_CELL_TIRE2:

                    break;
                case CellType.COLD_CELL_TIRE2:

                    break;
                case CellType.ACIDIC_CELL:
                    AcidicCell acidicCell = enterCell.GetComponent<AcidicCell>();
                    acidicCell.multidamagesources += acidicCell.AreaDamage;
                    acidicCell.multidamagesources += acidicCell.AreaDamage;

                    break;
                case CellType.ALKALI_CELL:
                    //AlkaliCell alkaliCell = enterCell.GetComponent<AlkaliCell>();
                    //alkaliCell.multidamagesources += alkaliCell.AreaDamage;
                    //alkaliCell.multidamagesources += alkaliCell.AreaDamage;

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
        if (collider.gameObject.tag == "Unit" && collider.gameObject.tag == "EnemyCell") {
            BaseCell enterCell = collider.gameObject.GetComponent<BaseCell>();
            if (!enterCell) return;

            switch (enterCell.celltype) {
                case CellType.STEM_CELL: {
                    StemCell stemCell = collider.gameObject.GetComponent<StemCell>();
                    if (stemCell) {
                        stemCell.isInAlkali = false;
                    }
                    break;
                }
                    
                case CellType.HEAT_CELL:
                    HeatCell heatCell = enterCell.GetComponent<HeatCell>();
                    heatCell.multidamagesources -= heatCell.AreaDamage;

                    break;
                case CellType.COLD_CELL:
                    ColdCell coldCell = enterCell.GetComponent<ColdCell>();
                    coldCell.multidamagesources -= coldCell.AreaDamage;

                    break;
                case CellType.HEAT_CELL_TIRE2:

                    break;
                case CellType.COLD_CELL_TIRE2:

                    break;
                case CellType.ACIDIC_CELL:
                    AcidicCell acidicCell = enterCell.GetComponent<AcidicCell>();
                    acidicCell.multidamagesources -= acidicCell.AreaDamage;
                    acidicCell.multidamagesources -= acidicCell.AreaDamage;

                    break;
                case CellType.ALKALI_CELL:
                    //AlkaliCell alkaliCell = enterCell.GetComponent<AlkaliCell>();
                    //alkaliCell.multidamagesources += alkaliCell.AreaDamage;
                    //alkaliCell.multidamagesources += alkaliCell.AreaDamage;

                    break;
                case CellType.CANCER_CELL:

                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator ReadyToConvert(float delayed, StemCell stemCell) {
        yield return new WaitForSeconds(delayed);
        // to toggle on the pending converting
        if (stemCell)
            stemCell.isInAlkali = true;

    }




}