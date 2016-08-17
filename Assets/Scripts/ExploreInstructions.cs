using UnityEngine;
using System.Collections;

public class ExploreInstructions : MonoBehaviour {
	[SerializeField] Animator anim;
	[SerializeField] private MenuDisplay menuDisplay;

	void OnEnable() {
		anim.Play ("PromptExploration");
		menuDisplay.MenuToggled += RemoveInstructions;
	}

	void OnDisable() {
		menuDisplay.MenuToggled -= RemoveInstructions;
	}

	void RemoveInstructions() {
		gameObject.SetActive (false);
	}
}
