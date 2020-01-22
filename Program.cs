using System;
using System.Collections.Generic;

namespace TestAssignment
{
    class Program
    {
        static void Main(string[] args)
        {

            var plist1 = new List<DayOfWeek>() {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday};
            var plist2 = new List<DayOfWeek>() {DayOfWeek.Thursday, DayOfWeek.Saturday};

            var product1 = new Product {ProductId = 1, 
            Name = "Product1", DeliveryDays = plist1,
            ProductType = ProductType.Normal, DaysInAdvance = 0 };

            var product2 = new Product {ProductId = 2, 
            Name = "Product2", DeliveryDays = plist1,
            ProductType = ProductType.Normal, DaysInAdvance = 0 };

            var product3 = new Product {ProductId = 3, 
            Name = "Product3", DeliveryDays = plist1};

            var product4 = new Product {ProductId = 4, 
            Name = "Product4", DeliveryDays = plist1,
            ProductType = ProductType.Temporary, DaysInAdvance = 0 };

            var productList = new List<Product>() {product4};

            var createDelivery = new CreateDelivery();

            var json = createDelivery.Create(13567, productList);

            System.Console.WriteLine(json);
        }
    }
}
