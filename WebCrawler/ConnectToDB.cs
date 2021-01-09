using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler
{
    public static class ConnectToDB
    {
        //Format sql command for inserting product to DB
        private static string GetFormattedProductInsertQuery(string id, string barcode, string name, string ingredients,
            string allergy)
        {
            return $"insert into Products (Id, Barcode, Name, Ingredients, Allergy values ({id}, {barcode}, {name}, {ingredients}, {allergy})";
        }

        //Insert product to DB asynchronysly
        public static async Task<bool> InsertProductToDbAsync(string id, string barcode, string name, string[] ingredients, string[] allergy)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectToConfig.ProductDbConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(GetFormattedProductInsertQuery(id, barcode, name, string.Join(",", ingredients), string.Join(",", allergy)), sqlConnection))
                {
                    try
                    {
                        await sqlConnection.OpenAsync();
                        await sqlCommand.ExecuteNonQueryAsync();

                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Cannot insert product with id {id}, received error {e.Message}");
                        return false;
                    }
                   
                }
            }
        }
    }
}
