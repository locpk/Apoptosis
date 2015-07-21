using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour
{
    public GameObject Target;
   public GameObject Owner;
   float timer = 0;
   public const float DEPLETE_TIME = 3.0f;
 
    // Use this for initialization
    void Awake()
    {
       
    }
    void DotEffect()
    {
        if (Target)
        {
            Target.GetComponent<BaseCell>().currentProtein -= Owner.GetComponent<BaseCell>().attackDamage;
            Target.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
            if (PhotonNetwork.connected)
            {
                Target.GetPhotonView().RPC("ApplyDamage", PhotonTargets.Others, Owner.GetComponent<BaseCell>().attackDamage);
            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, (Target.transform.position.z + Target.GetComponent<SphereCollider>().radius + .75f));
            if (!IsInvoking("DotEffect"))
            {
                InvokeRepeating("DotEffect", 1.0f, 1.0f);

            }
        }
        timer += 1* Time.fixedDeltaTime;
        if(timer >= 3)
        {
            Destroy(this.gameObject);
        }

    }


}
