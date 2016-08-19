using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {
	[SerializeField] private GameObject cameraHolder;
	[SerializeField] private ExperienceData data;

	[SerializeField] private AudioSource ambientAudio;
	[SerializeField] private AudioSource selectAudio;

	[SerializeField] private VRAssets.VRInput input;
	[SerializeField] private VRAssets.ReticleRadial radial;
	[SerializeField] private Animator cameraAnim;
	[SerializeField] private Animator canvasAnim;
	[SerializeField] private Animator flyer1Anim;
	[SerializeField] private Animator flyer2Anim;


	[SerializeField] private EarthInteraction earth;
	[SerializeField] private WorldSystem worldSys;

	[SerializeField] private GameObject explorePrompts;

	private void OnEnable() {
		radial.OnSelectionComplete += HandleStart;
		input.Back += ExitApplication;
	}

	private void OnDisable() {
		radial.OnSelectionComplete -= HandleStart;
		input.Back -= ExitApplication;
	}

	void Start() {
		if (data.started) {
			canvasAnim.Play ("FadeCanvas");

			cameraHolder.transform.position = new Vector3 (0, 0, 20);

			ambientAudio.Play ();
			earth.enabled = true;
			worldSys.enabled = true;
			explorePrompts.SetActive (false);

			Destroy (flyer1Anim.gameObject);
			Destroy (flyer2Anim.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if (!data.started)
			radial.Show ();
	}

	void HandleStart() {
		if (!data.started) {
			selectAudio.Play ();
			ambientAudio.PlayDelayed (0.2f);

			data.started = true;
			radial.Hide ();

			earth.enabled = true;
			worldSys.enabled = true;

			cameraAnim.Play ("TranslateCamera");
			canvasAnim.Play ("FadeCanvas");
			flyer1Anim.Play ("Flyer1");
			flyer2Anim.Play ("Flyer2");

			StartCoroutine (PromptExploration ());
		}
	}

	void ExitApplication() {
		Application.Quit ();
	}

	IEnumerator PromptExploration() {
		yield return new WaitForSeconds (5f);

		explorePrompts.SetActive (true);
	}
}
