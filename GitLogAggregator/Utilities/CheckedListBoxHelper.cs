using System;
using System.Windows.Forms;

// Enum đại diện cho các mục trong CheckedListBox
public enum SearchCriteria
{
    chkEnablePagination,    // "Bật phân trang"
    chkSearchAllWeeks,      // "Tìm kiếm tất cả tuần"
    chkSearchAllAuthors,    // "Tìm kiếm tất cả tác giả"
    chkIsSimpleView
}

// Class helper để kiểm tra trạng thái của các mục
public static class CheckedListBoxHelper
{
    // Kiểm tra trạng thái check của một mục
    public static bool IsItemChecked(this CheckedListBox clb, SearchCriteria criteria)
    {
        int index = (int)criteria;
        ValidateIndex(clb, index);
        return clb.GetItemChecked(index);
    }

    // Đặt trạng thái check cho một mục
    public static void SetItemChecked(this CheckedListBox clb, SearchCriteria criteria, bool isChecked)
    {
        int index = (int)criteria;
        ValidateIndex(clb, index);
        clb.SetItemChecked(index, isChecked);
    }

    // Thêm hoặc cập nhật một mục dựa trên enum
    public static void AddItem(this CheckedListBox clb, SearchCriteria criteria, string displayText, bool isChecked = false)
    {
        int index = (int)criteria;
        if (index < clb.Items.Count)
        {
            // Cập nhật mục đã tồn tại
            clb.Items[index] = displayText;
            clb.SetItemChecked(index, isChecked);
        }
        else
        {
            // Thêm mục mới
            clb.Items.Add(displayText, isChecked);
        }
    }

    // Kiểm tra index hợp lệ
    private static void ValidateIndex(CheckedListBox clb, int index)
    {
        if (index < 0 || index >= clb.Items.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Chỉ số không hợp lệ");
    }
}