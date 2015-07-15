using UnityEngine;
using System.Collections;
using com.erik.util;

namespace com.erik.util
{

    public class RS_Begin : RunState
    {
        
        // Use this for initialization
        public override void Start ()
        {
			Application.runInBackground = true;
			//iPhoneSettings.screenCanDarken = false;
			Screen.sleepTimeout = 0;

            // do any environment variables here
			// test stuff...
			// PlayerUI.controller.PlayerUIController.SetStore(); // testing only!!!

            #if UNITY_IPHONE

            #endif
            
            #if UNITY_ANDROID

            #endif
            
            #if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX

            #endif

            //nextState = typeof(RS_Load);
            //GoNext();
            
        }
        
    }

}