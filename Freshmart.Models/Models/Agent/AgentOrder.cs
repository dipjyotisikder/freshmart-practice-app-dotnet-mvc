using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Models
{
    public class AgentOrder
    {
        public int Id { get; set; }

        public int AgentId { get; set; }
        [ForeignKey("AgentId")]
        public Agent Agent { get; set; }


        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }


        public bool IsPaid { get; set; }
        public bool IsOnRoute { get; set; }
        public bool IsFullyCompleted { get; set; }
    }
}
