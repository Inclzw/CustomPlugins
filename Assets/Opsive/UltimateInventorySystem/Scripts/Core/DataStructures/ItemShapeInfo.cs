using System;
using Opsive.UltimateInventorySystem.UI.Item;
using UnityEngine;

namespace Opsive.UltimateInventorySystem.Core.DataStructures
{
    [Serializable]
    public class ItemShapeInfo
    {
        [NonSerialized] protected ItemShape m_ItemShape;
        [Tooltip("The item shape size in units.")]
        [SerializeField] protected Vector2Int m_Size = Vector2Int.one;
        [Tooltip("Use a custom shape? If not the shape will be rectangle.")]
        [SerializeField] protected bool m_UseCustomShape;
        [Tooltip("An array of boolean used to create a custom shape.")]
        [SerializeField] internal bool[] m_CustomShape;
        [Tooltip("The anchor position of the shape.")]
        [SerializeField] protected Vector2Int m_Anchor;
        [SerializeField] protected ItemRotation m_ItemRotation;
        protected uint m_ItemId;
        
        public Vector2Int Size => m_Size;
        public bool UseCustomShape => m_ItemShape.UseCustomShape;
        public int Rows => m_Size.y;
        public int Cols => m_Size.x;
        public int Count => m_Size.x * m_Size.y;
        public Vector2Int Anchor => m_Anchor;
        public int AnchorIndex => m_Anchor.y * m_Size.x + m_Anchor.x;
        public ItemRotation ItemRotation => m_ItemRotation;
        public bool Initialized => m_ItemId != 0;
        public uint ID => m_ItemId;

        public ItemShapeInfo()
        {
        }
        
        public ItemShapeInfo(ItemShape itemShape, uint id)
        {
            m_ItemShape = itemShape;
            m_Size = itemShape.Size;
            m_UseCustomShape = itemShape.UseCustomShape;
            m_CustomShape = itemShape.m_CustomShape;
            m_Anchor = itemShape.Anchor;
            m_ItemId = id;
        }
        
        public bool IsAnchor(int x, int y)
        {
            return Anchor.x == x && Anchor.y == y;
        }

        /// <summary>
        /// Starts at the top left corner and starts horizontally.
        /// </summary>
        /// <param name="x">The horizontal index.</param>
        /// <param name="y">The vertical index.</param>
        /// <returns></returns>
        public bool IsIndexOccupied(int x, int y)
        {
            if (Size.x < x || Size.y < y) { return false; }

            if (UseCustomShape == false)
            {
                return true;
            }
            
            return m_CustomShape[y * Cols + x];
        }

        public void RotateShape(bool isClockwise)
        {
            if (!UseCustomShape)
            {
                RotateGenericShape(isClockwise);
                SetRotationState(isClockwise);
                return;
            }
            
            RotateCustomShape(isClockwise);
            SetRotationState(isClockwise);
        }

        public void FlipShape()
        {
            var rotatedShape = (bool[])m_CustomShape.Clone();
            Array.Reverse(rotatedShape);
            m_CustomShape = rotatedShape;
            m_Anchor = new Vector2Int(Cols - 1, Rows - 1);
        }

        protected void RotateGenericShape(bool isClockwise)
        {
            var x = Size.y;
            var y = Size.x;
            if (AnchorIndex != 0)
            {
                SetRotatedAnchor(isClockwise);
            }

            m_Size = new Vector2Int(x, y);
        }

        private void SetRotatedAnchor(bool isClockwise)
        {
            int r = AnchorIndex / Cols;
            int c = AnchorIndex % Cols;

            int newRow, newCol;

            if (isClockwise)
            {
                newRow = c;
                newCol = Rows - 1 - r;
            }
            else
            {
                newRow = Cols - 1 - c;
                newCol = r;
            }

            m_Anchor = new Vector2Int(newCol, newRow);
        }
        
        protected void RotateCustomShape(bool isClockwise)
        {
            var rotatedShape = new bool[m_CustomShape.Length];
            var rows = Rows;
            var cols = Cols;
            var rotatedAnchor = new Vector2Int();
            if (isClockwise)
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        int originalIndex = r * cols + c;               
                        int newRow = c;                                 
                        int newCol = rows - r - 1;                      
                        int newIndex = newRow * rows + newCol;          
                        rotatedShape[newIndex] = m_CustomShape[originalIndex];
                        if (originalIndex == AnchorIndex)
                        {
                            rotatedAnchor = new Vector2Int(newCol, newRow);
                        }
                    }
                }
            }
            else
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        int originalIndex = r * cols + c;
                        int newRow = cols - c - 1;
                        int newCol = r;
                        int newIndex = newRow * rows + newCol;
                        rotatedShape[newIndex] = m_CustomShape[originalIndex];
                        if (originalIndex == AnchorIndex)
                        {
                            rotatedAnchor = new Vector2Int(newCol, newRow);
                        }
                    }
                }
            }
            
            m_Size = new Vector2Int(m_Size.y, m_Size.x);
            m_CustomShape = rotatedShape;
            m_Anchor = rotatedAnchor;
        }

        private void SetRotationState(bool isClockwise)
        {
            int currentAngle = (int)ItemRotation;
            int rotationStep = isClockwise ? 90 : -90;
            int newAngle = currentAngle + rotationStep;
            if (newAngle > 180) newAngle = -90;
            if (newAngle < -90) newAngle = 180;
            m_ItemRotation = (ItemRotation)newAngle;
        }
        
        /// <summary>
        /// Resize the 2D array.
        /// </summary>
        /// <param name="original">The original array.</param>
        /// <param name="previousSize">The previous size.</param>
        /// <param name="newSize">The new size.</param>
        /// <typeparam name="T">The element type.</typeparam>
        /// <returns>The new resized array.</returns>
        protected T[] ResizeArray<T>(T[] original, Vector2Int previousSize, Vector2Int newSize)
        {
            var newArray = new T[newSize.x * newSize.y];
            int minRows = Math.Min(newSize.y, previousSize.y);
            int minCols = Math.Min(newSize.x, previousSize.x);
            for (int y = 0; y < minRows; y++) {
                for (int x = 0; x < minCols; x++) {

                    newArray[y * newSize.x + x] = original[y * previousSize.x + x];
                }
            }

            return newArray;
        }

        /// <summary>
        /// To string.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return string.Format(UseCustomShape ? "[{0},{1}] Custom Shape" : "[{0},{1}]", Size.x, Size.y);
        }

        public void Clone(ItemShapeInfo itemShape)
        {
            if (itemShape == null)
            {
                return;
            }
            
            m_ItemShape = itemShape.m_ItemShape;
            m_Size = itemShape.m_Size;
            m_UseCustomShape = itemShape.m_UseCustomShape;
            m_CustomShape = itemShape.m_CustomShape;
            m_Anchor = itemShape.m_Anchor;
            m_ItemId = itemShape.m_ItemId;
            m_ItemRotation = itemShape.m_ItemRotation;
        }

        public void Reset()
        {
            m_ItemId = 0;
        }
    }
}