using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class WordDAL : DataAccess
    {
        private readonly string _selectCommand = "select * from Word";

        public WordDAL()
        {

        }

        public IEnumerable<Word> Load()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(_selectCommand, connection);
            adapter.SelectCommand.CommandType = CommandType.Text;

            try
            {
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    yield return new Word
                    {                        
                        Name = (row["Name"]).ToString(),
                        Count = Convert.ToInt32(row["Count"])
                    };
                }
            }
            finally
            {
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
    }
}