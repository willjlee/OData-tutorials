﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Odata.Models
{
    public class Product
    {
        public int ID { get; set; } //key
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        [ForeignKey("Supplier")]
        public string SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}