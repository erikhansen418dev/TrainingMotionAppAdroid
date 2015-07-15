using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExercisePopupView : MonoBehaviour {

	public delegate void ExercisePopupViewEventDelegate(bool bForCalibrate);
	public event ExercisePopupViewEventDelegate OnCloseWith;
//	public event ExercisePopupViewEventDelegate OnCloseForCalibrate;

	public Button buttonCalibrate;
	public Button buttonFinish;

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable()
	{
		buttonCalibrate.onClick.AddListener (OnButtonCalibrate);
		buttonFinish.onClick.AddListener (OnButtonFinish);
	}

	void OnDisable()
	{
		buttonCalibrate.onClick.RemoveListener (OnButtonCalibrate);
		buttonFinish.onClick.RemoveListener (OnButtonFinish);
	}

	void OnButtonFinish()
	{
		if (OnCloseWith != null)
			OnCloseWith (false);
	}

	void OnButtonCalibrate()
	{
		if (OnCloseWith != null)
			OnCloseWith (true);
	}

}
