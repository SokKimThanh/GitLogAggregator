using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

public class InternshipDirectoryDAL
{
    private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

    // liet ke
    public List<InternshipDirectoryET> GetAllInternshipDirectories()
    {
        var query = (from directory in db.InternshipDirectories
                     orderby directory.DateModified descending
                     select new InternshipDirectoryET
                     {
                         Id = directory.Id,
                         InternshipWeekFolder = directory.InternshipWeekFolder,
                         DateModified = (DateTime)directory.DateModified
                     }).ToList();
        return query;
    }
    // them
    public void InsertInternshipDirectory(string folderPath)
    {
        InternshipDirectory directory = new InternshipDirectory
        {
            InternshipWeekFolder = folderPath,
            DateModified = DateTime.Now
        };
        db.InternshipDirectories.InsertOnSubmit(directory);
        db.SubmitChanges();
    }

    public int GetLatestInternshipDirectoryId()
    {
        var query = from directory in db.InternshipDirectories
                    orderby directory.DateModified descending
                    select directory.Id;
        return query.FirstOrDefault();
    }
}
