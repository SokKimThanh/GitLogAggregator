using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLogAggregator.Utilities
{
    public enum ButtonImage
    {
        AddIcon,
        DeleteIcon,
        EditIcon
    }

    public static class ButtonImageMap
    {
        public static readonly Dictionary<ButtonImage, string> ImageKeys = new Dictionary<ButtonImage, string>
        {
            { ButtonImage.AddIcon, "add_icon.png" },
            { ButtonImage.DeleteIcon, "delete_icon.png" },
            { ButtonImage.EditIcon, "edit_icon.png" }
        };
    }
}
