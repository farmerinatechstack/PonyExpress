using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class MovieInteraction : MonoBehaviour {
	public bool paused;
	public GameObject pauseMenu;
	public event Action Play;
	public event Action Pause;

	public delegate void TransitionAction ();
	public static event TransitionAction FadeToBlack;

	public float fadeTime;
	public string sceneName;

	[SerializeField] private VRAssets.VRInput input;

	// Use this for initialization
	void Start () {
		paused = false;
	}
	
	void OnEnable() {
		input.OnDown += TogglePause;
		input.Back += ExitScene;
	}

	void OnDisable() {
		input.OnDown -= TogglePause;
		input.Back -= ExitScene;
	}

	void TogglePause() {
		if (paused) {
			pauseMenu.SetActive (false);
			paused = false;
			if (Play != null)
				Play ();
		} else {
			pauseMenu.SetActive (true);
			paused = true;
			if (Pause != null)
				Pause ();
		}
	}

	void ExitScene() {
		if (!paused) {
			TogglePause ();
		} else {
			if (FadeToBlack != null) {
				FadeToBlack ();
			}
			StartCoroutine (SwapScene ());
		}
	}

	private IEnumerator SwapScene() {
		yield return new WaitForSeconds (fadeTime);

		SceneManager.LoadScene (sceneName);
	}
}
