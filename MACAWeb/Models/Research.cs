using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Research
    {
        public IEnumerable<Interest> interest { get; set; }
        public IEnumerable<ConferenceTalk> conferenceTalk { get; set; }
        public IEnumerable<SeminarTalk> seminarTalk { get; set; }

    }
}