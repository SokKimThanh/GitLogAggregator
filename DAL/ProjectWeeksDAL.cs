using DAL;
using DocumentFormat.OpenXml.Wordprocessing;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProjectWeekDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();
        public List<WeekET> GetAll()
        {
            try
            {
                var query = from p in db.Weeks
                            orderby p.WeekName ascending
                            select new WeekET
                            {
                                WeekId = p.WeekId,
                                WeekName = p.WeekName,
                                WeekStartDate = p.WeekStartDate.Value,
                                WeekEndDate = p.WeekEndDate.Value,
                                CreatedAt = p.CreatedAt.Value,
                                UpdatedAt = p.UpdatedAt.Value
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAll: " + ex.Message);
            }
        }

        public WeekET GetByID(int id)
        {
            try
            {
                var query = from p in db.Weeks
                            where p.WeekId == id
                            select new WeekET
                            {
                                WeekId = p.WeekId,
                                WeekName = p.WeekName,
                                WeekStartDate = p.WeekStartDate.Value,
                                WeekEndDate = p.WeekEndDate.Value,
                                CreatedAt = p.CreatedAt.Value,
                                UpdatedAt = p.UpdatedAt.Value
                            };

                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAuthorByConfig: " + ex.Message);
            }
        }

        public void Add(WeekET et)
        {
            try
            {
                var entity = new Week
                {
                    WeekName = et.WeekName,
                    WeekStartDate = et.WeekStartDate,
                    WeekEndDate = et.WeekEndDate,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                db.Weeks.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Add: " + ex.Message);
            }
        }
        /// <summary>
        /// Lấy ra bản ghi cuối cùng
        /// </summary> 
        public WeekET GetLastInserted()
        {

            var projectweek = db.Weeks
                          .OrderByDescending(pw => pw.WeekId) // Giả sử Id là khóa chính, tự tăng
                          .FirstOrDefault();

            if (projectweek != null)
            {
                return new WeekET()
                {
                    WeekId = projectweek.WeekId,
                    WeekName = projectweek.WeekName,
                    WeekStartDate = projectweek.WeekStartDate.Value,
                    WeekEndDate = projectweek.WeekEndDate.Value,
                    CreatedAt = projectweek.CreatedAt.Value,
                    UpdatedAt = projectweek.UpdatedAt.Value
                };
            }
            return null;
        }

        public void Update(WeekET et)
        {
            try
            {
                var query = from p in db.Weeks
                            where p.WeekId == et.WeekId
                            select p;

                var entity = query.SingleOrDefault();
                if (entity == null) return;
                entity.WeekName = et.WeekName;
                entity.WeekStartDate = et.WeekStartDate;
                entity.WeekEndDate = et.WeekEndDate;
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
                var query = from p in db.Weeks
                            where p.WeekId == id
                            select p;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                db.Weeks.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Delete: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra xem có dữ liệu tuần thực tập trong database hay không
        /// </summary> 
        public bool IsInternshipWeekListCreated()
        {
            return db.Weeks.Any(); // Trả về true nếu có ít nhất một tuần thực tập
        }
    }
}