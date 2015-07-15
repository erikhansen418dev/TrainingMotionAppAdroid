using UnityEngine;
using UnityEngine.UI;
using Xtr3D.Net.ExtremeMotion.Data;
using Xtr3D.Net.ColorImage;
using Xtr3D.Net.ExtremeMotion;


public class RgbTextureUpdate : MonoBehaviour {

	public RawImage frameTexture;
	public RawImage calibrationImage;
	public Text rgbStateText;

	private Texture2D buffer;
	private long newFrameId=-1;
	private long lastDrawnFrameId= -1;
	private byte[] newRgbImage;
	bool drawRGB = true;
	
	private TrackingState lastEngineState = TrackingState.NotTracked;
	private TrackingState currentEngineState = TrackingState.Initializing;

	void Awake() {
		buffer = new Texture2D(640, 480, TextureFormat.RGB24, false);
	}

	private void MyDataFrameReadyEventHandler(object sender, DataFrameReadyEventArgs e)
	{
		using (DataFrame dataFrame = e.OpenFrame() as DataFrame)
		{
			if (dataFrame != null)
			{
				lastEngineState = dataFrame.Skeletons[0].TrackingState;
			}
		}
	}
	
	private void MyColorImageFrameReadyEventHandler(object sender, ColorImageFrameReadyEventArgs e)
	{
        using (ColorImageFrame colorImageFrame = e.OpenFrame() as ColorImageFrame)
        {
            if (colorImageFrame != null)
            {
				if (newFrameId == colorImageFrame.FrameKey.FrameNumberKey)
					return;
				
				ColorImage colorImage = colorImageFrame.ColorImage;
				newRgbImage = colorImage.Image;
				newFrameId = colorImageFrame.FrameKey.FrameNumberKey;

				if (SdkManager.IsDebugRun)
				{
					Debug.Log ("Raw image frame: " + newFrameId);
				}
            }
        }
	}

	void OnApplicationPause(bool paused) 
	{
		if (paused) {
			ExtremeMotionEventsManager.MyDataFrameReadyHandler -= MyDataFrameReadyEventHandler;
			ExtremeMotionEventsManager.MyColorImageFrameReadyHandler -= MyColorImageFrameReadyEventHandler;
		}
		else 
		{
			ExtremeMotionEventsManager.MyDataFrameReadyHandler += MyDataFrameReadyEventHandler;
			ExtremeMotionEventsManager.MyColorImageFrameReadyHandler += MyColorImageFrameReadyEventHandler;
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

	private void OnEnable()
	{
		ApplyCamera (true);
	}

	private void OnDisable()
	{
		ApplyCamera (false);
	}

	void ApplyCamera(bool bApply)
	{
		if (bApply) {
			ExtremeMotionEventsManager.MyDataFrameReadyHandler += MyDataFrameReadyEventHandler;
			ExtremeMotionEventsManager.MyColorImageFrameReadyHandler += MyColorImageFrameReadyEventHandler;
		}
		else 
		{
			ExtremeMotionEventsManager.MyDataFrameReadyHandler -= MyDataFrameReadyEventHandler;
			ExtremeMotionEventsManager.MyColorImageFrameReadyHandler -= MyColorImageFrameReadyEventHandler;		
		}
	}


	public void ToggleRgbFreeze() {
		drawRGB = !drawRGB;
		rgbStateText.text = drawRGB ? "" : "CLICK RGB IMAGE TO RESUME";
	}

	// Update is called once per frame
	void Update () {
		if (drawRGB)
		{
			if ((newRgbImage != null) && (lastDrawnFrameId != newFrameId))  //no need to draw again if RGB image was not changed
			{
				lastDrawnFrameId = newFrameId;	
				buffer.LoadRawTextureData(newRgbImage);
				buffer.Apply();
				frameTexture.texture = buffer;
			}
		}

		if(lastEngineState != currentEngineState)
		{
			currentEngineState = lastEngineState;

			switch (currentEngineState) {

			case  TrackingState.Tracked:
				calibrationImage.enabled = false;
				break;
			case  TrackingState.Calibrating:
				calibrationImage.enabled = true;
				break;
			default:
				break;
			}
		}
	}
}