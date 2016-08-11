using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

// Handles interaction with the back button on the menu.
public class SceneChangeInteraction : MonoBehaviour {
	public bool inGaze;
	public string sceneName;
	public float fadeTime;

	public delegate void TransitionAction ();
	public static event TransitionAction FadeToBlack;

	[SerializeField] private VRAssets.VRInteractiveItem interactiveItem;
	[SerializeField] private VRAssets.ReticleRadial radial;
	private void OnEnable() {
		interactiveItem.OnEnter += HandleEnter;
		interactiveItem.OnExit += HandleExit;
		interactiveItem.OnDown += HandleDown;
		radial.OnSelectionComplete += HandleSelected;
	}

	private void OnDisable() {
		interactiveItem.OnEnter -= HandleEnter;
		interactiveItem.OnExit -= HandleExit;
		interactiveItem.OnDown -= HandleDown;
		radial.OnSelectionComplete -= HandleSelected;
	}

	private void HandleDown() {

	}

	private void HandleEnter() {
		inGaze = true;
		radial.Show ();
	}

	private void HandleExit() {
		inGaze = false;
		radial.Hide ();
	}

	private void HandleSelected() {
		if (inGaze) {		
			if (FadeToBlack != null) {
				FadeToBlack ();
			}
			StartCoroutine (SwapScene ());
		}
	}

	private IEnumerator SwapScene() {
		yield return new WaitForSeconds (fadeTime);

		SceneManager.LoadScene (sceneName);
	}
}
