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
                    // Xóa dữ liệu từ bảng ChatbotSummaryET
                    db.ChatbotSummaries.DeleteAllOnSubmit(db.ChatbotSummaries);
                    db.SubmitChanges();
                    Console.Write("Đã xóa dữ liệu từ bảng ChatbotSummaryET.\n");

                    // Xóa dữ liệu từ các bảng con (có khóa ngoại)
                    db.CommitGroupMembers.DeleteAllOnSubmit(db.CommitGroupMembers); // Xóa dữ liệu trong bảng con CommitGroupMembers
                    db.SubmitChanges();
                    Console.Write("Đã xóa dữ liệu từ bảng CommitGroupMembers.\n");

                    db.Commits.DeleteAllOnSubmit(db.Commits); // Xóa dữ liệu trong bảng con Commits 
                    db.SubmitChanges();
                    Console.Write("Đã xóa dữ liệu từ bảng Commits.\n");

                    // Sau khi xóa các bảng con, xóa dữ liệu trong bảng cha CommitGroups
                    db.CommitGroups.DeleteAllOnSubmit(db.CommitGroups);
                    // Lưu thay đổi để xóa dữ liệu trong bảng chính
                    db.SubmitChanges();
                    Console.Write("Đã xóa dữ liệu từ bảng CommitGroups.\n");

                    // Xóa dữ liệu từ bảng ProjectWeeks
                    db.ProjectWeeks.DeleteAllOnSubmit(db.ProjectWeeks);
                    db.SubmitChanges();
                    Console.Write("Đã xóa dữ liệu từ bảng ProjectWeeks.\n"); 

                    // Xóa dữ liệu từ bảng ConfigFiles
                    //db.ConfigFiles.DeleteAllOnSubmit(db.ConfigFiles);
                    //db.SubmitChanges();
                    //Console.Write("Đã xóa dữ liệu từ bảng ConfigFiles.\n");

                    // Xóa dữ liệu từ bảng InternshipDirectories
                    //db.InternshipDirectories.DeleteAllOnSubmit(db.InternshipDirectories);
                    //db.SubmitChanges();
                    //Console.Write("Đã xóa dữ liệu từ bảng InternshipDirectories.\n");
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Lỗi khi xóa dữ liệu: {ex.Message}\n");
            }
        }
    }
}
