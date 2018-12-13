using System;
using System.Collections.Generic;
using System.IO;
using TesterSOAP.Clases;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesterSOAP
{
    class Program
    {
        private readonly char DELIMITE = ',';
        private bool Flag;
        private int NumPruebas, NumPeticiones, NumExito, NumFallo;
        private int i = 0;
        private string Ruta;
        private string ServiceURL, ActionSOAP;
        private string[] lineas;
        private List<string> matriculas = new List<string>();
        private List<Alumno> alumnos = new List<Alumno>();
        private DateTime fecha;
        StringBuilder log = new StringBuilder(); 
        public char delimite { get => DELIMITE; }

        static void Main(string[] args)
        {
            
            Program obj = new Program();
            
            obj.ImprimirEncabezado();
            obj.IniciarVariables();
            //obj.SetRutaArchivo();
            obj.GetLineas();
            obj.GetMatriculas();

            obj.IniciarLog();
            //obj.GetDatosAlumnos();

            while(obj.Flag)
            {
                obj.SetNumPruebas();
                obj.ImprimirInicio();
                obj.RealizarPruebas();
                obj.SetStatus();
            }
//            Console.Write(log.ToString());
            Console.ReadKey();
        }

        private void ImprimirEncabezado()
        {
            Console.WriteLine("\t\tPrograma Prueba de Peticiones SOAP");
            Console.WriteLine("--------------------------------------------------------------------------------");
        }

        private void ImprimirInicio()
        {
            Console.Clear();
            Console.WriteLine("Archivo de Lista:\t" + this.Ruta);
            Console.WriteLine("URL Request:\t\t{0}", this.ServiceURL);
            Console.WriteLine("SOPA Action:\t\t{0}", this.ActionSOAP);
            Console.WriteLine("Numero de Matriculas:\t{0}", this.matriculas.Count);
            Console.WriteLine("Numero de Pruebas:\t{0}", this.NumPruebas);
            Console.WriteLine("Numero de Peticiones:\t{0}", (this.matriculas.Count * this.NumPruebas));
            Console.WriteLine("------------------------------------------------------------");
        }        

        private void IniciarVariables()
        {
            Console.WriteLine("[{0}]:\tIniciando variables ...", i++);
            this.fecha = DateTime.Now;
            this.Flag = true;
            this.NumPruebas = 0; this.NumPeticiones = 0; this.NumExito = 0; this.NumFallo = 0;
            this.Ruta = @"C:\Users\hector.torresg\source\repos\WSDLTester\WSDLTester\Docs\MatriculasPruebas.csv";
            this.ServiceURL = @"http://148.234.19.240:8080/wsatest/wsa1";
            this.ActionSOAP = @"urn:SIASE:RegCodice";
        }

        private void SetRutaArchivo()
        {
            Console.WriteLine("[{0}]:\tObtener ruta al archivo de prueba ...", i++);
            bool flag = true;
            do {
                Console.Write("\tEspecifique la ruta al archivo: ");
                this.Ruta = Console.ReadLine();
                if (this.Ruta == null || this.Ruta == "")
                    Console.WriteLine("\t... Ruta de archivo no valida.\n");
                else
                {
                    flag = false;
                    Console.WriteLine("\t... Archivo cargado en memoria\n");
                }
            } while (flag);
        }

        private void GetLineas()
        {
            Console.WriteLine("[{0}]:\tObtener listados ...", i++);
            this.lineas = File.ReadAllLines(this.Ruta);
            if (this.lineas == null || this.lineas.Length == 0)
                Console.WriteLine("\tEl archivo esta vacío o no existe", i++);
            else
                Console.WriteLine("\tLista cargada", i++);
        }

        private void GetMatriculas()
        {
            Console.WriteLine("[{0}]:\tObtener listado de Matriculas ...", i++);
            try
            {
                foreach(var linea in this.lineas)
                {
                    string[] data = linea.Split(this.delimite);
                    if (data[0] != "Matricula")
                        this.matriculas.Add(data[0]);
                }
            } catch (Exception x)
            {
                Console.WriteLine("\tError: " + x.Message);
                Console.WriteLine("\tPresione una tecla para continuar.");
                Console.ReadKey();
            }
            Console.WriteLine("\t...Cantidad de matriculas cargadas: {0} ", this.matriculas.Count);
        }

        private void GetDatosAlumnos()
        {
            Console.WriteLine("[{0}]:\tObtener listado de Datos de Alumno ...", i++);
            try
            {
                foreach (var linea in this.lineas)
                {
                    string[] data = linea.Split(this.delimite);
                    if (data[0] != "Matricula")
                        this.alumnos.Add(new Alumno(data[0], data[1], data[2], data[3], data[4], data[5],data[6]));
                }
            }
            catch (Exception x)
            {
                Console.WriteLine("\tError: " + x.Message);
                Console.WriteLine("\tPresione una tecla para continuar.");
                Console.ReadKey();
            }
        }

        private void SetNumPruebas()
        {
            bool flag = true;
            Console.Write("[{0}]:\tCuantas pruebas quiere realizar: ", i++);
            do
            {
                if (Int32.TryParse(Console.ReadLine(), out this.NumPruebas))
                    flag = false;
                else
                    Console.WriteLine("\t... Caracter no valido. \nVuelva a teclear un numero: ");
            } while (flag);
            Console.WriteLine("\t...Cantidad de pruebas a realizar: {0} ", this.NumPruebas);
            Console.WriteLine("\t...Cantidad de peticiones a realizar: {0} ", (this.matriculas.Count * this.NumPruebas));
        }

        private void IniciarLog()
        {
            this.log.AppendLine("-----------------------------------------------------------------------------");
            this.log.AppendLine("Fecha de prueba:\t" + this.fecha.ToString());
            this.log.AppendLine("Archivo de Lista:\t" + this.Ruta);
            this.log.AppendLine("URL Request:\t\t" + this.ServiceURL);
            this.log.AppendLine("SOPA Action:\t\t" + this.ActionSOAP);
            this.log.AppendLine("Numero de Matriculas:\t" + this.matriculas.Count);
            this.log.AppendLine("Numero de Pruebas:\t" + this.NumPruebas);
            this.log.AppendLine("Numero de Peticiones:\t" + (this.matriculas.Count * this.NumPruebas));
            this.log.AppendLine("-----------------------------------------------------------------------------");
        }

        private void SetStatus()
        {
            string op;
            op = "";
            do
            {
                Console.Write("\n[" + (i++) + "]:\tEscriba C para continuar. \n\tEscriba S para salir. \n\tOpción: ");
                op = Console.ReadLine().ToUpper();
                if (op != "C" && op != "S")
                    Console.WriteLine("\t\tCaracter no valido.");
                if (op == "C")
                    this.Flag = true;
                if (op == "S")
                    this.Flag = false;
            } while (op != "C" && op != "S");
        }

        private void RealizarPruebas()
        {
            Console.WriteLine("[{0}]:\tInicio de Pruebas", i++);
            int cuenta = 0;
            string cadena = "";
            while (cuenta < NumPruebas)
            {
                Console.WriteLine("\tLote de pruebas N°{0}", cuenta+1);
                
                Console.WriteLine("\t----------------------------------------------------------------------");
                Console.WriteLine("\tEnviando petición al servidor: {0}", this.ServiceURL);
                Console.WriteLine("\tEnviando petición a la acción: {0}", this.ActionSOAP);
                Console.WriteLine("\t----------------------------------------------------------------------");

                this.log.AppendLine("-----------------------------------------------------------------------------");
                this.log.AppendLine("Lote de Pruebas N°" + (cuenta + 1));
                this.log.AppendLine("N° \tTiempo  \tMatricula \tTipo \tResultado \tMensaje");
                this.log.AppendLine("-----------------------------------------------------------------------------");

                int j = 0;
                foreach(string matricula in this.matriculas)
                {
                    DateTime fecha = DateTime.Now;
                    Console.WriteLine("\t[{0}]:{1}\tMatricula: {2}" , j++, fecha, matricula);
                    ResultadoReq result = new ResultadoReq();
                    
                    NumPeticiones++;
                    try
                    {
                        Console.WriteLine("\t... Enviando petición al servidor ...");
                        Peticion request = new Peticion(this.ServiceURL, this.ActionSOAP, "1", matricula);
                        result = request.InvocarServicio();
                        NumExito++;
                    } catch (Exception x)
                    {
                        Console.WriteLine("\t... Error: " + x.Message);
                        NumFallo++;
                    }
                    cadena = fecha.ToString() + "\t"+ matricula +"\t 1 \t" + result.status + "\t" + result.mensaje;
                    this.log.AppendLine(fecha.ToString());
                }
                cuenta++;
            }
        }
    }
}
