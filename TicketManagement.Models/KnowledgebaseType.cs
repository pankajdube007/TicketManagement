﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models
{
    [Table("KnowledgebaseType")]
    public class KnowledgebaseType
    {
        [Key]
        public int KnowledgebaseTypeId { get; set; }
        public string KnowledgebaseTypeName { get; set; }
    }
}
