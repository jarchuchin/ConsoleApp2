using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Threading;

namespace ConsoleApp2
{
    class Program
    {


        static int seconds = 0;
        static void Main(string[] args)
        {



            TimerCallback callback = new TimerCallback(Tick);
            Console.WriteLine("Inicia sistema: " + DateTime.Now);

            Timer stateTimer = new Timer(callback, null, 0, 60000);

            for (; ; )
            {
                Thread.Sleep(1000);
            }
        }



         static public void Tick(object stateInfo)
        {

            seconds++;

            ///Buscar archivos
           

            string carpetaSource = ConfigurationManager.AppSettings["carpetaSource"];
            string carpetaDestino = ConfigurationManager.AppSettings["carpetaDestino"];

            DirectoryInfo info = new DirectoryInfo(carpetaSource);
            // FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
            var files = from c in info.GetFiles("*.xml")
                        orderby c.Name
                        select c;
            // where c.CreationTime > DateTime.Now
            // Console.WriteLine(files.Count);
            Console.WriteLine("Buscar archivos: " + DateTime.Now);
            Console.WriteLine("Encontrados: " + files.Count());
            foreach (FileInfo file in files)
            {
                Console.WriteLine(" ");
                Console.WriteLine(file.Name + "---" + file.CreationTime);
                //cmp070924fca-B14873-a38463e8-7727-4023-9c11-f0ed7fc9d2ac
                string nombre = file.Name;
                string nombreSinExtension = file.Name.Substring(0, file.Name.LastIndexOf("."));


                string folio = nombre.Substring(14);
                folio = folio.Substring(0, folio.IndexOf("-"));
                // Console.WriteLine(folio);

                // Enviar datos a estelar
                Console.WriteLine("Enviando folio: " + folio + " a estelar..... " + DateTime.Now);
                folioFilesSR.folioFilesSoapClient myff = new folioFilesSR.folioFilesSoapClient();

                string regreso = "";

                try
                {
                    regreso = myff.folioFiles(folio, nombreSinExtension, "33234asdGGGF");
                }
                catch {
                    regreso = "Error: Fatal error al conectarse al estelar";
                }
               

                Console.WriteLine("Respuesta de estelar: ########" + regreso);



                //Copiando archivos xml
                if (regreso.ToLower() == "ok")
                {

                    if (File.Exists(carpetaSource + nombreSinExtension + ".xml"))
                    {


                        if (File.Exists(carpetaSource + nombreSinExtension + ".pdf"))
                        {
                            Console.WriteLine("Copiando archivo: " + nombreSinExtension + ".pdf - " + DateTime.Now);
                            File.Move(carpetaSource + nombreSinExtension + ".pdf", carpetaDestino + nombreSinExtension + ".pdf");
                            Console.WriteLine("Archivo copiado: " + nombreSinExtension + ".pdf - " + DateTime.Now);

                            Console.WriteLine("Copiando archivo: " + nombreSinExtension + ".xml - " + DateTime.Now);
                            File.Move(carpetaSource + nombreSinExtension + ".xml", carpetaDestino + nombreSinExtension + ".xml");
                            Console.WriteLine("Archivo copiado: " + nombreSinExtension + ".xml - " + DateTime.Now);
                        }
                        else
                        {
                            Console.WriteLine("Error no se encontro el archivo " + nombreSinExtension + ".pdf");
                        }


                    }
                    else
                    {
                        Console.WriteLine("Error no se encontro el archivo " + nombreSinExtension + ".xml");
                    }

                }
                else
                {
                    Console.WriteLine("Existe un error al conectarse al estelar: " + regreso);
                }
               

               
               




            }


            Console.WriteLine("Vuelta: " + seconds);



            GC.Collect();



        }



    }
}
