using UnityEngine;
using System.Collections;

public class VideoPreviewInteraction : MonoBehaviour {
	public bool inGaze;
	public bool ready = false;	// Used to wait for animation

	[SerializeField] private Animator menuAnimator;
	[SerializeField] private VideoPreview prev;
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
		if (inGaze) { 
			prev.StartOver ();

			// Reset the radial
			radial.Hide ();
			radial.Show ();
		}
	}
}
