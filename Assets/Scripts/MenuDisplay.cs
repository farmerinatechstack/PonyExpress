using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class MenuDisplay : MonoBehaviour {
	public bool display360;
	public string titleText;
	public Texture imageTexture;
	public string videoName;
	public float videoLength;

	public event Action MenuToggled;

	[SerializeField] Animator menuAnim;
	[SerializeField] Text titleTextUI;
	[SerializeField] GameObject gameObject360;
	[SerializeField] GameObject gameObject2D;

	private SceneChangeInteraction videoInteraction360;
	private VideoPreviewInteraction videoInteraction2D;
	private VideoPreview prev;

	void OnEnable() {
		if (MenuToggled != null) {
			MenuToggled ();
		}

		SetMenu ();
	}

	void SetMenu() {
		titleTextUI.text = titleText;

		if (display360) {
			gameObject2D.SetActive (false);

			videoInteraction360 = gameObject360.GetComponent<SceneChangeInteraction> ();
			videoInteraction360.ready = false;
			videoInteraction360.videoName = videoName;
			videoInteraction360.videoLength = videoLength;

			Renderer rend = videoInteraction360.GetComponent<Renderer> ();
			rend.material.mainTexture = imageTexture;
		} else {
			gameObject360.SetActive (false);

			videoInteraction2D = gameObject2D.GetComponent<VideoPreviewInteraction> ();
			prev = gameObject2D.GetComponent<VideoPreview> ();

			videoInteraction2D.ready = false;
			//prev.movieName = videoName;
			//prev.movieLength = videoLength;

			Renderer rend = gameObject2D.GetComponent<Renderer> ();
			rend.material.mainTexture = imageTexture;
		}
		menuAnim.Play ("Grow");
		Invoke ("SetReady", 1.1f);
	}

	public void Hide() {
		StartCoroutine (HideMenu ());
	}

	void SetReady() {
		if (display360) {
			videoInteraction360.ready = true;
		} else {
			videoInteraction2D.ready = true;

		}
	}



	IEnumerator HideMenu() {
		menuAnim.Play ("Shrink");

		yield return new WaitForSeconds (1.1f); // wait for the animation to end

		if (MenuToggled != null)
			MenuToggled ();

		// Reset the menu
		titleTextUI.text = "";
		gameObject360.SetActive (true);
		gameObject2D.SetActive (true);
			
		gameObject.SetActive (false);
		transform.localScale = Vector3.one;
	}
}
