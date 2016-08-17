using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class MenuDisplay : MonoBehaviour {
	public string titleText;
	public Texture imageTexture;
	public string videoName;
	public float videoLength;

	public event Action MenuToggled;
	public bool displayed = false;

	[SerializeField] Animator menuAnim;
	[SerializeField] Text titleTextUI;
	[SerializeField] GameObject videoPreview;

	void OnEnable() {
		if (!displayed) {
			displayed = true;
		}

		if (MenuToggled != null) {
			MenuToggled ();
		}

		SetMenuValues ();

		menuAnim.Play ("Grow");
	}

	void SetMenuValues() {
		titleTextUI.text = titleText;

		SceneChangeInteraction videoInteraction = videoPreview.GetComponent<SceneChangeInteraction> ();
		videoInteraction.videoName = videoName;
		videoInteraction.videoLength = videoLength;

		Renderer rend = videoPreview.GetComponent<Renderer> ();
		rend.material.mainTexture = imageTexture;
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
