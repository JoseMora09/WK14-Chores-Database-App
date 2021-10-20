using System;
using System.Text;
using System.Data.SqlClient;

namespace SqlServerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Table listing chores");

                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "DESKTOP-R4BMMB6\\SQLSERVER2019";
                builder.UserID = "sa";
                builder.Password = "Knights74!";
                builder.InitialCatalog = "master";

                // Connect to SQL
                Console.Write("Connecting to SQL Server ... ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Done.");

                    // Create a sample database
                    Console.Write("Dropping and creating database 'ChoresDB' ... ");
                    String sql = "DROP DATABASE IF EXISTS [ChoresDB]; CREATE DATABASE [ChoresDB]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Done.");
                    }

                    // Create a Table and insert some sample data
                    Console.Write("Creating list of chores with names assigned, press any key to continue...");
                    Console.ReadKey(true);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("USE ChoresDB; ");
                    sb.Append("CREATE TABLE Chores ( ");
                    sb.Append(" ChoreId INT IDENTITY(1,1) NOT NULL PRIMARY KEY, ");
                    sb.Append(" ChoreTask NVARCHAR(50), ");
                    sb.Append(" ChoreAssignedTo NVARCHAR(50) ");
                    sb.Append("); ");
                    sb.Append("INSERT INTO Chores (ChoreTask, ChoreAssignedTo) VALUES ");
                    sb.Append("(N'Jose', N'Laundry'), ");
                    sb.Append("(N'Jacob', N'Mop'), ");
                    sb.Append("(N'Ian', N'Vaccum'); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Done.");
                    }

                    // UPDATE demo
                    String userToUpdate = "Jose";
                    Console.Write("Updating 'ChoreAssignedTo' for user '" + userToUpdate + "', press any key to continue...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("UPDATE Chores SET ChoreAssignedTo = N'Cut Grass' WHERE ChoreAssignedTo = 'Laundry';");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("ChoreAssignedTo", userToUpdate);
                        int ChoreAffected = command.ExecuteNonQuery();
                        Console.WriteLine(ChoreAffected + " Chore task updated");
                    }

                    // DELETE demo
                    String ChoreToDelete = "Mop";
                    Console.Write("Deleting user '" + ChoreToDelete + "', press any key to continue...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("DELETE FROM Chores WHERE ChoreAssignedTo = @ChoreAssignedTo;");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ChoreAssignedTo", ChoreToDelete);
                        int ChoreAffected = command.ExecuteNonQuery();
                        Console.WriteLine(ChoreAffected + " row(s) deleted");
                    }

                    // READ demo
                    Console.WriteLine("Reading data from table, press any key to continue...");
                    Console.ReadKey(true);
                    sql = "SELECT ChoreId, ChoreTask, ChoreAssignedTo FROM Chores;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("All done. Press any key to finish...");
            Console.ReadKey(true);
        }
    }
}