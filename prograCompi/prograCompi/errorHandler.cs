using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prograCompi 
{
    // Clase que hereda de BaseErrorListener y sobreescribe el metodo SyntaxError
    class errorHandler : BaseErrorListener 
    {
        //Variable estatica global donde se guarda el error, de manera que se pueda mostrar posteriormente en un textbox
        public static String msjError;
        //Funcion que sobreescribe la funcion SyntaxError de la clase BaseErrorListener, identifica el tipo de excepcion que se presenta y añade a la variable msjError el respectivo mensaje
        //de errror
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) 
        {
            if(e != null)
            {
                msjError += ("Error en la linea: " + e.OffendingToken.Line + " columna: " + e.OffendingToken.Column +" ");
                if (e.GetType() == typeof(NoViableAltException))
                {
                    msjError += ("Error de tipo: No viable\n\n");
                }
                else if (e.GetType() == typeof(NotSupportedException))
                {
                    msjError += ("Error de tipo: No soportado\n\n");
                }
                else if (e.GetType() == typeof(NotImplementedException))
                {
                    msjError += ("Error de tipo: No implementado\n\n");
                }
                else if (e.GetType() == typeof(NotFiniteNumberException))
                {
                    msjError += ("Error de tipo: Numero no finito\n\n");
                }
                else if (e.GetType() == typeof(InputMismatchException ))
                {
                    msjError += ("Error de tipo: Desajuste de entrada\n\n");
                }
                else
                {
                    msjError += "Error de tipo desconocido\n\n";
                }
            }
        }
    }
}