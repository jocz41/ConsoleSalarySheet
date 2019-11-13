using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practica2
{
    class Nomina
    {
        #region Constantes      
        private const byte PC_ANTIGUEDAD = 4, PC_HORAS_EXTRAS = 1, HZTL_DATO = 25, HZTL = 50;
        private const double PC_COTIZACION_SS = 4.51, PC_COTIZACION_SD = 1.97;
        #endregion

        #region Atributos
        private Empleado empleadoNomina;
        private DateTime fechaNomina;
        private sbyte numHorasExtras;
        #endregion

        #region Constructores
        public Nomina()
        {
            empleadoNomina = null;
            fechaNomina = default(DateTime);
            numHorasExtras = -1;
        }

        public Nomina(Empleado e, DateTime fecha, sbyte n)
        {
            this.empleadoNomina = e;
            this.fechaNomina = fecha;
            this.numHorasExtras = n;
        }
        #endregion

        #region Propiedades
        internal Empleado EmpleadoNomina
        {
            get => empleadoNomina;
            set => empleadoNomina = value;
        }

        //No se admiten fechas futuras ni anteriores a 1900
        public DateTime FechaNomina
        {
            get => fechaNomina;
            set
            {
                if (value <= DateTime.Today && value > new DateTime(1899, 12, 31))
                    fechaNomina = value;
                else
                    Aplicacion.impError("\nERROR. Fecha no válida.\n");
            }
        }

        //No se admiten horas extras negativas o mayores a 80
        public sbyte NumHorasExtras
        {
            get => numHorasExtras;
            set
            {
                if (value >= 0 && value < 81)
                {
                    numHorasExtras = value;
                }
                else
                {
                    Aplicacion.impError("\nERROR. Horas extras negativas o superiores a 80.\n");
                }
            }
        }
        #endregion

        #region Métodos

        #region Cotizaciones
        private double cotizacionSegDesc()
        {
            return devengosPagaExtra() * PC_COTIZACION_SD / 100;
        }

        private double cotizacionSegSoc()
        {
            return (baseCotizacion()*PC_COTIZACION_SS/100);
        }
        #endregion

        private double devengosPagaExtra()
        {
            return (salarioBase()+importeAntiguedad());
        }

        public void hojaSalarial()
        {
            Console.Clear();

            Aplicacion.impAzul("\n HOJA SALARIAL\n");
            Console.WriteLine("\nLIQUIDACIÓN DE HABERES AL "+FechaNomina.ToString("dd/MM/yyyy")+"\n");
            imprimirDatosEmpleado();

            int  top = Console.CursorTop + 1;

            imprimirDevengos();
                        
            int total = Console.CursorTop + 1;
            Console.SetCursorPosition(HZTL, top);

            imprimirDescuentos();

            imprimirTotales(total);
            
            Console.WriteLine("\n--------------------------------");
            datoDevengo("LÍQUIDO A PERCIBIR", "" + this.liquidoAPercibir(), HZTL_DATO, Console.CursorTop);
            Console.WriteLine("--------------------------------");

            Console.WriteLine("\nPulse una tecla para calcular y presentar la hoja salarial.");
            Console.ReadKey();
        }

        #region Importes
        private double importeAntiguedad()
        {
            return (EmpleadoNomina.NumTrienios * salarioBase() * PC_ANTIGUEDAD/100);
        }

        private double importeHorasExtras()
        {
            return (numHorasExtras*salarioBase()*PC_HORAS_EXTRAS/100);
        }
        #endregion

        private double retencionIRPF()
        {
            double irpf = totalDevengado();

            switch(FechaNomina.Month)
            {
                case 6:
                case 12:
                    irpf += devengosPagaExtra();
                    break;
            }

            return (irpf * porcentajeIRPF() / 100);
        }

        private int salarioBase()
        {
            int sal = 0;

            switch(EmpleadoNomina.Categoria)
            {
                case 1:
                    sal = 2500;
                    break;
                case 2:
                    sal = 2000;
                    break;
                case 3:
                    sal = 1500;
                    break;
            }

            return sal;
        }

        #region Totales
        private double totalDescuentos()
        {
            return (cotizacionSegSoc()+cotizacionSegDesc()+retencionIRPF());
        }

        private double totalDevengado()
        {
            return (salarioBase()+importeAntiguedad()+importeHorasExtras());
        }
        #endregion

        #endregion

        #region Métodos Adicionales
        //En caso de que obtengamos un porcentaje negativo, lo supondremos como 0
        private double porcentajeIRPF()
        {
            double pc = 0;

            switch (EmpleadoNomina.Categoria)
            {
                case 1:
                    pc = 18 - EmpleadoNomina.NumHijos;
                    break;
                case 2:
                    pc = 15 - EmpleadoNomina.NumHijos;
                    break;
                case 3:
                    pc = 12 - EmpleadoNomina.NumHijos;
                    break;
            }

            return (pc < 0) ? 0:pc;
        }

        public double liquidoAPercibir()
        {
            double liquido = totalDevengado() - totalDescuentos();

            switch(fechaNomina.Month)
            {
                case 6:
                case 12:
                    liquido += devengosPagaExtra();
                    break;
            }

            return liquido;
        }

        private double baseCotizacion()
        {
            return devengosPagaExtra() * 7 / 6;
        }

        #region Imprimir Hoja Salarial

        #region Empleado
        private void imprimirDatosEmpleado()
        {
            datoEmpleado("Nombre........: ", EmpleadoNomina.Nombre);
            datoEmpleado("NIF...........: ", EmpleadoNomina.Nif);
            datoEmpleado("Categoría.....: ", "" + EmpleadoNomina.Categoria);
            datoEmpleado("Nº de Trienios: ", "" + EmpleadoNomina.NumTrienios);
            datoEmpleado("Nº de Hijos...: ", "" + EmpleadoNomina.NumHijos);
        }

        private void datoEmpleado(string s, string valor)
        {
            Aplicacion.impVerde(s);
            Console.WriteLine(valor);
        }
        #endregion

        #region Devengos
        private void imprimirDevengos()
        {
            Aplicacion.impAzul("\nDEVENGOS\n");
            Console.WriteLine("--------");
            datoDevengo("Salario Base", "" + salarioBase(), HZTL_DATO, Console.CursorTop);
            datoDevengo("Antigüedad", "" + importeAntiguedad(), HZTL_DATO, Console.CursorTop);
            datoDevengo("Importe Hor. Ext.", "" + importeHorasExtras(), HZTL_DATO, Console.CursorTop);

            if(fechaNomina.Month == 6 || fechaNomina.Month == 12)
                datoDevengo("Paga Extra", "" + devengosPagaExtra(), HZTL_DATO, Console.CursorTop);
        }

        private void datoDevengo(string s, string valor, int hztl, int top)
        {
            double v = 0;
            double.TryParse(valor, out v);
            v = Math.Round(v, 2);

            Aplicacion.impVerde(s);
            Console.SetCursorPosition(hztl, top);
            Console.WriteLine("" + v);
        }
        #endregion

        #region Descuentos
        private void imprimirDescuentos()
        {
            Aplicacion.impAzul("DESCUENTOS\n");
            Console.SetCursorPosition(HZTL, Console.CursorTop);
            Console.WriteLine("----------");
            datoDescuento("Cotización Seg. Soc.", "" + cotizacionSegSoc());
            datoDescuento("Cotización Seg. Desc.", "" + cotizacionSegDesc());
            datoDescuento("Retención IRPF", "" + retencionIRPF());
        }

        private void datoDescuento(string s, string valor)
        {
            Console.SetCursorPosition(HZTL, Console.CursorTop);
            datoDevengo(s, valor, HZTL + HZTL_DATO, Console.CursorTop);
        }
        #endregion

        private void imprimirTotales(int total)
        {
            Console.SetCursorPosition(0, total);
            datoDevengo("Total Devengos", "" + this.totalDevengado(), HZTL_DATO, total);

            Console.SetCursorPosition(HZTL, total);
            datoDevengo("Total Descuentos", "" + this.totalDescuentos(), HZTL + HZTL_DATO, total);
        }
        #endregion

        #endregion
    }
}
