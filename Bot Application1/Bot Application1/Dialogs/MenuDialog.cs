using Bot_Application1.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



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