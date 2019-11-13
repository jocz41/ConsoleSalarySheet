using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practica2
{
    class Aplicacion
    {
        static void Main(string[] args)
        {
            Empleado e = new Empleado();
            Aplicacion a = new Aplicacion();
            Nomina n = new Nomina();

            a.menu();
            ConsoleKey ck = Console.ReadKey().Key;

            while (ck != ConsoleKey.Escape)
            {
                a.seleccion(ck, n, e);
                System.Threading.Thread.Sleep(800);
                a.menu();
                ck = Console.ReadKey().Key;
            }
        }

        #region Menú
        public void menu()
        {
            Console.Clear();
            Console.WriteLine("┌──────────────────────────────────────────┐");
            Console.WriteLine("|    Seleccione una opción:                |");
            Console.WriteLine("| Enter.    Introducir Datos               |");
            Console.WriteLine("| Esc.      Salir                          |");
            Console.WriteLine("└──────────────────────────────────────────┘");
        }

        public void seleccion(ConsoleKey ck, Nomina n, Empleado e)
        {
            Console.Clear();

            switch (ck)
            {
                case ConsoleKey.Enter:
                    introducirDatosEmple(e);
                    introducirDatosNomina(n, e);
                    Console.WriteLine("\nPulse una tecla para calcular y presentar la hoja salarial.");
                    Console.ReadKey();
                    n.hojaSalarial(); 
                    break;
                case ConsoleKey.Escape:
                    break;
                default:
                    impError("\nERROR. Opción no válida.\n");
                    break;
            }
        }
        #endregion
        
        #region Imprimir Especiales
        public static void impError(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(s);
            Console.ResetColor();
        }

        public static void impVerde(string s)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(s);
            Console.ResetColor();
        }

        public static void impAzul(string s)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(s);
            Console.ResetColor();
        }
        #endregion

        #region Introducción de Datos

        #region Datos del Empleado
        private void introducirDatosEmple(Empleado e)
        {
            Console.WriteLine("Indique los datos del empleado:");

            for (int i = 0; i < 5; i++)
                introducirDatoEmpleado(e, i);
        }
        
        private void introducirDatoEmpleado (Empleado e, int opcion)
        {
            String dato = null;
            sbyte datoNum = -1;

            do
            {
                switch(opcion)
                {
                    case 0:
                        impVerde("\nNombre: ");
                        e.Nombre = Console.ReadLine();
                        dato = e.Nombre;
                        break;
                    case 1:
                        impVerde("\nNIF: ");
                        e.Nif = Console.ReadLine();
                        dato = e.Nif;
                        break;
                    case 2:
                        impVerde("\nCategoría: ");
                        e.Categoria = stringASByte(Console.ReadLine());
                        datoNum = e.Categoria;
                        break;
                    case 3:
                        impVerde("\nNº Trienios: ");
                        e.NumTrienios = stringASByte(Console.ReadLine());
                        datoNum = e.NumTrienios;
                        break;
                    case 4:
                        impVerde("\nNº Hijos: ");
                        e.NumHijos = stringASByte(Console.ReadLine());
                        datoNum = e.NumHijos;
                        break;
                }
            } while (dato == null && datoNum == -1);
        }
        #endregion

        #region Datos de la Nómina
        private void introducirDatosNomina(Nomina n, Empleado e)
        {
            Console.WriteLine("\nTeclee los datos referentes a la nómina.");

            n.EmpleadoNomina = e;
            for (int i = 0; i < 2; i++)
                introducirDatoNomina(n, e, i);
        }

        private void introducirDatoNomina(Nomina n, Empleado e, int opcion)
        {
            DateTime fecha = default(DateTime);
            sbyte nHorasExtras = -1;

            do
            {
                switch (opcion)
                {
                    case 0:
                        impAzul("\nFecha de liquidación (dd/mm/aaaa): ");
                        n.FechaNomina = stringAFecha(Console.ReadLine());
                        fecha = n.FechaNomina;
                        break;
                    case 1:
                        impAzul("\nNº de horas extras: ");
                        n.NumHorasExtras = stringASByte(Console.ReadLine());
                        nHorasExtras = n.NumHorasExtras;
                        break;
                }
            } while (fecha.Year == 1 && nHorasExtras == -1);
            
        }
        #endregion

        #endregion

        //En caso de que s no sea un valor lógico devuelve -1
        private sbyte stringASByte(string s)
        {
            sbyte n;

            try
            {
                n = SByte.Parse(s);
            }
            catch(FormatException)
            {
                n = -1;
            }
            catch(OverflowException)
            {
                n = -1;
            }
            catch(ArgumentException)
            {
                n = -1;
            }

            return n;
        }

        //En caso de que s no sea una fecha válida devuelve el 01/01/0001
        private DateTime stringAFecha(string s)
        {
            DateTime t = default(DateTime);

            try
            {
                t = Convert.ToDateTime(s);
            }
            catch (FormatException)
            {
                t = default(DateTime);
            }
            catch (ArgumentException)
            {
                t = default(DateTime);
            }
            catch (OverflowException)
            {
                t = default(DateTime);
            }

            return t;
        }
    }

}
