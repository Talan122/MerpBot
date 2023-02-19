using System;

namespace MerpBot.Destiny.ResponseTypes.Applications
{
    public class Application
    {
        public Int32 applicationId { get; set; }
        public string name { get; set; }
        public string redirectUrl { get; set; }
        public string link { get; set; }
        public Int64 scope { get; set; }
        public string origin { get; set; }
        public Int32 status { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime statusChanged { get; set; }
        public DateTime firstPublished { get; set; }
        public ApplicationDeveloper[] team { get; set; }
        public string overrideAuthorizeViewName { get; set; }
    }
}
