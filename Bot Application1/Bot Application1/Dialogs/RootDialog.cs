using Bot_Application1;
using Bot_Application1.Dialogs;
using Bot_Application1.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotApplication1
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        List<string> welcomeOptionList = new List<string> { "Menue", "Allergies" };

        public async Task StartAsync(IDialogContext context)
        {
            /* Wait until the first message is received from the conversation and call MessageReceivedAsync
               to process that message. */
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await this.SendWelcomeMessageAsync(context); // Alternative for multiple options
        }

        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            PromptDialog.Choice(
                context: context,
                resume: this.SendWelcomeMessageResumeAfter,
                options: welcomeOptionList,
                prompt: "Hi, do you want to know the menu or give me any allergies to consider?",
                retry: "Unfortunately this option is not available",
                attempts: 2);
        }

        private async Task SendWelcomeMessageResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var selectedOption = await result;

                switch (selectedOption)
                {
                    case "Menue":
                        context.Call(new MenuDialog(), this.MenueDialogResumeAfter);
                        break;
                    case "Allergies":
                        context.Call(new AllergyDialog(), this.AllergyDialogResumeAfter);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                await context.PostAsync("Try again");

                await this.StartAsync(context);
            }
        }

        private async Task AllergyDialogResumeAfter(IDialogContext context, IAwaitable<List<string>> result)
        {
            // Get result
            List<string> allergies = await result;

            // Save Allergies
            context.ConversationData.SetValue("Allergies", allergies);

            // Forward to Menu selection
            context.Call(new MenuDialog(), this.MenueDialogResumeAfter);
        }

        private async Task MenueDialogResumeAfter(IDialogContext context, IAwaitable<FoodResult> result)
        {
            var menuResponse = await result;

            var finalMessage = context.MakeMessage();
            finalMessage.Recipient = context.MakeMessage().From;
            finalMessage.Type = "message";
            finalMessage.Attachments = new List<Attachment>();
            finalMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            finalMessage.TextFormat = TextFormatTypes.Markdown;

            if (menuResponse.AvailableFood.Count > 0)
            {
                foreach (var dish in menuResponse.AvailableFood)
                {
                    // Check for allergies
                    if (DishIncludesAllergy(dish, context))
                        continue;

                    CardAction linkButton = new CardAction()
                    {
                        Title = "Link to menu",
                        Type = "openUrl",
                        Value = dish.MenuUrl
                    };

                    var cardText = "";

                    if (dish.Allergen != "")
                    {
                        cardText = $"Price: {dish.Price.ToString("0.00")}€ | Calories: {dish.Calories} kcal | Allergenes: {dish.Allergen}";
                    }
                    else
                    {
                        cardText = $"Price: {dish.Price.ToString("0.00")}€ | Calories: {dish.Calories} kcal";

                    }

                    var foodImage = dish.ImageURL == "" ?
                        "https://thumbs.dreamstime.com/t/italian-food-meals-set-products-other-elements-cuisine-50899935.jpg" :
                        dish.ImageURL;

                    if (dish.IsDailySpecial) { cardText = "(*)  " + cardText; }

                    HeroCard resultCard = new HeroCard()
                    {
                        Images = new List<CardImage> { new CardImage(foodImage) },
                        Title = dish.Dishes,
                        Text = cardText,
                        Subtitle = dish.Location,
                        Buttons = new List<CardAction> { linkButton }
                    };

                    Attachment cardAttachment = resultCard.ToAttachment();
                    finalMessage.Attachments.Add(cardAttachment);
                }

                await context.PostAsync(finalMessage);
                await this.StartAsync(context);
            }
            else
            {
                await context.PostAsync("No matching dishes found. Either loosen your filter or order from [Foodora](https://www.foodora.com/).");
                await this.StartAsync(context);
            }

            
        }

        /// <summary>
        /// Check the dish, if the included allergies collide with the users allergies.
        /// </summary>
        /// <param name="dish">The current dish to be analysed.</param>
        /// <param name="context">The message context, containing the stored allergies</param>
        /// <returns>Bool, if the dish contains one of the users allergies.</returns>
        private bool DishIncludesAllergy(AvailableFood dish, IDialogContext context)
        {
            try
            {
                var userAllergyList = context.ConversationData.Get<List<string>>("Allergies");

                foreach (var allergy in userAllergyList)
                {
                    if (dish.Allergen.Contains(allergy.ToLower()))
                        return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
                
        }
    }
}
