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
	[SerializeField] GameObject videoPreview360;
	[SerializeField] GameObject videoPreview2D;


	void OnEnable() {
		if (MenuToggled != null) {
			MenuToggled ();
		}

		SetMenu ();

		menuAnim.Play ("Grow");
	}

	void SetMenu() {
		titleTextUI.text = titleText;

		if (display360) {
			videoPreview2D.SetActive (false);

			SceneChangeInteraction videoInteraction = videoPreview360.GetComponent<SceneChangeInteraction> ();
			videoInteraction.videoName = videoName;
			videoInteraction.videoLength = videoLength;

			Renderer rend = videoPreview360.GetComponent<Renderer> ();
			rend.material.mainTexture = imageTexture;
		} else {
			videoPreview360.SetActive (false);

			VideoPreview player = videoPreview2D.GetComponent<VideoPreview> ();
			player.movieName = videoName;
			player.movieLength = videoLength;

			Renderer rend = videoPreview2D.GetComponent<Renderer> ();
			rend.material.mainTexture = imageTexture;
		}
	}

	public void Hide() {
		if (MenuToggled != null)
			MenuToggled ();

		StartCoroutine (HideMenu ());
	}

	IEnumerator HideMenu() {
		menuAnim.Play ("Shrink");
			
		yield return new WaitForSeconds (1.5f);
		gameObject.SetActive (false);
		transform.localScale = Vector3.one;
	}
}
