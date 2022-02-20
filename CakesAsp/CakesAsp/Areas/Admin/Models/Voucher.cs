using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CakesAsp.Areas.Admin.Models
{
    public class Voucher
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public string VoucherName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Limit { get; set; }
        public bool Status { get; set; }
    }
}
