using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ChatbotSummaryDAL
    {
        private readonly GitLogAggregatorDataContext db = new GitLogAggregatorDataContext();

        public List<ChatbotSummaryET> GetAll()
        {
            try
            {
                var query = from c in db.ChatbotSummaries
                            orderby c.CreatedAt descending
                            select new ChatbotSummaryET
                            {
                                ChatbotSummaryID = c.ChatbotSummaryID,
                                Attendance = c.Attendance,
                                AssignedTasks = c.AssignedTasks,
                                ContentResults = c.ContentResults,
                                SupervisorComments = c.SupervisorComments,
                                Notes = c.Notes,
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

        public ChatbotSummaryET GetByID(int chatbotSummaryID)
        {
            try
            {
                var query = from c in db.ChatbotSummaries
                            where c.ChatbotSummaryID == chatbotSummaryID
                            select new ChatbotSummaryET
                            {
                                ChatbotSummaryID = c.ChatbotSummaryID,
                                Attendance = c.Attendance,
                                AssignedTasks = c.AssignedTasks,
                                ContentResults = c.ContentResults,
                                SupervisorComments = c.SupervisorComments,
                                Notes = c.Notes,
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

        public void Add(ChatbotSummaryET et)
        {
            try
            {
                var entity = new ChatbotSummary
                {
                    Attendance = et.Attendance,
                    AssignedTasks = et.AssignedTasks,
                    ContentResults = et.ContentResults,
                    SupervisorComments = et.SupervisorComments,
                    Notes = et.Notes,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                db.ChatbotSummaries.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Add: " + ex.Message);
            }
        }

        public void Update(ChatbotSummaryET et)
        {
            try
            {
                var query = from c in db.ChatbotSummaries
                            where c.ChatbotSummaryID == et.ChatbotSummaryID
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                entity.Attendance = et.Attendance;
                entity.AssignedTasks = et.AssignedTasks;
                entity.ContentResults = et.ContentResults;
                entity.SupervisorComments = et.SupervisorComments;
                entity.Notes = et.Notes;
                entity.UpdatedAt = DateTime.Now;

                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Update: " + ex.Message);
            }
        }

        public void Delete(int chatbotSummaryID)
        {
            try
            {
                var query = from c in db.ChatbotSummaries
                            where c.ChatbotSummaryID == chatbotSummaryID
                            select c;

                var entity = query.SingleOrDefault();
                if (entity == null) return;

                db.ChatbotSummaries.DeleteOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Delete: " + ex.Message);
            }
        }
    }
}
