using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
//using System.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration.Json;
using Npgsql;
using System.IO;

namespace Componentes
{


    public class DBObject
    {
        // Fields
        private NpgsqlConnection connection;

        // Methods
        private NpgsqlCommand BuildQueryCommand(string mysql, IDataParameter[] myParameters)
        {
            NpgsqlCommand command2 = new NpgsqlCommand(mysql, this.connection)
            {
                CommandType = CommandType.Text
            };
            if (!(myParameters is null))
            {
                if (myParameters.Length > 0)
                {
                    IDataParameter[] parameterArray = myParameters;
                    int index = 0;
                    while (true)
                    {
                        if (index >= parameterArray.Length)
                        {
                            break;
                        }
                        NpgsqlParameter parameter = (NpgsqlParameter)parameterArray[index];
                        command2.Parameters.Add(parameter);
                        index++;
                    }
                }
            }

            return command2;
        }

        public int CloseConnection()
        {
            this.connection.Close();
            this.connection.Dispose();
            return 1;
        }

        public DataSet ExecuteDataSet(string mysql, IDataParameter[] myparameters)
        {
            this.OpenConnection();
            NpgsqlCommand selectCommand = this.BuildQueryCommand(mysql, myparameters);
            DataSet dataSet = new DataSet();
            new NpgsqlDataAdapter(selectCommand).Fill(dataSet);
            selectCommand.Dispose();
            this.CloseConnection();
            return dataSet;
        }

        public int ExecuteNonQuery(string mysql, IDataParameter[] myparameters, bool returnLastID = false)
        {
            this.OpenConnection();
            NpgsqlCommand command = this.BuildQueryCommand(mysql, myparameters);
            int num2 = command.ExecuteNonQuery();
            if (returnLastID)
            {
                command = new NpgsqlCommand("SELECT @@IDENTITY as lastId", this.connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (!Convert.IsDBNull(reader["lastId"]))
                {
                    num2 = Convert.ToInt32(reader["lastId"]);
                }
                reader.Close();
            }
            command.Dispose();
            this.CloseConnection();
            return num2;
        }

        public NpgsqlDataReader ExecuteReader(string mysql, IDataParameter[] myparameters)
        {
            this.OpenConnection();
            return this.BuildQueryCommand(mysql, myparameters).ExecuteReader(CommandBehavior.CloseConnection);
        }

        public object ExecuteScalar(string mysql, IDataParameter[] myParameters)
        {
            object obj2;
            this.OpenConnection();
            NpgsqlCommand command = this.BuildQueryCommand(mysql, myParameters);
            try
            {
                obj2 = command.ExecuteScalar();
            }
            finally
            {
                command.Dispose();
                this.CloseConnection();
            }
            return obj2;
        }

        private int OpenConnection()
        {

            //IConfiguration config = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json", true, true)
            //.Build();

            //Console.WriteLine(config["connectionString"]);

           // string mi = "Host=192.168.1.69;Port=5432;Username=jarchuchin;Password=chuchin;database=tabasco";
            string mi = "Host=postgresdbs;Port=5432;Username=jesusalvarado;Password=Hosting01;database=tabasco";
            //  this.connection = new  NpgsqlConnection (config["connectionString"]);
            this.connection = new NpgsqlConnection(mi);
            this.connection.Open();
            return 1;
        }
    }








}
