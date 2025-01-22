﻿using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class SearchBUS
    {
        private readonly SearchDAL _searchDAL = new SearchDAL();

        public List<SearchResult> SearchCommits(string keyword, int? projectWeekId, bool searchAllWeeks, bool searchAllAuthors, int? authorId = null)
        {
            return _searchDAL.SearchCommits(keyword, projectWeekId, searchAllWeeks, searchAllAuthors, authorId = null);
        }
        public DateTime? GetFirstCommitDateByProject(int projectId)
        {
            try
            {
                return _searchDAL.GetFirstCommitDateByProject(projectId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Đã xảy ra lỗi khi lấy ngày commit đầu tiên.", ex);
            }
        }
    }
}
