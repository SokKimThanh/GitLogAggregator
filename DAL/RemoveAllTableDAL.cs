using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RemoveDAL
    {
        public void ClearAllTables()
        {
            try
            {
                // Tạo một DataContext (thay thế bằng DataContext của bạn)
                using (var db = new GitLogAggregatorDataContext())
                {
                    // Xóa dữ liệu từ bảng ChatbotSummary
                    db.ChatbotSummaries.DeleteAllOnSubmit(db.ChatbotSummaries);
                    db.SubmitChanges();
                    Console.Write("Đã xóa dữ liệu từ bảng ChatbotSummary.\n");

                    // Xóa dữ liệu từ bảng Commits
                    db.Commits.DeleteAllOnSubmit(db.Commits);
                    db.SubmitChanges();
                    Console.Write("Đã xóa dữ liệu từ bảng Commits.\n");

                    // Xóa dữ liệu từ bảng ProjectWeeks
                    db.ProjectWeeks.DeleteAllOnSubmit(db.ProjectWeeks);
                    db.SubmitChanges();
                    Console.Write("Đã xóa dữ liệu từ bảng ProjectWeeks.\n");

                    // Xóa dữ liệu từ bảng ConfigFiles
                    db.ConfigFiles.DeleteAllOnSubmit(db.ConfigFiles);
                    db.SubmitChanges();
                    Console.Write("Đã xóa dữ liệu từ bảng ConfigFiles.\n");

                    // Xóa dữ liệu từ bảng InternshipDirectories
                    db.InternshipDirectories.DeleteAllOnSubmit(db.InternshipDirectories);
                    db.SubmitChanges();
                    Console.Write("Đã xóa dữ liệu từ bảng InternshipDirectories.\n");
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Lỗi khi xóa dữ liệu: {ex.Message}\n");
            }
        }
    }
}
