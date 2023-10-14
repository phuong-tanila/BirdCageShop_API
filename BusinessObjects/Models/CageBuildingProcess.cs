using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class CageBuildingProcess
    {
        public string Id { get; set; } = null!;
        public int Deposit { get; set; }
        public int Status { get; set; }
        public DateTime StaffRequestDate { get; set; }
        public DateTime? DepositDate { get; set; }
        public DateTime? EstimateNextDepositDate { get; set; }
        public int ProcessComplete { get; set; }
        public int EstimateNumberDayComplete { get; set; }
        public int Stage { get; set; }
        public int Price { get; set; }
        public string OrderDetailId { get; set; } = null!;
    }
}
