using UnityEngine;
using System.Collections;					
using System.Runtime.InteropServices;		
using System;								
using System.IO;							

/************************************************************************************
Modified for usage from the Oculus VR Sample Framework
************************************************************************************/

public class VideoPreview : MonoBehaviour
{
	public string 	movieName;
	public float	movieLength;
	public bool		videoPaused;
	private bool    videoPausedBeforeAppPause = false;


	private string	mediaFullPath = string.Empty;
	private bool	startedVideo = false;

	#if (UNITY_ANDROID && !UNITY_EDITOR)
	private Texture2D nativeTexture = null;
	private IntPtr	  nativeTexId = IntPtr.Zero;
	private int		  textureWidth = 2880;
	private int 	  textureHeight = 1440;
	private AndroidJavaObject 	mediaPlayer = null;
	#else
	private MovieTexture 		movieTexture = null;
	private AudioSource			audioEmitter = null;
	#endif
	private Renderer 			mediaRenderer = null;

	private enum MediaSurfaceEventType
	{
		Initialize = 0,
		Shutdown = 1,
		Update = 2,
		Max_EventType
	};


	public static int eventBase
	{
		get { return _eventBase; }
		set
		{
			_eventBase = value;
			#if (UNITY_ANDROID && !UNITY_EDITOR)
			OVR_Media_Surface_SetEventBase(_eventBase);
			#endif
		}
	}
	private static int _eventBase = 0;

	private static void IssuePluginEvent(MediaSurfaceEventType eventType)
	{
		GL.IssuePluginEvent((int)eventType + eventBase);
	}

	void OnEnable() {
	}

	void OnDisable() {
	}

	void ToggleState() {
		videoPaused = !videoPaused;
		SetPaused (videoPaused);
	}

	void Awake()
	{
		StartCoroutine (WaitToEnd());

		#if UNITY_ANDROID && !UNITY_EDITOR
		OVR_Media_Surface_Init();
		#endif

		mediaRenderer = GetComponent<Renderer>();
		#if !UNITY_ANDROID || UNITY_EDITOR
		audioEmitter = GetComponent<AudioSource>();
		#endif

		if (mediaRenderer.material == null || mediaRenderer.material.mainTexture == null)
		{
			Debug.LogError("No material for movie surface");
		}

		if (movieName != string.Empty)
		{
			StartCoroutine(RetrieveStreamingAsset(movieName));
		}
		else
		{
			Debug.LogError("No media file name provided");
		}

		#if UNITY_ANDROID && !UNITY_EDITOR
		nativeTexture = Texture2D.CreateExternalTexture(textureWidth, textureHeight,
		TextureFormat.RGBA32, true, false,
		IntPtr.Zero);

		IssuePluginEvent(MediaSurfaceEventType.Initialize);
		#endif
	}

	IEnumerator WaitToEnd() {
		yield return new WaitForSeconds (1.0f); // End the movie
	}

	IEnumerator RetrieveStreamingAsset(string mediaFileName)
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		string streamingMediaPath = Application.streamingAssetsPath + "/" + mediaFileName;
		string persistentPath = Application.persistentDataPath + "/" + mediaFileName;
		if (!File.Exists(persistentPath))
		{
		WWW wwwReader = new WWW(streamingMediaPath);
		yield return wwwReader;

		if (wwwReader.error != null)
		{
		Debug.LogError("wwwReader error: " + wwwReader.error);
		}

		System.IO.File.WriteAllBytes(persistentPath, wwwReader.bytes);
		}
		mediaFullPath = persistentPath;
		#else
		string mediaFileNameOgv = Path.GetFileNameWithoutExtension(mediaFileName) + ".ogv";
		string streamingMediaPath = "file:///" + Application.streamingAssetsPath + "/" + mediaFileNameOgv;
		WWW wwwReader = new WWW(streamingMediaPath);
		yield return wwwReader;

		if (wwwReader.error != null)
		{
			Debug.LogError("wwwReader error: " + wwwReader.error);
		}

