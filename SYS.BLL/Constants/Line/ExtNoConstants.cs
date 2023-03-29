using isRock.LineBot.Conversation;

namespace SYS.BLL.Constants.Line
{
    public class ExtNoConstants : ConversationEntity
    {
        [Order(1)]
        [Question("請輸入欲查詢的員工名稱(英文)")]
        public string Ext_EmpName { get; set; }
    }
}
