using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEmailer
{
    public class Emailer
    {
        #region :Properties
        private string _email_from { get; set; }
        private string _email_to { get; set; }
        private string _email_bcc { get; set; }
        private string _email_cc { get; set; }
        private string _email_body { get; set; }
        private string _email_subject { get; set; }
        #endregion

        #region :MX Configuration
        private readonly string _mx_email = ""; //
        private readonly string _mx_emailpwd = "";
        private readonly string _mx_smtpclient = ""; //dev:smtpout.secureserver.net | Live:relay-hosting.secureserver.net
        private readonly int _mx_smtpnum = 80; //Dev:80 | Live:25
        private readonly bool _mx_ssl = true;
        private readonly string _mx_displayname = "Test Email";
        #endregion

        #region :Constructor
        public Emailer(string email_from, string email_to, string email_subject, string email_body_html, string email_cc, string email_bcc)
        {
            _email_from = email_from;
            _email_to = email_to;
            _email_subject = email_subject;
            _email_body = email_body_html;
            _email_cc = email_cc;
            _email_bcc = email_bcc;
        }
        #endregion

        #region :Send Email
        /// <summary>
        /// Send email with optional attachments from an array of files paths. 
        /// Suitable for any platform i.e. html, mvc, webform, etc.
        /// Suitable for sending out attachment to customer reading from a folder
        /// </summary>
        /// <param name="optional_attmts">Optional array of attachments paths</param>
        /// <returns>Message, either sent or exception details</returns>
        public string SendEmail(params string[] optional_attmts)
        {
            string s = "";
            try
            {
                using (MailMessage msg = new MailMessage())
                {
                    //Set email values
                    msg.Subject = _email_subject;

                    //string[] emailToList = _email_to.Split(',');
                    //foreach (string emailTo in emailToList)
                    //{
                    //    msg.To.Add(new MailAddress(emailTo));
                    //}
                    msg.To.Add(_email_to);

                    ////Using mailing list
                    //string Bcc = "charles.aryee@hotmail.com,services@ayitech.com";
                    //string[] MailingList = Bcc.Split(',');
                    //foreach (string multiple_email in MailingList)
                    //{
                    //    mailMessage.Bcc.Add(new MailAddress(multiple_email));
                    //}

                    if (_email_cc.Length > 0)
                    {
                        msg.CC.Add(_email_cc);
                    }
                    if (_email_bcc.Length > 0)
                    {
                        msg.Bcc.Add(_email_bcc);
                    }

                    //Configure the address we are sending the mail from   
                    //MailAddress address = new MailAddress(mx_email, mx_displayname);
                    MailAddress address = new MailAddress(_email_from, _mx_displayname);
                    msg.From = address;

                    msg.IsBodyHtml = true;
                    msg.Body = _email_body; //Ex: InquiryHTML_Admin(em.sender_name, em.sender_phone, em.sender_message); //em.email_message;

                    //Multiple File Attachment 
                    if (optional_attmts.Length > 0)
                    {
                        foreach (string fileName in optional_attmts)
                        {
                            msg.Attachments.Add(new Attachment(fileName));
                        }

                    }

                    //Mx settings
                    SmtpClient client = new SmtpClient();
                    client = new SmtpClient(_mx_smtpclient, _mx_smtpnum);
                    client.EnableSsl = _mx_ssl;
                    NetworkCredential credentials = new NetworkCredential(_mx_email, _mx_emailpwd);
                    client.Credentials = credentials;
                    client.Send(msg);

                    //SetMxValues(msg);

                }

                s = "Email Sent.";
                //return true;

            }
            catch (System.Exception ex)
            {
                s = ex.InnerException.ToString();
                //return false;
            }

            return s;
        }

        //Use this appproach if Godaddy hosting
        /// <summary>
        /// Send email with optional attachments from an array of files paths. 
        /// Suitable for any platform i.e. html, mvc, webform, etc.
        /// Suitable for sending out attachment to customer reading from a folder
        /// </summary>
        /// <param name="is_live_host">Set to true when going live usually for godaddy hosting</param>
        /// <param name="optional_attmts">Optional array of attachments paths</param>
        /// <returns>Message, either sent or exception details</returns>
        public string SendEmail(bool is_live_host, params string[] optional_attmts)
        {
            string s = "";
            try
            {
                using (MailMessage msg = new MailMessage())
                {
                    //Set email values
                    msg.Subject = _email_subject;
                    msg.To.Add(_email_to);

                    ////Using mailing list
                    //string Bcc = "charles.aryee@hotmail.com,services@ayitech.com";
                    //string[] MailingList = Bcc.Split(',');
                    //foreach (string multiple_email in MailingList)
                    //{
                    //    mailMessage.Bcc.Add(new MailAddress(multiple_email));
                    //}

                    if (_email_cc.Length > 0)
                    {
                        msg.CC.Add(_email_cc);
                    }
                    if (_email_bcc.Length > 0)
                    {
                        msg.Bcc.Add(_email_bcc);
                    }

                    //Configure the address we are sending the mail from   
                    //MailAddress address = new MailAddress(mx_email, mx_displayname);
                    MailAddress address = new MailAddress(_mx_email, _mx_displayname);
                    msg.From = address;

                    msg.IsBodyHtml = true;
                    msg.Body = _email_body; //Ex: InquiryHTML_Admin(em.sender_name, em.sender_phone, em.sender_message); //em.email_message;

                    //Multiple File Attachment 
                    if (optional_attmts.Length > 0)
                    {
                        foreach (string fileName in optional_attmts)
                        {
                            msg.Attachments.Add(new Attachment(fileName));
                        }

                    }

                    //Mx settings                    
                    SmtpClient client = new SmtpClient();
                    if (is_live_host)
                    {
                        //Ref: http://vandelayweb.com/sending-asp-net-emails-godaddy-gmail-godaddy-hosted/
                        client.Host = _mx_smtpclient; //"relay-hosting.secureserver.net";
                        client.Port = _mx_smtpnum; //25;
                    }
                    else
                    {
                        client = new SmtpClient(_mx_smtpclient, _mx_smtpnum);
                        client.EnableSsl = _mx_ssl;
                        NetworkCredential credentials = new NetworkCredential(_mx_email, _mx_emailpwd);
                        client.Credentials = credentials;
                    }

                    client.Send(msg);

                }

                s = "Email Sent.";
                //return true;

            }
            catch (System.Exception ex)
            {
                s = ex.InnerException.ToString();
                //return false;
            }

            return s;
        }
        #endregion
    }
}
