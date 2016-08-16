using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

// Handles interaction with the back button on the menu.
public class SceneChangeInteraction : MonoBehaviour {
	public bool inGaze;
	public string videoName;
	public float videoLength;

	public delegate void TransitionAction ();
	public static event TransitionAction FadeToBlack;

	[SerializeField] private ExperienceData data;
	[SerializeField] private VRAssets.VRInteractiveItem interactiveItem;
	[SerializeField] private VRAssets.ReticleRadial radial;

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
		inGaze = true;
		radial.Show ();
	}

	private void HandleExit() {
		inGaze = false;
		radial.Hide ();
	}

	private void HandleSelected() {
		if (inGaze & FadeToBlack != null) { 
			print ("Switching to: " + videoName);
			data.videoName = videoName;
			data.videoLength = videoLength;
			FadeToBlack ();
		}
	}
}
