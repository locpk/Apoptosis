using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AcidicArea : BaseArea {

    public float damagePerSecond;
    public float pendingConvertDelayed = 5.0f;

    public GameObject acidicButton;

    private Sound_Manager sound_manager;
 

	public override void Awake() {
        base.Awake();

        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();
    }

	public override void Start () {
            acidicButton.GetComponent<Button>().interactable = false;
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

            if (!sound_manager.sounds_miscellaneous[6].isPlaying)
            {
                sound_manager.sounds_miscellaneous[6].Play();

            }

            if (!enterCell) return;

            switch (enterCell.celltype) {
                case CellType.STEM_CELL: {
                    StemCell stemCell = enterCell.GetComponent<StemCell>();
                    StopCoroutine("ReadyToConvert");
                    StartCoroutine(ReadyToConvert(pendingConvertDelayed, stemCell));
                    acidicButton.GetComponent<Button>().interactable = true;
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
                    Tier2HeatCell t2HeatCell = enterCell.GetComponent<Tier2HeatCell>();
                    t2HeatCell.multidamagesources += t2HeatCell.AreaDamage;
                    break;

                case CellType.COLD_CELL_TIRE2:
                    Tier2ColdCell t2ColdCell = enterCell.GetComponent<Tier2ColdCell>();
                    t2ColdCell.multidamagesources += t2ColdCell.AreaDamage;
                    break;
                case CellType.ACIDIC_CELL:
                    //AcidicCell acidicCell = enterCell.GetComponent<AcidicCell>();
                    //acidicCell.multidamagesources += acidicCell.AreaDamage;

                    break;
                case CellType.ALKALI_CELL:
                    AlkaliCell alkaliCell = enterCell.GetComponent<AlkaliCell>();
                    alkaliCell.multidamagesources += alkaliCell.AreaDamage;
                    alkaliCell.multidamagesources += alkaliCell.AreaDamage;

                    break;
                case CellType.CANCER_CELL:

                    break;
                case CellType.NERVE_CELL:
                    NerveCell nerveCell = enterCell.GetComponent<NerveCell>();
                    nerveCell.multidamagesources += nerveCell.AreaDamage;
                    break;
                default:
                    break;
            }
        }
    }


    void OnTriggerStay(Collider collider) {
        if (collider.gameObject.tag == "Unit")
        {
            BaseCell enterCell = collider.gameObject.GetComponent<BaseCell>();
            if (!enterCell) return;

            switch (enterCell.celltype)
            {
                case CellType.STEM_CELL:
                    {
                        acidicButton.GetComponent<Button>().interactable = true;
                        break;
                    }
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Unit" && collider.gameObject.tag == "EnemyCell") {
            BaseCell enterCell = collider.gameObject.GetComponent<BaseCell>();
            if (!enterCell) return;

            switch (enterCell.celltype) {
                case CellType.STEM_CELL: {
                    StemCell stemCell = collider.gameObject.GetComponent<StemCell>();
                    if (stemCell) {
                        stemCell.isInAcidic = false;
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
                    Tier2HeatCell t2HeatCell = enterCell.GetComponent<Tier2HeatCell>();
                    t2HeatCell.multidamagesources -= t2HeatCell.AreaDamage;
                    break;

                case CellType.COLD_CELL_TIRE2:
                    Tier2ColdCell t2ColdCell = enterCell.GetComponent<Tier2ColdCell>();
                    t2ColdCell.multidamagesources -= t2ColdCell.AreaDamage;
                    break;

                case CellType.ACIDIC_CELL:
                    //AcidicCell acidicCell = enterCell.GetComponent<AcidicCell>();
                    //acidicCell.multidamagesources -= acidicCell.AreaDamage;
                    break;
                case CellType.ALKALI_CELL:
                    AlkaliCell alkaliCell = enterCell.GetComponent<AlkaliCell>();
                    alkaliCell.multidamagesources -= alkaliCell.AreaDamage;
                    alkaliCell.multidamagesources -= alkaliCell.AreaDamage;

                    break;
                case CellType.CANCER_CELL:

                    break;
                case CellType.NERVE_CELL:
                    NerveCell nerveCell = enterCell.GetComponent<NerveCell>();
                    nerveCell.multidamagesources -= nerveCell.AreaDamage;
                    break;

                default:
                    break;
            }
            acidicButton.GetComponent<Button>().interactable = false;
        }
    }

    // Coroutine function
    IEnumerator ReadyToConvert(float delayed, StemCell stemCell) {
        yield return new WaitForSeconds(delayed);
        // to toggle on the pending converting
        if (stemCell)
            stemCell.isInAcidic = true;
    }
}