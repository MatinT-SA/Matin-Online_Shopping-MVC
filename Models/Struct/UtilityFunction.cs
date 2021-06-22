using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.WebPages.Html;
using System.Web.Mvc;
using System.Globalization;

public class UtilityFunction
{
    /// <summary>
    /// متد ارسال ایمیل
    /// </summary>
    /// <param name="Smtp">smtp</param>
    /// <param name="From">ایمیل فرستنده</param>
    /// <param name="pass">رمز ایمیل فرستنده</param>
    /// <param name="To">ایمیل گیرنده</param>
    /// <param name="Subject">موضوع ایمیل</param>
    /// <param name="Body">بدنه ایمیل</param>
    public void SendEmail(string Smtp, string From, string Pass, string To, string Subject, string Body)
    {
        MailMessage MyEmail = new MailMessage();

        MyEmail.From = new MailAddress(From);
        MyEmail.To.Add(To);
        MyEmail.Subject = Subject;
        MyEmail.Body = Body;
        MyEmail.IsBodyHtml = true;
        MyEmail.Priority = MailPriority.High;


        SmtpClient mysmtp = new SmtpClient(Smtp);
        mysmtp.UseDefaultCredentials = false;
        mysmtp.EnableSsl = true;
        mysmtp.Port = 587;
        mysmtp.Credentials = new NetworkCredential(From, Pass);
        mysmtp.Send(MyEmail);

    }

    /// <summary>
    /// متد ارسال پیامک
    /// </summary>
    /// <param name="userName">نام کاربری در سامانه</param>
    /// <param name="password">رمز عبور در سامانه</param>
    /// <param name="mobileNo">شماره موبایل دریافت کننده</param>
    /// <param name="messageBody">متن پیامک</param>
    /// <param name="lineNumberID">شماره ارسال کننده</param>
    public bool SendMessage(string userName, string password, string mobileNo, string messageBody, string lineNumberID)
    {
        try
        {
            Sourceiran_MVC.ir.sms.ip.SendReceive sms = new Sourceiran_MVC.ir.sms.ip.SendReceive();
            string error = string.Empty;
            long[] mobile = new long[1];//شماره های دریافت کننده
            string[] text = new string[1];//متن پیامک

            mobile[0] = long.Parse(mobileNo);
            text[0] = messageBody;

            long[] result = sms.SendMessageWithLineNumber(userName, password, mobile, text, messageBody, DateTime.Now, ref error);

            if (error == "" || error == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public DateTime Shamsi(DateTime dt)
    {
        PersianCalendar Date = new PersianCalendar();

        int Day = Date.GetDayOfMonth(dt);
        int Month = Date.GetMonth(dt);
        int Year = Date.GetYear(dt);

        return new DateTime(Year, Month, Day);
    }

    public string Time(string dt, DateTime dt1)
    {
        DateTime T = Convert.ToDateTime(dt);
        var dt2 = T - dt1;

        //باید کاری کنیم که زمان به طور معکوس شروع کند و بخواند
        return dt2.Days + " روز و " + dt2.Hours + " ساعت و " + dt2.Minutes + " دقیقه و ";
    }
}
