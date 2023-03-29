using isRock.LineBot;
using Newtonsoft.Json;
using SYS.BLL.Base;
using SYS.BLL.Constants;
using SYS.DAL.Base;
using SYS.DAL.Default;
using SYS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL.Domain.Line
{
    public interface ILineBotReplyLogic : IDataDrivenLogic
    {
        // Repository        
        ITransactionLogRepository _TransactionLogRepository { get; set; }
        // Function
        string ReplyMessage(string ReplyToken, string Message, string ChannelAccessToken);
        string ReplyMessageWithJSON(string ReplyToken, string JSONMessages, string ChannelAccessToken);
        string ReplyStickerMessage(string ReplyToken, int packageId, int stickerId, string ChannelAccessToken);
        string ReplyImageMessage(string ReplyToken, string originalContentUrl, string previewImageUrl, string ChannelAccessToken);    
        string ReplyTemplateMessage(string ReplyToken, ButtonsTemplate TemplateMessage, string ChannelAccessToken);
        string ReplyTemplateMessage(string ReplyToken, ConfirmTemplate TemplateMessage, string ChannelAccessToken);
    }
    internal class LineBotReplyLogic : DataDrivenLogic, ILineBotReplyLogic
    {
        // Repository
        public ITransactionLogRepository _TransactionLogRepository { get; set; }
        public LineBotReplyLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {
            _TransactionLogRepository = CreateSqlRepository<ITransactionLogRepository>(Database.Default);            
        }
        /// <summary>
        /// 回覆訊息
        /// </summary>
        /// <param name="ReplyToken">ReplyToken</param>
        /// <param name="Message">訊息</param>
        /// <param name="ChannelAccessToken">ChannelAccessToken</param>
        /// <returns></returns>
        public string ReplyMessage(string ReplyToken, string Message, string ChannelAccessToken)
        {
            string text = "\r\n{{\r\n    'replyToken':'{0}',\r\n    'messages':[\r\n        {{\r\n            'type':'text',\r\n            'text':'{1}'\r\n        }}\r\n    ]\r\n}}";
            try
            {
                Message = Message.Replace("\n", "\\n");
                Message = Message.Replace("\r", "\\r");
                Message = Message.Replace("\"", "'");
                if (string.IsNullOrEmpty(Message))
                {
                    throw new Exception("要傳送的訊息不得為空白!");
                }
                text = text.Replace("'", "\"");
                text = string.Format(text, ReplyToken, Message);
                WebClient webClient = new WebClient();
                webClient.Headers.Clear();
                webClient.Headers.Add("Content-Type", "application/json");
                webClient.Headers.Add("Authorization", "Bearer " + ChannelAccessToken);
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] bytes2 = webClient.UploadData("https://api.line.me/v2/bot/message/reply", bytes);
                return Encoding.UTF8.GetString(bytes2);
            }
            catch (WebException ex)
            {
                _TransactionLogRepository.Create(TransactionLogConstants.LineAPI, "Error: API input", $"ReplyMessage API ERROR", ex.ToString(), SpecialEditorConstants.LineAPI);
                using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string text2 = streamReader.ReadToEnd();
                    throw new Exception("ReplyMessage API ERROR: " + text2, ex);
                }
            }
        }
        /// <summary>
        /// 回覆訊息
        /// </summary>
        /// <param name="ReplyToken">ReplyToken</param>
        /// <param name="JSONMessages">JSON格式的訊息，參考LINE API Reference</param>
        /// <param name="ChannelAccessToken">ChannelAccessToken</param>
        /// <returns></returns>
        public string ReplyMessageWithJSON(string ReplyToken, string JSONMessages, string ChannelAccessToken)
        {
            string text = "\r\n{{\r\n    'replyToken':'{0}',\r\n      'messages' : {1}\r\n}}";
            try
            {
                if (string.IsNullOrEmpty(JSONMessages))
                {
                    throw new Exception("JSONMessages 不得為空.");
                }
                if (string.IsNullOrEmpty(ReplyToken))
                {
                    throw new Exception("ReplyToken 不得為空.");
                }
                if (string.IsNullOrEmpty(ChannelAccessToken))
                {
                    throw new Exception("ChannelAccessToken 不得為空.");
                }
                if (!JSONMessages.Trim().StartsWith("[") || !JSONMessages.Trim().EndsWith("]"))
                {
                    throw new Exception("Line的Messages物件必須為JSON陣列. ex. [ {}, {} ] ");
                }
                text = text.Replace("'", "\"");
                text = string.Format(text, ReplyToken, JSONMessages);
                WebClient webClient = new WebClient();
                webClient.Headers.Clear();
                webClient.Headers.Add("Content-Type", "application/json");
                webClient.Headers.Add("Authorization", "Bearer " + ChannelAccessToken);
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] bytes2 = webClient.UploadData("https://api.line.me/v2/bot/message/reply", bytes);
                return Encoding.UTF8.GetString(bytes2);
            }
            catch (WebException ex)
            {
                _TransactionLogRepository.Create(TransactionLogConstants.LineAPI, "Error: API input", $"ReplyMessageWithJSON API ERROR", ex.ToString(), SpecialEditorConstants.LineAPI);

                using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string text2 = streamReader.ReadToEnd();
                    throw new Exception("ReplyMessageWithJSON API ERROR: " + text2, ex);
                }
            }
        }
        /// <summary>
        /// 回覆貼圖訊息
        /// </summary>
        /// <param name="ReplyToken">ReplyToken</param>
        /// <param name="packageId">packageId</param>
        ///   /// <param name="stickerId">stickerId</param>
        /// <param name="ChannelAccessToken">ChannelAccessToken</param>
        /// <returns></returns>
        public string ReplyStickerMessage(string ReplyToken, int packageId, int stickerId, string ChannelAccessToken)
        {
            string text = "\r\n{{\r\n    'replyToken': '{0}',\r\n    'messages':[\r\n        {{\r\n            'type':'sticker',\r\n            'packageId':'{1}',\r\n            'stickerId':'{2}'\r\n        }}\r\n    ]\r\n}}\r\n";
            try
            {
                text = text.Replace("'", "\"");
                text = string.Format(text, ReplyToken, packageId, stickerId);
                WebClient webClient = new WebClient();
                webClient.Headers.Clear();
                webClient.Headers.Add("Content-Type", "application/json");
                webClient.Headers.Add("Authorization", "Bearer " + ChannelAccessToken);
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] bytes2 = webClient.UploadData("https://api.line.me/v2/bot/message/reply", bytes);
                return Encoding.UTF8.GetString(bytes2);
            }
            catch (WebException ex)
            {
                _TransactionLogRepository.Create(TransactionLogConstants.LineAPI, "Error: API input", $"ReplyStickerMessage API ERROR", ex.ToString(), SpecialEditorConstants.LineAPI);

                using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string text2 = streamReader.ReadToEnd();
                    throw new Exception("ReplyStickerMessage API ERROR: " + text2, ex);
                }
            }
        }
        /// <summary>
        ///  回覆圖片訊息
        /// </summary>
        /// <param name="ToUserID">用戶UID(注意並非Line用戶id)</param>
        /// <param name="originalContentUrl">要傳送的訊息</param>
        ///   /// <param name="previewImageUrl">要傳送的訊息</param>
        /// <param name="ChannelAccessToken">ChannelAccessToken</param>
        /// <returns></returns>
        public string ReplyImageMessage(string ReplyToken, string originalContentUrl, string previewImageUrl, string ChannelAccessToken)
        {
            string text = "\r\n{{\r\n    'replyToken': '{0}',\r\n    'messages':[\r\n        {{\r\n            'type':'image',\r\n            'originalContentUrl':'{1}',\r\n            'previewImageUrl':'{2}'\r\n        }}\r\n    ]\r\n}}\r\n";
            try
            {
                if (!originalContentUrl.ToLower().StartsWith("https://") || !previewImageUrl.ToLower().StartsWith("https://"))
                {
                    throw new Exception("圖片網址必須是 https:// ");
                }
                text = text.Replace("'", "\"");
                text = string.Format(text, ReplyToken, originalContentUrl, previewImageUrl);
                WebClient webClient = new WebClient();
                webClient.Headers.Clear();
                webClient.Headers.Add("Content-Type", "application/json");
                webClient.Headers.Add("Authorization", "Bearer " + ChannelAccessToken);
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] bytes2 = webClient.UploadData("https://api.line.me/v2/bot/message/reply", bytes);
                return Encoding.UTF8.GetString(bytes2);
            }
            catch (WebException ex)
            {
                _TransactionLogRepository.Create(TransactionLogConstants.LineAPI, "Error: API input", $"ReplyImageMessage API ERROR", ex.ToString(), SpecialEditorConstants.LineAPI);

                using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string text2 = streamReader.ReadToEnd();
                    throw new Exception("ReplyImageMessage API ERROR: " + text2, ex);
                }
            }
        }
        /// <summary>
        /// Reply ButtonsTemplate Message
        /// </summary>
        /// <param name="ReplyToken"></param>
        /// <param name="TemplateMessage"></param>
        /// <param name="ChannelAccessToken">ChannelAccessToken</param>
        /// <returns></returns>
        public string ReplyTemplateMessage(string ReplyToken, ButtonsTemplate TemplateMessage, string ChannelAccessToken)
        {
            string text = "\r\n{{\r\n    'replyToken': '{0}',\r\n    'messages':[\r\n       {1}\r\n    ]\r\n}}\r\n";
            try
            {
                string text2 = "\r\n{{\r\n 'type': 'template',\r\n  'altText': '{0}',\r\n  'template': {{\r\n      'type': 'buttons',\r\n      'thumbnailImageUrl': '{1}',\r\n      'title': '{2}',\r\n      'text': '{3}',\r\n      'actions': {4}\r\n  }}\r\n}}       \r\n                    ";
                if (TemplateMessage == null)
                {
                    throw new Exception("TemplateMessage不得為null.");
                }
                if (TemplateMessage.thumbnailImageUrl == null)
                {
                    TemplateMessage.thumbnailImageUrl = new Uri("dmy://dummy");
                }
                if (string.IsNullOrEmpty(TemplateMessage.text))
                {
                    throw new Exception("必須指定text.");
                }
                if (TemplateMessage.actions == null || TemplateMessage.actions.Count < 1 || TemplateMessage.actions.Count > 4)
                {
                    throw new Exception("actions數量必須是1-4之間");
                }
                foreach (TemplateActionBase action in TemplateMessage.actions)
                {
                    if (action.GetType().Equals(typeof(UriAction)))
                    {
                        UriAction uriAction = action as UriAction;
                        if (uriAction.uri == null)
                        {
                            throw new Exception("uriAction 中的 Url不得為null.");
                        }
                    }
                }
                string text3 = "";
                text3 = JsonConvert.SerializeObject(TemplateMessage.actions);
                text2 = text2.Replace("'", "\"");
                text2 = string.Format(text2, TemplateMessage.altText, TemplateMessage.thumbnailImageUrl.ToString(), TemplateMessage.title, TemplateMessage.text, text3);
                text2 = text2.Replace("\"thumbnailImageUrl\": \"dmy://dummy/\",", "");
                text2 = text2.Replace("\"title\": \"\",", "");
                text2 = text2.Replace("\"text\": \"\",", "");
                text = text.Replace("'", "\"");
                text = string.Format(text, ReplyToken, text2);
                WebClient webClient = new WebClient();
                webClient.Headers.Clear();
                webClient.Headers.Add("Content-Type", "application/json");
                webClient.Headers.Add("Authorization", "Bearer " + ChannelAccessToken);
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] bytes2 = webClient.UploadData("https://api.line.me/v2/bot/message/reply", bytes);
                return Encoding.UTF8.GetString(bytes2);
            }
            catch (WebException ex)
            {
                _TransactionLogRepository.Create(TransactionLogConstants.LineAPI, "Error: API input", $"ReplyTemplateMessage(button) API ERROR", ex.ToString(), SpecialEditorConstants.LineAPI);

                using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string text4 = streamReader.ReadToEnd();
                    throw new Exception("PushImageMessage API ERROR: " + text4, ex);
                }
            }
        }
        /// <summary>
        /// Reply ConfirmTemplate Message
        /// </summary>
        /// <param name="ReplyToken"></param>
        /// <param name="TemplateMessage"></param>
        /// <param name="ChannelAccessToken">ChannelAccessToken</param>
        /// <returns></returns>
        public string ReplyTemplateMessage(string ReplyToken, ConfirmTemplate TemplateMessage, string ChannelAccessToken)
        {
            string text = "\r\n{{\r\n    'replyToken': '{0}',\r\n    'messages':[\r\n       {1}\r\n    ]\r\n}}\r\n";
            try
            {
                string text2 = "\r\n{{\r\n 'type': 'template',\r\n  'altText': '{0}',\r\n  'template': {{\r\n      'type': 'confirm',\r\n      'text': '{1}',\r\n      'actions': {2}\r\n  }}\r\n}}       \r\n                    ";
                if (TemplateMessage == null)
                {
                    throw new Exception("TemplateMessage不得為null.");
                }
                if (TemplateMessage.actions == null || TemplateMessage.actions.Count < 1 || TemplateMessage.actions.Count > 2)
                {
                    throw new Exception("actions數量必須是1-2之間");
                }
                foreach (TemplateActionBase action in TemplateMessage.actions)
                {
                    if (action.GetType().Equals(typeof(UriAction)))
                    {
                        UriAction uriAction = action as UriAction;
                        if (uriAction.uri == null)
                        {
                            throw new Exception("uriAction 中的 Url不得為null.");
                        }
                    }
                }
                string text3 = "";
                text3 = JsonConvert.SerializeObject(TemplateMessage.actions);
                text2 = text2.Replace("'", "\"");
                text2 = string.Format(text2, TemplateMessage.altText, TemplateMessage.text, text3);
                text = text.Replace("'", "\"");
                text = string.Format(text, ReplyToken, text2);
                WebClient webClient = new WebClient();
                webClient.Headers.Clear();
                webClient.Headers.Add("Content-Type", "application/json");
                webClient.Headers.Add("Authorization", "Bearer " + ChannelAccessToken);
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] bytes2 = webClient.UploadData("https://api.line.me/v2/bot/message/reply", bytes);
                return Encoding.UTF8.GetString(bytes2);
            }
            catch (WebException ex)
            {
                _TransactionLogRepository.Create(TransactionLogConstants.LineAPI, "Error: API input", $"ReplyTemplateMessage(confirm) API ERROR", ex.ToString(), SpecialEditorConstants.LineAPI);

                using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string text4 = streamReader.ReadToEnd();
                    throw new Exception("ReplyTemplateMessage(ConfirmTemplate) API ERROR: " + text4, ex);
                }
            }
        }

    }
}
