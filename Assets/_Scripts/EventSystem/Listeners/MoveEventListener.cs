using UnityEngine;
using System.Collections;

public class MoveEventListener : MonoBehaviour {

    void OnEnable()
    {
        EventManager.OnMove += Move;
    }


    void OnDisable()
    {
        EventManager.OnMove -= Move;
    }

    void Move(Vector3 _dest)
    {
        BaseCell curCell = this.GetComponent<BaseCell>();
        if (!curCell.isSelected)
        {
            return;
        }
        curCell.SetPrimaryTarget(null);
        curCell.Move(_dest);
        if (PhotonNetwork.connected)
        {
            curCell.photonView.RPC("Move", PhotonTargets.Others, _dest);
        }
    }
}
