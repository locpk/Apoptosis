﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColdArea:BaseArea {

    public float damagePerSecond;
    public float speedCoefficient = 0.5f;
    private float enterCellSpeed;

    private Sound_Manager sound_manager;
    public override void Awake() {
        base.Awake();
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();
    }

    public override void Start() {
        base.Start();

    }

    public override void Update() {
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

            if (!sound_manager.sounds_miscellaneous[6].isPlaying && collider.gameObject.tag == "Unit" && collider.GetComponent<BaseCell>().isMine)
            {
                sound_manager.sounds_miscellaneous[6].Play();

            }

            if (!enterCell) return;
            enterCellSpeed = enterCell.navAgent.speed;

            switch (enterCell.celltype) {
                case CellType.STEM_CELL:
                    enterCell.navAgent.speed *= speedCoefficient;
                    break;
                case CellType.HEAT_CELL:
                    enterCell.navAgent.speed *= speedCoefficient;
                    break;
                case CellType.COLD_CELL:
                    enterCell.GetComponent<ColdCell>().InCold = true;
                    break;
                case CellType.HEAT_CELL_TIRE2:
                    enterCell.navAgent.speed *= speedCoefficient;
                    break;
                case CellType.COLD_CELL_TIRE2:
                    break;
                case CellType.ACIDIC_CELL:
                    enterCell.navAgent.speed *= speedCoefficient;
                    break;
                case CellType.ALKALI_CELL:
                    enterCell.navAgent.speed *= speedCoefficient;
                    break;
                case CellType.CANCER_CELL:
                    enterCell.navAgent.speed *= speedCoefficient;
                    break;
                case CellType.NERVE_CELL:
                    enterCell.navAgent.speed *= speedCoefficient;
                    break;
                default:
                    break;
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

            switch (enterCell.celltype) {
                case CellType.STEM_CELL:
                    //enterCell.navAgent.speed /= speedCoefficient;
                    break;
                case CellType.HEAT_CELL:
                    break;
                case CellType.COLD_CELL:
                    enterCell.GetComponent<ColdCell>().InCold = false;
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
                case CellType.NERVE_CELL:
                    break;
                default:
                    break;
            }
        }
    }
}