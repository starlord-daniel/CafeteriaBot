using Bot_Application1.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Linq;



namespace Bot_Application1
{
    [Serializable]
    public class MenuDialog : IDialog<FoodResult>
    {
        private LuisResult luisResult;

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("What do you want for lunch");

            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            var TextToLuis = (await argument).Text;

            luisResult = await LuisApi.GetLuisResult(TextToLuis);

            await this.HandleLuisMessage(context);
        }

        private async Task HandleLuisMessage(IDialogContext context)
        {
            List<AvailableFood> foodResult = new List<AvailableFood>();

            var foodOptions = SqlConnector.GetDishes();

            switch (luisResult.topScoringIntent.intent)
            {
                // Just show menu for selected food type (e.g. Italian)
                case "menueLookUp.intent.showMenue":
                    {
                        var entity = (from l in luisResult.entities where l.type == "food" select l).FirstOrDefault();
                        // var entity = luisResult.entities.Where(x => x.type == "food").FirstOrDefault();

                        if (entity != null)
                        {
                            foodResult = foodOptions.Where(x => x.Kitchen.ToLower() == entity.entity).ToList();
                        }
                        else
                        {
                            foodResult = foodOptions.Where(x => x.IsDailySpecial == true).ToList();
                        }
                    }
                    break;
                case "menueLookUp.intent.showCosts":
                    {
                        var entity = (from l in luisResult.entities where l.type == "highestAmount" select l).FirstOrDefault();

                        if (entity != null)
                        {
                            foodResult = foodOptions.Where(x => x.Price < Convert.ToDecimal(entity.entity)).ToList();
                        }
                        else
                        {
                            foodResult = foodOptions.Where(x => x.IsDailySpecial == true).ToList();
                        }
                    }
                    break;
                case "menueLookUp.intent.showCalories":
                    {
                        var entity = (from l in luisResult.entities where l.type == "calories" select l).FirstOrDefault();

                        if (entity != null)
                        {
                            foodResult = foodOptions.Where(x => x.Calories < Convert.ToDecimal(entity.entity)).ToList();
                        }
                        else
                        {
                            foodResult = foodOptions.Where(x => x.IsDailySpecial == true).ToList();
                        }
                    }
                    break;
                default:
                    {

                        foodResult = foodOptions.Where(x => x.IsDailySpecial == true).ToList();
                    }
                    break;
            }

            context.Done(new FoodResult { AvailableFood = foodResult });
        }

        public async Task MainDish(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I am looking for your daily special!");
        }
    }
}

//private FoodResult foodOptions = new FoodResult
//{
//    AvailableFood = new List<AvailableFood>
//        {
//                new AvailableFood
//                {
//                    Id  = 1,
//                    Location = "Canteen",
//                    Date = new DateTime(2017,03,17),
//                    Dishes = "Haddock with herb and mustard sauce",
//                    Price = 3.7m,
//                    IsDailySpecial = false,
//                    Kitchen = "Seafood"
//                },
//                new AvailableFood
//                {
//                    Id  = 2,
//                    Location = "Canteen",
//                    Date = new DateTime(2017,03,17),
//                    Dishes = "Swabian bread pudding with almonds, raisins and vanilla custard",
//                    Price = 3.4m,
//                    IsDailySpecial = false,
//                    Kitchen = "Swabian"
//                },
//                new AvailableFood
//                {
//                    Id  = 3,
//                    Location = "Canteen",
//                    Date = new DateTime(2017,03,17),
//                    Dishes = "Whole roast pork tenderloin with mixed beans and paprika-aioli dip",
//                    Price = 6.8m,
//                    IsDailySpecial = false,
//                    Kitchen = "German"
//                },
//                new AvailableFood
//                {
//                    Id  = 4,
//                    Location = "Canteen",
//                    Date = new DateTime(2017,03,17),
//                    Dishes = "Carrot and orange soup",
//                    Price = 0.5m,
//                    IsDailySpecial = false,
//                    Kitchen = "Soup"
//                },
//                new AvailableFood
//                {
//                    Id  = 5,
//                    Location = "Canteen",
//                    Date = new DateTime(2017,03,17),
//                    Dishes = "Ras el Hanout-spiced chicken breast with chili-vanilla-kraut and steamed rice",
//                    Price = 4.3m,
//                    IsDailySpecial = true,
//                    Kitchen = "Arabian"
//                },
//                new AvailableFood
//                {
//                    Id  = 6,
//                    Location = "Theaterhaus",
//                    Date = new DateTime(2017,03,17),
//                    Dishes = "Homemade Ravioli with pesto and parmesan",
//                    Price = 7.2m,
//                    IsDailySpecial = false,
//                    Kitchen = "Italian"
//                },
//                new AvailableFood
//                {
//                    Id  = 7,
//                    Location = "Theateraus",
//                    Date = new DateTime(2017,03,17),
//                    Dishes = "Pulled Pork Burger with lettuce and onionrings",
//                    Price = 8.6m,
//                    IsDailySpecial = false,
//                    Kitchen = "American"
//                },
//                new AvailableFood
//                {
//                    Id  = 8,
//                    Location = "Theaterhaus",
//                    Date = new DateTime(2017,03,17),
//                    Dishes = "Spaguetti Bolognese with parmesan",
//                    Price = 6.4m,
//                    IsDailySpecial = false,
//                    Kitchen = "Italian"
//                },
//                new AvailableFood
//                {
//                    Id  = 9,
//                    Location = "Theaterhaus",
//                    Date = new DateTime(2017,03,17),
//                    Dishes = "sirloin steak with beans and wedges",
//                    Price = 12.1m,
//                    IsDailySpecial = true,
//                    Kitchen = "American"
//                },
//                new AvailableFood
//                {
//                    Id  = 10,
//                    Location = "Canteen",
//                    Date = new DateTime(2017,03,17),
//                    Dishes = "Gyros with metaxa sauce and fries",
//                    Price = 8.60m,
//                    IsDailySpecial = false,
//                    Kitchen = "Greek"
//                }
//            }
//};