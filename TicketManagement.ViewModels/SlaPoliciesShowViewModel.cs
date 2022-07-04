using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TicketManagement.ViewModels
{
    public class SlaPoliciesShowViewModel
    {
        public long? SlaPoliciesId { get; set; }
        public string PriorityName { get; set; }
        public long? FirstResponseDay { get; set; }
        public long? FirstResponseHour { get; set; }
        public long? FirstResponseMins { get; set; }
        public long? EveryResponseDay { get; set; }
        public long? EveryResponseHour { get; set; }
        public long? EveryResponseMins { get; set; }
        public long? ResolutionResponseDay { get; set; }
        public long? ResolutionResponseHour { get; set; }
        public long? ResolutionResponseMins { get; set; }
        public long? EscalationDay { get; set; }
        public long? EscalationHour { get; set; }
        public long? EscalationMins { get; set; }
    }
}
