using SYS.Model.Constants;
using SYS.Web.Session;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.SessionState; // 使用 Session 所以會用到


namespace DarfonTemplate_MVC.Services
{
    /// <summary>
    /// Summary description for ValidatePicture
    /// </summary>
    public class ValidatePicture : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int NumCount = 5;  // 預設產生5位亂數

            if (!string.IsNullOrEmpty(context.Request.QueryString["NumCount"]))
            {   // 您也可以透過網址傳遞數值，例如 ValidateCode.ashx?NumCount=5  指定產生幾位數
                // 字串轉數字，轉型成功則儲存到 NumCount。不成功的話，NumCount為0
                Int32.TryParse(context.Request.QueryString["NumCount"].Replace("'", "''"), out NumCount);
            }

            if (NumCount <= 5)
            {
                NumCount = 5;
            }
            // 取得亂數 -- 自己寫的副程式 GetRandomNumberString
            string str_ValidatePictureCode = GetRandomNumberString(NumCount);
            //**** 用於驗證的Session ***********************
            SessionManager.ImageCode = str_ValidatePictureCode;
            //**********************************************
            Image image = CreateCheckCodeImage(str_ValidatePictureCode);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            // 輸出圖片，呈現在網頁（.aspx檔）上的 <asp:Image>或 <Img>
            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(ms.ToArray());
            ms.Close();
        }

        #region 產生亂數 

        private string GetRandomNumberString(int int_NumberLength)
        {
            System.Text.StringBuilder str_resultNumber = new System.Text.StringBuilder();
            Random rand = new Random(Guid.NewGuid().GetHashCode());  // 亂數物件(Salt)            

            // === 英文大小寫與數字。===================
            // 資料來源 https://blog.kkbruce.net/2013/11/asp-net-mvc-captcha.html
            //                 http://joeshua.pixnet.net/blog/post/36256920

            string allChar = CodeBase.CodeBaseCaptcha;
            for (int i = 0; i < int_NumberLength; i++)
            {
                int t = rand.Next(allChar.Length);  // 必須放在迴圈裡面，每次執行都會產生新的亂數！
                str_resultNumber.Append(allChar.Substring(t, 1));   // 產生的亂數，組合成字串。
            }

            //== 共用，輸出結果（亂數文字）================
            return str_resultNumber.ToString();
        }
        #endregion

        #region 產生圖片

        private Image CreateCheckCodeImage(string checkCode)
        {
            Bitmap image = new Bitmap((checkCode.Length * 32), 38);
            // 產生圖片，寬32（自動依照文字長度，增加寬度），高38像素
            Graphics g = Graphics.FromImage(image);

            // 隨機生成器
            Random random = new Random(Guid.NewGuid().GetHashCode());

            //int int_Red = 0;
            //int int_Green = 0;
            //int int_Blue = 0;
            int int_Red = random.Next(240, 256);  // 產生0~255    // 必須放在迴圈裡面，每次執行都會產生新的亂數！
            int int_Green = random.Next(240, 256);  // 產生0~255
            int int_Blue = (int_Red + int_Green > 400 ? 150 : 400 - int_Red - int_Green);
            //int_Blue = (int_Blue > 255 ? 255 : int_Blue);
            int int_bkack = random.Next(150, 200);

            // 清空圖片背景色
            g.Clear(Color.FromArgb(int_Red, int_Green, int_Blue));
            int blackbg = random.Next(0, 120);   // 必須放在迴圈裡面，每次執行都會產生新的亂數！
            int garykbg = random.Next(120, 160);

            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            //Brush brushBack = new LinearGradientBrush(rect, Color.FromArgb(random.Next(0, 100), random.Next(230, 256), 255),
            //Color.FromArgb(255, random.Next(200, 256), 255), random.Next(45));

            // 新增黑白漸層 
            Brush brushBack = new LinearGradientBrush(rect, Color.FromArgb(int_bkack, int_bkack, int_bkack),
            Color.FromArgb(255, 255, 255), 255);
            g.FillRectangle(brushBack, rect);

            //畫圖片的背景噪音線
            for (int i = 0; i <= 6; i++)
            {
                int x1 = random.Next(image.Width);   // 必須放在迴圈裡面，每次執行都會產生新的亂數！
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                int b1 = random.Next(70, 150);
                int b2 = random.Next(40, 80);
                //Color line1 = Color.FromArgb(random.Next(0, 100), random.Next(220, 255), random.Next(220, 255));
                //Color line2 = Color.FromArgb(random.Next(0, 100), 255, random.Next(200, 240));

                Color line1 = Color.FromArgb(b1, b1, b1);
                Color line2 = Color.FromArgb(b2, b2, b2);

                g.DrawLine(new Pen(line1), x1, y1, x2, y2);
                g.DrawEllipse(new Pen(line2), new Rectangle(x1, y1, x2, y2));
            }

            for (int i = 0; i < checkCode.Length; i++)
            {
                int Cr = 1, Cg = 1, Cb = 1;
                while (Cr + Cg + Cb != 1)
                {
                    Cr = random.Next(0, 2);   // 必須放在迴圈裡面，每次執行都會產生新的亂數！
                    Cg = random.Next(0, 2);
                    Cb = random.Next(0, 2);
                }
                //// 增強對比
                //while (Cr == Cg && Cg == Cb)   {
                //    Cr = random.Next(0, 2);
                //    Cg = random.Next(0, 2);
                //    Cb = random.Next(0, 2); 
                //}                    
                //// 字體顏色
                Color wc = Color.FromArgb(Cr * 255, Cg * 130, Cb * 255);
                Color wc2 = Color.FromArgb(Cb * 255, Cr * 130, Cg * 255);

                int y = random.Next(0, 6);   // 必須放在迴圈裡面，每次執行都會產生新的亂數！
                // 字體 Size 如果設定太大可能出錯，無法產生圖片
                Font font = new Font("Tahoma", 15 + y, FontStyle.Italic);   // 字型、字體大小、粗細
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), wc, wc2, 1.2F, true);

                // 設置繪筆 
                g.DrawString(checkCode.Substring(i, 1), font, brush, 8 + i * 25, 2 + random.Next(0, 6 - y));
            }

            for (int i = 0; i <= 35; i++)
            {   // 圖片的 "前景" 噪音點
                int x = random.Next(image.Width);     // 必須放在迴圈裡面，每次執行都會產生新的亂數！
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            // 圖片的邊框線
            g.DrawRectangle(new Pen(Color.DarkGray, 2), 2, 2, image.Width - 5, image.Height - 5);

            return image;
        }
        #endregion

        // 泛型處理常式（.ashx檔）內建的，不可以刪除！
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}