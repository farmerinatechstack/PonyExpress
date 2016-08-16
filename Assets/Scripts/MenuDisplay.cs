using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuDisplay : MonoBehaviour {
	public string titleText;
	public Texture imageTexture;
	public string videoName;
	public float videoLength;

	[SerializeField] Animator menuAnim;
	[SerializeField] Text titleTextUI;
	[SerializeField] GameObject videoPreview;

	void OnEnable() {
		titleTextUI.text = titleText;

		SceneChangeInteraction videoInteraction = videoPreview.GetComponent<SceneChangeInteraction> ();
		videoInteraction.videoName = videoName;
		videoInteraction.videoLength = videoLength;

		Renderer rend = videoPreview.GetComponent<Renderer> ();
		rend.material.mainTexture = imageTexture;

		menuAnim.Play ("Grow");
	}

	public void Hide() {
		StartCoroutine (HideMenu ());
	}

	IEnumerator HideMenu() {
		menuAnim.Play ("Shrink");
			
		yield return new WaitForSeconds (1.5f);
		gameObject.SetActive (false);
		transform.localScale = Vector3.one;
	}
}
