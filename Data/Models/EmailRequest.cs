using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class EmailRequest
    {
        public string[] Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
