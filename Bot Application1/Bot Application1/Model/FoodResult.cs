using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.Model
{
    [Serializable]
    public class FoodResult
    {
        public List<AvailableFood> AvailableFood;
    }

    [Serializable]
    public class AvailableFood
    {
        public int Id { get; set; }

        public string Location { get; set; }

        public DateTime Date { get; set; }

        public string Dishes { get; set; }

        public decimal Price { get; set; }

        public string ImageURL { get; set; }

        public bool IsDailySpecial { get; set; }

        public string Kitchen { get; set; }

        public int Calories { get; set; }

        public string Allergen { get; set; }

        public string MenuUrl { get; set; }

        public bool IsDailyDish { get; set; }
    }
}