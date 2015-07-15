using UnityEngine;
using UnityEngine.UI;
using Xtr3D.Net.ExtremeMotion.Data;
using Xtr3D.Net.ColorImage;
using Xtr3D.Net.ExtremeMotion;


public class FpsUpdate : MonoBehaviour {

	public Text RgbFpsText;
	public Text SkeletonFpsText;

	private FrameRateCalc rgbFrameRate;
	private FrameRateCalc skeletonFrameRate;

	void Awake() {
		rgbFrameRate = new FrameRateCalc(50);
		skeletonFrameRate = new FrameRateCalc(50);
	}

	private void MyDataFrameReadyEventHandler(object sender, DataFrameReadyEventArgs e)
	{
		using (DataFrame dataFrame = e.OpenFrame() as DataFrame)
		{
			if (dataFrame != null)
			{
				skeletonFrameRate.UpdateAvgFps();
			}
		}
	}
	
	private void MyColorImageFrameReadyEventHandler(object sender, ColorImageFrameReadyEventArgs e)
	{
        using (ColorImageFrame colorImageFrame = e.OpenFrame() as ColorImageFrame)
        {
            if (colorImageFrame != null)
            {
				rgbFrameRate.UpdateAvgFps();
            }
        }
	}

	void OnApplicationPause(bool paused) 
	{
		if (paused) {
			GeneratorSingleton.Instance.DataFrameReady -= MyDataFrameReadyEventHandler;
			GeneratorSingleton.Instance.ColorImageFrameReady -= MyColorImageFrameReadyEventHandler;
		}
		else 
		{
			GeneratorSingleton.Instance.DataFrameReady += MyDataFrameReadyEventHandler;
			GeneratorSingleton.Instance.ColorImageFrameReady += MyColorImageFrameReadyEventHandler;
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
		RgbFpsText.text = string.Format("Camera RGB:\n{0:F2}", rgbFrameRate.GetAvgFps());
		SkeletonFpsText.text = System.String.Format("Skeleton:\n{0:F2}",skeletonFrameRate.GetAvgFps());
	}
}