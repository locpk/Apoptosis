using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Sprites;
using System.Collections;



public class Menu_ShowGlow : StateMachineBehaviour {

    GameObject highlight; // this is the highlight sprite.
 
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//

        highlight = GameObject.FindGameObjectWithTag("Highlight");
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        highlight.GetComponent<Image>().enabled = true ;
 	}

	//OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        // stops the highlight image from showing
        highlight.GetComponent<Image>().enabled = false;
	
	}

	
}
