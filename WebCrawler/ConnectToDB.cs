using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebCrawler.Enums;

namespace WebCrawler
{
    public static class ConnectToDB
    {
        //Format sql command for inserting product to DB
        private static string GetSqlRecipeQueryByCommand(RecipeData recipeData, SqlCommandType sqlCommandType)
        {
            switch(sqlCommandType)
            {
                case SqlCommandType.Update:
                    return $"{ ConnectToConfig.RecipesDbUpdateSqlQueryPrefix} nameRecipes = '{recipeData.Name}', categoryRecipes = '{recipeData.RecipeCategory}', " +
                $"ingredientsRecipes = '{string.Join(",", recipeData.RecipeIngredients)}', urlRecipes = '{recipeData.RecipeUrl}' WHERE nameRecipes = '{recipeData.Name}'";
                case SqlCommandType.Insert:
                    return $"{ ConnectToConfig.RecipesDbInsertSqlQueryPrefix} ('{recipeData.Name}', '{recipeData.RecipeCategory}', '{string.Join(",", recipeData.RecipeIngredients)}', '{recipeData.RecipeUrl}')";
                default:
                    return $"{ ConnectToConfig.RecipesDbCountSqlQueryPrefix} '{recipeData.Name}'";
            }
        }

        public static async Task<bool> ExecuteCountRecipeRowsQueryCommandAsync(RecipeData recipeData, SqlCommandType sqlCommandType)
        {
            using (MySqlConnection recipeSqlConnection = new MySqlConnection(ConnectToConfig.ProductDbConnectionString))
            {
                using (MySqlCommand sqlCommand = new MySqlCommand(GetSqlRecipeQueryByCommand(recipeData, sqlCommandType), recipeSqlConnection))
                {
                    try
                    {
                        await recipeSqlConnection.OpenAsync();
                        var recipeRecordsCount = (int)(long)await sqlCommand.ExecuteScalarAsync();

                        return recipeRecordsCount != 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Cannot perform non query action for recipe with name {recipeData.Name}, received error {e.Message}");
                        return false;
                    }
                }
            }
        }

        public static async Task<bool> ExecuteNonQueryRecipeCommandAsync(RecipeData recipeData, SqlCommandType sqlCommandType)
        {
            using (MySqlConnection recipeSqlConnection = new MySqlConnection(ConnectToConfig.ProductDbConnectionString))
            {
                using (MySqlCommand sqlCommand = new MySqlCommand(GetSqlRecipeQueryByCommand(recipeData, sqlCommandType), recipeSqlConnection))
                {
                    try
                    {
                        await recipeSqlConnection.OpenAsync();
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
