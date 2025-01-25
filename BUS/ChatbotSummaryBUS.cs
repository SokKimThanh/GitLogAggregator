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

        public List<SummaryET> GetAll()
        {
            return dal.GetAll();
        }

        public SummaryET GetByID(int id)
        {
            return dal.GetByID(id);
        }

        public void Add(SummaryET entity)
        {
            dal.Add(entity);
        }

        public void Update(SummaryET entity)
        {
            dal.Update(entity);
        }

        public void Delete(int id)
        {
            dal.Delete(id);
        }
    }

}
