using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel {
    public class Product {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemsLeft { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
