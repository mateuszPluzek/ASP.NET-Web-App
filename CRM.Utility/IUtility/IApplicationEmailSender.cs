using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Utility.IUtility
{
    public interface IApplicationEmailSender
    {
        Task SendEmailAsync(MailMessage input);
    }
}
