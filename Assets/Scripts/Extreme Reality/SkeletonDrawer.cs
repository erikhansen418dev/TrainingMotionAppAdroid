using UnityEngine;
using System.Collections.Generic;
using System;
using Xtr3D.Net;
using Xtr3D.Net.BaseTypes;
using Xtr3D.Net.ExtremeMotion;
using Xtr3D.Net.ExtremeMotion.Data;
using Xtr3D.Net.ExtremeMotion.Interop.Types;
using Vectrosity;

public class SkeletonDrawer : MonoBehaviour 
{
	public UnityEngine.UI.RawImage RgbImage;

	readonly Vector2 TARGET_RESOLUTION = new Vector2 (1024, 768);

	public static float rgbWidth;
	public static float rgbHeight;
	public static float upperRightX;
	public static float upperRightY;
	
	private JointCollection mySkeleton;
	public BonesRenderer leftHandBone;
	public BonesRenderer rightHandBone;
	public BonesRenderer leftArmBone;
	public BonesRenderer rightArmBone;
	public BonesRenderer leftShouldersBone;
	public BonesRenderer rightShouldersBone;
	public BonesRenderer spineBone;
	public BonesRenderer neckBone;
	public BonesRenderer shoulderCenterToSpineBone;

	private List<BonesRenderer> m_Bones = new List<BonesRenderer>();
	private TrackingState state;

	void Awake()
	{
		leftHandBone.InitLine("LeftHandLine");
		rightHandBone.InitLine("RightHandLine");
		leftArmBone.InitLine("LeftArmLine");
		rightArmBone.InitLine("RightArmLine");
		leftShouldersBone.InitLine("LeftShouldersLine");
		rightShouldersBone.InitLine("RightShouldersLine");
		spineBone.InitLine("SpineLine");
		neckBone.InitLine("NeckLine");
		shoulderCenterToSpineBone.InitLine("ShoulderCenterToSpineLine");
	}

    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
			ExtremeMotionEventsManager.MyDataFrameReadyHandler -= MyDataFrameReady;
            GeneratorSingleton.Instance.GeneratorStateChanged -= HandleGeneratorStateChanged;
        }
        else
        {
			ExtremeMotionEventsManager.MyDataFrameReadyHandler += MyDataFrameReady;
            GeneratorSingleton.Instance.GeneratorStateChanged += HandleGeneratorStateChanged;
        }
    }

	void Start () {
		if (GeneratorSingleton.Instance.IsInitialized)
		{
			OnApplicationPause(false);
		}
		m_Bones.Add(leftHandBone);
		m_Bones.Add(rightHandBone);
		m_Bones.Add(leftArmBone);
		m_Bones.Add(rightArmBone);
		m_Bones.Add(leftShouldersBone);
		m_Bones.Add(rightShouldersBone);
		m_Bones.Add(spineBone);
		m_Bones.Add(neckBone);
		m_Bones.Add(shoulderCenterToSpineBone);

		rgbWidth = RgbImage.rectTransform.sizeDelta.x;
		rgbHeight = RgbImage.rectTransform.sizeDelta.y;

		float skeletonScalingFactor = Screen.height / TARGET_RESOLUTION.y;

		rgbWidth = rgbWidth * skeletonScalingFactor ;
		rgbHeight = rgbHeight * skeletonScalingFactor;

		upperRightX = RgbImage.rectTransform.position.x - rgbWidth/2;
		upperRightY = RgbImage.rectTransform.position.y + rgbHeight/2;
	}

	void HandleGeneratorStateChanged (GeneratorBase.GeneratorState state)
	{
		switch (state)
		{
		case GeneratorBase.GeneratorState.Initialized:
			EnableBones(false);
			break;
		}
	}


	private void MyDataFrameReady(object sender, DataFrameReadyEventArgs e)
	{
		using (var dataFrame = e.OpenFrame() as DataFrame)
		{
			if (dataFrame != null)
			{
				state = dataFrame.Skeletons[0].TrackingState;
				mySkeleton = dataFrame.Skeletons[0].Joints;
			}
		}
	}
	
	void Update ()
	{
		JointCollection skl = mySkeleton;
		if(skl != null){
			switch (state) {
			case TrackingState.Tracked:
				EnableBones(true);
				UpdateJointsPositions();
				break;
			case TrackingState.Calibrating:
			case TrackingState.NotTracked:
			case TrackingState.Initializing:
					EnableBones(false);
				break;
			default:
				break;
			}
		}
	}

	private void UpdateJointsPositions()
	{
		JointCollection skl = mySkeleton;
		
		leftHandBone.UpdateJointsPosition(skl.ElbowLeft, skl.HandLeft);
		rightHandBone.UpdateJointsPosition(skl.ElbowRight, skl.HandRight);
		leftArmBone.UpdateJointsPosition(skl.ShoulderLeft, skl.ElbowLeft);
		rightArmBone.UpdateJointsPosition(skl.ShoulderRight, skl.ElbowRight);
		leftShouldersBone.UpdateJointsPosition(skl.ShoulderLeft, skl.Spine);
		rightShouldersBone.UpdateJointsPosition(skl.Spine, skl.ShoulderRight);
		spineBone.UpdateJointsPosition(skl.Spine, skl.HipCenter);
		neckBone.UpdateJointsPosition(skl.Head, skl.ShoulderCenter);
		shoulderCenterToSpineBone.UpdateJointsPosition(skl.Spine, skl.ShoulderCenter);
		//Draw the bones. All bones should be drawn at once so they don't look disconnected
		foreach(BonesRenderer bone in m_Bones)
		{
			bone.Draw ();
		}
	}
	

	private void EnableBones(bool state)
	{	
		foreach (var bone in m_Bones)
		{
			bone.BoneEnabled = state;
		}
	}
}
