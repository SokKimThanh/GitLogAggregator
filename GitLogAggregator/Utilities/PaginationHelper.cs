using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLogAggregator.Utilities
{
    public class PaginationHelper<T>
    {
        private List<T> _dataSource;
        private int _pageSize;
        private int _currentPage;

        public PaginationHelper(List<T> dataSource, int pageSize)
        {
            _dataSource = dataSource;
            _pageSize = pageSize;
            _currentPage = 1;
        }

        // Lấy dữ liệu cho trang hiện tại
        public List<T> GetCurrentPageData()
        {
            return _dataSource
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();
        }

        // Tính tổng số trang
        public int GetTotalPages()
        {
            return (int)Math.Ceiling((double)_dataSource.Count / _pageSize);
        }

        // Chuyển đến trang tiếp theo
        public void NextPage()
        {
            if (_currentPage < GetTotalPages())
            {
                _currentPage++;
            }
        }

        // Quay lại trang trước
        public void PreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
            }
        }

        // Đặt lại trang hiện tại
        public void SetCurrentPage(int page)
        {
            if (page >= 1 && page <= GetTotalPages())
            {
                _currentPage = page;
            }
        }

        // Lấy trang hiện tại
        public int GetCurrentPage()
        {
            return _currentPage;
        }
    }
}
