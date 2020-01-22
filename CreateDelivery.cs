using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestAssignment
{
    public class CreateDelivery
    {
        public string Create(int postalCode, List<Product> products)
        {
            var delivery = new Delivery();
            delivery.Data = new List<Data>();
            delivery.Status = Status.Failure;

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Converters = {new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)}            
            };
            //serializeOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));


            if (!IsValidPostalCode(postalCode))
            {                
                return JsonSerializer.Serialize(delivery, serializeOptions);
            }

            var posibleDates = GetDateRange();
            var workingDates = new List<DateTime>();

            foreach (var date in posibleDates)
            {
                if (IsWorkingDate(date, products))
                {
                    workingDates.Add(date);
                }
            }

            if (workingDates.Count > 0 )
            {
                delivery.Status = Status.Success;
            }
            
            CheckSorting(workingDates);
            foreach (var date in workingDates)
            {
                var data = new Data();
                data.PostalCode = postalCode;
                data.DeliveryDate = date;
                data.IsGreenDelivery = IsGreen(data.DeliveryDate);
              //  data.day = date.DayOfWeek;

                delivery.Data.Add(data);
            }
            
            
           
            var jsonString = JsonSerializer.Serialize(delivery, serializeOptions);

            return jsonString;
        }

        private bool IsValidPostalCode(int postalCode)
        {
            return postalCode >= 10000 && postalCode <= 99999;
        }

        private bool IsGreen(DateTime date)
        {
            return date.Day == 5 || date.Day == 15 || date.Day == 25;
        }

        private bool IsWorkingDate(DateTime date, List<Product> products)
        {
            var isworkingDay = true;
            foreach (var item in products)
            {
               if (IsGoodWeekday(date, item) == false)
               {
                   return false;
               }

                if (IsGoodType(date, item) == false)
                {
                   return false;
                }

               if (date < DateTime.Today.AddDays(item.DaysInAdvance) )
               {
                   return false;
               }

            }
            return isworkingDay;
        }

        private bool IsGoodWeekday(DateTime date, Product product)
        {
            var isGoodDay = false;
            foreach (var day in product.DeliveryDays)
            {
                if (date.DayOfWeek == day)
                {
                    isGoodDay = true;
                }
            }
            return isGoodDay;
        }

        private bool IsGoodType(DateTime date, Product product)
        {
            var isGoodType = true;

            //  external products need to be ordered 5 days in in advance.
            // - temporary products can only be ordered within the current week (Mon-Sun)
            if( (product.ProductType == ProductType.External && date < DateTime.Today.AddDays(5) ) ||
                (product.ProductType == ProductType.Temporary && IsInTheSameWeek(DateTime.Today, date) == false))
            {
                isGoodType = false;
            }

            return isGoodType;
        }

        private IEnumerable<DateTime> GetDateRange()
        {
            var dates = new List<DateTime>();
            var startDate = DateTime.Today.AddDays(1);
            var endDate = DateTime.Today.AddDays(14);

            for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            return dates;
        }
        private bool IsInTheSameWeek(DateTime date1, DateTime date2)
        {
            var d1 = date1.AddDays(-1 * ((int)date1.DayOfWeek == 0 ? 7 : (int)date1.DayOfWeek) +1);
            var d2 = date2.AddDays(-1 * ((int)date2.DayOfWeek == 0 ? 7 : (int)date2.DayOfWeek) +1);

            return d1.Date == d2.Date;
        }

        private void CheckSorting(List<DateTime> dates)
        {
            var datesToMove = new List<int>();

            for (int i = 0; i < dates.Count; i++)
            {
                if (dates[i] <= DateTime.Today.AddDays(3) && IsGreen(dates[i]))
                {
                    datesToMove.Add(i);
                }
            }

            datesToMove.Reverse();
            foreach (var index in datesToMove)
            {
                DateTime date = dates[index];
                dates.RemoveAt(index);
                dates.Insert(0, date);
            }
          
        }


        private class Delivery
        {
            public Status Status { get; set; }
            [JsonPropertyName("deliveryDates")]
            public List<Data> Data {get; set;}
        }

        private class Data
        {
            public int PostalCode {get; set;}
            public DateTime DeliveryDate {get; set;}

            public bool IsGreenDelivery {get; set;}

         //   public DayOfWeek day { get; set; }

        }


        private  enum Status {Success, Failure, Canceled, Pending}


    }
}