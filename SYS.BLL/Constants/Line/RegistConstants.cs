using isRock.LineBot.Conversation;

namespace SYS.BLL.Constants.Line
{
    public class RegistConstants : ConversationEntity
    {
        [Order(1)]
        [Question("請輸入員工工號")]
        public string EmpId { get; set; }        

        [Order(2)]
        [Question("請輸入員工信箱(example.its@darfon.com.tw), 稍後將會寄送驗證碼至您的信箱[注意此號碼不得重複註冊]}")]
        public string Emp_mail { get; set; }

        [Order(3)]
        [Question("請輸入 Line Auth Code")]
        public string RegistCode { get; set; }
    }
}
