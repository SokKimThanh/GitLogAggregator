using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ChatbotSummaryDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        // Thêm phản hồi mới từ chatbot
        public void Create(ChatbotSummary chatbotSummary)
        {
            db.ChatbotSummaries.InsertOnSubmit(chatbotSummary);
            db.SubmitChanges();
        }

        // Xóa phản hồi từ chatbot theo ID
        public void Delete(int chatbotSummaryId)
        {
            var summary = db.ChatbotSummaries.SingleOrDefault(s => s.ID == chatbotSummaryId);
            if (summary != null)
            {
                db.ChatbotSummaries.DeleteOnSubmit(summary);
                db.SubmitChanges();
            }
        }

        // Cập nhật thông tin phản hồi từ chatbot
        public void Update(ChatbotSummary chatbotSummary)
        {
            var existingSummary = db.ChatbotSummaries.SingleOrDefault(s => s.ID == chatbotSummary.ID);
            if (existingSummary != null)
            {
                existingSummary.CommitId = chatbotSummary.CommitId;
                existingSummary.Attendance = chatbotSummary.Attendance;
                existingSummary.AssignedTasks = chatbotSummary.AssignedTasks;
                existingSummary.ContentResults = chatbotSummary.ContentResults;
                existingSummary.SupervisorComments = chatbotSummary.SupervisorComments;
                existingSummary.Notes = chatbotSummary.Notes;
                existingSummary.CreatedAt = chatbotSummary.CreatedAt;
                existingSummary.UpdatedAt = chatbotSummary.UpdatedAt;
                db.SubmitChanges();
            }
        }

        // Lấy tất cả phản hồi từ chatbot
        public List<ChatbotSummary> GetAll()
        {
            return db.ChatbotSummaries.Select(s => new ChatbotSummary
            {
                ID = s.ID,
                CommitId = s.CommitId,
                Attendance = s.Attendance,
                AssignedTasks = s.AssignedTasks,
                ContentResults = s.ContentResults,
                SupervisorComments = s.SupervisorComments,
                Notes = s.Notes,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();
        }

        // Lấy phản hồi từ chatbot theo ID
        public ChatbotSummary GetById(int chatbotSummaryId)
        {
            var summary = db.ChatbotSummaries.SingleOrDefault(s => s.ID == chatbotSummaryId);
            if (summary != null)
            {
                return new ChatbotSummary
                {
                    ID = summary.ID,
                    CommitId = summary.CommitId,
                    Attendance = summary.Attendance,
                    AssignedTasks = summary.AssignedTasks,
                    ContentResults = summary.ContentResults,
                    SupervisorComments = summary.SupervisorComments,
                    Notes = summary.Notes,
                    CreatedAt = summary.CreatedAt,
                    UpdatedAt = summary.UpdatedAt
                };
            }
            return null;
        }
    }

}
