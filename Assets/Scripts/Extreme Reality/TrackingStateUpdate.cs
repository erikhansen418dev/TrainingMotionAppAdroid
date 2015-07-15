using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xtr3D.Net.ColorImage;
using Xtr3D.Net.ExtremeMotion;
using Xtr3D.Net.ExtremeMotion.Data;


public class TrackingStateUpdate: MonoBehaviour {
	private TrackingState trackingState;
	private const string basicTrackingText = "Tracking State: ";

	public Text TrackingText;
	private string text;
    private long FrameId;
	
	private Dictionary<TrackingState, string> stateTextDictionary = new Dictionary<TrackingState, string>() { 
		{TrackingState.Initializing, "Initializing"},
		{TrackingState.Calibrating, "Calibrating"},
		{TrackingState.NotTracked, "Not Tracked"},
		{TrackingState.Tracked, "Tracked"}
	};

	private void MyDataFrameReadyEventHandler(object sender, DataFrameReadyEventArgs e)
	{
		using (DataFrame dataFrame = e.OpenFrame() as DataFrame)
		{
			if (dataFrame != null)
			{
                Skeleton skl = dataFrame.Skeletons[0];
				trackingState = skl.TrackingState;
				
				if (SdkManager.IsDebugRun)
				{
	                FrameId = dataFrame.FrameKey.FrameNumberKey;
	                Debug.Log ("Skeleton frame: " + FrameId + ", state: " + skl.TrackingState + ", proximity: " + skl.Proximity.SkeletonProximity);
				}
			}
		}
	}

	void OnApplicationPause(bool paused) 
	{
		if (paused) {
			ExtremeMotionEventsManager.MyDataFrameReadyHandler -= MyDataFrameReadyEventHandler;
		}
		else 
		{
			ExtremeMotionEventsManager.MyDataFrameReadyHandler += MyDataFrameReadyEventHandler;
		}
	}

	public void Start()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer
		    || Application.platform == RuntimePlatform.Android)
		{
			OnApplicationPause(false);
		}
	}


	void Update () {
		if (!stateTextDictionary.TryGetValue(trackingState, out text))
		{
			text = "UNRECOGNIZED STATE";
		}

		TrackingText.text = basicTrackingText + text;
	}
}