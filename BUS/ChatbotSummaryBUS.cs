using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ChatbotSummaryBUS
    {
        private ChatbotSummaryDAL dal = new ChatbotSummaryDAL();

        public List<ChatbotSummaryET> GetAll()
        {
            return dal.GetAll();
        }

        public ChatbotSummaryET GetByID(int id)
        {
            return dal.GetByID(id);
        }

        public void Add(ChatbotSummaryET entity)
        {
            dal.Add(entity);
        }

        public void Update(ChatbotSummaryET entity)
        {
            dal.Update(entity);
        }

        public void Delete(int id)
        {
            dal.Delete(id);
        }
    }

}