		movieTexture = wwwReader.movie;
		mediaRenderer.material.mainTexture = movieTexture;
		audioEmitter.clip = movieTexture.audioClip;
		mediaFullPath = streamingMediaPath;
		#endif
		// Video must start only after mediaFullPath is filled in
		StartCoroutine(DelayedStartVideo());
	}

	IEnumerator DelayedStartVideo()
	{
		yield return null; // delay 1 frame to allow MediaSurfaceInit from the render thread.

		if (!startedVideo)
		{
			startedVideo = true;
			#if (UNITY_ANDROID && !UNITY_EDITOR)
			mediaPlayer = StartVideoPlayerOnTextureId(textureWidth, textureHeight, mediaFullPath);
			mediaRenderer.material.mainTexture = nativeTexture;
			#else
			if (movieTexture != null && movieTexture.isReadyToPlay && !videoPaused)
			{
				movieTexture.Play();
				if (audioEmitter != null)
				{
					audioEmitter.Play();
				}
			}
			#endif
		}
	}

	void Update()
	{
		#if (UNITY_ANDROID && !UNITY_EDITOR)
		if (!videoPaused) {
		IntPtr currTexId = OVR_Media_Surface_GetNativeTexture();
		if (currTexId != nativeTexId)
		{
		nativeTexId = currTexId;
		nativeTexture.UpdateExternalTexture(currTexId);
		}

		IssuePluginEvent(MediaSurfaceEventType.Update);
		}
		#else
		if (movieTexture != null)
		{
			if (movieTexture.isReadyToPlay != movieTexture.isPlaying && !videoPaused)
			{
				movieTexture.Play();
				if (audioEmitter != null)
				{
					audioEmitter.Play();
				}
			}
		}
		#endif
	}

	public void StartOver()
	{
		#if (UNITY_ANDROID && !UNITY_EDITOR)
		if (mediaPlayer != null)
		{
		try
		{
		mediaPlayer.Call("seekTo", 0);
		}
		catch (Exception e)
		{
		Debug.Log("Failed to stop mediaPlayer with message " + e.Message);
		}
		}
		#else
		if (movieTexture != null)
		{
			movieTexture.Stop();
			if (audioEmitter != null)
			{
				audioEmitter.Stop();
			}
		}
		#endif
	}

	public void SetPaused(bool wasPaused)
	{
		#if (UNITY_ANDROID && !UNITY_EDITOR)
		if (mediaPlayer != null)
		{
		try
		{
		mediaPlayer.Call((videoPaused) ? "pause" : "start");
		}
		catch (Exception e)
		{
		Debug.Log("Failed to start/pause mediaPlayer with message " + e.Message);
		}
		}
		#else
		if (movieTexture != null)
		{
			if (videoPaused)
			{
				movieTexture.Pause();
				if (audioEmitter != null)
				{
					audioEmitter.Pause();
				}
			}
			else
			{
				movieTexture.Play();
				if (audioEmitter != null)
				{
					audioEmitter.Play();
				}
			}
		}
		#endif
	}


	void OnApplicationPause(bool appWasPaused)
	{
		if (appWasPaused)
		{
			videoPausedBeforeAppPause = videoPaused;
		}

		// Pause/unpause the video only if it had been playing prior to app pause
		if (!videoPausedBeforeAppPause)
		{
			SetPaused(appWasPaused);
		}
	}

	private void OnDestroy()
	{
		#if (UNITY_ANDROID && !UNITY_EDITOR)
		Debug.Log("Shutting down video");
		// This will trigger the shutdown on the render thread
		IssuePluginEvent(MediaSurfaceEventType.Shutdown);
		mediaPlayer.Call("stop");
		mediaPlayer.Call("release");
		mediaPlayer = null;
		#endif
	}

	#if (UNITY_ANDROID && !UNITY_EDITOR)
	AndroidJavaObject StartVideoPlayerOnTextureId(int texWidth, int texHeight, string mediaPath)
	{
	Debug.Log("MoviePlayer: StartVideoPlayerOnTextureId");

	OVR_Media_Surface_SetTextureParms(textureWidth, textureHeight);

	IntPtr androidSurface = OVR_Media_Surface_GetObject();
	AndroidJavaObject mediaPlayer = new AndroidJavaObject("android/media/MediaPlayer");

	IntPtr setSurfaceMethodId = AndroidJNI.GetMethodID(mediaPlayer.GetRawClass(),"setSurface","(Landroid/view/Surface;)V");
	jvalue[] parms = new jvalue[1];
	parms[0] = new jvalue();
	parms[0].l = androidSurface;
	AndroidJNI.CallVoidMethod(mediaPlayer.GetRawObject(), setSurfaceMethodId, parms);

	try
	{
	mediaPlayer.Call("setDataSource", mediaPath);
	mediaPlayer.Call("prepare");
	mediaPlayer.Call("setLooping", true);
	mediaPlayer.Call("start");
	}
	catch (Exception e)
	{
	Debug.Log("Failed to start mediaPlayer with message " + e.Message);
	}

	return mediaPlayer;
	}
	#endif

	#if (UNITY_ANDROID && !UNITY_EDITOR)
	[DllImport("OculusMediaSurface")]
	private static extern void OVR_Media_Surface_Init();

	[DllImport("OculusMediaSurface")]
	private static extern void OVR_Media_Surface_SetEventBase(int eventBase);

	[DllImport("OculusMediaSurface")]
	private static extern IntPtr OVR_Media_Surface_GetObject();

	[DllImport("OculusMediaSurface")]
	private static extern IntPtr OVR_Media_Surface_GetNativeTexture();

	[DllImport("OculusMediaSurface")]
	private static extern void OVR_Media_Surface_SetTextureParms(int texWidth, int texHeight);
	#endif
}
