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
                            orderby c.GroupId descending
                            select new CommitGroupMemberET
                            {
                                CommitGroupId = c.GroupId,
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
                            where c.GroupId == groupId && c.CommitId == commitId
                            select new CommitGroupMemberET
                            {
                                CommitGroupId = c.GroupId,
                                CommitId = c.CommitId,
                                AddedAt = c.AddedAt.Value
                            };

                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetByID: " + ex.Message);
            }
        }

        public void Add(CommitGroupMemberET et)
        {
            try
            {
                var entity = new CommitGroupMember
                {
                    GroupId = et.CommitGroupId,
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
                            where c.GroupId == et.CommitGroupId && c.CommitId == et.CommitId
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                entity.GroupId = et.CommitGroupId;
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
                            where c.GroupId == groupId && c.CommitId == commitId
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