using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace prograCompi
{
    class objetoTabla
    {
        public string ID;
        public Boolean inicializado;
        public ParserRuleContext tipoPuntero;
        public int nivel;
        public Boolean esMetodo;
        public string tipo;

        public objetoTabla(int Nivel, string nombre, ParserRuleContext tipoP,Boolean metodo, string tipoP2)
        {
            this.tipoPuntero = tipoP;
            this.ID = nombre;
            this.inicializado = false;
            this.nivel=Nivel;
            this.esMetodo = metodo;
            this.tipo = tipoP2;
        }
    }
}
