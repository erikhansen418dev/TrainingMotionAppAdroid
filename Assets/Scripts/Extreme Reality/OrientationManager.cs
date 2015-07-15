using UnityEngine;
using System.Collections;

public class OrientationManager : MonoBehaviour {

	public GameObject notSupportedMessage;
	
	void Update () {
		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (notSupportedMessage.activeSelf)
			{
				if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft ||
				    Input.deviceOrientation == DeviceOrientation.LandscapeRight)
				{
					notSupportedMessage.SetActive(false);
				}
			}
			else
			{
				if (Input.deviceOrientation == DeviceOrientation.Portrait)
				{
					notSupportedMessage.transform.localRotation = Quaternion.Euler(new Vector3(0,0,90));
					notSupportedMessage.SetActive(true);
				}
				if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
				{
					notSupportedMessage.transform.localRotation = Quaternion.Euler(new Vector3(0,0,270));
					notSupportedMessage.SetActive(true);
				}
			}
		}
	}
}
