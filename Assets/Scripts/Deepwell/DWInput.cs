using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deepwell
{
    public class DWInput : Input
    {
        public static bool userInputEnabled = true;
        public static event Action jumpEvent; 
        private static Dictionary<string, float> _simulatedAxis = new Dictionary<string, float>();
        private static bool _simulateJump = false;
        private static int _jumpCheckFrame = -1;

        static DWInput()
        {
            // CrossPlatformInputManager.SwitchActiveInputMethod(CrossPlatformInputManager.ActiveInputMethod.Touch);
        }

        public static void SimulateJump()
        {
            _simulateJump = true;
        }

        public static void SimulateAxis(string axisName, float axisValue)
        {
            _simulatedAxis[axisName] = axisValue;
        }

        public static void CheckForJump() // should be called from Update
        {
            if (_simulateJump)
            {
                _simulateJump = false;
                jumpEvent?.Invoke();
            }
            else if (_jumpCheckFrame != Time.frameCount && DWInput.GetButtonDown("Jump"))
            {
                _jumpCheckFrame = Time.frameCount;
                jumpEvent?.Invoke();
            }
        }
        
        public new static bool GetButtonDown(string name)
        {
            if (!DWInput.userInputEnabled) return false;
            else return Input.GetButtonDown(name);
        }

        public new static bool GetKeyDown(KeyCode key)
        {
            if (!DWInput.userInputEnabled) return false;
            return Input.GetKeyDown(key);
        }

        public new static float GetAxis(string name)
        {
            if (!DWInput.userInputEnabled) return 0f;
#if UNITY_IOS || UNITY_ANDROID
            if (_simulatedAxis.ContainsKey(name)) return _simulatedAxis[name];
            else return Input.GetAxis(name);
#else
            return Input.GetAxis(name);
#endif
        }

        public static Touch? GetTouchByFinderId(int id)
        {
            foreach (var aTouch in DWInput.touches)
            {
                if (aTouch.fingerId == id) return aTouch;
            }
            return null;
        }
    }
}
