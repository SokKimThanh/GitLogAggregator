using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class SummaryBUS
    {
        private readonly SummaryDAL dal = new SummaryDAL();

        // Lưu Summary
        public void Add(SummaryET summaryET)
        {
            dal.Add(summaryET);
        }

        public SummaryET GetLastInserted()
        {
            return dal.GetLastInserted();
        }


        // READ BY ID
        public SummaryET GetById(int id)
        {
            return dal.GetById(id);
        }
        // READ ALL
        public List<SummaryET> GetAll()
        {
            return dal.GetAll();
        }

        // UPDATE
        public void Update(SummaryET updatedSummary)
        {
            dal.Update(updatedSummary);
        }

        // DELETE
        public void Delete(int id)
        {
            dal.Delete(id);
        }

        public SummaryET GetByDateAndPeriod(DateTime date, TimeSpan periodStartTime, TimeSpan periodEndTime)
        {
            return dal.GetByDateAndPeriod(date, periodStartTime, periodEndTime);
        }
    }
}
