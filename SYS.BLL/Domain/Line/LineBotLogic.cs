using isRock.LineBot;
using isRock.LineBot.Conversation;
using Newtonsoft.Json;
using SYS.BLL.Base;
using SYS.BLL.Constants;
using SYS.BLL.Constants.Line;
using SYS.DAL.Base;
using SYS.DAL.ChatBot;
using SYS.Model;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SYS.DAL.Default;
using SYS.BLL.Domain.GAIA;
using SYS.Model.SQL.ChatBot;
using System.Collections.Generic;

namespace SYS.BLL.Domain.Line
{
    public interface ILineBotLogic : IDataDrivenLogic
    {
        // Logic
        IDateTimeLogic _DateTimeLogic { get; set; }
        IGAIALogic _GAIALogic { get; set; }
        IHttpContextStateLogic _HttpContextStateLogic { get; set; }
        IMailLogic _MailLogic { get; set; }

        // Repository
        IAccountRegistRepository _AccountRegistRepository { get; set; }
        ITransactionLogRepository _TransactionLogRepository { get; set; }

        // Function
        string EventHandelResponse(string postData);
        string EventAZHandel(string postData);
        string EventHandel(string postData);
        bool IsRegisted(string lineId);
        string ValidateEmailandEmpNo(string lineId, string email);
        string ValidateRegistCode(string lineId, string code);
        void CreateAcc(AccountRegist input);
    }

    internal class LineBotLogic : DataDrivenLogic, ILineBotLogic
    {
        private string _accessToken;
        // Line 系統的api key，來源於SSO網站的SSO註冊頁面
        private string _apiKey;
        private string _userId;
        private string _replyToken;
        private bool _isRegisted;
        private DARFON.ChatBot.ChatBotWebService _chatbotlineWS;

        // Logic
        public IDateTimeLogic _DateTimeLogic { get; set; }
        public IGAIALogic _GAIALogic { get; set; }
        public IHttpContextStateLogic _HttpContextStateLogic { get; set; }
        public IMailLogic _MailLogic { get; set; }
        public ILineBotReplyLogic _LineBotReplyLogic { get; set; }
        
        // Repository
        public IAccountRegistRepository _AccountRegistRepository { get; set; }
        public ITransactionLogRepository _TransactionLogRepository { get; set; }
        public LineBotLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {
            this._accessToken = "RCGMQrgQmpEKboLZu0TREWK/7klJ1gsWGaq2vM9D6/1Q3/Xs3J33vnavCSxATwo4Ikj4dBgc0QM0IA3/tdNE1qa+/bMJGws8/d2e7W6fKDUYDiozqFDQ/Ks7ICZlpMEjDp2AmD47JOklHCTrB3ogKwdB04t89/1O/w1cDnyilFU=";
            this._apiKey = "da1d13ee5e5e49b7aa890d4691d2182d";


            _chatbotlineWS = new DARFON.ChatBot.ChatBotWebService();
            DARFON.ChatBot.AuthHeader header = new DARFON.ChatBot.AuthHeader { UserName = "dfITS", Password = "ITS@dmin01" };
            _chatbotlineWS.AuthHeaderValue = header;

            _DateTimeLogic = CreateLogic<IDateTimeLogic>();
            _GAIALogic = CreateLogic<IGAIALogic>();            
            _HttpContextStateLogic = CreateLogic<IHttpContextStateLogic>();
            _LineBotReplyLogic = CreateLogic<ILineBotReplyLogic>();
            _MailLogic = CreateLogic<IMailLogic>();

            _AccountRegistRepository = CreateSqlRepository<IAccountRegistRepository>(Database.Default);
            _TransactionLogRepository = CreateSqlRepository<ITransactionLogRepository>(Database.Default);
        }
        /// <summary>
        /// test only response message from user
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string EventHandelResponse(string postData)
        {
            var message = "";
            try
            {
                //_TransactionLogRepository.Create(TransactionLogConstants.LineAPI, postData, "orig message", "Input", SpecialEditorConstants.LineAPI);

                var ReceivedMessage = Utility.Parsing(postData);

                _replyToken = ReceivedMessage.events[0].replyToken;
                var userSays = ReceivedMessage.events[0].message.text;
                _LineBotReplyLogic.ReplyMessage(_replyToken, userSays, _accessToken);

                //_TransactionLogRepository.Create(TransactionLogConstants.LineAPI, userSays, "orig message", "Done", SpecialEditorConstants.LineAPI);
                return message;
            }
            catch (Exception ex)
            {
                // _TransactionLogRepository.Create(TransactionLogConstants.LineAPI, ex.ToString(), "orig message", "Error", SpecialEditorConstants.LineAPI);
                return message;
            }
        }

