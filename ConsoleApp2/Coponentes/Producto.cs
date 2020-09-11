using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Data.SqlClient;
using System.IO;
using Npgsql;


namespace Componentes
{
    public class Producto : DBObject
    {
        // Fields
        public int id;
        public string clave;
        public string descripcion;
        public decimal precio = 0;
        public bool Existe;





        // Methods
        public Producto()
        {
            this.Existe = false;
        }

        public Producto(int claveProducto)
        {
            this.Existe = false;
            string mysql = "SELECT * FROM producto WHERE id = @id";
            NpgsqlParameter[] myparameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", claveProducto) };
            NpgsqlDataReader reader = this.ExecuteReader(mysql, myparameters);
            if (reader.Read())
            {
                this.id = Convert.ToInt32(reader["id"]);
                this.clave = Convert.ToString(reader["clave"]);
                this.descripcion = Convert.ToString(reader["descripcion"]);

                if (!Convert.IsDBNull(reader["precio"]))
                {
                    this.precio = Convert.ToDecimal(reader["precio"]);
                }

                this.Existe = true;
            }
            reader.Close();
        }



        public DataSet GetDS()
        {
            string mysql = "SELECT * from producto order by descripcion asc";
            return this.ExecuteDataSet(mysql, null);


        }



        public int GrabarProductos()
        {
            DataSet ds = this.GetDS();
            DataTable dv = ds.Tables[0];
            ConsoleApp2.srEnviarDatos.wsRecibirDatosSoapClient srED = new ConsoleApp2.srEnviarDatos.wsRecibirDatosSoapClient();




            foreach (DataRow dr in dv.Rows)
            {
                String desc = Convert.ToString(dr["descripcion"]);
                if (desc.Length > 200)
                {
                    desc = desc.Substring(0, 199);
                }
                Console.WriteLine(dr["id"] + "-" + desc);

               Console.WriteLine("----->>>>>>>>>>>> " + srED.HelloWorld(Convert.ToString(dr["id"])));

                //###############
                //grabar en server



            }

            return 1;


        }

        //public int GrabarProdcutos2(String Archivo)
        //{
        //    string line;
        //    int counter = 0;
        //    System.IO.StreamReader file = new System.IO.StreamReader(Archivo);
        //    Proveedor proveedor;
        //    while ((line = file.ReadLine()) != null) {
        //        string[] strArray = line.Split("|");
        //       // System.Console.WriteLine(line);
        //        proveedor = new Proveedor(strArray[0]);
        //        try
        //        {
        //            if (proveedor.Existe)
        //            {
        //                proveedor.Nombre = strArray[1].ToString();
        //                proveedor.Pais = strArray[2].ToString();
        //                proveedor.Moneda1 = strArray[3].ToString();
        //                if (strArray.Length>4){
        //                    proveedor.SaldoInicial2019 = decimal.Parse(strArray[4].ToString().Trim());
        //                }
        //                proveedor.Update();

        //                Console.WriteLine(line + "  Registro actualizado...");
        //            }
        //            else
        //            {
        //                proveedor.Clave = strArray[0].ToString();
        //                proveedor.Nombre = strArray[1].ToString();
        //                proveedor.Pais = strArray[2].ToString();
        //                proveedor.Moneda1 = strArray[3].ToString();
        //                if (strArray.Length>4){
        //                    proveedor.SaldoInicial2019 = decimal.Parse(strArray[4].ToString().Trim());
        //                }else{
        //                    proveedor.SaldoInicial2019 = 0;
        //                }
        //                proveedor.Add();

        //                Console.WriteLine(line + "  Registro nuevo registrado...");
        //            }
        //        }
        //        catch (Exception er)
        //        {
        //            Console.WriteLine(line + "  Esta línea contiene errores " + er.Message );
        //        }

        //        counter++;
        //    }
        //    file.Close();

        //    Console.WriteLine("Finalizando carga de proveedores. Se encontraron {0} registros en el archivo {1}", counter, Archivo);

        //    return 0;
        //}
    }

}
