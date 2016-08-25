using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class FadeTransition : MonoBehaviour {
	public Material m;
	public float fadeTime = 2.5f;	// Time to fade
	public string sceneName;		// Name of scene to transition to

	private AsyncOperation async;

	private void OnEnable() {
		SceneChangeInteraction.FadeToBlack += FadeToBlack;
		MovieInteraction.FadeToBlack += FadeToBlack;
		MoviePlayerSample.FadeToBlack += FadeToBlack;
	}

	private void OnDisable() {
		SceneChangeInteraction.FadeToBlack -= FadeToBlack;
		MovieInteraction.FadeToBlack -= FadeToBlack;
		MoviePlayerSample.FadeToBlack -= FadeToBlack;
	}

	// On start, the material always fades from black to transparent
	void Start () {
		StartCoroutine (LoadScene ());

		m.color = new Color (m.color.r, m.color.g, m.color.b, 1.0f);
		StartCoroutine (FadeTo (0.0f, fadeTime));
	}

	// Fades to black and transitions to sceneName in fadeTime
	void FadeToBlack() {
		StartCoroutine (FadeTo(1.0f, fadeTime));

		StartCoroutine (SwapScene());
	}

	IEnumerator FadeTo(float targetAlpha, float time) {
		float startAlpha = m.color.a;

		for (float t = 0.0f; t <= time; t += Time.deltaTime) {
			float fadeAlpha = Mathf.Lerp (startAlpha, targetAlpha, t / time);
			Color newColor = new Color(m.color.r, m.color.g, m.color.b, fadeAlpha);
			m.color = newColor;
			yield return null; 		// Wait for one frame
		}

		Color finalColor = new Color(m.color.r, m.color.g, m.color.b, targetAlpha);
		m.color = finalColor;		// Set the alpha to the target alpha
	}

	IEnumerator LoadScene() {
		async = SceneManager.LoadSceneAsync(sceneName);
		async.allowSceneActivation = false;

		yield return async;
	}

	IEnumerator SwapScene() {
		yield return new WaitForSeconds (fadeTime);

		if (async != null) {
			async.allowSceneActivation = true;
		}
	}
}
