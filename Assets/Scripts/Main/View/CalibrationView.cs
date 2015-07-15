using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Xtr3D.Net.ExtremeMotion.Data;
using Xtr3D.Net.ColorImage;
using Xtr3D.Net.ExtremeMotion;

namespace com.erik.training.view{

	public class CalibrationView : MonoBehaviour {
		
		public delegate void CalibrationViewEventDelegate();
		public static event CalibrationViewEventDelegate OnGoHome;
		public static event CalibrationViewEventDelegate OnCalibrationSuccess;

		public Button buttonGoHome;

		private TrackingState lastEngineState = TrackingState.NotTracked;
		private TrackingState currentEngineState = TrackingState.Initializing;
		
		// Use this for initialization
		void Start () {
			
			buttonGoHome.onClick.AddListener (OnButtonGoHome);	
			ExtremeMotionEventsManager.MyDataFrameReadyHandler += MyDataFrameReadyEventHandler;
		}
		
		// Update is called once per frame
		void Update () {

			if(lastEngineState != currentEngineState)
			{
				currentEngineState = lastEngineState;
				
				switch (currentEngineState) {
					
				case  TrackingState.Tracked:
					OnTracked();
					break;
				case  TrackingState.Calibrating:
					break;
				default:
					break;
				}
			}
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
		
		private void OnButtonGoHome()
		{
			ExtremeMotionEventsManager.MyDataFrameReadyHandler -= MyDataFrameReadyEventHandler;
			if (OnGoHome != null)
				OnGoHome ();
		}

		private void OnTracked()
		{
			Debug.Log("OncalibrationSuccess");

			ExtremeMotionEventsManager.MyDataFrameReadyHandler -= MyDataFrameReadyEventHandler;
			if (OnCalibrationSuccess != null)
				OnCalibrationSuccess ();
		}
		
		public void Show()
		{
			gameObject.SetActive (true);
		}
		
		public void Hide()
		{
			gameObject.SetActive (false);
		}
		
	}

}
