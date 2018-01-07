using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashDesk.Model
{
    class Deposit:IDeposit
    {
        [Required]
        public IMembership Membership { get; set; }

        [Required]
        // greater than 0----------------------
        public decimal Amount { get; set; }
    }
}
