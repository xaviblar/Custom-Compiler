using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prograCompi
{
    class analisisContextual : ParserPrograBaseVisitor<object>
    {
        //Variable global que indica el nivel en el que se encuentra el arbol
        public int nivelActual=0;
        //Variable de tipo TablaSimbolos, tabla donde se almacenaran todos los identificadores declarados
        public TablaSimbolos tablaSimbolos = new TablaSimbolos();

        public List<bool> pilaCiclos= new List<bool>();
        
        public static RichTextBox txtBoxAnalisisContextual = new RichTextBox();

        public void push()
        {
            pilaCiclos.Add(true);
        }

        public void pop()
        {
            pilaCiclos.RemoveAt(pilaCiclos.Count - 1);
        }

        public bool palabraReservada(string nombre)
        {
            if(nombre=="int" || nombre=="float" || nombre=="boolean" || nombre=="char" || nombre=="ord" || nombre=="chr" || nombre=="len" || nombre=="null" || nombre=="true" || nombre=="false")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void openScope()
        {
            nivelActual++;
        }
        
        public void closeScope()
        {
            nivelActual--;
        }
        //Metodos que sobreescriben los metodos de la clase ParserPrograBaseVisitor, llama a los respectivos metodo para visitar las etiquetas que se requiera segun las reglas del parser
        //y a su vez aumenta y disminuye el nivel del arbol e inserta nuevos identificadores en la tabla chequeando posibles errores

        public override object VisitProgramAuxLabelAST([NotNull] ParserProgra.ProgramAuxLabelASTContext context)
        {
            if (context.constDecl() != null)
            {
                Visit(context.constDecl());
            }

            else if (context.varDecl() != null)
            {
                Visit(context.varDecl());
            }

            else if (context.classDecl() != null)
            {
                Visit(context.classDecl());
            }

            return null;
        }

        public override object VisitCondTermConditionAST([NotNull] ParserProgra.CondTermConditionASTContext context)
        {

            for (int i = 0; i < context.condTerm().Count; i++)
            {
                bool condicion = (bool)Visit(context.condTerm(i));
                if(condicion==false)
                {
                    return false;
                }
            }

            return true;
        }

        public override object VisitMinorEqualAST([NotNull] ParserProgra.MinorEqualASTContext context)
        {
            return "<=";
        }

        public override object VisitTypeIDFormParsAST([NotNull] ParserProgra.TypeIDFormParsASTContext context)
        {
            List<string> sublista = new List<string>();
            for (int i = 0; i < context.type().Count; i++)
            {
                if (tablaSimbolos.buscar(context.type(i).GetChild(0).GetText(), 0) != null)
                {
                    objetoTabla obj = tablaSimbolos.buscar(context.type(i).GetChild(0).GetText(), 0);
                    if (obj.tipoPuntero.GetChild(0).GetText().Equals("class"))
                    {
                        if (palabraReservada(context.ID(i).GetText()) == true)
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(i).Symbol.Line + " columna " + ((context.ID(i).Symbol.Column) - 1).ToString() +
                                                                ", " + context.ID(i).GetText() + " es una palabra reservada\n");
                        }
                        else if (context.type(i).GetChild(2) != null)
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(i).Symbol.Line + " columna " + context.ID(i).Symbol.Column +
                                                                ", " + context.ID(i).GetText() + " no se permiten arreglos de clases\n");
                        }

                        else if (tablaSimbolos.buscar(context.ID(i).GetText(), nivelActual) != null)
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(i).Symbol.Line + " columna " + context.ID(i).Symbol.Column +
                                                                ", El ID: " + context.ID(i).GetText() + " ya ha sido declarado\n");
                        }

                        else
                        {
                            tablaSimbolos.insertar(context.ID(i).GetText(), context, nivelActual,false,obj.ID);
                            sublista.Add(context.ID(i).GetText());
                        }
                    }
                }
                else if(palabraReservada(context.type(i).GetChild(0).GetText()) == true)
                {
                    if (context.type(i).GetChild(0).GetText() != "ord" && context.type(i).GetChild(0).GetText() != "chr" && context.type(i).GetChild(0).GetText() != "len")
                    {
                        if (context.type(i).GetChild(0).GetText().Equals("boolean") && context.type(i).GetChild(1) != null)
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(0).Symbol.Line + " columna " + (context.ID(0).Symbol.Column - 1).ToString() +
                                                                    ", no se permiten arreglos de boolean\n");
                            return sublista;
                        }
                        else if (palabraReservada(context.ID(i).GetText()) == true)
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(i).Symbol.Line + " columna " + context.ID(i).Symbol.Column +
                                                                ", " + context.ID(i).GetText() + " es una palabra reservada\n");
                        }

                        else if (tablaSimbolos.buscar(context.ID(i).GetText(), nivelActual) != null)
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(i).Symbol.Line + " columna " + context.ID(i).Symbol.Column +
                                                                ", El ID: " + context.ID(i).GetText() + " ya ha sido declarado\n");
                        }

                        else
                        {
                            if (context.type(i).GetChild(1) != null)
                            {
                                tablaSimbolos.insertar(context.ID(i).GetText(), context, nivelActual, false, (context.type(i).GetChild(0).GetText() + "[]"));
                            }
                            else
                            {
                                tablaSimbolos.insertar(context.ID(i).GetText(), context, nivelActual, false, context.type(i).GetChild(0).GetText());
                            }
                            sublista.Add(context.ID(i).GetText());
                        }
                    }
                    else
                    {
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(0).Symbol.Line + " columna " + ((context.ID(0).Symbol.Column) - 1).ToString() +
                                                                    ", " + context.type(i).GetChild(0).GetText() + " es una palabra reservada\n");
                    }
                }
                else
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(0).Symbol.Line + " columna " + ((context.ID(0).Symbol.Column) - 1).ToString() +
                                                                    ", " + context.type(i).GetChild(0).GetText() + " no es un tipo valido\n");
                }
            }
            return sublista;
        }

        public override object VisitDivAST([NotNull] ParserProgra.DivASTContext context)
        {
            return (context.DIV().Symbol.Line + " Columna: " + context.DIV().Symbol.Column);
        }

        public override object VisitBoolFactorAST([NotNull] ParserProgra.BoolFactorASTContext context)
        {
            return "boolean";
        }

        public override object VisitExprFactorAST([NotNull] ParserProgra.ExprFactorASTContext context)
        {
            string tipo = (string)Visit(context.expr());
            if(tipo!= null)
            {
                return tipo;
            }
            return null;
        }

        public override object VisitStatePyCAST([NotNull] ParserProgra.StatePyCASTContext context)
        {
            return true;
        }

        public override object VisitExprRelopExprCondFactAST([NotNull] ParserProgra.ExprRelopExprCondFactASTContext context)
        {
            string tipo1 = (string)Visit(context.expr(0));
            string tipo2 = (string)Visit(context.expr(1));
            string operador = (string)Visit(context.relop());

            if (operador.Equals("=="))
            {
                Antlr4.Runtime.Tree.TerminalNodeImpl op = (Antlr4.Runtime.Tree.TerminalNodeImpl)context.relop().GetChild(0);
                tipo1 = tipo1.Replace("const ", "");
                tipo2 = tipo2.Replace("const ", "");
                if (tipo1 != tipo2)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + op.Symbol.Line + " columna " + op.Symbol.Column +
                                                    ", no se pueden comparar expresiones de distinto tipo\n");
                }
            }

            else if (operador.Equals("!="))
            {
                tipo1 = tipo1.Replace("const ", "");
                tipo2 = tipo2.Replace("const ", "");
                Antlr4.Runtime.Tree.TerminalNodeImpl op = (Antlr4.Runtime.Tree.TerminalNodeImpl)context.relop().GetChild(0);
                if (tipo1 != tipo2)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + op.Symbol.Line + " columna " + op.Symbol.Column +
                                                    ", no se pueden comparar expresiones de distinto tipo\n");
                    return false;
                }
            }
            else if (operador.Equals(">"))
            {
                tipo1 = tipo1.Replace("const ", "");
                tipo2 = tipo2.Replace("const ", "");
                Antlr4.Runtime.Tree.TerminalNodeImpl op = (Antlr4.Runtime.Tree.TerminalNodeImpl)context.relop().GetChild(0);
                if (tipo1 != tipo2)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + op.Symbol.Line + " columna " + op.Symbol.Column +
                                                    ", no se pueden comparar expresiones de distinto tipo\n");
                    return false;
                }
                else if(tipo1 != "int" && tipo1!="float")
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + op.Symbol.Line + " columna " + op.Symbol.Column +
                                                    ", el operador solo puede usarse para tipos int o float\n");
                    return false;
                }
            }
            else if (operador.Equals(">="))
            {
                tipo1 = tipo1.Replace("const ", "");
                tipo2 = tipo2.Replace("const ", "");
                Antlr4.Runtime.Tree.TerminalNodeImpl op = (Antlr4.Runtime.Tree.TerminalNodeImpl)context.relop().GetChild(0);
                if (tipo1 != tipo2)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + op.Symbol.Line + " columna " + op.Symbol.Column +
                                                    ", no se pueden comparar expresiones de distinto tipo\n");
                    return false;
                }
                else if (tipo1 != "int" && tipo1 != "float")
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + op.Symbol.Line + " columna " + op.Symbol.Column +
                                                    ", el operador solo puede usarse para tipos int o float\n");
                    return false;
                }
            }
            else if (operador.Equals("<"))
            {
                tipo1 = tipo1.Replace("const ", "");
                tipo2 = tipo2.Replace("const ", "");
                Antlr4.Runtime.Tree.TerminalNodeImpl op = (Antlr4.Runtime.Tree.TerminalNodeImpl)context.relop().GetChild(0);
                if (tipo1 != tipo2)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + op.Symbol.Line + " columna " + op.Symbol.Column +
                                                    ", no se pueden comparar expresiones de distinto tipo\n");
                    return false;
                }
                else if (tipo1 != "int" && tipo1 != "float")
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + op.Symbol.Line + " columna " + op.Symbol.Column +
                                                    ", el operador solo puede usarse para tipos int o float\n");
                    return false;
                }
            }
            else
            {
                tipo1 = tipo1.Replace("const ", "");
                tipo2 = tipo2.Replace("const ", "");
                Antlr4.Runtime.Tree.TerminalNodeImpl op = (Antlr4.Runtime.Tree.TerminalNodeImpl)context.relop().GetChild(0);
                if (tipo1 != tipo2)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + op.Symbol.Line + " columna " + op.Symbol.Column +
                                                    ", no se pueden comparar expresiones de distinto tipo\n");
                    return false;
                }
                else if (tipo1 != "int" && tipo1 != "float")
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + op.Symbol.Line + " columna " + op.Symbol.Column +
                                                    ", el operador solo puede usarse para tipos int o float\n");
                    return false;
                }
            }
            return true;
        }

        public override object VisitModAST([NotNull] ParserProgra.ModASTContext context)
        {
            return (context.MOD().Symbol.Line + " Columna: " + context.MOD().Symbol.Column);
        }
        
        public override object VisitDesignatorStatementAST([NotNull] ParserProgra.DesignatorStatementASTContext context)
        {
            string tipoDesig = (string)Visit(context.designator());
            if (tipoDesig != null)
            {
                ParserProgra.DesignLabelASTContext design = (ParserProgra.DesignLabelASTContext)context.designator();
                if (design.ID().Count > 1)
                {
                    if(context.PI() != null)
                    {
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.PI().Symbol.Line + " columna " + context.PI().Symbol.Column +
                                                    ", " + design.ID(1).GetText() + " no es un metodo\n");
                        return false;
                    }
                    else if(context.EQUAL() != null)
                    {
                        string tipoExpr = (string)Visit(context.expr());
                        if(tipoExpr != null)
                        {
                            if (tipoExpr != tipoDesig)
                            {
                                txtBoxAnalisisContextual.AppendText("Error en linea " + context.EQUAL().Symbol.Line + " columna " + context.EQUAL().Symbol.Column +
                                                        ", se esperaba asignacion de tipo " + tipoDesig + "\n");
                                return false;
                            }
                        }   
                    }
                    else if(context.Inc() != null)
                    {
                        if(tipoDesig != "int")
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.Inc().Symbol.Line + " columna " + context.Inc().Symbol.Column +
                                                        ", operacion disponible solamente para variables de tipo int\n");
                            return false;
                        }
                    }
                    else
                    {
                        if(tipoDesig != "int")
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.Dec().Symbol.Line + " columna " + context.Dec().Symbol.Column +
                                                        ", operacion disponible solamente para variables de tipo int\n");
                            return false;
                        }
                    }
                }
                else
                {
                    objetoTabla obj = tablaSimbolos.buscar(design.ID(0).GetText(), 0);
                    if(context.PI() != null)
                    {
                        if (obj.esMetodo == true)
                        {
                            ParserProgra.TypeFormVarMethodDeclASTContext methodDecl = (ParserProgra.TypeFormVarMethodDeclASTContext)obj.tipoPuntero;
                            if (context.PI() == null)
                            {
                                txtBoxAnalisisContextual.AppendText("Error en linea " + design.ID(0).Symbol.Line + " columna " + design.ID(0).Symbol.Column +
                                                        ", " + design.ID(0).GetText() + " es un metodo, faltan los parentesis\n");
                                return false;
                            }
                            else if (methodDecl.formPars() != null)
                            {
                                List<string> parametrosRequeridos = new List<string>();
                                ParserProgra.TypeIDFormParsASTContext formP = (ParserProgra.TypeIDFormParsASTContext)methodDecl.formPars();
                                for (int i = 0; i < formP.ID().Count; i++)
                                {
                                    ParserProgra.IdTypeASTContext tipo = (ParserProgra.IdTypeASTContext)formP.type(i);
                                    if (tipo.PCI() == null)
                                    {
                                        parametrosRequeridos.Add(tipo.ID().GetText());
                                    }
                                    else
                                    {
                                        parametrosRequeridos.Add(tipo.ID().GetText() + "[]");
                                    }
                                }
                                string parametrosRequeridosStr = string.Join(", ", parametrosRequeridos);
                                if (context.actPars() == null)
                                {
                                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.PI().Symbol.Line + " columna " + context.PI().Symbol.Column +
                                                        ", parametros invalidos, se esperaba: " + parametrosRequeridosStr + "\n");
                                    return false;
                                }
                                else
                                {
                                    List<string> parametrosRecibidos = (List<string>)Visit(context.actPars());
                                    if (parametrosRequeridos.Count != parametrosRecibidos.Count)
                                    {
                                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.PI().Symbol.Line + " columna " + context.PI().Symbol.Column +
                                                        ", parametros invalidos, se esperaba: " + parametrosRequeridosStr + "\n");
                                        return false;
                                    }
                                    else
                                    {
                                        for (int i = 0; i < parametrosRecibidos.Count; i++)
                                        {
                                            if (parametrosRecibidos.ElementAt(i) != parametrosRequeridos.ElementAt(i))
                                            {
                                                txtBoxAnalisisContextual.AppendText("Error en linea " + context.PI().Symbol.Line + " columna " + context.PI().Symbol.Column +
                                                        ", parametros invalidos, se esperaba: " + parametrosRequeridosStr + "\n");
                                                return false;
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                if (context.actPars() != null)
                                {
                                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.PI().Symbol.Line + " columna " + context.PI().Symbol.Column +
                                                        ", el metodo " + design.ID(0).GetText() + "no debe recibir parametros\n");
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.PI().Symbol.Line + " columna " + context.PI().Symbol.Column +
                                                        ", el metodo " + design.ID(0).GetText() + "no es un metodo\n");
                            return false;
                        }
                    }

                    else if (context.EQUAL() != null)
                    {
                        string tipoExpr = (string)Visit(context.expr());
                        if (tipoExpr != null)
                        {
                            string tipoAux=tipoDesig.Replace("]","");
                            string tipoAux2=tipoAux.Replace("[","");
                            if (tipoExpr != tipoDesig && tipoExpr!=("new "+tipoAux2))
                            {
                                txtBoxAnalisisContextual.AppendText("Error en linea " + context.EQUAL().Symbol.Line + " columna " + context.EQUAL().Symbol.Column +
                                                        ", se esperaba asignacion de tipo " + tipoDesig + "\n");
                                return false;
                            }
                        }
                    }
                    else if (context.Inc() != null)
                    {
                        if (tipoDesig != "int")
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.Inc().Symbol.Line + " columna " + context.Inc().Symbol.Column +
                                                        ", operacion disponible solamente para variables de tipo int\n");
                            return false;
                        }
                    }
                    else
                    {
                        if (tipoDesig != "int")
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.Dec().Symbol.Line + " columna " + context.Dec().Symbol.Column +
                                                        ", operacion disponible solamente para variables de tipo int\n");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public override object VisitIdTypeAST([NotNull] ParserProgra.IdTypeASTContext context)
        {
            return null;
        }

        public override object VisitTokenClassClassDeclAST([NotNull] ParserProgra.TokenClassClassDeclASTContext context)
        {
            if (palabraReservada(context.ID().GetText()) == true)
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                    ", " + context.ID().GetText() + " es una palabra reservada\n");
            }

            else if (tablaSimbolos.buscar(context.ID().GetText(), nivelActual) != null)
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                    ", El ID: " + context.ID().GetText() + " ya ha sido declarado\n");
            }

            else
            {
                tablaSimbolos.insertar(context.ID().GetText(), context, nivelActual,false,"class");
            }

            openScope();
            List<List<string>> deletable = new List<List<string>>();
            List<string> sublist=new List<string>();
            for (int i = 0; i < context.varDecl().Count; i++)
            {
                sublist=(List<string>)(Visit(context.varDecl(i)));
                deletable.Add(sublist);
            }
            closeScope();

            for (int i = 0; i < deletable.Count;i++ )
            {
                for(int x=0;x<deletable.ElementAt(i).Count;x++)
                {
                    if(tablaSimbolos.buscar(deletable.ElementAt(i).ElementAt(x),1) != null)
                    {
                        tablaSimbolos.eliminarSimbolo(deletable.ElementAt(i).ElementAt(x));
                    }
                }
            }

            return null;            
        }

        public override object VisitTypeVarDeclAST([NotNull] ParserProgra.TypeVarDeclASTContext context)
        {
            List<string> sublista = new List<string>();
            if (palabraReservada(context.type().GetChild(0).GetText()) == false)
            {
                if (tablaSimbolos.buscar(context.type().GetChild(0).GetText(), nivelActual) != null)
                {
                    objetoTabla obj = tablaSimbolos.buscar(context.type().GetChild(0).GetText(), nivelActual);
                    for (int i = 0; i < obj.tipoPuntero.children.Count; i++)
                    {
                        if (obj.tipoPuntero.GetChild(i).GetText().Equals("class"))
                        {
                            for(int x=0;x<context.ID().Count;x++)
                            {
                                if (palabraReservada(context.ID(x).GetText()) == true)
                                {
                                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(x).Symbol.Line + " columna " + context.ID(x).Symbol.Column +
                                                                        ", " + context.ID(x).GetText() + " es una palabra reservada\n");
                                }

                                else if (context.type().GetChild(1) != null)
                                {
                                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(x).Symbol.Line + " columna " + context.ID(x).Symbol.Column +
                                                                        ", " + context.ID(x).GetText() + " no se permiten arreglos de clases\n");
                                }

                                else if (tablaSimbolos.buscar(context.ID(x).GetText(), nivelActual) != null)
                                {
                                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(x).Symbol.Line + " columna " + context.ID(x).Symbol.Column +
                                                                        ", El ID: " + context.ID(x).GetText() + " ya ha sido declarado\n");
                                }

                                else
                                {
                                    tablaSimbolos.insertar(context.ID(x).GetText(), context, nivelActual,false,obj.ID);
                                    sublista.Add(context.ID(x).GetText());
                                }
                            }
                            return sublista;
                        }
                    }
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(0).Symbol.Line + " columna " + ((context.ID(0).Symbol.Column) -1).ToString() +
                                                         ", " + context.ID(0).GetText() + " tipo invalido\n");
                }
                else
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(0).Symbol.Line + " columna " + ((context.ID(0).Symbol.Column) - 1).ToString() +
                                                        ", " + context.ID(0).GetText() + " tipo invalido\n");
                }
            }

            else if (context.type().GetChild(0).GetText() != "ord" && context.type().GetChild(0).GetText() != "chr" && context.type().GetChild(0).GetText() != "len")
            {
                if (context.type().GetChild(0).GetText().Equals("boolean") && context.type().GetChild(1)!=null)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(0).Symbol.Line + " columna " + (context.ID(0).Symbol.Column -1).ToString() +
                                                            ", no se permiten arreglos de boolean\n");
                    return sublista;
                }
                for (int x = 0; x < context.ID().Count; x++)
                {
                    if (palabraReservada(context.ID(x).GetText()) == true)
                    {
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(x).Symbol.Line + " columna " + context.ID(x).Symbol.Column +
                                                            ", " + context.ID(x).GetText() + " es una palabra reservada\n");
                    }

                    else if (tablaSimbolos.buscar(context.ID(x).GetText(), nivelActual) != null)
                    {
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(x).Symbol.Line + " columna " + context.ID(x).Symbol.Column +
                                                            ", El ID: " + context.ID(x).GetText() + " ya ha sido declarado\n");
                    }

                    else
                    {
                        if (context.type().GetChild(1) != null)
                        {
                            tablaSimbolos.insertar(context.ID(x).GetText(), context, nivelActual, false, (context.type().GetChild(0).GetText() + "[]"));
                        }
                        else
                        {
                            tablaSimbolos.insertar(context.ID(x).GetText(), context, nivelActual, false, context.type().GetChild(0).GetText());
                        }
                        sublista.Add(context.ID(x).GetText());
                    }
                }
            }

            else
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(0).Symbol.Line + " columna " + ((context.ID(0).Symbol.Column)-1).ToString() +
                                                            ", " + context.type().GetChild(0).GetText() + " es una palabra reservada\n");
            }

            return sublista; 
        }

        public override object VisitSubAST([NotNull] ParserProgra.SubASTContext context)
        {
            return (context.SUB().Symbol.Line + " Columna: " + context.SUB().Symbol.Column);
        }

        public override object VisitDesignLabelAST([NotNull] ParserProgra.DesignLabelASTContext context)
        {
            if(palabraReservada(context.ID(0).GetText()) == true)
            {
                if (context.ID(0).GetText() == "true" || context.ID(0).GetText() == "false")
                {
                    return "boolean";
                }
                else if(context.ID(0).GetText() == "len" || context.ID(0).GetText() == "int" || context.ID(0).GetText() == "ord")
                {
                    return "int";
                }
                else if(context.ID(0).GetText() == "chr" || context.ID(0).GetText() == "char")
                {
                    return "char";
                }
                else if(context.ID(0).GetText() == "float")
                {
                    return "float";
                }
                else
                {
                    return null;
                }
            }
            else if(tablaSimbolos.buscar(context.ID(0).GetText(),nivelActual) != null)
            {
                objetoTabla obj = tablaSimbolos.buscar(context.ID(0).GetText(), nivelActual);
                if(obj.tipo=="class")
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(0).Symbol.Line + " columna " + context.ID(0).Symbol.Column+
                                                            ", entrada invalida\n");
                    return null;
                }
                else if(palabraReservada(obj.tipo) == true || obj.tipo.Contains("[]"))
                {
                    if(context.ID().Count > 1)
                    {
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(1).Symbol.Line + " columna " + ((context.ID(1).Symbol.Column) -1).ToString() +
                                                            ", la variable " + context.ID(0).GetText() + " no es de tipo clase\n");
                        return null;
                    }
                    else
                    {
                        if(context.PCI().Count>1)
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.PCI(1).Symbol.Line + " columna " + context.PCI(1).Symbol.Column +
                                                            ", Los arreglos multidimensionales no estan permitidos\n");
                            return null;
                        }
                        else if(context.PCI().Count==1)
                        {
                            if(obj.tipo.Contains("[]") == false)
                            {
                                txtBoxAnalisisContextual.AppendText("Error en linea " + context.PCI(0).Symbol.Line + " columna " + context.PCI(0).Symbol.Column +
                                                            ", la variable " + context.ID(0).GetText() + " no es de tipo arreglo\n");
                                return null;
                            }
                            string tipo = (string) Visit(context.expr(0));
                            if (tipo != "int")
                            {
                                txtBoxAnalisisContextual.AppendText("Error en linea " + context.PCI(0).Symbol.Line + " columna " + (context.PCI(0).Symbol.Column + 1).ToString() +
                                                            ", Solamente se permiten indices de tipo entero\n");
                                return null;
                            }
                            else
                            {
                                string tipoF = obj.tipo.Replace("[", "");
                                string tipoFi = tipoF.Replace("]", "");
                                return tipoFi;
                            }
                        }
                        else
                        {
                            return obj.tipo;
                        }
                    }
                }
                else 
                {
                    if(context.ID().Count==1)
                    {
                        if(context.PCI().Count>0)
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.PCI(0).Symbol.Line + " columna " + context.PCI(0).Symbol.Column +
                                                            ", No se permiten arreglos de clases\n");
                            return null;
                        }
                        else
                        {
                            return obj.tipo;
                        }
                    }
                    else if(context.ID().Count==2)
                    {
                        if(context.GetText().Contains((context.ID(0).GetText() + "[")) == true)
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.PCI(0).Symbol.Line + " columna " + context.PCI(0).Symbol.Column +
                                                            ", No se permiten arreglos de clases\n");
                            return null;
                        }
                        else
                        {
                            string tipoP=obj.tipo;
                            obj=tablaSimbolos.buscar(tipoP,nivelActual);
                            ParserProgra.TokenClassClassDeclASTContext classDecl = (ParserProgra.TokenClassClassDeclASTContext)obj.tipoPuntero;
                            for (int x = 0; x < classDecl.varDecl().Count; x++)
                            {
                                ParserProgra.TypeVarDeclASTContext varDecl = (ParserProgra.TypeVarDeclASTContext)classDecl.varDecl(x);
                                for(int i=0; i < varDecl.ID().Count; i++)
                                {
                                    if(context.ID(1).GetText().Equals(varDecl.ID(i).GetText()) == true)
                                    {
                                        if(palabraReservada(varDecl.type().GetChild(0).GetText()) == false)
                                        {
                                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(1).Symbol.Line + " columna " + context.ID(1).Symbol.Column +
                                                            ", No puede acceder a una clase dentro de otra clase\n");
                                            return null;
                                        }
                                        else
                                        {
                                            if(varDecl.type().GetChild(1) != null)
                                            {
                                                if(context.PCI().Count>1)
                                                {
                                                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.PCI(1).Symbol.Line + " columna " + context.PCI(1).Symbol.Column +
                                                            ", Los arreglos multidimensionales no estan permitidos\n");
                                                    return null;
                                                }
                                                else if(context.PCI().Count==1)
                                                {
                                                    string tipo = (string)Visit(context.expr(0));
                                                    if (tipo != "int")
                                                    {
                                                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.PCI(0).Symbol.Line + " columna " + (context.PCI(0).Symbol.Column + 1).ToString() +
                                                                                    ", Solamente se permiten indices de tipo entero\n");
                                                        return null;
                                                    }
                                                    else
                                                    {
                                                        return (varDecl.type().GetChild(0).GetText());
                                                    }
                                                }
                                                else
                                                {
                                                    return (varDecl.type().GetChild(0).GetText() + "[]");
                                                }                                           
                                            }
                                            else
                                            {
                                                if (context.PCI().Count > 0)
                                                {
                                                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.PCI(0).Symbol.Line + " columna " + context.PCI(0).Symbol.Column +
                                                            ", la variable " + context.ID(1).GetText() + " no es de tipo arreglo\n");
                                                    return null;
                                                }
                                                else
                                                {
                                                    return varDecl.type().GetChild(0).GetText();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(1).Symbol.Line + " columna " + context.ID(1).Symbol.Column +
                                                            ", " + context.ID(1).GetText() + " No es atributo de "+context.ID(0).GetText());
                            return null;
                        }
                    }
                    else
                    {
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(1).Symbol.Line + " columna " + context.ID(1).Symbol.Column +
                                                            ", No se puede acceder a los atributos");
                        return null;
                    }
                }
                
                
            }
            else
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID(0).Symbol.Line + " columna " + context.ID(0).Symbol.Column +
                                                            ", la variable " + context.ID(0).GetText() + " no ha sido declarada\n");
                return null;
            }
        }

        public override object VisitExprActParsAST([NotNull] ParserProgra.ExprActParsASTContext context)
        {
            List<string> lista = new List<string>();
            for (int i = 0; i < context.expr().Count; i++)
            {
                if (Visit(context.expr(i)) != null)
                {
                    Visit(context.expr(i));
                    lista.Add((string)Visit(context.expr(i)));

                }
                else if (Visit(context.expr(i)) == null)
                {
                    if (i == 0)
                    {
                        //((context.ID(0).Symbol.Column)-1).ToString()
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.C() + " columna " + ((context.C(i).Symbol.Column) - 1).ToString() +
                                                           " expresion invalida\n");
                    }
                    else
                    {
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.C() + " columna " + ((context.C(i).Symbol.Column) + 1).ToString() +
                                                         " expresion invalida\n");
                    }

                    return null;
                }
            }
            return lista; ;
        }

        public override object VisitStateBreakAST([NotNull] ParserProgra.StateBreakASTContext context)
        {
            bool ult = pilaCiclos.ElementAt(pilaCiclos.Count - 1);
            if (ult == false)
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + context.TokenBreak().Symbol.Line + " columna " + context.TokenBreak().Symbol.Column +
                                                     " break fuera de contexto de ciclo \n");
                return false;
            }
            else
            {
                pilaCiclos.RemoveAt(pilaCiclos.Count -1);
                pilaCiclos.Add(false);
                return true;
            }
        }

        public override object VisitStateBlockAST([NotNull] ParserProgra.StateBlockASTContext context)
        {
            bool valido = (bool)Visit(context.block());
            if(valido==false)
            {
                return false;
            }
            return true;
        }

        public override object VisitDesignatorFactorAST([NotNull] ParserProgra.DesignatorFactorASTContext context)
        {
            string tipoDesig=(string) Visit(context.designator());
            if(tipoDesig  != null)
            {
                if(tipoDesig=="boolean" || tipoDesig=="int" || tipoDesig== "float" || tipoDesig=="char" )
                {
                    return tipoDesig;
                }
                ParserProgra.DesignLabelASTContext design=(ParserProgra.DesignLabelASTContext)context.designator();
                if (design.ID().Count>1)
                {
                    return tipoDesig;
                }
                else
                {
                    objetoTabla obj = tablaSimbolos.buscar(design.ID(0).GetText(), 0);
                    if(obj.esMetodo == true)
                    {
                        ParserProgra.TypeFormVarMethodDeclASTContext methodDecl = (ParserProgra.TypeFormVarMethodDeclASTContext)obj.tipoPuntero;
                        if(context.PI() == null)
                        {
                            txtBoxAnalisisContextual.AppendText("Error en linea " + design.ID(0).Symbol.Line + " columna " + design.ID(0).Symbol.Column +
                                                    ", " + design.ID(0).GetText() + " es un metodo, faltan los parentesis\n");
                            return null;
                        }
                        else if(methodDecl.formPars() != null)
                        {
                            List<string> parametrosRequeridos = new List<string>();
                            ParserProgra.TypeIDFormParsASTContext formP = (ParserProgra.TypeIDFormParsASTContext)methodDecl.formPars();
                            for (int i = 0; i < formP.ID().Count;i++)
                            {
                                ParserProgra.IdTypeASTContext tipo=(ParserProgra.IdTypeASTContext)formP.type(i);
                                if(tipo.PCI()==null)
                                {
                                    parametrosRequeridos.Add(tipo.ID().GetText());
                                }
                                else
                                {
                                    parametrosRequeridos.Add(tipo.ID().GetText() + "[]");
                                }
                            }
                            string parametrosRequeridosStr = string.Join(", ", parametrosRequeridos);
                            if(context.actPars() == null)
                            {
                                txtBoxAnalisisContextual.AppendText("Error en linea " + context.PI().Symbol.Line + " columna " + context.PI().Symbol.Column +
                                                    ", parametros invalidos, se esperaba: " + parametrosRequeridosStr + "\n");
                                return null;
                            }
                            else
                            {
                                List<string> parametrosRecibidos = (List<string>)Visit(context.actPars());
                                if(parametrosRequeridos.Count != parametrosRecibidos.Count)
                                {
                                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.PI().Symbol.Line + " columna " + context.PI().Symbol.Column +
                                                    ", parametros invalidos, se esperaba: " + parametrosRequeridosStr + "\n");
                                    return null;
                                }
                                else
                                {
                                    for (int i=0;i<parametrosRecibidos.Count;i++)
                                    {
                                        if(parametrosRecibidos.ElementAt(i) != parametrosRequeridos.ElementAt(i))
                                        {
                                            txtBoxAnalisisContextual.AppendText("Error en linea " + context.PI().Symbol.Line + " columna " + context.PI().Symbol.Column +
                                                    ", parametros invalidos, se esperaba: " + parametrosRequeridosStr + "\n");
                                            return null;
                                        }
                                    }
                                    return tipoDesig;
                                }
                            }

                        }
                        else
                        {
                            if(context.actPars()!= null)
                            {
                                txtBoxAnalisisContextual.AppendText("Error en linea " + context.PI().Symbol.Line + " columna " + context.PI().Symbol.Column +
                                                    ", el metodo " + design.ID(0).GetText() + "no debe recibir parametros\n");
                                return null;
                            }
                            else
                            {
                                return tipoDesig;
                            }
                        }
                    }
                    else
                    {
                        return tipoDesig;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public override object VisitTermAddlopExprAST([NotNull] ParserProgra.TermAddlopExprASTContext context)
        {
            string x = (string)Visit(context.term(0));
            if (x == null)
            {
                return null;
            }
            else if (x.Equals("int"))
            {
                for (int i = 1; i < context.term().Count; i++)
                {
                    string tipo = (string)Visit(context.term(i));
                    if (!tipo.Equals("int"))
                    {
                        string posicion=(string)Visit(context.addlop(i-1));
                        txtBoxAnalisisContextual.AppendText("Error en linea " +  posicion + ", la operacion es invalida con los tipos dados\n");
                        return null;
                    }
                }
                context.tipo = "int";
                return "int";
            }
            else if (x.Equals("float"))
            {
                for (int i = 1; i < context.term().Count; i++)
                {
                    string tipo = (string)Visit(context.term(i));
                    if (!tipo.Equals("float"))
                    {
                        string posicion=(string)Visit(context.addlop(i-1));
                        txtBoxAnalisisContextual.AppendText("Error en linea " + posicion + ", la operacion es invalida con los tipos dados\n");
                        return null;
                    }
                }
                context.tipo = "float";
                return "float";
            }
            else if ((!x.Equals("int") || !x.Equals("float")) && (context.term(1) != null || context.SUB()!=null))
            {
                if( context.SUB()!=null)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " +  context.SUB().Symbol.Line + " columna " + context.SUB().Symbol.Column +
                                                        ", la operacion es invalida con los tipos dados\n");
                }
                else if(context.term(1) != null )
                {
                    string posicion=(string)Visit(context.addlop(0));
                    txtBoxAnalisisContextual.AppendText("Error en linea " + posicion + ", la operacion es invalida con los tipos dados\n");
                }
                return null;
            }
            else if (x.Equals("char"))
            {
                context.tipo = "char";
                return "char";
            }
            else if (x == "boolean")
            {
                context.tipo = "char";
                return "boolean";
            }
            else if (x.Contains("new "))
            {
                objetoTabla obj = tablaSimbolos.buscar(x.Substring(4, x.Length - 4), 0);
                if (obj == null)
                {
                    if (x.Substring(4, x.Length - 4).Equals("int"))
                    {
                        context.tipo = "int[]";
                    }
                    else if (x.Substring(4, x.Length - 4).Equals("char"))
                    {
                        context.tipo = "char[]";
                    }
                }
                else if (obj != null)
                {
                    context.tipo = x.Substring(4, x.Length - 4);
                }
                return x;
            }
            else if(x.Contains("const"))
            {
                context.tipo = x.Substring(5, x.Length - 5);
                return x;
            }
            return null;
        }
        
        public override object VisitStatementBlockAST([NotNull] ParserProgra.StatementBlockASTContext context)
        {
            for (int i = 0; i < context.statement().Count; i++)
            {
                bool valido = (bool)Visit(context.statement(i));
                if (valido == false)
                {
                    return false;
                }
            }
            return true;
        }

        public override object VisitFactorMulopFactorTermAST([NotNull] ParserProgra.FactorMulopFactorTermASTContext context)
        {
            string x = (string)Visit(context.factor(0));
            if (x == null)
            {
                return null;
            }
            else if (x.Equals("int"))
            {
                for(int i=1;i<context.factor().Count;i++){
                    string tipo=(string)Visit(context.factor(i));
                    if(!tipo.Equals("int")){
                        string posicion = (string)Visit(context.mulop(i - 1));
                        txtBoxAnalisisContextual.AppendText("Error en linea " + posicion + ", la operacion es invalida con los tipos dados\n");
                        return null;
                    }
                }
                return "int";
            }
            else if (x.Equals("float"))
            {
                for (int i = 1; i < context.factor().Count; i++)
                {
                    string tipo = (string)Visit(context.factor(i));
                    if (!tipo.Equals("float"))
                    {
                        string posicion = (string)Visit(context.mulop(i - 1));
                        txtBoxAnalisisContextual.AppendText("Error en linea " + posicion + ", la operacion es invalida con los tipos dados\n");
                        return null;
                    }
                }
                return "float";
            }
            else if((!x.Equals("int") || !x.Equals("float")) && context.factor(1)!=null)
            {
                string posicion = (string)Visit(context.mulop(0));
                txtBoxAnalisisContextual.AppendText("Error en linea " + posicion + ", la operacion es invalida con los tipos dados\n");
                return null;
            }
            else if (x.Equals("char"))
            {
                return "char";
            }
            else if (x == "boolean")
            {
                return "boolean";
            }
            else if (x.Contains("new "))
            {
                return x;
            }
            else if(x.Contains("const")==true)
            {
                return x;
            }
            return null;
        }

        public override object VisitEqualAST([NotNull] ParserProgra.EqualASTContext context)
        {
            return "==";
        }

        public override object VisitProgramTokenClassAST([NotNull] ParserProgra.ProgramTokenClassASTContext context)
       {
            pilaCiclos.Add(false);
            if (palabraReservada(context.ID().GetText()) == true)
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                    ", " + context.ID().GetText() + " es una palabra reservada\n");
            }

            else if (tablaSimbolos.buscar(context.ID().GetText(),nivelActual) != null)
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                    ", El ID: " + context.ID().GetText() + " ya ha sido declarado\n");
            }

            else
            {
                tablaSimbolos.insertar(context.ID().GetText(), context, nivelActual,false,"mainClass");
            }

            for (int i = 0; i < context.programAux().Count; i++)
            {
                Visit(context.programAux(i));
            }

            for (int i = 0; i < context.methodDecl().Count; i++)
            {
                Visit(context.methodDecl(i));
            }
            return null;
        }

        public override object VisitCondFactCondTermAST([NotNull] ParserProgra.CondFactCondTermASTContext context)
        {
            for (int i = 0; i < context.condFact().Count; i++)
            {
                bool condicion=(bool)Visit(context.condFact(i));
                if(condicion==false)
                {
                    return false;
                }
            }
            return true;
        }

        public override object VisitStatementForAST([NotNull] ParserProgra.StatementForASTContext context)
        {
            string expression = (string)Visit(context.expr());
            if(expression.Equals("int") == false)
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + (context.PI().Symbol.Line + 1).ToString() + " columna " + (context.PI().Symbol.Column + 1).ToString() +
                                                                    ", se esperaba expresion de tipo int\n");
                return false;
            }
            else
            {
                if(context.condition() != null)
                {
                    bool condicion = (bool)Visit(context.condition());
                    if(condicion==false)
                    {
                        return false;
                    }
                }
                if(context.statement().Count > 1)
                {
                    bool condicion2 = (bool)Visit(context.statement(0));
                    if(condicion2==false)
                    {
                        return false;
                    }
                    push();
                    Visit(context.statement(1));
                    pop();
                }
                else
                {
                    push();
                    Visit(context.statement(0));
                    pop();
                }
                
            }
            return true;
        }

        public override object VisitDifferentAST([NotNull] ParserProgra.DifferentASTContext context)
        {
            return "!=";
        }

        public override object VisitStateReadAST([NotNull] ParserProgra.StateReadASTContext context)
        {
            string valido = (string)Visit(context.designator());
            if (valido == null)
            {
                return false;
            }
            return true;
        }

        public override object VisitStateWriteAST([NotNull] ParserProgra.StateWriteASTContext context)
        {
            string valido = (string)Visit(context.expr());
            if (valido == null)
            {
                return false;
            }
            return true;
        }

        public override object VisitAddAST([NotNull] ParserProgra.AddASTContext context)
        {
            return (context.ADD().Symbol.Line + " Columna: " + context.ADD().Symbol.Column);
        }

        public override object VisitMajorAST([NotNull] ParserProgra.MajorASTContext context)
        {
            return ">";
        }

        public override object VisitMulAST([NotNull] ParserProgra.MulASTContext context)
        {
            return (context.MUL().Symbol.Line + " Columna: " + context.MUL().Symbol.Column);
        }

        public override object VisitStateWhileAST([NotNull] ParserProgra.StateWhileASTContext context)
        {
            bool condicion = (bool)Visit(context.condition());
            if(condicion==false)
            {
                return false;
            }
            else
            {
                push();
                Visit(context.statement());
                pop();
                return true;
            }
        }

        public override object VisitMinorAST([NotNull] ParserProgra.MinorASTContext context)
        {
            return "<";
        }

        public override object VisitMajorEqualAST([NotNull] ParserProgra.MajorEqualASTContext context)
        {
            return ">=";
        }

        public override object VisitStatementIFAST([NotNull] ParserProgra.StatementIFASTContext context)
        {
            bool condicion= (bool)Visit(context.condition());
            if(condicion==true)
            {
                Visit(context.statement(0));
                if(context.TokenElse() != null)
                {
                    Visit(context.statement(1));
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public override object VisitTypeFormVarMethodDeclAST([NotNull] ParserProgra.TypeFormVarMethodDeclASTContext context)
        {
            if(context.type()!=null)
            {
                if (palabraReservada(context.type().GetChild(0).GetText()) == false)
                {
                    if (tablaSimbolos.buscar(context.type().GetChild(0).GetText(), nivelActual) != null)
                    {
                        objetoTabla obj = tablaSimbolos.buscar(context.type().GetChild(0).GetText(), nivelActual);
                        for (int i = 0; i < obj.tipoPuntero.children.Count; i++)
                        {
                            if (obj.tipoPuntero.GetChild(i).GetText().Equals("class"))
                            {
                                if (palabraReservada(context.ID().GetText()) == true)
                                {
                                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                                        ", " + context.ID().GetText() + " es una palabra reservada\n");
                                }

                                else if (context.type().GetChild(1) != null)
                                {
                                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                                        ", " + context.ID().GetText() + " no se permiten arreglos de clases\n");
                                }

                                else if (tablaSimbolos.buscar(context.ID().GetText(), nivelActual) != null)
                                {
                                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                                        ", El ID: " + context.ID().GetText() + " ya ha sido declarado\n");
                                }

                                else
                                {
                                    tablaSimbolos.insertar(context.ID().GetText(), context, nivelActual, true, obj.ID);
                                }
                            }
                        }
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + ((context.ID().Symbol.Column) - 1).ToString() +
                                                             ", " + context.ID().GetText() + " tipo invalido\n");
                    }
                    else
                    {
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + ((context.ID().Symbol.Column) - 1).ToString() +
                                                            ", " + context.ID().GetText() + " tipo invalido\n");
                    }
                }

                else if (context.type().GetChild(0).GetText() != "ord" && context.type().GetChild(0).GetText() != "chr" && context.type().GetChild(0).GetText() != "len")
                {
                    if (context.type().GetChild(0).GetText().Equals("boolean") && context.type().GetChild(1) != null)
                    {
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + (context.ID().Symbol.Column - 1).ToString() +
                                                                ", no se permiten arreglos de boolean\n");
                    }
                    else if (palabraReservada(context.ID().GetText()) == true)
                    {
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                            ", " + context.ID().GetText() + " es una palabra reservada\n");
                    }

                    else if (tablaSimbolos.buscar(context.ID().GetText(), nivelActual) != null)
                    {
                        txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                            ", El ID: " + context.ID().GetText() + " ya ha sido declarado\n");
                    }

                    else
                    {
                        if (context.type().GetChild(1) != null)
                        {
                            tablaSimbolos.insertar(context.ID().GetText(), context, nivelActual, true, (context.type().GetChild(0).GetText() + "[]"));
                        }
                        else
                        {
                            tablaSimbolos.insertar(context.ID().GetText(), context, nivelActual, true, context.type().GetChild(0).GetText());
                        }
                    }
                }

                else
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + ((context.ID().Symbol.Column) - 1).ToString() +
                                                        ", " + context.type().GetChild(0).GetText() + " es una palabra reservada\n");

                }
            }

            else
            {
                if (palabraReservada(context.ID().GetText()) == true)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                        ", " + context.ID().GetText() + " es una palabra reservada\n");
                }
                else if (tablaSimbolos.buscar(context.ID().GetText(), nivelActual) != null)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                        ", El ID: " + context.ID().GetText() + " ya ha sido declarado\n");
                }
                else
                {
                    tablaSimbolos.insertar(context.ID().GetText(), context, nivelActual, true, "void");
                }
            }

            openScope();
            List<List<string>> deletable = new List<List<string>>();
            List<string> sublist = new List<string>();
            if (context.formParsL != null)
            {
                sublist=(List<string>)(Visit(context.formPars()));
                deletable.Add(sublist);
            }
            for (int i = 0; i < context.varDecl().Count; i++)
            {
                sublist = (List<string>)(Visit(context.varDecl(i)));
                deletable.Add(sublist);
            }
            Visit(context.block());
            closeScope();

            for (int i = 0; i < deletable.Count; i++)
            {
                for (int x = 0; x < deletable.ElementAt(i).Count; x++)
                {
                    if (tablaSimbolos.buscar(deletable.ElementAt(i).ElementAt(x), 1) != null)
                    {
                        tablaSimbolos.eliminarSimbolo(deletable.ElementAt(i).ElementAt(x));
                    }
                }
            }

            return null;
        }

        public override object VisitTokenNewFactorAST([NotNull] ParserProgra.TokenNewFactorASTContext context)
        {
            if (palabraReservada(context.ID().GetText()) == false && (tablaSimbolos.buscar(context.ID().GetText(),0))==null)
            {
                txtBoxAnalisisContextual.AppendText("Error en la linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                   ",El ID: " + context.ID().GetText() + " es un tipo invalido\n");
                return null;
            }
            else if (context.ID().GetText().Equals("int"))
            {
                if (context.PCI() == null)
                {
                    txtBoxAnalisisContextual.AppendText("Error en la linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                    " se esperaba [");
                    return null;
                }
                string visitT = (string)Visit(context.expr());
                if (visitT != "int")
                {
                    txtBoxAnalisisContextual.AppendText("Error en la linea " + context.PCI().Symbol.Line + " columna " + context.PCI().Symbol.Column +
                    " los indices deben ser de tipo int");
                    return null;
                }
                return "new int";
            }
            else if (context.ID().GetText().Equals("char"))
            {
                if (context.PCI() == null)
                {
                    txtBoxAnalisisContextual.AppendText("Error en la linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                    " se esperaba [");
                    return null;
                }
                string visitT = (string)Visit(context.expr());
                if (visitT != "int")
                {
                    txtBoxAnalisisContextual.AppendText("Error en la linea " + context.PCI().Symbol.Line + " columna " + context.PCI().Symbol.Column +
                    " los indices deben ser de tipo int");
                    return null;
                }
                return "new char";
            }
            else if (context.ID().GetText().Equals("boolean"))
            {
                txtBoxAnalisisContextual.AppendText("Error en la linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                    ", no se permiten arreglos booleanos");
                return null;
            }
            else if (context.ID().GetText().Equals("float"))
            {
                txtBoxAnalisisContextual.AppendText("Error en la linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                    ", no se permiten arreglos flotantes");
                return null;
            }
            else
            {
                objetoTabla obj = tablaSimbolos.buscar(context.ID().GetText(), nivelActual);
                if (obj == null)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                        ", El ID: " + context.ID().GetText() + " no ha sido declarado\n");
                    return null;
                }
                if (context.PCI() != null)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + ((context.ID().Symbol.Column) + 1).ToString() +
                                                    " no se permiten arreglos de clases\n");
                    return null;
                }

                return "new " + context.ID();
            }
            //return null;
        }

        public override object VisitStateReturnAST([NotNull] ParserProgra.StateReturnASTContext context)
        {
            ParserProgra.TypeFormVarMethodDeclASTContext obj = (ParserProgra.TypeFormVarMethodDeclASTContext)context.parent.parent;
            if (obj.TokenVoid() != null)
            {
                if (context.expr() != null)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.TokenReturn().Symbol.Line + " columna " + context.TokenReturn().Symbol.Column +
                                                    "tipo de retorno invalido\n");
                    return false;
                }
            }
            else
            {
                if (context.expr() == null)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.TokenReturn().Symbol.Line + " columna " + context.TokenReturn().Symbol.Column +
                                                    "se esperaba un tipo de retorno\n");
                    return false;
                }
                string tipoExpr = (string)Visit(context.expr());
                if (!tablaSimbolos.buscar(obj.ID().GetText(), 0).tipo.Equals(tipoExpr))
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.TokenReturn().Symbol.Line + " columna " + context.TokenReturn().Symbol.Column +
                                                    "tipo de retorno invalido\n");
                    return false;
                }
            }
            return true;
        }

        public override object VisitTokenConstConstDeclAST([NotNull] ParserProgra.TokenConstConstDeclASTContext context)
        {

            if (palabraReservada(context.ID().GetText()) == true)
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                    ", " + context.ID().GetText() + " es una palabra reservada\n");
                return null;
            }

            else if (tablaSimbolos.buscar(context.ID().GetText(),nivelActual) != null)
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                    ", El ID: " + context.ID().GetText() + " ya ha sido declarado\n");
                return null;
            }

            if (context.type().GetChild(1) != null || context.type().GetChild(2) != null)
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                    ", tipo invalido");
        
            }

            else if(context.type().GetChild(0).GetText().Equals("int"))
            {
                if(context.Num() == null)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.CHAR().Symbol.Line + " columna " + context.CHAR().Symbol.Column +
                                                        ", Error de tipo, la variable " + context.ID().GetText() + " es de tipo int\n");
                }
                else
                {
                    tablaSimbolos.insertar(context.ID().GetText(), context, nivelActual, false, "const int");
                    tablaSimbolos.inicializar(context.ID().GetText(), nivelActual);
                }
            }

            else if (context.type().GetChild(0).GetText().Equals("char"))
            {
                if (context.CHAR() == null)
                {
                    txtBoxAnalisisContextual.AppendText("Error en linea " + context.Num().Symbol.Line + " columna " + context.Num().Symbol.Column +
                                                        ", Error de tipo, la variable " + context.ID().GetText() + " es de tipo char\n");
                }
                else
                {
                    tablaSimbolos.insertar(context.ID().GetText(), context, nivelActual, false, "const char");
                    tablaSimbolos.inicializar(context.ID().GetText(), nivelActual);
                }
                    
            }

            else
            {
                txtBoxAnalisisContextual.AppendText("Error en linea " + context.ID().Symbol.Line + " columna " + context.ID().Symbol.Column +
                                                        ", tipo invalido");
            }
            return null; 
        }

        public override object VisitCharFactorAST([NotNull] ParserProgra.CharFactorASTContext context)
        {
            return "char";
        }

        public override object VisitNumFactoAST([NotNull] ParserProgra.NumFactoASTContext context)
        {
            return "int";
        }
   
    }
}