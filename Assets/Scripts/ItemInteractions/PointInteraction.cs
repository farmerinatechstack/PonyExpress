using UnityEngine;
using System.Collections;

// Handles interaction with the SAC -> LA light journey
public class PointInteraction : MonoBehaviour {
	public string titleText;
	public Texture imageTexture;
	public string videoName;
	public float videoLength;

	public bool inGaze;
	public GameObject menu;

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
		radial.Show ();
		inGaze = true;
	}

	private void HandleExit() {
		radial.Hide ();
		inGaze = false;
	}

	private void HandleSelected() {
		if (inGaze) {
			MenuDisplay disp = menu.GetComponent<MenuDisplay> ();
			disp.titleText = titleText;
			disp.imageTexture = imageTexture;
			disp.videoName = videoName;
			disp.videoLength = videoLength;

			menu.SetActive (true);
		}
	}
}
