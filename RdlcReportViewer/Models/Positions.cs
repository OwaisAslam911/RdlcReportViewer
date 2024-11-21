using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RdlcReportViewer.Models
{
    public class Positions
    {
        public int PositionId { get; set; }
        public string PositionTitle { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}