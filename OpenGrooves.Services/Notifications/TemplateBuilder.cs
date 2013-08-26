using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Net.Mail;
using OpenGrooves.Core;

namespace OpenGrooves.Services.Notifications
{
    public static class TemplateBuilder
    {
        public static MailMessage LoadTemplate(NotificationType type)
        {
            #region Load Template XML
            var xml = new XmlDocument();
            using (var reader = new XmlTextReader(System.Web.HttpContext.Current.Server.MapPath("~/static/templates.xml")))
            {
                xml.Load(reader); 
            }

            var shellHtml = xml.SelectSingleNode("/templates/template[@name='Master']/html").InnerText;
            var temp = xml.SelectSingleNode("/templates/template[@name='" + type.ToString() + "']");
            #endregion

            #region Do replacements
            

            var subject = temp.SelectSingleNode("subject").InnerText;
            var body = temp.SelectSingleNode("html").InnerText;

            var html = ReplaceTokens(shellHtml, new Dictionary<string, string> { { "BODY", body } }); 
            #endregion

            #region Create Message
            var msg = new MailMessage();
            msg.Subject = subject;
            msg.Body = html;
            msg.IsBodyHtml = true; 
            #endregion

            return msg;
        }

        public static MailMessage LoadTemplate(NotificationType type, IDictionary<string, string> data)
        {
            var msg = LoadTemplate(type);

            msg.Body = ReplaceTokens(msg.Body, data);

            return msg;
        }
        public static string ReplaceTokens(string html, IDictionary<string, string> data)
        {
            data.ToList().ForEach(d =>
            {
                var key = d.Key;

                // pad if [[ ]] aren't there
                if (!key.StartsWith("[[") || !key.EndsWith("]]"))
                {
                    key = "[[" + key + "]]";
                }

                html = html.Replace(key, d.Value);
            });

            return html;
        }
    }
}