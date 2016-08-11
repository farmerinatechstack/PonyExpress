using UnityEngine;
using System.Collections;

// Handles interaction with the SAC -> LA light journey
public class PointInteraction : MonoBehaviour {
	[SerializeField] private VRAssets.VRInteractiveItem interactiveItem;
	[SerializeField] private VRAssets.ReticleRadial radial;

	public bool inGaze;
	public GameObject menu;
	public Animator menuAnimator;

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
		radial.Show ();
		inGaze = true;
	}

	private void HandleExit() {
		radial.Hide ();
		inGaze = false;
	}

	private void HandleSelected() {
		if (inGaze) {
			menu.SetActive (true);
			menuAnimator.Play ("Grow");
		}
	}
}
