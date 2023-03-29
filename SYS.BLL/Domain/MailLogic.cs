using SYS.BLL.Base;
using SYS.BLL.Common.Mail;
using SYS.BLL.Constants;
using SYS.BLL.Domain.GAIA;
using SYS.BLL.Entities.Mail;
using SYS.DAL.Base;
using SYS.DAL.ChatBot;
using SYS.DAL.Default;
using SYS.Model;
using SYS.Model.SQL.ChatBot;
using SYS.Model.SQL.Default;
using SYS.Utilities.Net.Mail;
using System;
using System.Collections.Generic;
using System.Net;

namespace SYS.BLL.Domain
{
    public interface IMailLogic : IDataDrivenLogic
    {
        // Logic
        IDateTimeLogic _DateTimeLogic { get; set; }
        IGAIALogic _GAIALogic { get; set; }        

        // Repository
        IAccountRegistRepository _AccountRegistRepository { get; set; }
        ITransactionLogRepository _TransactionLogRepository { get; set; }

        // Functions
        string SendRegistCode(string lineId, string empNo, string empMail);
        void SendMail(MailEntity mail, List<string> attachments);
    }
    internal class MailLogic : DataDrivenLogic, IMailLogic
    {
        // Logic
        protected IMailClient _MailClient { get; set; }
        public IDateTimeLogic _DateTimeLogic { get; set; }
        public IGAIALogic _GAIALogic { get; set; }        
        // Repository
        public IAccountRegistRepository _AccountRegistRepository { get; set; }
        public ITransactionLogRepository _TransactionLogRepository { get; set; }

        public MailLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {
            _DateTimeLogic = CreateLogic<IDateTimeLogic>();
            _GAIALogic = CreateLogic<IGAIALogic>();            
            _MailClient = new MailClient();

            _AccountRegistRepository= CreateSqlRepository<IAccountRegistRepository>(Database.Default);
            _TransactionLogRepository = CreateSqlRepository<ITransactionLogRepository>(Database.Default);
        }
        public string SendRegistCode(string lineId, string empNo, string empMail)
        {
            var dt = _DateTimeLogic.GetCurrentTime();
            var rd = new Random();
            var code = rd.Next(1001, 9999);

            var gaiaEmp = _GAIALogic.GetEmpByEmpNo(empNo);
            var result = "OK";
            try
            {
                // is registed?
                var acc = _AccountRegistRepository.GetAccountByEmpNo(empNo);

                if (acc != null)
                {
                    acc.Line_Id = lineId;
                    acc.Emp_Id = gaiaEmp.GAIAId;
                    acc.Emp_mail = empMail;
                    acc.Active = false;
                    _AccountRegistRepository.Update(acc);
                }
                else
                {
                    _AccountRegistRepository.Create(new AccountRegist
                    {
                        Id = Guid.NewGuid(),
                        Line_Id = lineId,
                        Emp_Id = gaiaEmp.GAIAId,
                        Emp_no = empNo,
                        Emp_phone= "",
                        Emp_mail = empMail,
                        Active = false,
                        Setting="",
                        Cdt = dt,
                    });
                }

                // log code
                _TransactionLogRepository.Create(new TransactionLog
                {
                    Id = Guid.NewGuid(),
                    Application_Name = TransactionLogConstants.DFChatBotWS,
                    Data = lineId,
                    Description = "RegistCode",
                    Message = code.ToString(),
                    Editor = SpecialEditorConstants.WebService,
                    Cdt = dt
                });

                var htmlMail = new HtmlMailRoot();
                var content = htmlMail.AddNormalContent();
                content.AppendFormat($"您的 Line Auth Code 為 : {code} ");
                var mailBody = htmlMail.GetContentResult();
                // send regist code
                this.SendMail(new MailEntity
                {
                    Sender = "line.regist@darfon.com.tw",
                    Receiver = empMail,
                    Title = "Line MyDarfon 驗證碼",
                    Content = mailBody
                }, new List<string>());
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }

            return result;
        }
        public void SendMail(MailEntity mail, List<string> attachments)
        {
            var credential = new NetworkCredential("", "", "mailtw.dty.darfon.com");
            var server = new MailServer("mailtw.dty.darfon.com", credential);

            try
            {
                var sendSetting = new MailSendSetting(mail.Sender, mail.Receiver, mail.CC, mail.BCC);

                // send out mail
                this._MailClient.SendMail(server, sendSetting, mail.Title, mail.Content, attachments, true);
            }
            catch (Exception ex)
            {
                _TransactionLogRepository.Create(TransactionLogConstants.SendMail, mail.Title, "SendMail Fail", ex.ToString(), SpecialEditorConstants.LineAPI);

            }
        }
    }
}
