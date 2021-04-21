using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebCrawler
{
    public static class ConnectToDB
    {
        //Format sql command for inserting product to DB

        private static string GetFormattedProductInsertQuery(RecipeData recipeData)
        {
            return $"insert into recipe_db.recipes (nameRecipes, categoryRecipes, ingredientsRecipes, urlRecipes) values" +
                $" ('{recipeData.Name}', '{recipeData.RecipeCategory}', '{string.Join(",", recipeData.RecipeIngredient)}', '{recipeData.RecipeUrl}')";
        }

        //Insert product to DB asynchronysly
        public static async Task<bool> InsertProductToDbAsync(RecipeData recipeData)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectToConfig.ProductDbConnectionString))
            {
                using (MySqlCommand sqlCommand = new MySqlCommand(GetFormattedProductInsertQuery(recipeData), mySqlConnection))
                {
                    try
                    {
                        await mySqlConnection.OpenAsync();
                        await sqlCommand.ExecuteNonQueryAsync();

                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Cannot insert product with name {recipeData.Name}, received error {e.Message}");
                        return false;
                    }

                }
            }
        }
    }
}
