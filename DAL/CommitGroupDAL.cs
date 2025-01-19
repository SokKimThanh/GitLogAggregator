using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CommitGroupDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        public List<CommitGroupET> GetAll()
        {
            try
            {
                var query = from c in db.CommitGroups
                            orderby c.CreatedAt descending
                            select new CommitGroupET
                            {
                                CommitGroupId = c.CommitGroupId,
                                GroupName = c.GroupName,
                                TimeRange = c.TimeRange,
                                WeekStartDate = c.StartDate,
                                WeekEndDate = c.EndDate,
                                CreatedAt = c.CreatedAt.Value,
                                UpdatedAt = c.UpdatedAt.Value
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAll: " + ex.Message);
            }
        }

        public CommitGroupET GetByID(int id)
        {
            try
            {
                var query = from c in db.CommitGroups
                            where c.CommitGroupId == id
                            select new CommitGroupET
                            {
                                CommitGroupId = c.CommitGroupId,
                                GroupName = c.GroupName,
                                TimeRange = c.TimeRange,
                                WeekStartDate = c.StartDate,
                                WeekEndDate = c.EndDate,
                                CreatedAt = c.CreatedAt.Value,
                                UpdatedAt = c.UpdatedAt.Value
                            };

                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetByID: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin của bản ghi cuối cùng được thêm vào
        /// </summary> 
        public CommitGroupET GetLastInserted()
        {
            try
            {
                var query = from c in db.CommitGroups
                            orderby c.CreatedAt descending
                            select new CommitGroupET
                            {
                                CommitGroupId = c.CommitGroupId,
                                GroupName = c.GroupName,
                                TimeRange = c.TimeRange,
                                WeekStartDate = c.StartDate,
                                WeekEndDate = c.EndDate,
                                CreatedAt = c.CreatedAt.Value,
                                UpdatedAt = c.UpdatedAt.Value
                            };
                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetLastInserted: " + ex.Message);
            }
        }

        public void Add(CommitGroupET et)
        {
            try
            {
                var entity = new CommitGroup
                {
                    GroupName = et.GroupName,
                    TimeRange = et.TimeRange,
                    StartDate = et.WeekStartDate,
                    EndDate = et.WeekEndDate,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                db.CommitGroups.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Add: " + ex.Message);
            }
        }

        public void Update(CommitGroupET et)
        {
            try
            {
                var query = from c in db.CommitGroups
                            where c.CommitGroupId == et.CommitGroupId
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                entity.GroupName = et.GroupName;
                entity.TimeRange = et.TimeRange;
                entity.StartDate = et.WeekStartDate;
                entity.EndDate = et.WeekEndDate;
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
                var query = from c in db.CommitGroups
                            where c.CommitGroupId == id
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                db.CommitGroups.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Delete: " + ex.Message);
            }
        }
    }
}
