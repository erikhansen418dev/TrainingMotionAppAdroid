using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace com.erik.training.view{

	public class FramePanelView : MonoBehaviour {

		public delegate void FramePanelEventDelegate();
		public static event FramePanelEventDelegate OnGoUserInfo;
		public static event FramePanelEventDelegate OnGoHome;
		public static event FramePanelEventDelegate OnAppExit;

		
		public Text textScreenTitle;

		public Button buttonUserInfo;
		public Text textUserInfo;

		public Sprite spriteUserInfoActive;
		public Sprite spriteUserInfoInactive;

		public Color UserInfoColor;
		public Color ScreenTitleBGColor;

		public Button buttonClose;
		public Button buttonHome;
		public GameObject cameraFeed;
		
		// Use this for initialization
		void Start () {
			
			buttonClose.onClick.AddListener (OnButtonClose);	
			buttonUserInfo.onClick.AddListener (OnButtonUserInfo);
			buttonHome.onClick.AddListener (OnButtonHome);

		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
		
		private void OnButtonClose()
		{	
			Debug.Log ("OnButtonClose");
			if (OnAppExit != null) {
				OnAppExit ();		
				Debug.Log (" Send OnButtonClose");
			}
		}

		private void OnButtonUserInfo()
		{
			Debug.Log ("OnButtonUserInfo");
			if (OnGoUserInfo != null) {
				OnGoUserInfo ();
				Debug.Log (" Send OnButtonUserInfo");
			}
		}

		private void OnButtonHome()
		{
			if (OnGoHome != null)
				OnGoHome ();
		}

		public void SetTitle(string strTitle)
		{
			textScreenTitle.text = strTitle;
		}
		
		public void SetUserInfo(string strInfo)
		{
			textUserInfo.text = strInfo;
		}

		public void EnableUserInfoButton(bool bEnable)
		{
			buttonUserInfo.enabled = bEnable;
			if (bEnable) {
				buttonUserInfo.GetComponent<Image> ().sprite = spriteUserInfoActive;
				textUserInfo.color = Color.white;
			} else {
				buttonUserInfo.GetComponent<Image> ().sprite = spriteUserInfoInactive;
				textUserInfo.color = UserInfoColor;
			}
		}

		public void ActivateCameraFeed(bool bActivate)
		{
			cameraFeed.SetActive (bActivate);
		}

		public void ShowHomeButton(bool bShow)
		{
			buttonHome.gameObject.SetActive (bShow);
		}

	}


}
