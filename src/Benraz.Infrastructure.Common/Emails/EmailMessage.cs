namespace Benraz.Infrastructure.Common.Emails
{
    /// <summary>
    /// Email message.
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Message subject.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Message body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Creates message.
        /// </summary>
        public EmailMessage()
        {
        }

        /// <summary>
        /// Creates message.
        /// </summary>
        /// <param name="title">Subject.</param>
        /// <param name="body">Body.</param>
        public EmailMessage(string title, string body)
        {
            Title = title;
            Body = body;
        }
    }
}




