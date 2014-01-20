using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarshipManagementSystem.Models
{
    public class PunishmentDTO
    {
        public int Id { get; set; }
        public String SId { get; set; }
        public String Name { get; set; }
        public String Class { get; set; }
        public String Type { get; set; }
        public String Date { get; set; }
        public String Notes { get; set; }
        public bool Qualification { get; set; }
    }
}
