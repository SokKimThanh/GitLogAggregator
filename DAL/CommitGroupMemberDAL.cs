using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CommitGroupMemberDAL
    {
        private GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        public List<CommitGroupMemberET> GetAll()
        {
            try
            {
                var query = from c in db.CommitGroupMembers
                            orderby c.PeriodID descending
                            select new CommitGroupMemberET
                            {
                                PeriodID = c.PeriodID,
                                CommitId = c.CommitId,
                                AddedAt = c.AddedAt.Value
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAll: " + ex.Message);
            }
        }

        public CommitGroupMemberET GetByID(int groupId, int commitId)
        {
            try
            {
                var query = from c in db.CommitGroupMembers
                            where c.PeriodID == groupId && c.CommitId == commitId
                            select new CommitGroupMemberET
                            {
                                PeriodID = c.PeriodID,
                                CommitId = c.CommitId,
                                AddedAt = c.AddedAt.Value
                            };

                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAuthorByConfig: " + ex.Message);
            }
        }

        public void Add(CommitGroupMemberET et)
        {
            try
            {
                var entity = new CommitGroupMember
                {
                    PeriodID = et.PeriodID,
                    CommitId = et.CommitId,
                    AddedAt = et.AddedAt
                };
                db.CommitGroupMembers.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Add: " + ex.Message);
            }
        }

        public void Update(CommitGroupMemberET et)
        {
            try
            {
                var query = from c in db.CommitGroupMembers
                            where c.PeriodID == et.PeriodID && c.CommitId == et.CommitId
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                entity.PeriodID = et.PeriodID;
                entity.CommitId = et.CommitId;
                entity.AddedAt = et.AddedAt;

                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Update: " + ex.Message);
            }
        }

        public void Delete(int groupId, int commitId)
        {
            try
            {
                var query = from c in db.CommitGroupMembers
                            where c.PeriodID == groupId && c.CommitId == commitId
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                db.CommitGroupMembers.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Delete: " + ex.Message);
            }
        }
    }
}