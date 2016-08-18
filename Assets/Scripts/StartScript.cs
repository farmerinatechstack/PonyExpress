using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {
	[SerializeField] private ExperienceData data;

	[SerializeField] private AudioSource ambientAudio;
	[SerializeField] private AudioSource selectAudio;


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
	}

	private void OnDisable() {
		radial.OnSelectionComplete -= HandleStart;
	}
	
	// Update is called once per frame
	void Update () {
		if (!data.started)
			radial.Show ();
	}

	void HandleStart() {
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

	IEnumerator PromptExploration() {
		yield return new WaitForSeconds (5f);

		explorePrompts.SetActive (true);
	}
}
