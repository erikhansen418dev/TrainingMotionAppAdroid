using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Xtr3D.Net.ColorImage;
using Xtr3D.Net.ExtremeMotion;
using Xtr3D.Net.ExtremeMotion.Data;
using Xtr3D.Net.ExtremeMotion.Gesture;

class GestureMessage
{
	public GestureMessage(String text)
	{
		timeToLiveCounter = 0;
		Text = text;
	}

	public int timeToLiveCounter;
	public String Text;
};


public class GesturesUpdate : MonoBehaviour {

	public Text GesturesText;
	private StringBuilder gesturesSB = new StringBuilder();

    private Dictionary<int, GestureMessage> gestureMessages = new Dictionary<int, GestureMessage> {};  
	private Dictionary<BaseGesture.GestureType, int> gestureTypeDelay = new Dictionary<BaseGesture.GestureType, int>() { 
		{BaseGesture.GestureType.STATIC_POSITION, 1},
		{BaseGesture.GestureType.HEAD_POSITION, 1},
		{BaseGesture.GestureType.SWIPE, 40},
		{BaseGesture.GestureType.WINGS, 5},
		{BaseGesture.GestureType.SEQUENCE, 30},
		{BaseGesture.GestureType.UP, 40},
		{BaseGesture.GestureType.DOWN, 40},
		{BaseGesture.GestureType.RELATIVE_HOT_SPOT, 10}};


	//code sample of getting gestures from xtr3d engine. These gestures are defined inside SamplePoses.xml
	private void MyGestureFrameReadyEventHandler(object sender, GesturesFrameReadyEventArgs e)
	{
		// Opening the received frame
		using (var gesturesFrame = e.OpenFrame() as GesturesFrame)
		{
			lock (gesturesSB)
			{
				// Generate gestures text
				if (gesturesSB.Length > 0)
					gesturesSB.Remove (0, gesturesSB.Length - 1);

				if (gesturesFrame != null) 
				{
					BaseGesture[] gestures = gesturesFrame.FirstSkeletonGestures();
					foreach (BaseGesture gesture in gestures)
					{
						// Update messages for gesture
	                    if (!gestureMessages.ContainsKey(gesture.ID))
	                    {
							gestureMessages.Add(gesture.ID, new GestureMessage(gesture.Description));
						}
							gestureMessages [gesture.ID].timeToLiveCounter = gestureTypeDelay [gesture.Type];

						switch (gesture.Type)
						{
							case BaseGesture.GestureType.HEAD_POSITION:
								{
									HeadPositionGesture headPositionGesture = gesture as HeadPositionGesture;
									gestureMessages [gesture.ID].Text = gesture.Description + " " + headPositionGesture.RegionIndex;
									break;
								}
							case BaseGesture.GestureType.WINGS:
								{
									WingsGesture wingsGesture = gesture as WingsGesture;
									gestureMessages [gesture.ID].Text = gesture.Description + " " + wingsGesture.ArmsAngle;
									break;
								}
							}
						}

					if (SdkManager.IsDebugRun)
					{
						LogGestures(gestures, gesturesFrame.FrameKey.FrameNumberKey);
					}
				}

            
	            // Generate gestures text
				foreach (int id in gestureMessages.Keys)
				{
					if (gestureMessages[id].timeToLiveCounter > 0)
					{
						gesturesSB.AppendFormat("{0} - {1}\n" , id, gestureMessages[id].Text);
						gestureMessages[id].timeToLiveCounter--;
					}
				}
			}
		}
	}

	void LogGestures(BaseGesture[] gestures, long frameKey) {
		StringBuilder logLine = new StringBuilder();
		logLine.AppendFormat("Gestures frame: {0}, contains {1} gestures\n", frameKey, gestures.Length);
		
		int gestureCounter = 0;
		
		foreach (BaseGesture gesture in gestures)
		{
			string additionalGestureData = "";
			// Update messages for gesture

			switch (gesture.Type)
			{
			case BaseGesture.GestureType.HEAD_POSITION:
			{
				HeadPositionGesture headPositionGesture = gesture as HeadPositionGesture;
				additionalGestureData = " (" + headPositionGesture.RegionIndex+")";
				break;
			}
			case BaseGesture.GestureType.WINGS:
			{
				WingsGesture wingsGesture = gesture as WingsGesture;
				additionalGestureData = " (" + wingsGesture.ArmsAngle + ")";
				break;
			}
			default:
				break;
			}
			logLine.AppendFormat("{0}. Gesture id: {1} -  {2}{3}\n",gestureCounter, gesture.ID , gesture.Description, additionalGestureData);
			gestureCounter++;
		}
		Debug.Log(logLine);
	}

	void OnApplicationPause(bool paused) 
	{
		if (paused) {
			ExtremeMotionEventsManager.MyGesturesFrameReadyHandler -= MyGestureFrameReadyEventHandler;
		}
		else 
		{
			ExtremeMotionEventsManager.MyGesturesFrameReadyHandler += MyGestureFrameReadyEventHandler;
		}
	}

	void Update () 
	{
		String text;
		lock (gesturesSB)
		{
			text = gesturesSB.ToString ();
		}
		GesturesText.text = text;
	}

	public void Start()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer
		    || Application.platform == RuntimePlatform.Android)
		{
			OnApplicationPause(false);
		}
	}

}