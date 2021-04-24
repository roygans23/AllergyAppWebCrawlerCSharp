using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebCrawler
{
    public static class ConnectToDB
    {
        //Format sql command for inserting product to DB

        private static string GetFormattedRecipeInsertQuery(RecipeData recipeData)
        {
            return $"{ ConnectToConfig.RecipesDbInsertSqlQueryPrefix} ('{recipeData.Name}', '{recipeData.RecipeCategory}', '{string.Join(",", recipeData.RecipeIngredient)}', '{recipeData.RecipeUrl}')";
        }

        private static string GetFormattedRecipeUpdateQuery(RecipeData recipeData)
        {
            return $"{ ConnectToConfig.RecipesDbUpdateSqlQueryPrefix} nameRecipes = '{recipeData.Name}', categoryRecipes = '{recipeData.RecipeCategory}', " +
                $"ingredientsRecipes = '{string.Join(",", recipeData.RecipeIngredient)}', urlRecipes = '{recipeData.RecipeUrl}' WHERE nameRecipes = '{recipeData.Name}'";
        }

        private static string GetFormattedRecipeCountQuery(string recipeName)
        {
            return $"{ ConnectToConfig.RecipesDbCountSqlQueryPrefix} '{recipeName}'";
        }

        public static async Task<bool> UpdateProductToDbAsync(RecipeData recipeData)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectToConfig.ProductDbConnectionString))
            {
                using (MySqlCommand sqlCommand = new MySqlCommand(GetFormattedRecipeUpdateQuery(recipeData), mySqlConnection))
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

        //Insert product to DB asynchronysly
        public static async Task<bool> InsertProductToDbAsync(RecipeData recipeData)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectToConfig.ProductDbConnectionString))
            {
                using (MySqlCommand sqlCommand = new MySqlCommand(GetFormattedRecipeInsertQuery(recipeData), mySqlConnection))
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

        public static async Task<bool> DoesRecipeAlreadyExistAsync(string recipeName)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectToConfig.ProductDbConnectionString))
            {
                using (MySqlCommand sqlCommand = new MySqlCommand(GetFormattedRecipeCountQuery(recipeName), mySqlConnection))
                {
                    try
                    {
                        await mySqlConnection.OpenAsync();
                        var recipeRecordsCount = (int)(long)await sqlCommand.ExecuteScalarAsync();

                        return recipeRecordsCount != 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Cannot perform non query action for recipe with name {recipeName}, received error {e.Message}");
                        return false;
                    }
                }
            }
        }

        public static async Task<bool> ExecuteNonQueryRecipeToDbAsync(RecipeData recipeData)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectToConfig.ProductDbConnectionString))
            {
                using (MySqlCommand sqlCommand = new MySqlCommand(GetFormattedRecipeInsertQuery(recipeData), mySqlConnection))
                {
                    try
                    {
                        await mySqlConnection.OpenAsync();
                        await sqlCommand.ExecuteNonQueryAsync();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Cannot perform non query action for recipe with name {recipeData.Name}, received error {e.Message}");
                        return false;
                    }
                }
            }
        }
    }
}
