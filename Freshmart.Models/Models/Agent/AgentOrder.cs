using FreshMart.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Models
{
    public class AgentOrder : BaseEntity
    {
        public long AgentId { get; set; }
        [ForeignKey("AgentId")]
        public Agent Agent { get; set; }


        public long OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }


        public bool IsPaid { get; set; }
        public bool IsOnRoute { get; set; }
        public bool IsFullyCompleted { get; set; }
    }
}
