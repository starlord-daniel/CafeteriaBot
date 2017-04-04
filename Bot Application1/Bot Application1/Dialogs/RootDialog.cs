using Bot_Application1;
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
        public async Task StartAsync(IDialogContext context)
        {
            /* Wait until the first message is received from the conversation and call MessageReceivedAsync
               to process that message. */
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            /* When MessageReceivedAsync is called, it's passed an IAwaitable<IMessageActivity>. To get the message,
               await the result. */
            var message = await result;

            context.Call(new MenuDialog(), this.MenueDialogResumeAfter);
            //await this.SendWelcomeMessageAsync(context); // Alternative for multiple options
        }

        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            context.Call(new MenuDialog(), this.MenueDialogResumeAfter);
            //PromptDialog.Choice(
            //    context: context,
            //    resume: this.SendWelcomeMessageResumeAfter,
            //    options: new List<string> { "Menüs" },
            //    prompt: "Hi, please tell me what you want to eat today",
            //    retry: "Unfortunately this option is not available",
            //    attempts: 2);
        }

        private async Task SendWelcomeMessageResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var selectedOption = await result;

                context.Call(new MenuDialog(), this.MenueDialogResumeAfter);

            }
            catch (Exception)
            {
                await context.PostAsync("Try again");

                await this.StartAsync(context);
            }
        }

        private async Task MenueDialogResumeAfter(IDialogContext context, IAwaitable<FoodResult> result)
        {
            var menuResponse = await result;

            var m = context.MakeMessage();
            m.Recipient = context.MakeMessage().From;
            m.Type = "message";
            m.Attachments = new List<Attachment>();
            m.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            foreach (var dish in menuResponse.AvailableFood)
            {
                CardAction linkButton = new CardAction()
                {
                    Title = "Url",
                    Type = "openUrl",
                    Value = "Link to Menu"
                };

                dish.Calories = 500;
                dish.Allergen = "Nüsse, Gluten";

                var cardText = $"Price: {dish.Price.ToString("0.00")}€ | Kalorien: {dish.Calories} kcal | Allergene: {dish.Allergen}";

                HeroCard resultCard = new HeroCard()
                {
                    Images = new List<CardImage> { new CardImage("https://thumbs.dreamstime.com/t/italian-food-meals-set-products-other-elements-cuisine-50899935.jpg") },
                    Title = dish.Dishes,
                    Text = cardText,
                    Subtitle = dish.Location,
                    Buttons = new List<CardAction> { linkButton }
                };

                Attachment cardAttachment = resultCard.ToAttachment();
                m.Attachments.Add(cardAttachment);
            }

            await context.PostAsync(m);
            await this.StartAsync(context);
        }
    }
}
