using UnityEngine;
using System.Collections;

public class UpdateProtein : MonoBehaviour {

    public Sprite health_10;
    public Sprite health_50;
    public Sprite health_100;
	void FixedUpdate()
    {
        float healthRatio = transform.parent.GetComponent<CellSplitAnimation>().currentProtein / BaseCell.MAX_PROTEIN;
        if (healthRatio <= 0.5f && healthRatio > 0.1f)
        {
            transform.FindChild("Nucleus").GetComponent<SpriteRenderer>().sprite = health_50;
        }
        else if (healthRatio <= 0.1f)
        {
            transform.FindChild("Nucleus").GetComponent<SpriteRenderer>().sprite = health_10;
        }
        else
        {
            transform.FindChild("Nucleus").GetComponent<SpriteRenderer>().sprite = health_100;
        }
    }
}
