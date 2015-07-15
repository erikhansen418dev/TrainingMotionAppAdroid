using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Xtr3D.Net;
using Xtr3D.Net.BaseTypes;
using Xtr3D.Net.Exceptions;
using Xtr3D.Net.ExtremeMotion;
using Xtr3D.Net.ExtremeMotion.Data;
using Xtr3D.Net.ExtremeMotion.Interop.Types;
using Xtr3D.Net.AllFrames;
using Xtr3D.Net.ColorImage;
using Xtr3D.Net.ExtremeMotion.Gesture;

public class SdkManager : MonoBehaviour {
	
	public Text StatusText;
	public static bool IsDebugRun = false;
	
	void Awake() {
		
		if (Application.platform != RuntimePlatform.IPhonePlayer
		    && Application.platform != RuntimePlatform.Android)
		{
			string[] args = Environment.GetCommandLineArgs();
			if (args != null)
			{
				foreach (string arg in args)
				{
					if (arg.ToLower() == "debug")
					{
						IsDebugRun = true;
						break;
					}
				}
			}
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			IsDebugRun = true;
		}
		Debug.Log("Initializing and starting manager");
		ImageInfo info = new ImageInfo(ImageResolution.Resolution640x480, ImageInfo.ImageFormat.RGB888);
		string message = String.Empty;
		
		try
		{
			GeneratorSingleton.Instance.Initialize(GetPlatformType(), info);
			SetGestureFile();
		}
		catch (InvalidLicenseException ex)
		{
			message = "License Error: Invalid license";
			Debug.LogError(message  + ex.Message);
		}
		catch (MissingLicenseException ex)
		{
			message = "License Error: Missing license";
			Debug.LogError(message  + ex.Message);
		}
		catch (ExpiredLicenseException ex)
		{
			message = "License Error: License expired";
			Debug.LogError(message  + ex.Message);
		}
		catch (NotInitializedException ex)
		{
			message = "Please verify Init was successfully called";
			Debug.LogError(message  + ex.Message);
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("DllNotFoundException: Xtr3dManager")) 
			{
				message = "dll not found in the directory, or xtr3d prerequisites not installed properly";
			}
			else 
			{
				message = "Generic exception: \n";
				Debug.LogError(message + ex.ToString());
			}
		}
		
		StatusText.text = message;
	}
	
	void StartManager ()
	{
		Debug.Log("SDK Manager Start04");
		
		if (GeneratorSingleton.Instance.IsInitialized) {
			
			Debug.Log("SDK Manager Start05");
			string message = String.Empty;
			try {
				GeneratorSingleton.Instance.Start ();
				message = "Camera found. Engine started.";
				Debug.Log("SDK Manager Start06");
				GeneratorSingleton.Instance.AllFramesReady += MyAllFramesReadyEventHandler;
			}
			catch (CameraAbsentException ex) {
				message = "Camera failure: Camera not found. Connect it and then click Reset Button.";
				Debug.LogError (message + "\n" + ex.Message);
			}
			catch (CameraBusyException ex) {
				message = "Camera failure:  Camera busy. Free it and then click Reset Button.";
				Debug.LogError (message + "\n" + ex.Message);
			}
			catch (Exception ex) {
				message = "General exception: " + ex.Message;
				Debug.LogError (message + "\n" + ex.ToString ());
			}
			StatusText.text = message;
		}
	}
	void StopManager ()
	{
		string message = String.Empty;
		try {
			GeneratorSingleton.Instance.Stop ();
			GeneratorSingleton.Instance.AllFramesReady -= MyAllFramesReadyEventHandler;
		}
		catch (Exception ex) {
			message = "General exception: " + ex.Message;
			Debug.LogError (message + "\n" + ex.ToString ());
		}
		StatusText.text = message;
	}
	
	void Start () {
		
		Debug.Log("SDK Manager Start");
		
		if (Application.platform == RuntimePlatform.IPhonePlayer
		    || Application.platform == RuntimePlatform.Android)
		{
			OnApplicationPause(false);
			Debug.Log("SDK Manager Start 02");
			
		}
		
		StartManager ();
		Debug.Log("SDK Manager Start03");
	}
	
	
	void OnApplicationPause(bool paused)
	{
		Debug.Log ("OnApplicationPause(bool paused) :"  + paused);
		if (paused)
		{
			StopManager ();
		}
		else
		{
			StartManager();
		}
	}
	
	public void ResetManager()
	{
		if (GeneratorSingleton.Instance.IsInitialized)
		{
			Debug.Log("Call for Reset");
			GeneratorSingleton.Instance.Reset();
		}
	}
	
	private void SetGestureFile() {
		string path = String.Empty;
		
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			#if UNITY_WINRT  
			//System.IO do not compile exist on WinRT, so we also need a compilation ifdef
			#else
			try {
				
				System.IO.File.Copy(Application.streamingAssetsPath + "/SamplePoses.xml", Application.persistentDataPath + "/SamplePoses.xml",true);
			} catch (Exception ex) {
				Debug.LogError("copying SamplePoses.xml file to Data folder failed: " + ex.Message);
			}
			#endif
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			path = Application.persistentDataPath + "/";
		}
		
		GeneratorSingleton.Instance.SetGestureRecognitionFile(path + "Pose4.xml");
	}
	
	private PlatformType GetPlatformType()
	{
		PlatformType platform = PlatformType.WINDOWS; 
		
		if (Application.platform == RuntimePlatform.Android)
			platform = PlatformType.ANDROID;
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			platform = PlatformType.IOS;
		else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			platform = PlatformType.MAC;
		return platform;
	}
	
	private void MyAllFramesReadyEventHandler(object sender, AllFramesReadyEventArgs e)
	{
		try
		{
			using (var allFrames = e.OpenFrame() as AllFramesFrame)
			{
				if (allFrames != null)
				{
					foreach (var evtArgs in allFrames.FramesReadyEventArgs)
					{
						var colorImageFrameReady = evtArgs as ColorImageFrameReadyEventArgs;
						if ((ExtremeMotionEventsManager.MyColorImageFrameReadyHandler != null) && (null != colorImageFrameReady))
						{
							ExtremeMotionEventsManager.MyColorImageFrameReadyHandler(sender, colorImageFrameReady);
							continue;
						}
						var dataFrameReady = evtArgs as DataFrameReadyEventArgs;
						if ((ExtremeMotionEventsManager.MyDataFrameReadyHandler != null) && (null != dataFrameReady))
						{
							ExtremeMotionEventsManager.MyDataFrameReadyHandler(sender, dataFrameReady);
							continue;
						}
						var gesturesFrameReady = evtArgs as GesturesFrameReadyEventArgs;
						if ((ExtremeMotionEventsManager.MyGesturesFrameReadyHandler != null) && (null != gesturesFrameReady))
						{
							ExtremeMotionEventsManager.MyGesturesFrameReadyHandler(sender, gesturesFrameReady);
							continue;
						}
					}
				}
			}
		}
		catch (System.Exception ex)
		{
			Debug.LogError("Error in MyAllFramesReadyEventHandler: \n" + ex.ToString());
		}
	}
	
	void OnApplicationQuit()
	{
		if (GeneratorSingleton.Instance.IsInitialized)
		{
			Debug.Log("Shutting down");
			GeneratorSingleton.Instance.Stop();
			GeneratorSingleton.Instance.Shutdown();
		}
	}
}


