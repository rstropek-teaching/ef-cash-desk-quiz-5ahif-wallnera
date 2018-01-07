using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashDesk.Model
{
    class Membership:IMembership
    {
        [Required]
        public IMember Member { get; set; }

        [Required]
        public DateTime Begin { get; set; }
         
        //greater than Begin ----------------
        public DateTime End { get; set; }
    }
}
