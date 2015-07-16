using UnityEngine;
using System.Collections;

public class Tier2ColdCell : BaseCell
{

    public GameObject stemCell;
    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    private Sound_Manager sound_manager;
    // Use this for initialization
    void Start()
    {
        base.bStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1) && isSelected)
        {
            Vector3 trackingPos = this.transform.position;
            Quaternion trackingRot = this.transform.rotation;
            Die();
            GameObject gstem = Instantiate(stemCell, trackingPos, trackingRot) as GameObject;
            GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(gstem.GetComponent<BaseCell>());
        }
    }
    void Awake()
    {
        base.bAwake();
        multidamagesources += nothing;
        InvokeRepeating("MUltiDMg", 1.0f, 1.0f);
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();


    }
    void MUltiDMg()
    {
        multidamagesources();
        if (!sound_manager.sounds_miscellaneous[6].isPlaying)
        {
            sound_manager.sounds_miscellaneous[6].Play();

        }

    }
    public void AreaDamage()
    {
        currentProtein -= 10;
    }
    void nothing()
    {


    }

    public override void Attack(GameObject _target)
    {
        if (_target)
        {
            SetPrimaryTarget(_target);
            currentState = CellState.ATTACK;
        }
    }

    void LateUpdate()
    {
        base.bLateUpdate();
    }

    void FixedUpdate()
    {
        base.bFixedUpdate();
    }


}
