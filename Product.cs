using System;
using System.Collections.Generic;

namespace TestAssignment
{
    public class Product
    {

        public int ProductId { get; set; }
        public string Name { get; set; }
        public IEnumerable<DayOfWeek> DeliveryDays { get; set; }
        public ProductType ProductType { get; set; } = ProductType.Normal;
        public int DaysInAdvance { get; set; } = 0;
    }
}