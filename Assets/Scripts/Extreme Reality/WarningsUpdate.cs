using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Xtr3D.Net;
using Xtr3D.Net.ColorImage;
using Xtr3D.Net.ExtremeMotion;
using Xtr3D.Net.ExtremeMotion.Data;
using Xtr3D.Net.Patterns;


public class WarningsUpdate : MonoBehaviour {
	public Text WarningsText;
	
	private FrameEdges edgeWarnings = FrameEdges.None;
	private ImageWarnings imageWarnings = ImageWarnings.None;
	private ImageStreamWarnings imageSteamWarnings = ImageStreamWarnings.None;
	
	
	void Awake() {
		if (Application.platform == RuntimePlatform.WindowsPlayer)
		{
			WarningsText.fontSize = 20;
		}
	}

	private void MyDataFrameReadyEventHandler(object sender, DataFrameReadyEventArgs e)
	{
		using (DataFrame dataFrame = e.OpenFrame() as DataFrame)
		{
			if (dataFrame != null)
			{
				Skeleton skl = dataFrame.Skeletons[0];
				edgeWarnings = skl.ClippedEdges;
				StringBuilder sb = new StringBuilder();
				int warningsCount = CheckEdgeWarnings(sb);

				if (SdkManager.IsDebugRun)
				{
					Debug.Log(String.Format("Warnings frame: {0}, contains {1} Warnings:\n{2}", dataFrame.FrameKey.FrameNumberKey, warningsCount, sb.ToString()));
				}
			}
		}
	}
	
	private void MyColorImageFrameReadyEventHandler(object sender, ColorImageFrameReadyEventArgs e)
	{
        using (ColorImageFrame colorImageFrame = e.OpenFrame() as ColorImageFrame)
        {
            if (colorImageFrame != null)
            {
				imageWarnings = colorImageFrame.Warnings;
				
				// some of the warnings are on the stream and not on the image itself.
				var colorImageStream = colorImageFrame.Stream as Xtr3D.Net.BaseTypes.ImageStreamBase<FrameKey, ColorImage>;
				imageSteamWarnings = colorImageStream.Warnings;
				StringBuilder sb = new StringBuilder();
				int warningsCount = CheckImageWarnings(sb);

				if (SdkManager.IsDebugRun)
				{
					Debug.Log(String.Format("Warnings frame: {0}, contains {1} Warnings:\n{2}", colorImageFrame.FrameKey.FrameNumberKey, warningsCount, sb.ToString()));
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

	void Update () {
		StringBuilder sb = new StringBuilder();
		
		if (edgeWarnings != FrameEdges.None)
		{
			CheckEdgeWarnings(sb);
		}
		
		if (imageWarnings != ImageWarnings.None)
		{
			CheckImageWarnings(sb);
		}
		
		if (imageSteamWarnings != ImageStreamWarnings.None)
		{
			CheckImageStreamWarnings(sb);
		}
		
		string text = sb.ToString();
		WarningsText.text = text;
	}
	
	private int CheckEdgeWarnings(StringBuilder sb)
	{
		int count = 0;
		if (edgeWarnings.HasFlag(FrameEdges.Far))
		{
			sb.Append(" - Too far\n");
			count++;
		}
		if (edgeWarnings.HasFlag(FrameEdges.Near))
		{
			sb.Append(" - Too close\n");
			count++;
		}
		if (edgeWarnings.HasFlag(FrameEdges.Left))
		{
			sb.Append(" - Too left\n");
			count++;
		}
		if (edgeWarnings.HasFlag(FrameEdges.Right))
		{
			sb.Append(" - Too right\n");
			count++;
		}
		return count;
	}
	
	private int CheckImageWarnings(StringBuilder sb)
	{
		int count = 0;
		if (imageWarnings.HasFlag(ImageWarnings.LightLow))
		{
			sb.Append(" - Low lighting\n");
			count++;
		}
		if (imageWarnings.HasFlag(ImageWarnings.StrongBacklighting))
		{
			sb.Append(" - Strong backlighting\n");
			count++;
		}
		if (imageWarnings.HasFlag(ImageWarnings.TooManyPeople))
		{
			sb.Append(" - Too many people\n");
			count++;
		}
		return count;
	}
	
	private void CheckImageStreamWarnings(StringBuilder sb)
	{
		if (imageSteamWarnings.HasFlag(ImageStreamWarnings.EnvironmentChanged))
		{
			sb.Append(" - Environment changed\n");
		}
		if (imageSteamWarnings.HasFlag(ImageStreamWarnings.LowFrameRate))
		{
			sb.Append(" - Low frame rate\n");
		}
	}
}