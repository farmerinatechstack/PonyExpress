using UnityEngine;
using System.Collections;

// Handles interaction with the SAC -> LA light journey
public class PointInteraction : MonoBehaviour {
	public string titleText;
	public Texture imageTexture;
	public string videoName;
	public float videoLength;

	public bool canBeSelected;
	public bool contains360;

	public bool inGaze;
	public GameObject menu;

	[SerializeField] private MenuDisplay menuDisplay;
	[SerializeField] private VRAssets.VRInteractiveItem interactiveItem;
	[SerializeField] private VRAssets.ReticleRadial radial;

	private void OnEnable() {
		interactiveItem.OnEnter += HandleEnter;
		interactiveItem.OnExit += HandleExit;
		radial.OnSelectionComplete += HandleSelected;
		menuDisplay.MenuToggled += ToggleCanBeSelected;
	}

	private void OnDisable() {
		interactiveItem.OnEnter -= HandleEnter;
		interactiveItem.OnExit -= HandleExit;
		radial.OnSelectionComplete -= HandleSelected;
		menuDisplay.MenuToggled -= ToggleCanBeSelected;
	}

	private void HandleEnter() {
		if (canBeSelected) {
			radial.Show ();
			inGaze = true;
		}
	}

	private void HandleExit() {
		if (canBeSelected) {
			radial.Hide ();
			inGaze = false;
		}
	}

	private void HandleSelected() {
		if (inGaze && canBeSelected) {
			MenuDisplay disp = menu.GetComponent<MenuDisplay> ();
			disp.titleText = titleText;
			disp.imageTexture = imageTexture;
			disp.videoName = videoName;
			disp.videoLength = videoLength;
			disp.display360 = contains360;

			menu.SetActive (true);
		}
	}

	public void ToggleCanBeSelected() {
		canBeSelected = !canBeSelected;
	}
}
