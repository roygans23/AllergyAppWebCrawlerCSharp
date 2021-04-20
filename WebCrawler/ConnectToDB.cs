using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebCrawler
{
    public static class ConnectToDB
    {
        //Format sql command for inserting product to DB
        
        private static string GetFormattedProductInsertQuery(RecipeData recipeData)
        {
            return $"insert into DatabaseName.TableName (recipeId, recipeName, recipeCategory, recipeCalories, recipeIngredients, recipeInstructions, recipeImageUrl) values" +
                $" ({recipeData.Id}, {recipeData.Name}, {recipeData.Category}, {recipeData.Calories}, {recipeData.Ingredients}," +
                $" {recipeData.instructions}, {recipeData.ImageUrl})";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {       
                string MyConnection2 = "datasource=localhost;port=3307;username=root;password=root";
                string Query = "insert into student.studentinfo(idStudentInfo,Name,Father_Name,Age,Semester) values('" + this.IdTextBox.Text + "','" + this.NameTextBox.Text + "','" + this.FnameTextBox.Text + "','" + this.AgeTextBox.Text + "','" + this.SemesterTextBox.Text + "');";
                MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataReader MyReader2;
                MyConn2.Open();
                MyReader2 = MyCommand2.ExecuteReader();      
                MessageBox.Show("Save Data");
                while (MyReader2.Read())
                {
                }
                MyConn2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
