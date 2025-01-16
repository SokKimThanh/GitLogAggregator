using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ChatbotSummaryBUS
    {
        private ChatbotSummaryDAL chatbotSummaryDAL = new ChatbotSummaryDAL();

        public void CreateChatbotSummary(ChatbotSummary chatbotSummary)
        {
            chatbotSummaryDAL.Create(chatbotSummary);
        }

        public void DeleteChatbotSummary(int chatbotSummaryId)
        {
            chatbotSummaryDAL.Delete(chatbotSummaryId);
        }

        public void UpdateChatbotSummary(ChatbotSummary chatbotSummary)
        {
            chatbotSummaryDAL.Update(chatbotSummary);
        }

        public List<ChatbotSummary> GetAllChatbotSummaries()
        {
            return chatbotSummaryDAL.GetAll();
        }

        public ChatbotSummary GetChatbotSummaryById(int chatbotSummaryId)
        {
            return chatbotSummaryDAL.GetById(chatbotSummaryId);
        }
    }

}
