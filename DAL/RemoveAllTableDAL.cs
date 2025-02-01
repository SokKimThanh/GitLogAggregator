using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RemoveDAL
    {
        GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();
        public void ClearAllProjects()
        {
            try
            {
                //Xóa dữ liệu từ bảng ConfigFiles
                db.ConfigFiles.DeleteAllOnSubmit(db.ConfigFiles);
                db.SubmitChanges();
                Console.Write("Đã xóa dữ liệu từ bảng ConfigFiles.\n");
            }
            catch (Exception ex)
            {
                Console.Write($"Lỗi khi xóa dữ liệu: {ex.Message}\n");
            }
        }

        public void ClearAllTables()
        {
            try
            {
                var tableNames = new List<string>
                {
                    "CommitSummary",
                    "Commits",
                    "Summary",
                    "Authors",
                    "ConfigFiles",
                    "CommitPeriods",
                    "Weeks",
                    "InternshipDirectories"
                };

                // Vô hiệu hóa ràng buộc khóa ngoại
                foreach (var tableName in tableNames)
                {
                    db.ExecuteCommand($"ALTER TABLE {tableName} NOCHECK CONSTRAINT ALL;");
                }

                // Xóa dữ liệu từ các bảng
                foreach (var tableName in tableNames)
                {
                    db.ExecuteCommand($"DELETE FROM {tableName};");
                    Console.WriteLine($"Đã xóa dữ liệu từ bảng {tableName}.");
                }

                // Kích hoạt lại ràng buộc khóa ngoại
                foreach (var tableName in tableNames)
                {
                    db.ExecuteCommand($"ALTER TABLE {tableName} CHECK CONSTRAINT ALL;");
                }

                Console.WriteLine("Đã xóa dữ liệu từ tất cả các bảng.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa dữ liệu: {ex.Message}");
            }
        }
    }
}
