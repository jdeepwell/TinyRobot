using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class DeviceDisplay {
    public struct ContentInsets
    {
        public float top;
        public float bottom;
        public float left;
        public float right;
    };

#if UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern float DeviceDisplay_GetScaleFactor();
    [DllImport ("__Internal")]
    private static extern void DeviceDisplay_GetContentSafeAreaInsets(out float top, out float bot);
#endif
	public static float scaleFactor
    {
		get
        {
			float scale = 1;
			// We check for UNITY_IPHONE again so we don't try this if it isn't iOS platform.
			#if UNITY_IPHONE
			// Now we check that it's actually an iOS device/simulator, not the Unity Player. You only get plugins on the actual device or iOS Simulator.
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				scale = DeviceDisplay_GetScaleFactor();
			}
			#endif
			// TODO:  You could test for Android, PC, Mac, Web, etc and do something with a plugin for them here.
		
			return scale;
		}
	}
}
