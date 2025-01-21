using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConfigAuthorDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        // Lấy tất cả mối quan hệ ConfigAuthor
        public List<ConfigAuthorET> GetAll()
        {
            return db.ConfigAuthors.Select(ca => new ConfigAuthorET
            {
                ConfigID = ca.ConfigID,
                AuthorID = ca.AuthorID

            }).ToList();
        }

        // Lấy mối quan hệ ConfigAuthor theo ConfigID và AuthorID
        public ConfigAuthorET GetByID(int configID, int authorID)
        {
            return db.ConfigAuthors.Where(ca => ca.ConfigID == configID && ca.AuthorID == authorID)
                                   .Select(ca => new ConfigAuthorET
                                   {
                                       ConfigID = ca.ConfigID,
                                       AuthorID = ca.AuthorID
                                   }).FirstOrDefault();
        }

        // Thêm mối quan hệ ConfigAuthor
        public void Add(ConfigAuthorET configAuthor)
        {
            var newConfigAuthor = new ConfigAuthor
            {
                ConfigID = configAuthor.ConfigID,
                AuthorID = configAuthor.AuthorID,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            db.ConfigAuthors.InsertOnSubmit(newConfigAuthor);
            db.SubmitChanges();
        }

        // Xóa mối quan hệ ConfigAuthor
        public void Delete(int configID, int authorID)
        {
            var configAuthorToDelete = db.ConfigAuthors
                .SingleOrDefault(ca => ca.ConfigID == configID && ca.AuthorID == authorID);

            if (configAuthorToDelete != null)
            {
                db.ConfigAuthors.DeleteOnSubmit(configAuthorToDelete);
                db.SubmitChanges();
            }
        }

        // Lấy danh sách AuthorID theo ConfigID (phương thức bổ sung)
        public List<int> GetAuthorIDsByConfigID(int configID)
        {
            return db.ConfigAuthors.Where(ca => ca.ConfigID == configID)
                                   .Select(ca => ca.AuthorID)
                                   .ToList();
        }

        // Lấy danh sách ConfigID theo AuthorID (phương thức bổ sung)
        public List<int> GetConfigIDsByAuthorID(int authorID)
        {
            return db.ConfigAuthors.Where(ca => ca.AuthorID == authorID)
                                   .Select(ca => ca.ConfigID)
                                   .ToList();
        }

        public bool Exists(int configID, int authorID)
        {
            return db.ConfigAuthors.Any(ca => ca.ConfigID == configID && ca.AuthorID == authorID);
        }
    }
}
