using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.erik.util;
using com.erik.training.model;
using com.erik.training.view;

namespace com.erik.training.controller{

	public class ViewController : Singleton<ViewController> {
		
		public delegate void ViewControllerEventDelegate();
		public static event ViewControllerEventDelegate OnReady;
		
		private static GameObject canvasRoot;
		private static GameObject eventSystem = null;
		private static GameObject currentPanel = null;
		private static GameObject frameViewPanel = null;
		private static FramePanelView frameView = null;
		
		private string screenTitle;
		
		protected override void Init ()
		{
			base.Init ();
			
			Object _canvasRootResource = Resources.Load<Object> (Constants.PATH_CANVAS_ROOT_KEY);
			canvasRoot = (GameObject)Instantiate (_canvasRootResource);
			
			Object _framViewResource = Resources.Load<Object> (Constants.PATH_FRAME_VIEW_KEY);
			frameViewPanel = InstantiatePanel (_framViewResource);
			
			if (frameViewPanel != null) {
				frameView = frameViewPanel.GetComponent<FramePanelView> ();
			}

			frameView.ActivateCameraFeed (false);
			
			Object _eventSystemResource = Resources.Load<Object> (Constants.PATH_EVENT_SYSTEM_KEY);
			eventSystem = (GameObject)Instantiate (_eventSystemResource);
			
			if (OnReady != null)
				OnReady ();
		}
		
		// Use this for initialization
		void Start () {
			
		}
		
		
		public void SetViewState(ViewState newState)
		{
			if (currentPanel != null)
				Destroy (currentPanel);
			
			Object _panelResource = null;
			bool bShowCameraFeed = false;
			bool bEnableUserInfoButton = false;
			bool bShowHomeButton = false;

			switch (newState) {
				
			case ViewState.VS_CALIBRATION:
				
				_panelResource = Resources.Load<Object>(Constants.PATH_CALIBRATION_VIEW_KEY);
				screenTitle = Constants.TITLE_CALIBRATION_VIEW;

				bShowCameraFeed = true;
				bShowHomeButton = true;
				break;

			case ViewState.VS_EXERCISE:
				
				_panelResource = Resources.Load<Object>(Constants.PATH_EXCERCISE_VIEW_KEY);
				screenTitle = Constants.TITLE_EXCERCISE_VIEW;

				bShowCameraFeed = true;
				bShowHomeButton = true;
				break;
				
			case ViewState.VS_HOME:
				
				_panelResource = Resources.Load<Object>(Constants.PATH_HOME_VIEW_KEY);
				screenTitle = Constants.TITLE_HOME_VIEW;

				bEnableUserInfoButton = true;
				frameView.SetUserInfo (UserData.user.firstName +" " + UserData.user.lastName);
				break;
				
			case ViewState.VS_REGISTER:
				
				_panelResource = Resources.Load<Object>(Constants.PATH_REGISTER_VIEW_KEY);
				screenTitle = Constants.TITLE_REGISTER_VIEW;
				frameView.SetUserInfo ("User Info");
				break;
				
			case ViewState.VS_SUMMARY:
				
				_panelResource = Resources.Load<Object>(Constants.PATH_SUMMARY_VIEW_KEY);
				screenTitle = Constants.TITLE_SUMMARY_VIEW;

				bShowHomeButton = true;
				break;
				
			case ViewState.VS_TUTORIAL:
				
				_panelResource = Resources.Load<Object>(Constants.PATH_TUTORIAL_VIEW_KEY);
				screenTitle = Constants.TITLE_TUTORIAL_VIEW;

				bShowHomeButton = true;
				break;
				
			default:
				break;
			}
			
			currentPanel = InstantiatePanel (_panelResource);
			frameView.SetTitle (screenTitle);
			frameView.ActivateCameraFeed(bShowCameraFeed);
			frameView.EnableUserInfoButton (bEnableUserInfoButton);
			frameView.ShowHomeButton (bShowHomeButton);
			
			if (OnReady != null) {
				OnReady ();		
			}
		}
		
		private GameObject InstantiatePanel(Object panelResource)
		{
			GameObject panel = (GameObject)Instantiate(panelResource);
			panel.transform.SetParent(canvasRoot.transform, false);
			RectTransform rt = panel.GetComponent<RectTransform> ();
			rt.sizeDelta = Vector2.zero;
			rt.localScale = Vector3.one;
			rt.anchoredPosition = Vector2.zero;
			
			return panel;
		}
	}
}

