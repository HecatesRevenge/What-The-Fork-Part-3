using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WhatTheForkMVC.Classes.Emails
{
    public class Email
    {
        private MailMessage objMail = new MailMessage();
        private MailAddress toAddress;
        private MailAddress fromAddress;
        private String subject;
        private String messageBody;
        private bool isHTMLBody = true;
        private MailPriority priority = MailPriority.Normal;
        private String mailHost = "smtp.temple.edu";

        public Email()
        {
        }

        public void SendMail(String recipient, String sender, String subject, String body)
        {
            this.Recipient = recipient;
            this.Sender = sender;
            this.Subject = subject;
            this.Message = body;

            try
            {
                objMail.To.Add(this.toAddress);
                objMail.From = this.fromAddress;
                objMail.Subject = this.subject;
                objMail.Body = this.messageBody;
                objMail.IsBodyHtml = this.isHTMLBody;
                objMail.Priority = this.priority;

                SmtpClient smtpMailClient = new SmtpClient(this.mailHost);

                smtpMailClient.Timeout = 10000;
                smtpMailClient.Send(objMail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Recipient
        {
            get { return this.toAddress.Address; }
            set { this.toAddress = new MailAddress(value); }
        }

        public String Sender
        {
            get { return this.fromAddress.Address; }
            set { this.fromAddress = new MailAddress(value); }
        }

        public String Subject
        {
            get { return this.subject; }
            set { this.subject = value; }
        }

        public String Message
        {
            get { return this.messageBody; }
            set { this.messageBody = value; }
        }

        public bool HTMLBody
        {
            get { return this.isHTMLBody; }
            set { this.isHTMLBody = value; }
        }

        public MailPriority Priority
        {
            get { return this.priority; }
            set { this.priority = value; }
        }

        public String MailHost
        {
            get { return this.mailHost; }
            set { this.mailHost = value; }
        }
    }
}
