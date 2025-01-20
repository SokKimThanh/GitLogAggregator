﻿using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AuthorDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        // Lấy tất cả tác giả
        public List<AuthorET> GetAll()
        {
            return db.Authors.Select(a => new AuthorET
            {
                AuthorID = a.AuthorID,
                AuthorName = a.AuthorName,
                AuthorEmail = a.AuthorEmail,
                CreatedAt = a.CreatedAt.Value,
                UpdatedAt = a.UpdatedAt.Value
            }).ToList();
        }

        // Lấy tác giả theo ID
        public AuthorET GetByID(int authorID)
        {
            return db.Authors.Where(a => a.AuthorID == authorID)
                             .Select(a => new AuthorET
                             {
                                 AuthorID = a.AuthorID,
                                 AuthorName = a.AuthorName,
                                 AuthorEmail = a.AuthorEmail,
                                 CreatedAt = a.CreatedAt.Value,
                                 UpdatedAt = a.UpdatedAt.Value
                             }).FirstOrDefault();
        }

        // Lấy tác giả theo tên 
        public AuthorET GetByName(string authorName)
        {
            return db.Authors.Where(a => a.AuthorName == authorName)
                             .Select(a => new AuthorET
                             {
                                 AuthorID = a.AuthorID,
                                 AuthorName = a.AuthorName,
                                 CreatedAt = a.CreatedAt.Value,
                                 UpdatedAt = a.UpdatedAt.Value
                             }).FirstOrDefault();
        }
        // Lấy tác giả theo email  
        public AuthorET GetByEmail(string authorEmail)
        {
            return db.Authors.Where(a => a.AuthorEmail == authorEmail)
                             .Select(a => new AuthorET
                             {
                                 AuthorID = a.AuthorID,
                                 AuthorName = a.AuthorName,
                                 CreatedAt = a.CreatedAt.Value,
                                 UpdatedAt = a.UpdatedAt.Value
                             }).FirstOrDefault();
        }
        // Thêm tác giả mới
        public void Add(AuthorET author)
        {
            var newAuthor = new Author
            {
                AuthorName = author.AuthorName,
                AuthorEmail = author.AuthorEmail,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            db.Authors.InsertOnSubmit(newAuthor);
            db.SubmitChanges();
        }

        // Cập nhật thông tin tác giả
        public void Update(AuthorET author)
        {
            var existingAuthor = db.Authors.SingleOrDefault(a => a.AuthorID == author.AuthorID);
            if (existingAuthor != null)
            {
                existingAuthor.AuthorName = author.AuthorName;
                existingAuthor.AuthorEmail = author.AuthorEmail;
                existingAuthor.UpdatedAt = DateTime.Now;
                db.SubmitChanges();
            }
        }

        // Xóa tác giả
        public void Delete(int authorID)
        {
            var authorToDelete = db.Authors.SingleOrDefault(a => a.AuthorID == authorID);
            if (authorToDelete != null)
            {
                db.Authors.DeleteOnSubmit(authorToDelete);
                db.SubmitChanges();
            }
        }

    }
}
