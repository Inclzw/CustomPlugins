/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateInventorySystem.UI.Item.DragAndDrop.DropActions
{
    using System;

    /// <summary>
    /// The ITem View Drop Move Index Action.
    /// </summary>
    [Serializable]
    public class ItemViewDropMoveIndexAction : ItemViewDropAction
    {
        /// <summary>
        /// Drop Action.
        /// </summary>
        /// <param name="itemViewDropHandler">The Item View Drop Handler.</param>
        public override void Drop(ItemViewDropHandler itemViewDropHandler)
        {
            itemViewDropHandler.SourceContainer.MoveItem(itemViewDropHandler.StreamData.SourceIndex, itemViewDropHandler.StreamData.DestinationIndex);
            
            var item = itemViewDropHandler.SourceItemViewSlot.ItemInfo.Item;
            var previewItemShape = item.PreviewItemShape;
            if (previewItemShape == null || previewItemShape.ID != item.ID)
            {
                return;
            }

            item.ItemShape.Clone(previewItemShape);
            item.RemovePreviewItemShape();
            itemViewDropHandler.SourceItemViewSlot.ItemView.Refresh();
        }
    }
}