using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {
	[SerializeField] private VRAssets.ReticleRadial radial;
	[SerializeField] private Animator cameraAnim;
	[SerializeField] private Animator canvasAnim;
	[SerializeField] private Animator flyer1Anim;

	[SerializeField] private EarthInteraction earth;
	[SerializeField] private WorldSystem worldSys;
	[SerializeField] private WorldPoint point;
	[SerializeField] private PointInteraction pointInteraction;

	[SerializeField] private GameObject explorePrompts;

	private void OnEnable() {
		radial.OnSelectionComplete += HandleStart;
	}

	private void OnDisable() {
		radial.OnSelectionComplete -= HandleStart;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		radial.Show ();
	}

	void HandleStart() {
		print ("Start selected");
		radial.Hide ();

		earth.enabled = true;
		worldSys.enabled = true;
		point.enabled = true;
		pointInteraction.enabled = true;

		cameraAnim.Play ("TranslateCamera");
		canvasAnim.Play ("FadeCanvas");
		flyer1Anim.Play ("Flyer");


		StartCoroutine (PromptExploration ());
	}

	IEnumerator PromptExploration() {
		yield return new WaitForSeconds (5f);

		explorePrompts.SetActive (true);
	}
}
