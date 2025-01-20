using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CommitPeriodDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        public List<CommitPeriodET> GetAll()
        {
            try
            {
                var query = from c in db.CommitPeriods
                            orderby c.CreatedAt descending
                            select new CommitPeriodET
                            {
                                PeriodID = c.PeriodID,
                                PeriodName = c.PeriodName,
                                PeriodDuration = c.PeriodDuration,
                                PeriodStartDate = c.PeriodStartDate,
                                PeriodEndDate = c.PeriodEndDate,
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

        public CommitPeriodET GetByID(int id)
        {
            try
            {
                var query = from c in db.CommitPeriods
                            where c.PeriodID == id
                            select new CommitPeriodET
                            {
                                PeriodID = c.PeriodID,
                                PeriodName = c.PeriodName,
                                PeriodDuration = c.PeriodDuration,
                                PeriodStartDate = c.PeriodStartDate,
                                PeriodEndDate = c.PeriodEndDate,
                                CreatedAt = c.CreatedAt.Value,
                                UpdatedAt = c.UpdatedAt.Value
                            };

                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAuthorByConfig: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin của bản ghi cuối cùng được thêm vào
        /// </summary> 
        public CommitPeriodET GetLastInserted()
        {
            try
            {
                var query = from c in db.CommitPeriods
                            orderby c.CreatedAt descending
                            select new CommitPeriodET
                            {
                                PeriodID = c.PeriodID,
                                PeriodName = c.PeriodName,
                                PeriodDuration = c.PeriodDuration,
                                PeriodStartDate = c.PeriodStartDate,
                                PeriodEndDate = c.PeriodEndDate,
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

        public void Add(CommitPeriodET et)
        {
            try
            {
                var entity = new CommitPeriod
                {
                    PeriodName = et.PeriodName,
                    PeriodDuration = et.PeriodDuration,
                    PeriodStartDate = et.PeriodStartDate,
                    PeriodEndDate = et.PeriodEndDate,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                db.CommitPeriods.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Add: " + ex.Message);
            }
        }

        public void Update(CommitPeriodET et)
        {
            try
            {
                var query = from c in db.CommitPeriods
                            where c.PeriodID == et.PeriodID
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                entity.PeriodName = et.PeriodName;
                entity.PeriodDuration = et.PeriodDuration;
                entity.PeriodStartDate = et.PeriodStartDate;
                entity.PeriodEndDate = et.PeriodEndDate;
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
                var query = from c in db.CommitPeriods
                            where c.PeriodID == id
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                db.CommitPeriods.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Delete: " + ex.Message);
            }
        }
    }
}
