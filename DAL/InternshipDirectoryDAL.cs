using DAL;
using DocumentFormat.OpenXml.Office2010.Excel;
using ET;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
public class InternshipDirectoryDAL
{
    private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

    public List<InternshipDirectoryET> GetAll()
    {
        try
        {
            var query = from i in db.InternshipDirectories
                        orderby i.CreatedAt descending
                        select new InternshipDirectoryET
                        {
                            ID = i.ID,
                            InternshipWeekFolder = i.InternshipWeekFolder,
                            DateModified = i.DateModified,
                            CreatedAt = i.CreatedAt.Value,
                            UpdatedAt = i.UpdatedAt.Value
                        };

            return query.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Error in GetAll: " + ex.Message);
        }
    }

    public InternshipDirectoryET GetByID(int id)
    {
        try
        {
            var query = from i in db.InternshipDirectories
                        where i.ID == id
                        select new InternshipDirectoryET
                        {
                            ID = i.ID,
                            InternshipWeekFolder = i.InternshipWeekFolder,
                            DateModified = i.DateModified,
                            CreatedAt = i.CreatedAt.Value,
                            UpdatedAt = i.UpdatedAt.Value
                        };

            return query.SingleOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception("Error in GetAuthorByConfig: " + ex.Message);
        }
    }
    public InternshipDirectoryET GetByPath(string internshipWeekFolder)
    {
        try
        {
            var query = from i in db.InternshipDirectories
                        where i.InternshipWeekFolder == internshipWeekFolder
                        select new InternshipDirectoryET
                        {
                            ID = i.ID,
                            InternshipWeekFolder = i.InternshipWeekFolder,
                            DateModified = i.DateModified,
                            CreatedAt = i.CreatedAt.Value,
                            UpdatedAt = i.UpdatedAt.Value
                        };

            return query.SingleOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception("Error in GetByPath: " + ex.Message);
        }
    }
    public void Add(InternshipDirectoryET et)
    {
        try
        {
            var entity = new InternshipDirectory
            {
                InternshipWeekFolder = et.InternshipWeekFolder,
                DateModified = et.DateModified,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            db.InternshipDirectories.InsertOnSubmit(entity);
            db.SubmitChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Error in Add: " + ex.Message);
        }
    }

    public void Update(InternshipDirectoryET et)
    {
        try
        {
            var query = from i in db.InternshipDirectories
                        where i.ID == et.ID
                        select i;

            var entity = query.SingleOrDefault();
            if (entity == null) return;

            entity.InternshipWeekFolder = et.InternshipWeekFolder;
            entity.DateModified = et.DateModified;
            entity.UpdatedAt = DateTime.Now;

            db.SubmitChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Error in Update: " + ex.Message);
        }
    }

    public void Delete(int id)
    {
        try
        {
            var query = from i in db.InternshipDirectories
                        where i.ID == id
                        select i;

            var entity = query.SingleOrDefault();
            if (entity == null) return;

            db.InternshipDirectories.DeleteOnSubmit(entity);
            db.SubmitChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Error in Delete: " + ex.Message);
        }
    }
    public int GetLatestInternshipDirectoryId()
    {
        var query = from directory in db.InternshipDirectories
                    orderby directory.DateModified descending
                    select directory.ID;
        return query.FirstOrDefault();
    }
}
