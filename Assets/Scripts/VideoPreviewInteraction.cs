using UnityEngine;
using System.Collections;

public class VideoPreviewInteraction : MonoBehaviour {
	public bool inGaze;
	public bool ready = false;	// Used to wait for animation

	[SerializeField] private VideoPreview prev;
	[SerializeField] private VRAssets.VRInteractiveItem interactiveItem;

	private VRAssets.ReticleRadial radial;
	private Animator menuAnimator;

	void Awake() {
		radial = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<VRAssets.ReticleRadial> ();
		menuAnimator = GameObject.FindGameObjectWithTag ("Menu").GetComponent<Animator> ();

		GameObject menu = GameObject.FindGameObjectWithTag ("Menu");
		MenuDisplay disp = menu.GetComponent<MenuDisplay> ();
		if (disp.videoName == "Lingtong.mp4") {
			gameObject.transform.Rotate(new Vector3(0,0,90));
		}
	}

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
			prev.Select ();
		}
	}
}
