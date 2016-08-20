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

			cameraHolder.transform.position = new Vector3 (0, 0, 18);

			earth.enabled = true;
			worldSys.enabled = true;
			explorePrompts.SetActive (false);
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

			data.started = true;
			radial.Hide ();

			earth.enabled = true;
			worldSys.enabled = true;

			cameraAnim.Play ("CameraTranslation");
			canvasAnim.Play ("FadeCanvas");

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
