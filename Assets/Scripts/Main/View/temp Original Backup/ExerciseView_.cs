/*using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.erik.util;
using com.erik.training.model;

namespace com.erik.training.view{

	public class ExerciseView : MonoBehaviour {

		public GameObject caliGO;
		public GameObject statusGO;

		private CalibrationView caliView;
		private ExerciseStatusView exStatusView;
		public RawImage cameraFeed;

		// Use this for initialization
		void Start () {

			Init ();
						
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		private bool Init()
		{
			if (caliGO == null || statusGO == null) {
				Debug.Log("Calibration Panel Or Status Panel not set");
				return false;
			}

			if ((caliView = Extention.GetSafeComponent<CalibrationView> (caliGO)) == null) {
				return false;				
			}

			if ((exStatusView = Extention.GetSafeComponent<ExerciseStatusView> (statusGO)) == null) {
				return false;				
			}

			ShowCalibration ();

			return true;
		}

		private void ShowStatus()
		{
			if (caliView == null || exStatusView == null)
				return;

			caliView.Hide ();
			exStatusView.Show ();
		}

		private void ShowCalibration()
		{			
			if (caliView == null || exStatusView == null)
				return;
			
			caliView.Show ();
			exStatusView.Hide ();
		}
	}

}
*/
