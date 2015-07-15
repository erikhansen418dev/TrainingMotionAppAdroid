using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Xtr3D.Net.AllFrames;
using Xtr3D.Net.ColorImage;
using Xtr3D.Net.ExtremeMotion;
using Xtr3D.Net.ExtremeMotion.Data;
using Xtr3D.Net.ExtremeMotion.Gesture;
using Xtr3D.Net.ExtremeMotion.Interop.Types;

public static class ExtremeMotionEventsManager {
	
	public static EventHandler<ColorImageFrameReadyEventArgs> MyColorImageFrameReadyHandler = null;
	public static EventHandler<DataFrameReadyEventArgs> MyDataFrameReadyHandler = null;
	public static EventHandler<GesturesFrameReadyEventArgs> MyGesturesFrameReadyHandler = null;

	public static void RegisterColorImageCallback(EventHandler<ColorImageFrameReadyEventArgs> h)
	{
		MyColorImageFrameReadyHandler += h;
	}
	
	public static void RegisterDataCallback(EventHandler<DataFrameReadyEventArgs> h)
	{
		MyDataFrameReadyHandler += h;
	}	
	public static void RegisterGesturesCallback(EventHandler<GesturesFrameReadyEventArgs> h)
	{
		MyGesturesFrameReadyHandler += h;
	}
}
