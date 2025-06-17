using System;
namespace WCA.Models
{
    public class Email_DataModel
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Encryption { get; set; }
        public string Incomingport { get; set; }
        public string Outgoingport { get; set; }
        public string Message { get; set; }
        public string SenderEmail { get; set; }
        public string CopyEmail { get; set; }
    }
}

