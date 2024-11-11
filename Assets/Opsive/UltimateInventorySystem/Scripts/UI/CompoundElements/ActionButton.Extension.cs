using System;
using Opsive.UltimateInventorySystem.UI.Item;
using UnityEngine;
using RotateDirection = Opsive.UltimateInventorySystem.UI.Item.RotateDirection;

namespace Opsive.UltimateInventorySystem.UI.CompoundElements
{
    public partial class ActionButton
    {
        public event Action<ItemViewSlotRotateEventData> OnRotateE;
        private static ItemViewSlotRotateEventData m_RotateEventData = new();

        private void Update()
        {
            if (!m_Selected)
            {
                return;
            }

            //TODO New Input System
            if (UnityEngine.Input.GetKeyDown(KeyCode.Q) || UnityEngine.Input.GetButtonDown("Previous"))
            {
                m_RotateEventData.RotateDir = RotateDirection.CounterClockwise;
                OnRotate(m_RotateEventData);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetButtonDown("Next"))
            {
                m_RotateEventData.RotateDir = RotateDirection.Clockwise;
                OnRotate(m_RotateEventData);
            }
        }

        private void OnRotate(ItemViewSlotRotateEventData eventData)
        {
            OnRotateE?.Invoke(eventData);
        }
    }
}