// Last modified: 03.08.2021 8:45 PM
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GitLogAggregator.Utilities
{
    public enum BtnIconCrudEnum
    {
        AddIcon,
        DeleteIcon,
        EditIcon,
        SearchIcon
    }

    public enum BtnIconActionMDIEnum
    {
        CloseAllIcon,
        SortIcon,
    }

    public static class ButtonImageMap
    {
        // Từ điển cho Crud icons
        public static readonly Dictionary<BtnIconCrudEnum, string> CrudImageKeys = new Dictionary<BtnIconCrudEnum, string>
        {
            { BtnIconCrudEnum.AddIcon, "add_icon.png" },
            { BtnIconCrudEnum.DeleteIcon, "delete_icon.png" },
            { BtnIconCrudEnum.EditIcon, "edit_icon.png" },
            { BtnIconCrudEnum.SearchIcon, "search_icon.png" }
        };

        // Từ điển mới cho MDI Action icons
        public static readonly Dictionary<BtnIconActionMDIEnum, string> ActionMDIImageKeys = new Dictionary<BtnIconActionMDIEnum, string>
        {
            { BtnIconActionMDIEnum.CloseAllIcon, "close_all_icon.png" },
            { BtnIconActionMDIEnum.SortIcon, "sort_icon.png" }
        };

        /// <summary>
        /// Lấy hình ảnh từ ImageList dựa trên enum icon
        /// </summary>
        /// <typeparam name="T">Loại enum (BtnIconCrudEnum hoặc BtnIconActionMDIEnum)</typeparam>
        /// <param name="imageList">ImageList chứa hình ảnh</param>
        /// <param name="iconEnum">Giá trị enum xác định icon cần lấy</param>
        /// <returns>Hình ảnh tương ứng hoặc exception nếu không tìm thấy</returns>
        public static Image GetButtonImage<T>(ImageList imageList, T iconEnum) where T : Enum
        {
            string key;

            // Xác định từ điển dựa trên loại enum
            if (typeof(T) == typeof(BtnIconCrudEnum))
            {
                key = CrudImageKeys[(BtnIconCrudEnum)(object)iconEnum];
            }
            else if (typeof(T) == typeof(BtnIconActionMDIEnum))
            {
                key = ActionMDIImageKeys[(BtnIconActionMDIEnum)(object)iconEnum];
            }
            else
            {
                throw new ArgumentException("Loại enum không được hỗ trợ");
            }

            // Lấy hình ảnh từ ImageList
            var image = imageList.Images[key];
            return image ?? throw new KeyNotFoundException($"Không tìm thấy hình ảnh với key '{key}' trong ImageList");
        }
    }
}