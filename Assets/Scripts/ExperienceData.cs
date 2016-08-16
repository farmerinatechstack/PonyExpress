using UnityEngine;
using System.Collections;

public class ExperienceData : MonoBehaviour {
	public string videoName;
	public float videoLength;
	public bool started = false;

	void Awake() {
		DontDestroyOnLoad(this);
	}
}
