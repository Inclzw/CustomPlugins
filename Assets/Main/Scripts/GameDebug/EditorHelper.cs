#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Main.GameDebug
{
    /// <summary>
    /// 仅在编辑器编译和运行的调试辅助类
    /// </summary>
    public class EditorHelper : MonoBehaviour
    {
        private readonly List<RaycastResult> _raycastResults = new();

        private void Update()
        {
            if (Keyboard.current.ctrlKey.isPressed)
            {
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    Debug.Log("PingUI");
                    PingUIGameObject();
                }
            }
        }

        private void PingUIGameObject()
        {
            var data = new PointerEventData(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };
            EventSystem.current.RaycastAll(data, _raycastResults);
            if (_raycastResults.Count > 0)
            {
                EditorGUIUtility.PingObject(_raycastResults[0].gameObject);
            }
        }
    }
}
#endif