using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalsheBogaNet.Mvvm.Model
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; } = "0";
        public string Description { get; set; } = "0";
        public decimal Price { get; set; } = 0;
        public double Size { get; set; } = 0;
        public bool Breakable { get; set; } = false;
        public int Postavhik {  get; set; } = 1;
        public int Amount { get; set; } = 0;
        public string Code { get; set; }
        public DateTime Time { get; set; }
    }
}
