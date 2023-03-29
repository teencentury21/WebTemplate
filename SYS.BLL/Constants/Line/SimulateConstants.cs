using isRock.LineBot.Conversation;

namespace SYS.BLL.Constants.Line
{
    public class SimulateConstants : ConversationEntity
    {
        [Order(1)]
        [Question("請輸入員工工號")]
        public string EmpNo { get; set; }

    }
}
