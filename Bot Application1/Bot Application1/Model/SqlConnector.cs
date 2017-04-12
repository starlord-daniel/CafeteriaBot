using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bot_Application1.Model
{
    [Serializable]
    public static class SqlConnector
    {
        /// <summary>
        /// Call the database to get all dishes 
        /// </summary>
        /// <param name="project">All available dishes.</param>
        internal static List<AvailableFood> GetDishes()
        {
            List<AvailableFood> availableFood = new List<AvailableFood>();
            using (SqlConnection connection = new SqlConnection("Server=tcp:dfsbotmenue.database.windows.net,1433;Initial Catalog=dfsbot-sqldb;Persist Security Info=False;User ID=adminbot;Password=Public01.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                var query = String.Format("SELECT * FROM DFSFood;");

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var location = reader["LOCATION"].ToString().ToString();
                        var dishes = reader["DISHES"].ToString();
                        var price = Convert.ToDecimal(reader["PRICE"]);
                        var isDailySpecial = Convert.ToBoolean(reader["ISDAILYSPECIAL"].ToString());
                        var kitchen = reader["KITCHEN"].ToString();
                        var imageUrl = reader["IMAGEURL"].ToString();
                        var allergenes = reader["ALLERGEN"].ToString();
                        var calories = Convert.ToInt32(reader["CALORIES"]);
                        var date = Convert.ToDateTime(reader["DATE"]);
                        var menuUrl = reader["MENUURL"].ToString();


                        availableFood.Add(new AvailableFood
                        {
                            Dishes = dishes,
                            ImageURL = imageUrl,
                            IsDailySpecial = isDailySpecial,
                            Kitchen = kitchen,
                            Location = location,
                            Price = price,
                            Allergen = allergenes,
                            Calories = calories,
                            Date = date,
                            MenuUrl = menuUrl
                        });
                    }

                }
                connection.Close();
            }

            return availableFood;
        }

    }
}