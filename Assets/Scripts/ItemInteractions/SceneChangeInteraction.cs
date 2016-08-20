using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

// Handles interaction with the back button on the menu.
public class SceneChangeInteraction : MonoBehaviour {
	public bool inGaze;
	public bool ready = false;
	public bool contains360;
	public string videoName;
	public float videoLength;

	public delegate void TransitionAction ();
	public static event TransitionAction FadeToBlack;

	[SerializeField] private Animator menuAnimator;
	[SerializeField] private VRAssets.VRInteractiveItem interactiveItem;
	[SerializeField] private VRAssets.ReticleRadial radial;

	private ExperienceData data;

	private void OnEnable() {
		interactiveItem.OnEnter += HandleEnter;
		interactiveItem.OnExit += HandleExit;
		radial.OnSelectionComplete += HandleSelected;
	}

	private void OnDisable() {
		interactiveItem.OnEnter -= HandleEnter;
		interactiveItem.OnExit -= HandleExit;
		radial.OnSelectionComplete -= HandleSelected;
	}

	private void HandleEnter() {
		if (ready) {
			inGaze = true;
			radial.Show ();
			menuAnimator.Play ("VideoPop");
		}
	}

	private void HandleExit() {
		if (ready) {
			inGaze = false;
			radial.Hide ();
			menuAnimator.Play ("VideoShrink");
		}
	}

	private void HandleSelected() {
		if (inGaze & FadeToBlack != null) {
			data = GameObject.FindGameObjectWithTag ("Data").GetComponent<ExperienceData> ();

			data.started = true;
			data.videoName = videoName;
			data.videoLength = videoLength;
			FadeToBlack ();
		}
	}
}
