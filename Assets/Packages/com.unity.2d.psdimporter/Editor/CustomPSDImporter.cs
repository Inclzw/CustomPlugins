using PDNWrapper;
using UnityEngine;

namespace UnityEditor.U2D.PSD
{
    public partial class PSDImporter
    {
        public enum SpriteImportOrder
        {
            Forward = 1,
            Reverse = 2
        }

        /// <summary>
        /// 根据PSB文件中的参考线，自动设置精灵的中心点
        /// </summary>
        /// <param name="doc"></param>
        private void SetCustomPivotFromGuideLine(Document doc)
        {
            // 先判断参考线是否是两条，并且是横纵各一条，否则不更新设置
            if (doc.GuideLine.AllNum != 2)
            {
                return;
            }

            if (doc.GuideLine.singleGuideList[0].LineType == doc.GuideLine.singleGuideList[1].LineType)
            {
                return;
            }

            // 计算x偏移，数据存储的是像素值，需要转换为比例值
            var pX = 0.5f;
            var pY = 0f;

            foreach (var line in doc.GuideLine.singleGuideList)
            {
                switch (line.LineType)
                {
                    case 0:
                        pX = line.Cor / doc.width;
                        break;
                    case 1:
                        pY = 1 - line.Cor / doc.height; // pivot的Y是从底部向上计算的，所以要用1减去水平参考线的坐标
                        break;
                }
            }

            // 将计算好的pivot值更新到配置项中，之后切分精灵会自动应用
            m_TextureImporterSettings.spriteAlignment = (int)SpriteAlignment.Custom;
            m_TextureImporterSettings.spritePivot = new Vector2(pX, pY);
        }

        private Vector2 GetCustomPivotFromGuideLine(Document doc, PSDLayer psdLayer)
        {
            var pX = 0.5f;
            var pY = 0f;
            var pivot = new Vector2(pX, pY);
            // TODO 优化大量文件导入时循环判断次数
            if (doc.GuideLine.AllNum != 2 ||
                doc.GuideLine.singleGuideList[0].LineType == doc.GuideLine.singleGuideList[1].LineType)
            {
                return pivot;
            }

            foreach (var line in doc.GuideLine.singleGuideList)
            {
                switch (line.LineType)
                {
                    case 0:
                        var offsetX = line.Cor - psdLayer.layerPosition.x;
                        pX = Mathf.Clamp01(offsetX / psdLayer.width);
                        break;
                    case 1:
                        // 参考线由下往上的相对图层底部的Y坐标偏移
                        var offsetY = doc.height - line.Cor - psdLayer.layerPosition.y;
                        pY = Mathf.Clamp01(offsetY / psdLayer.height);
                        break;
                }
            }

            return new Vector2(pX, pY);
        }
    }
}