using UnityEngine;
using System;
using System.Collections;

public class MovieInteraction : MonoBehaviour {
	public GameObject pauseMenu;		// Menu to be displayed on pause
	public event Action ToggleState;	// Toggles movie play or pause

	public delegate void TransitionAction ();
	public static event TransitionAction FadeToBlack;

	private bool paused;
	[SerializeField] private VRAssets.VRInput input;

	// Use this for initialization
	void Start () {
		paused = false;
	}
	
	void OnEnable() {
		input.OnDown += TogglePause;
		input.Back += ExitScene;
	}

	void OnDisable() {
		input.OnDown -= TogglePause;
		input.Back -= ExitScene;
	}

	void TogglePause() {
		paused = !paused;

		if (ToggleState != null) {
			pauseMenu.SetActive (paused);
			ToggleState ();
		}
	}

	void ExitScene() {
		if (!paused) {	// If the scene is not paused, pause the scene without exiting
			TogglePause ();
		} else {
			if (FadeToBlack != null)	// Execute a transition action if not null
				FadeToBlack ();
		}
	}
}
