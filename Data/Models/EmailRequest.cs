﻿namespace Data.Models
{
    public class EmailRequest
    {
        public string[] Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
