using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Xtr3D.Net.ExtremeMotion.Data;
using Xtr3D.Net.ExtremeMotion.Interop.Types;
using Vectrosity;

public class BonesRenderer : MonoBehaviour 
{
	private VectorLine boneLine;
	private bool boneEnabled = false;
	private bool isTracked = false;
	
	internal bool BoneEnabled 
	{
		get 
		{
			return boneEnabled;
		}
		set
		{
			if (boneEnabled != value)
			{
				boneEnabled = value;
				if (!boneEnabled)
				{
					boneLine.points2.Clear();
					boneLine.points2.Add(new Vector3(0, 0));
					boneLine.points2.Add(new Vector3(0, 0));
					boneLine.Draw();
				}
			}
		}
	}
	
	public void InitLine(string name)
	{
		boneLine = new VectorLine(name, new Vector2[2] , null, 6f);
		boneLine.color = Color.green;
		
		boneLine.rectTransform.anchorMin.Set(0.5f, 0.5f);
		boneLine.rectTransform.anchorMax.Set(0.5f, 0.5f);
		boneLine.rectTransform.pivot.Set(0.5f, 0.5f);
	}
	
	public void UpdateJointsPosition(Xtr3D.Net.ExtremeMotion.Data.Joint firstJoint, Xtr3D.Net.ExtremeMotion.Data.Joint secondJoint)
	{
		if(firstJoint.jointTrackingState == JointTrackingState.NotTracked || secondJoint.jointTrackingState == JointTrackingState.NotTracked)
		{
			isTracked = false;
			return;
		}
		else
		{
			Vector3 firstJointPosition = GetJointPositionOnRgb(firstJoint);
			Vector3 secondJointPosition = GetJointPositionOnRgb(secondJoint);
			boneLine.points2.Clear();
			boneLine.points2.Add(firstJointPosition);
			boneLine.points2.Add(secondJointPosition);
			isTracked = true;
		}
	}

	private Vector3 GetJointPositionOnRgb(Xtr3D.Net.ExtremeMotion.Data.Joint joint)
	{
		float jointX = joint.skeletonPoint.ImgCoordNormHorizontal;
		float jointY = joint.skeletonPoint.ImgCoordNormVertical;
		
		float displayX = SkeletonDrawer.upperRightX + jointX * SkeletonDrawer.rgbWidth;
		float displayY = SkeletonDrawer.upperRightY - jointY * SkeletonDrawer.rgbHeight;
		
		return new Vector3(displayX, displayY);
	}
	
	public void Draw()
	{
		if (BoneEnabled && isTracked)
		{
			boneLine.Draw();
		}
	}
}
