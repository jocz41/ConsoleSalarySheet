using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practica2
{
    class Empleado
    {
        #region Atributos
        private sbyte categoria, numHijos, numTrienios;
        private string nif, nombre;
        #endregion

        #region Constructores
        public Empleado()
        {
            categoria = -1;
            numHijos = -1;
            numTrienios = -1;
            nif = null;
            nombre = null;
        }

        public Empleado(sbyte c, sbyte nH, sbyte nT, string dni, string name)
        {
            Categoria = c;
            NumHijos = nH;
            NumTrienios = nT;
            Nif = dni;
            Nombre = name;
        }
        #endregion

        #region Propiedades
        public sbyte Categoria
        {
            get => categoria;
            set
            {
                if (value > 0 && value < 4)
                    categoria = value;
                else
                {
                    Aplicacion.impError("\nERROR. Categoría no válida.\n");
                    categoria = -1;
                }
            }
        }

        public sbyte NumHijos
        {
            get => numHijos;
            set
            {
                if (value >= 0 && value < 21)
                    numHijos = value;
                else
                {
                    Aplicacion.impError("\nERROR. Nº de hijos no válido.\n");
                    numHijos = -1;
                }
            }
        }

        public sbyte NumTrienios
        {
            get => numTrienios;
            set
            {
                if (value < 0)
                {
                    Console.WriteLine("\nNº de Trienios: 0");
                    value = 0;
                }
                else if (value > 12)
                    value = 12;

                numTrienios = value;
            }
        }

        public string Nif
        {
            get => nif;
            set
            {
                value = value.Replace(" ", "");
                value = value.Replace("-", "");

                if (value.Length == 9 && estructuraNIFValida(value.ToUpper()))
                    nif = value;
                else
                {
                    Aplicacion.impError("\nERROR. NIF no válido.\n");
                    nif = null;
                }
            }
        }

        public string Nombre
        {
            get => nombre;
            set
            {
                if ((value.Length > 0 && value.Length < 30) && soloLetras(value))
                    nombre = value;
                else
                    Aplicacion.impError("\nERROR. Nombre no válido o demasiado largo.\n");
            }
        }
        #endregion

        #region Métodos de Comprobación
        //Método que comprueba que el nombre solo contenga letras
        private bool soloLetras(string s)
        {
            int c = 0;
            
            s = s.Replace(" ", "");
            char letra = s.ElementAt(c);

            while (c < s.Length && char.IsLetter(letra))  
            {
                c++;
                if(c < s.Length)
                    letra = s.ElementAt(c);
            }

            return (c == s.Length);
        }

        //Método que comprueba que el NIF sea correcto y corresponda su letra con el número
        private bool estructuraNIFValida(string s)
        {
            bool correcta = false;
            byte c = 0;
            const string LETRA = "TRWAGMYFPDXBNJZSQVHLCKE", NUM = "0123456789"; ;

            char letra = s.ElementAt(s.Length - 1);
            int n = 0;

            int.TryParse(s.Substring(0, 8), out n);

            //Comprobación de 8 dígitos numéricos
            while (c < (s.Length-1) && NUM.Contains(s.ElementAt(c)))
            {
                c++;
            }

            //Comprobación de la letra correspondiente a los números
            if((c == s.Length-1) && (letra == LETRA[n%23]))
            {
                correcta = true;
            }

            return correcta;
        }
        #endregion
    }
}
