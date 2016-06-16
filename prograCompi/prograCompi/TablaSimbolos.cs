using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace prograCompi
{
    class TablaSimbolos
    {
        List<objetoTabla> tabla;

        public TablaSimbolos()
        {
            tabla = new List<objetoTabla>();
        }

        public void insertar(string nombre, ParserRuleContext tipo, int nivel, Boolean metodo, string tipoP)
        {
            objetoTabla objeto = new objetoTabla(nivel ,nombre, tipo,metodo,tipoP);
            tabla.Add(objeto);
        }

        public objetoTabla buscar(string nombre, int nivelP)
        {
            for(int i=0;i<tabla.Count;i++)
            {
                if (tabla.ElementAt(i).ID==nombre && (tabla.ElementAt(i).nivel==nivelP || tabla.ElementAt(i).nivel==0))
                {
                    return tabla.ElementAt(i);
                }
            }
            return null;
        }

        public void inicializar(string nombre, int nivelP)
        {
            for (int i = 0; i < tabla.Count; i++)
            {
                if (tabla.ElementAt(i).ID == nombre && tabla.ElementAt(i).nivel == nivelP)
                {
                    tabla.ElementAt(i).inicializado = true;
                    break;
                }
            }
        }

        public void eliminarSimbolo(string nombre)
        {
            objetoTabla obj = buscar(nombre, 1);
            tabla.Remove(obj);
        }

        public void imprimir()
        {
            for (int i = 0; i < tabla.Count; i++)
            {
                objetoTabla obj = tabla.ElementAt(i);
                Console.WriteLine("Nombre: " + obj.ID + "\nTipo: " + obj.tipo +"\nNivel: " + obj.nivel + "\nMetodo?: " + obj.esMetodo + "\nInicializado?: " + obj.inicializado + "\n");
            }
        }
    }
}
