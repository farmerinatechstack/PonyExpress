using UnityEngine;
using System.Collections;

public class ExploreInstructions : MonoBehaviour {
	[SerializeField] Animator anim;

	void OnEnable() {
		anim.Play ("PromptExploration");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
