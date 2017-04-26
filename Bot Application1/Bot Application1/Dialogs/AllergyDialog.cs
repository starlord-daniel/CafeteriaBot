using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class AllergyDialog : IDialog<List<string>>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("What allergies do you have? Please put them in, seperated by commas.");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            var allergyList = new List<string>();
            try
            {
                var seperatedByComma = message.Text.Split(',');
                allergyList = new List<string>();

                foreach (var allergy in seperatedByComma.ToList())
                {
                    allergyList.Add(allergy.Trim());
                }

                // Return the list of allergies
                context.Done(allergyList);
            }
            catch (Exception)
            {
                await context.PostAsync("Wrong input, please seperate the Allergies with a comma.");
                await StartAsync(context);
            }
        }
    }
}