        /// <summary>
        /// Line bot server on Azure and call webservice to DMZ
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string EventAZHandel (string postData)
        {
            var responseMsg = "";
            try
            {
                var ReceivedMessage = Utility.Parsing(postData);

                _userId = ReceivedMessage.events[0].source.userId;
                _replyToken = ReceivedMessage.events[0].replyToken;
                _isRegisted = _chatbotlineWS.IsRegisted(_userId);

                var userSays = ReceivedMessage.events[0].message.text;
                var categoryProcess = _HttpContextStateLogic.GetState($"{_userId}-category");

                if (userSays == "#請給我驗證" || categoryProcess == "1")
                {
                    //定義資訊蒐集者
                    isRock.LineBot.Conversation.InformationCollector<RegistConstants> CIC =
                        new isRock.LineBot.Conversation.InformationCollector<RegistConstants>(_accessToken);
                    //定義接收CIC結果的類別
                    ProcessResult<RegistConstants> processResult;
                    if (userSays == "#請給我驗證")
                    {
                        _HttpContextStateLogic.SetState($"{_userId}-category", "1");
                        processResult = CIC.Process(ReceivedMessage.events[0], true);
                        ProcessCIC(processResult, 1, postData);
                    }
                    else
                    {
                        // data validate
                        CIC.OnMessageTypeCheck += (s, e) =>
                        {
                            switch (e.CurrentPropertyName)
                            {
                                case "EmpId":
                                    // validate DF employee                                     
                                    var isDF = _chatbotlineWS.GetEmpByEmpNo(userSays);
                                    if (isDF.EmpNo == null)
                                    {
                                        e.isMismatch = true;
                                        e.ResponseMessage = "非Darfon公司員工，請重新輸入員工工號";
                                    }
                                    else
                                    {
                                        if (isDF.IsActive == 0)
                                        {
                                            e.isMismatch = true;
                                            e.ResponseMessage = "非Darfon公司員工，請重新輸入員工工號";
                                        }
                                        else
                                        {
                                            _chatbotlineWS.CreateAcc(new DARFON.ChatBot.AccountRegist
                                            {
                                                Id = Guid.NewGuid(),
                                                Line_Id = _userId,
                                                Emp_Id = isDF.GAIAId,
                                                Emp_no = userSays,
                                                Emp_mail = isDF.Email,
                                                Active = false,
                                                Cdt = _DateTimeLogic.GetCurrentTime()
                                            });
                                        }
                                    }
                                    break;
                                case "Emp_mail":
                                    if (!Regex.IsMatch(userSays.ToLower(), @"^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})*$"))
                                    {
                                        e.isMismatch = true;
                                        e.ResponseMessage = "不正確的email 格式，請重新輸入員工信箱(example.its@darfon.com.tw)";
                                    }
                                    else
                                    {
                                        var arrayMail = userSays.ToLower().Split('@');
                                        if (arrayMail[1] != "darfon.com.tw")
                                        {
                                            e.isMismatch = true;
                                            e.ResponseMessage = "請重輸入達方員工信箱(darfon.com.tw 結尾)";
                                        }
                                        else
                                        {
                                            var validateMail = _chatbotlineWS.ValidateEmailandEmpNo(_userId, userSays.ToLower());
                                            if (validateMail != "OK")
                                            {
                                                e.isMismatch = true;
                                                e.ResponseMessage = "輸入的信箱與工號不符，請重輸入達方員工信箱(darfon.com.tw 結尾)";
                                            }
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }
                        };
                        processResult = CIC.Process(ReceivedMessage.events[0]);
                        ProcessCIC(processResult, 1, postData);
                    }
                }
                else if ((userSays == "#同仁分機" && _isRegisted) || categoryProcess == "2")
                {
                    //定義同仁查詢CIC
                    isRock.LineBot.Conversation.InformationCollector<ExtNoConstants> extNoCIC =
                        new isRock.LineBot.Conversation.InformationCollector<ExtNoConstants>(_accessToken);
                    //定義接收CIC結果的類別
                    ProcessResult<ExtNoConstants> extNoProcessResult;
                    if (userSays == "#同仁分機")
                    {
                        _HttpContextStateLogic.SetState($"{_userId}-category", "2");
                        extNoProcessResult = extNoCIC.Process(ReceivedMessage.events[0], true);
                        ProcessCIC(extNoProcessResult, 2, postData);
                    }
                    else
                    {
                        extNoProcessResult = extNoCIC.Process(ReceivedMessage.events[0]);
                        ProcessCIC(extNoProcessResult, 2, postData);
                    }
                }
                else if ((userSays == "#模擬登入" && _isRegisted) || categoryProcess == "3")
                {
                    var acc = _chatbotlineWS.GetEmpByLineId(_userId);
                    var allowList = new List<string>();
                    allowList.Add("220900004");
                    if (allowList.Contains(acc.Emp_no))
                    {
                        //定義同仁查詢CIC
                        isRock.LineBot.Conversation.InformationCollector<SimulateConstants> simulateCIC =
                            new isRock.LineBot.Conversation.InformationCollector<SimulateConstants>(_accessToken);
                        //定義接收CIC結果的類別
                        ProcessResult<SimulateConstants> simulateProcessResult;
                        if (userSays == "#模擬登入")
                        {
                            _HttpContextStateLogic.SetState($"{_userId}-category", "3");
                            simulateProcessResult = simulateCIC.Process(ReceivedMessage.events[0], true);
                            ProcessCIC(simulateProcessResult, 3, postData);
                        }
                        else
                        {
                            simulateProcessResult = simulateCIC.Process(ReceivedMessage.events[0]);
                            ProcessCIC(simulateProcessResult, 3, postData);
                        }
                    }
                    else
                    {
                        CommonTextProcess(postData);
                        responseMsg += "Registed.";
                    }
                }
                else
                {
                    if (!_isRegisted)
                    {
                        RegistConfirmTemplate(_userId, _replyToken);
                        responseMsg += "Not registed.";
                    }
                    else
                    {
                        CommonTextProcess(postData);
                        responseMsg += "Registed.";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return JsonConvert.SerializeObject(responseMsg);
        }
        /// <summary>
        /// Line bot server on DMZ and directly get data from internal
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string EventHandel (string postData)
        {
            var responseMsg = "";
            try
            {
                _TransactionLogRepository.Create(TransactionLogConstants.LineAPI, "API input", postData, "orig message", SpecialEditorConstants.LineAPI);

                var ReceivedMessage = Utility.Parsing(postData);

                _userId = ReceivedMessage.events[0].source.userId;
                _replyToken = ReceivedMessage.events[0].replyToken;
                _isRegisted = IsRegisted(_userId);

                var userSays = ReceivedMessage.events[0].message.text;
                var categoryProcess = _HttpContextStateLogic.GetState($"{_userId}-category");

                if (userSays == "#請給我驗證" || categoryProcess == "1")
                {
                    //定義資訊蒐集者
                    isRock.LineBot.Conversation.InformationCollector<RegistConstants> CIC =
                        new isRock.LineBot.Conversation.InformationCollector<RegistConstants>(_accessToken);
                    //定義接收CIC結果的類別
                    ProcessResult<RegistConstants> processResult;
                    if (userSays == "#請給我驗證")
                    {
                        _HttpContextStateLogic.SetState($"{_userId}-category", "1");
                        processResult = CIC.Process(ReceivedMessage.events[0], true);
                        ProcessCIC(processResult, 1, postData);
                    }
                    else
                    {
                        CIC.OnMessageTypeCheck += (s, e) => {
                            switch (e.CurrentPropertyName)
                            {
                                case "EmpId":
                                    // validate DF employee
                                    var isDF = _GAIALogic.GetEmpByEmpNo(userSays);
                                    if (isDF.EmpNo == null)
                                    {
                                        e.isMismatch = true;
                                        e.ResponseMessage = "非Darfon公司員工，請重新輸入員工工號";
                                    }
                                    else
                                    {
                                        if (isDF.IsActive == 0)
                                        {
                                            e.isMismatch = true;
                                            e.ResponseMessage = "非Darfon公司員工，請重新輸入員工工號";
                                        }
                                        else
                                        {
                                            _AccountRegistRepository.Create(new AccountRegist {
                                                Id = Guid.NewGuid(),
                                                Line_Id = _userId,
                                                Emp_Id = isDF.GAIAId,
                                                Emp_no = userSays,
                                                Emp_mail = isDF.Email,
                                                Active = false,
                                                Cdt=_DateTimeLogic.GetCurrentTime()
                                            });
                                        }
                                    }
                                    break;                                
                                case "Emp_mail":
                                    if (!Regex.IsMatch(userSays.ToLower(), @"^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})*$"))
                                    {
                                        e.isMismatch = true;
                                        e.ResponseMessage = "不正確的email 格式，請重新輸入員工信箱(example.its@darfon.com.tw)";
                                    }
                                    else
                                    {
                                        var arrayMail = userSays.ToLower().Split('@');
                                        if (arrayMail[1] != "darfon.com.tw")
                                        {
                                            e.isMismatch = true;
                                            e.ResponseMessage = "請重輸入達方員工信箱(darfon.com.tw 結尾)";
                                        }
                                        else
                                        {
                                            
                                            var validateMail =  ValidateEmailandEmpNo(_userId, userSays.ToLower());
                                            if (validateMail != "OK")
                                            {
                                                e.isMismatch = true;
                                                e.ResponseMessage = "輸入的信箱與工號不符，請重輸入達方員工信箱(darfon.com.tw 結尾)";
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        };

                        processResult = CIC.Process(ReceivedMessage.events[0]);
                        ProcessCIC(processResult, 1, postData);
                    }
                }
                else if ((userSays == "#同仁分機" && _isRegisted) || categoryProcess == "2")
                {
                    //定義同仁查詢CIC
                    isRock.LineBot.Conversation.InformationCollector<ExtNoConstants> extNoCIC =
                        new isRock.LineBot.Conversation.InformationCollector<ExtNoConstants>(_accessToken);
                    //定義接收CIC結果的類別
                    ProcessResult<ExtNoConstants> extNoProcessResult;
                    if (userSays == "#同仁分機")
                    {
                        _HttpContextStateLogic.SetState($"{_userId}-category", "2");
                        extNoProcessResult = extNoCIC.Process(ReceivedMessage.events[0], true);
                        ProcessCIC(extNoProcessResult, 2, postData);
                    }
                    else
                    {
                        extNoProcessResult = extNoCIC.Process(ReceivedMessage.events[0]);
                        ProcessCIC(extNoProcessResult, 2, postData);
                    }
                }
                else
                {
                    if (!_isRegisted)
                    {
                        RegistConfirmTemplate(_userId, _replyToken);
                        responseMsg += "Not registed.";
                    }
                    else
                    {
                        CommonTextProcess(postData);
                        responseMsg += "Registed.";
                    }
                }
            }
            catch (Exception ex)
            {
                _TransactionLogRepository.Create(TransactionLogConstants.LineAPI, "Error: API input", postData, ex.ToString(), SpecialEditorConstants.Default);
                return JsonConvert.SerializeObject(ex.ToString());
            }
            return JsonConvert.SerializeObject(responseMsg);
        }
        private void CommonTextProcess(string postData)
        {           
            var ReceivedMessage = Utility.Parsing(postData);

            // 不支援連續對談的指令，將會以普通文字命令處理
            var replyToken = ReceivedMessage.events[0].replyToken;
            var TimeStamp = ReceivedMessage.events[0].timestamp;
            var inputType = ReceivedMessage.events[0].message.type;
            var userId = ReceivedMessage.events[0].source.userId;

            switch (inputType.ToLower())
            {
                case "sticker":
                    //回覆貼圖
                    //_LineBotReplyLogic.ReplyStickerMessage(replyToken, 1, 1, _accessToken);
                    var stickerId = new Random().Next(1, 40);
                    _LineBotReplyLogic.ReplyStickerMessage(_replyToken, 789, 10854 + stickerId, _accessToken);
                    break;
                case "text":
                    //回覆文字
                    var userSays = ReceivedMessage.events[0].message.text;
                    string Message = $"哈囉, 你在 {TimeStamp} 時候說了: {userSays}";

                    if (userSays == "#表單查詢")
                    {
                        // Let WS get user ID
                        string mySystemSid = userId;
                        var targetPage = "/bpm/mobile/approve";

                        var redirectURL = _chatbotlineWS.GetGAIASSOredirectURL(_apiKey, GAIASystemsConstants.Line, GAIASystemsConstants.GAIA, mySystemSid, targetPage);

                        // new button template
                        ButtonsTemplate resultTemplate = new ButtonsTemplate();
                        resultTemplate.title = "表單查詢";
                        resultTemplate.text = "請點擊下方按鈕查看您的待簽核表單";
                        UriAction uriAction = new UriAction();
                        uriAction.label = "表單查詢";
                        uriAction.uri = new Uri(redirectURL);
                        resultTemplate.actions.Add(uriAction);
                        _LineBotReplyLogic.ReplyTemplateMessage(replyToken, resultTemplate, _accessToken);
                    }
                    else if (userSays == "#COVID-19健康通報")
                    {
                        var redirectURL = $"https://www.darfon.com.tw/QuestionnaireForm/SelfReport_AnswerMain/create";
                        // new button template
                        ButtonsTemplate resultTemplate = new ButtonsTemplate();
                        resultTemplate.title = "COVID-19 健康通報";
                        resultTemplate.text = "請點擊下方按鈕進行 COVID-19 COVID-19健康通報";
                        UriAction uriAction = new UriAction();
                        uriAction.label = "健康通報";
                        uriAction.uri = new Uri(redirectURL);
                        resultTemplate.actions.Add(uriAction);
                        _LineBotReplyLogic.ReplyTemplateMessage(replyToken, resultTemplate, _accessToken);
                    }
                    else if (userSays == "驗證")
                    {
                        RegistConfirmTemplate(userId, replyToken);
                    }
                    else if (userSays == "年節賀卡")
                    {
                        var picUrl = new Uri("https://png.pngtree.com/png-clipart/20221126/ourmid/pngtree-2023-year-of-the-rabbit-chinese-new-year-ink-rabbit-png-image_6480628.png");
                        _LineBotReplyLogic.ReplyImageMessage(replyToken, picUrl.OriginalString, picUrl.OriginalString, _accessToken);
                    }
                    else if (userSays == "#userInfo")
                    {
                        var info = Utility.GetUserInfo(userId, _accessToken);
                        //Utility.ReplyMessage(replyToken, $"您的資訊如下: {JsonConvert.SerializeObject(info)}", _accessToken);
                        _LineBotReplyLogic.ReplyMessage(replyToken, $"您的資訊如下: {JsonConvert.SerializeObject(info)}", _accessToken);
                    }
                    else if (userSays == "#url")
                    {
                        _LineBotReplyLogic.ReplyMessage(replyToken, $"帶你去google https://www.google.com", _accessToken);
                    }
                    // reply message with json (flex message response)
                    else if (userSays == "#confirm")
                    {
                        var resopnseJSON = "[{\"type\": \"template\", \"altText\": \"this is a confirm template\", \"template\": { \"type\": \"confirm\", \"text\": \"Are you sure?\",\"actions\": [{\"type\": \"message\",\"label\": \"Yes\",\"text\": \"yes\"},{\"type\": \"message\",\"label\": \"No\",\"text\": \"no\"}]}}]";
                        _LineBotReplyLogic.ReplyMessageWithJSON(replyToken, resopnseJSON, _accessToken);
                    }
                    else
                    {
                        if (userSays != "稍後驗證")
                        {                            
                            _LineBotReplyLogic.ReplyMessage(replyToken, Message, _accessToken);
                        }
                    }
                    break;
                case "image":
                    //回覆圖片
                    var url = new Uri("https://png.pngtree.com/png-clipart/20221126/ourmid/pngtree-2023-year-of-the-rabbit-chinese-new-year-ink-rabbit-png-image_6480628.png");
                    _LineBotReplyLogic.ReplyImageMessage(replyToken, url.OriginalString, url.OriginalString, _accessToken);
                    break;
                default:
                    //回覆訊息
                    string df_response = $"我無法識別你傳入的訊息類型:{inputType}";
                    //回覆用戶                   
                    _LineBotReplyLogic.ReplyMessage(replyToken, df_response, _accessToken);
                    break;
            }
        }
        private string ProcessCIC(Object inputProcess, int category, string postData)
        {
            var responseMsg = "";
            var receivedMessage = Utility.Parsing(postData);
            
            var userSays = receivedMessage.events[0].message.text;

            // var isRegisted = chatbotWS.IsRegisted(userId);
           

            if (category == 1)
            {
                var processResult = (ProcessResult<RegistConstants>)inputProcess;
                var registEntity = processResult.ConversationState.ConversationEntity;

                //處理 CIC回覆的結果
                switch (processResult.ProcessResultStatus)
                {
                    // 後續都會進來
                    case ProcessResultStatus.Processed:
                        if (registEntity != null)
                        {                            
                            if (registEntity.Emp_mail != null)
                            {
                                // call DFAPI save emp_id, phone, lineId
                                _chatbotlineWS.SendRegistCode(_userId, registEntity.EmpId, registEntity.Emp_mail);
                            }
                        }
                        //取得候選訊息發送
                        responseMsg += processResult.ResponseMessageCandidate;
                        // ReplyMessage(_replyToken, responseMsg, _accessToken);
                        _LineBotReplyLogic.ReplyMessage(_replyToken, responseMsg, _accessToken);
                        break;
                    case ProcessResultStatus.Done:
                        _HttpContextStateLogic.SetState($"{_userId}-category", "0");
                        var result = _chatbotlineWS.ValidateRegistCode(_userId, userSays.Trim());
                        responseMsg += result;
                        // Utility.ReplyMessage(_replyToken, responseMsg, _accessToken);
                        _LineBotReplyLogic.ReplyMessage(_replyToken, responseMsg, _accessToken);
                        break;

                    case ProcessResultStatus.Pass:
                        // 不支援連續對談的指令，將會以普通文字命令處理                       
                        // 沒註冊過導入註冊流程
                        _HttpContextStateLogic.SetState($"{_userId}-category", "0");
                        if (!_isRegisted)
                        {
                            RegistConfirmTemplate(_userId, _replyToken);
                            responseMsg += "Not registed.";
                        }
                        else
                        {
                            CommonTextProcess(postData);
                            responseMsg += "Registed.";
                        }
                        break;
                    case ProcessResultStatus.Exception:
                        //取得候選訊息發送
                        responseMsg += processResult.ResponseMessageCandidate;
                        // Utility.ReplyMessage(_replyToken, responseMsg, _accessToken);
                        _LineBotReplyLogic.ReplyMessage(_replyToken, responseMsg, _accessToken);
                        break;
                    case ProcessResultStatus.Break:
                        //取得候選訊息發送
                        responseMsg += processResult.ResponseMessageCandidate;
                        // Utility.ReplyMessage(_replyToken, responseMsg, _accessToken);
                        _LineBotReplyLogic.ReplyMessage(_replyToken, responseMsg, _accessToken);
                        break;
                    case ProcessResultStatus.InputDataFitError:
                        // responseMsg += "\n資料型態不合\n";
                        responseMsg += processResult.ResponseMessageCandidate;
                        // Utility.ReplyMessage(_replyToken, responseMsg, _accessToken);
                        _LineBotReplyLogic.ReplyMessage(_replyToken, responseMsg, _accessToken);
                        break;
                    default:
                        //取得候選訊息發送
                        responseMsg += processResult.ResponseMessageCandidate;
                        // Utility.ReplyMessage(_replyToken, responseMsg, _accessToken);
                        _LineBotReplyLogic.ReplyMessage(_replyToken, responseMsg, _accessToken);
                        break;
                }
            }
            else if (category == 2)
            {
                var processResult = (ProcessResult<ExtNoConstants>)inputProcess;
                var registEntity = processResult.ConversationState.ConversationEntity;

                //處理 CIC回覆的結果
                switch (processResult.ProcessResultStatus)
                {
                    case ProcessResultStatus.Processed:
                        //取得候選訊息發送                        
                        responseMsg += processResult.ResponseMessageCandidate;
                        break;
                    case ProcessResultStatus.Done:
                        _HttpContextStateLogic.SetState($"{_userId}-category", "0");
                        // var ws = WebService.GetDFChatBotWebService();
                        var lang = "en";
                        var empName = registEntity.Ext_EmpName;
                        var emps = _chatbotlineWS.GetEmpByName(lang, empName);
                        if (emps.Length == 5)
                        {
                            responseMsg += "搜尋項目超過5位，僅列出前五位同仁分機資訊，若不再下列請輸入更精準的同仁名稱\n";
                        }
                        else
                        {
                            responseMsg += "您搜尋的同仁分機資訊如下:\n";
                        }
                        if (emps.Length > 0)
                        {
                            foreach (var item in emps)
                            {
                                responseMsg += $"{item.Name}, {item.EnName}, 分機: {item.ExtNo} \n";
                            }                            
                        }
                        break;
                    case ProcessResultStatus.Pass:
                        // 沒註冊過導入註冊流程
                        _HttpContextStateLogic.SetState($"{_userId}-category", "0");
                        if (!_isRegisted)
                        {
                            RegistConfirmTemplate(_userId, _replyToken);
                            responseMsg += "Not registed.";
                        }
                        else
                        {
                            CommonTextProcess(postData);
                            responseMsg += "Registed.";
                        }
                        break;                        
                    case ProcessResultStatus.Exception:
                        //取得候選訊息發送
                        responseMsg += processResult.ResponseMessageCandidate;
                        break;
                    case ProcessResultStatus.Break:
                        //取得候選訊息發送
                        responseMsg += processResult.ResponseMessageCandidate;
                        break;
                    case ProcessResultStatus.InputDataFitError:
                        responseMsg += processResult.ResponseMessageCandidate;
                        break;
                    default:
                        //取得候選訊息發送
                        responseMsg += processResult.ResponseMessageCandidate;
                        break;
                }
                //回覆用戶訊息
                // Utility.ReplyMessage(_replyToken, responseMsg, _accessToken);                
                _LineBotReplyLogic.ReplyMessage(_replyToken, responseMsg, _accessToken);
                //回覆API OK
            }
            else if (category == 3)
            {
                var processResult = (ProcessResult<SimulateConstants>)inputProcess;
                var registEntity = processResult.ConversationState.ConversationEntity;

                //處理 CIC回覆的結果
                switch (processResult.ProcessResultStatus)
                {
                    case ProcessResultStatus.Processed:
                        //取得候選訊息發送                        
                        responseMsg += processResult.ResponseMessageCandidate;
                        break;
                    case ProcessResultStatus.Done:
                        _HttpContextStateLogic.SetState($"{_userId}-category", "0");

                        var empNo = registEntity.EmpNo;
                        var acc = _chatbotlineWS.GetSimulateEmpByEmpNo(empNo);
                        
                        if (acc != null && acc.Active)
                        {
                            var targetPage = "/bpm/mobile/approve";
                            var redirectURL = _chatbotlineWS.GetGAIASSOredirectURL(_apiKey, GAIASystemsConstants.Line, GAIASystemsConstants.GAIA, acc.Line_Id, targetPage);

                            // new button template
                            ButtonsTemplate resultTemplate = new ButtonsTemplate();
                            resultTemplate.title = "模擬表單查詢";
                            resultTemplate.text = $"請點擊下方按鈕查看 {empNo} 的待簽核表單";
                            UriAction uriAction = new UriAction();
                            uriAction.label = "表單查詢";
                            uriAction.uri = new Uri(redirectURL);
                            resultTemplate.actions.Add(uriAction);
                            _LineBotReplyLogic.ReplyTemplateMessage(_replyToken, resultTemplate, _accessToken);
                        }
                        else
                        {
                            responseMsg += $"查無對應員工 {empNo}\n";
                        }

                        break;
                    case ProcessResultStatus.Pass:
                        // 沒註冊過導入註冊流程
                        _HttpContextStateLogic.SetState($"{_userId}-category", "0");
                        if (!_isRegisted)
                        {
                            RegistConfirmTemplate(_userId, _replyToken);
                            responseMsg += "Not registed.";
                        }
                        else
                        {
                            CommonTextProcess(postData);
                            responseMsg += "Registed.";
                        }
                        break;
                    case ProcessResultStatus.Exception:
                        //取得候選訊息發送
                        responseMsg += processResult.ResponseMessageCandidate;
                        break;
                    case ProcessResultStatus.Break:
                        //取得候選訊息發送
                        responseMsg += processResult.ResponseMessageCandidate;
                        break;
                    case ProcessResultStatus.InputDataFitError:
                        responseMsg += processResult.ResponseMessageCandidate;
                        break;
                    default:
                        //取得候選訊息發送
                        responseMsg += processResult.ResponseMessageCandidate;
                        break;
                }
                //回覆用戶訊息
                // Utility.ReplyMessage(_replyToken, responseMsg, _accessToken);                
                _LineBotReplyLogic.ReplyMessage(_replyToken, responseMsg, _accessToken);
                //回覆API OK
            }
            return responseMsg;
        }
        public bool IsRegisted(string lineId)
        {
            var result = false;
            var acc = _AccountRegistRepository.GetAccountByLineId(lineId);
            if (acc != null)
            {
                // validat GAIA status, webservice 不給力，先註解
                //var ws = WebService.GetGaiaEmpWebService();
                //var emp = ws.GetEmpByEmpNo(acc.EmpNo);            
                if (acc != null && acc.Active /*&& emp.IsActive == 1*/)
                {
                    result = true;
                }
            }
            return result;
        }
        public string ValidateEmailandEmpNo(string lineId, string email)
        {
            var result = "OK";
            var acc = _AccountRegistRepository.GetAccountByLineId(lineId);
            var gaiaEmp = _GAIALogic.GetEmpByEmpNo(acc.Emp_no);
            if (gaiaEmp.Email != "" && gaiaEmp.Email.ToLower() != email.ToLower())
            {
                result = "Mail not match";
            }
            return result;
        }
        public string ValidateRegistCode(string lineId, string code)
        {
            var result = "驗證成功";
            try
            {
                var lastCode = _TransactionLogRepository.GetItmeByAppNameNData(TransactionLogConstants.DFChatBotWS, lineId).FirstOrDefault();
                if (lastCode.Message != null)
                {
                    if (lastCode.Message != code)
                    {
                        result = "驗證失敗，請重新驗證流程";
                    }
                    else
                    {
                        var acc = _AccountRegistRepository.GetAccountByLineId(lineId);
                        acc.Active = true;
                        _AccountRegistRepository.Update(acc);
                    }
                }
                lastCode.Application_Name= TransactionLogConstants.Registed;
                _TransactionLogRepository.Update(lastCode);
            }
            catch (Exception)
            {
                result = "驗證失敗，請重新驗證流程";
            }
            return result;
        }
        public void CreateAcc(AccountRegist input)
        {
            var exists = _AccountRegistRepository.GetAccountByLineId(input.Line_Id);
            if (exists!=null && exists.Active)
            {
                exists.Active = false;
                _AccountRegistRepository.Update(exists);
            }
            else
            {
                _AccountRegistRepository.Create(input);
            }
        }
        private void RegistConfirmTemplate(string userId, string replyToken)
        {
            var info = Utility.GetUserInfo(userId, _accessToken);
            var message = new StringBuilder();
            message.Append($"親愛的{info.displayName}:");
            message.Append($"使用MyDarfon前必須執行身分驗證，若驗證未通過將無法使用功能。");
            message.Append("請問是否要執行驗證？");

            ConfirmTemplate RegistConfirmTemplate = new ConfirmTemplate();
            RegistConfirmTemplate.text = message.ToString();
            MessageAction yesAction = new MessageAction();
            yesAction.label = "Yes";
            yesAction.text = "#請給我驗證";
            MessageAction noAction = new MessageAction();
            noAction.label = "No";
            noAction.text = "稍後驗證";
            RegistConfirmTemplate.actions.Add(yesAction);
            RegistConfirmTemplate.actions.Add(noAction);
            _LineBotReplyLogic.ReplyTemplateMessage(replyToken, RegistConfirmTemplate, _accessToken);
        }
                
    }
}
