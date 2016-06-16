using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prograCompi
{
    class printTree : prograCompi.ParserPrograBaseVisitor<object>
    {
        public int cont = 0;

        //variable estatica global que almacena todo el arbol de manera que luego pueda ser copiado en el textBox deseado
        public static System.Windows.Forms.RichTextBox txtBoxTreeVisitor = new RichTextBox();

        //Metodo que imprime una cantidad de guiones las veces que indique el contador para indicar la indexacion del nodo en el arbol
        public void printTab(int cont)
        {
            for (int i = 0; i < cont; i++)
            {
                txtBoxTreeVisitor.AppendText("-----");
            }
        }

        //Metodos que sobreescriben los metodos de la clase ParserPrograBaseVisitor, llaman a la funcion printTab para imprimir los contadores de indexacion,
        //seguido de el texto respectivo de la etiqueta a la que se esta visitando, llama a los respectivos metodo para visitar las etiquetas que se requiera segun las reglas del parser
        //y a su vez aumenta y disminuye la variable cont conforme se visitan hijos y se devuelve de manera recursiva

        public override object VisitProgramAuxLabelAST([NotNull] ParserProgra.ProgramAuxLabelASTContext context)
        {
            if (context.constDecl() != null)
            {
                cont++;
                Visit(context.constDecl());
                cont--;
            }

            else if (context.varDecl() != null)
            {
                cont++;
                Visit(context.varDecl());
                cont--;
            }

            else if (context.classDecl() != null)
            {
                cont++;
                Visit(context.classDecl());
                cont--;
            }

            return null;
        }

        public override object VisitCondTermConditionAST([NotNull] ParserProgra.CondTermConditionASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("condition: \n");
            cont++;
            for (int i = 0; i < context.condTerm().Count; i++)
            {
                Visit(context.condTerm(i));
            }
            cont--;


            //Analisis contextual

            return null;
        }


        public override object VisitMinorEqualAST([NotNull] ParserProgra.MinorEqualASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("relop: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenIfMinorEqual\n");
            cont--;
            return null;
        }

        public override object VisitTypeIDFormParsAST([NotNull] ParserProgra.TypeIDFormParsASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("formPars: \n");
            cont++;
            Visit(context.type(0));
            printTab(cont);
            txtBoxTreeVisitor.AppendText("ID => " + context.ID(0).GetText() + "\n");
            for (int i = 1; i < context.type().Count; i++)
            {
                Visit(context.type(i));
                printTab(cont);
                txtBoxTreeVisitor.AppendText("ID => " + context.ID(i).GetText() + "\n");
            }
            cont--;
            return null;
        }

        public override object VisitDivAST([NotNull] ParserProgra.DivASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("mulop: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenDiv\n");
            cont--;
            return null;
        }

        public override object VisitBoolFactorAST([NotNull] ParserProgra.BoolFactorASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("factor: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("BOOL => " + context.BOOL().GetText() + "\n");
            printTab(cont);
            //txtBoxTreeVisitor.AppendText("Tipo => " + "boolean");
            //txtBoxTreeVisitor.AppendText("Posicion Memoria => " + context.BOOL().GetHashCode().ToString() + "\n");
            cont--;
            return null;
        }

        public override object VisitExprFactorAST([NotNull] ParserProgra.ExprFactorASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("factor: \n");
            cont++;
            Visit(context.expr());
            cont--;
            return null;
        }

        public override object VisitStatePyCAST([NotNull] ParserProgra.StatePyCASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("StatementPyComa: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenPyComa\n");
            cont--;
            return null;
        }

        public override object VisitExprRelopExprCondFactAST([NotNull] ParserProgra.ExprRelopExprCondFactASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("condFact\n");
            cont++;
            Visit(context.expr(0));
            Visit(context.relop());
            Visit(context.expr(1));
            cont--;
            return null;
        }

        public override object VisitModAST([NotNull] ParserProgra.ModASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("mulop: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenMod\n");
            cont--;
            return null;
        }

        public override object VisitDesignatorStatementAST([NotNull] ParserProgra.DesignatorStatementASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("statementDesignator: \n");
            cont++;

            Visit(context.designator());

            if (context.exprL != null)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("TokenEqual\n");
                Visit(context.expr());
            }
            else if (context.actParsL != null)
            {
                Visit(context.actPars());
            }
            else if (context.incL != null)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("TokenINC\n");
            }
            else if (context.decL != null)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("TokenDEC\n");
            }

            cont--;
            return null;
        }

        public override object VisitIdTypeAST([NotNull] ParserProgra.IdTypeASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("type: \n");
            if (context.pci == null && context.pcd == null)
            {
                cont++;
                printTab(cont);
                txtBoxTreeVisitor.AppendText(context.ID().GetText() + "\n");
                cont--;
            }

            if (context.pci != null && context.pcd != null)
            {
                cont++;
                printTab(cont);
                txtBoxTreeVisitor.AppendText(context.ID().GetText() + context.PCI().GetText() + context.PCD().GetText() + "\n");
                cont--;
            }
            return null;
        }

        public override object VisitTokenClassClassDeclAST([NotNull] ParserProgra.TokenClassClassDeclASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("classDecl: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenClass\n");
            printTab(cont);
            txtBoxTreeVisitor.AppendText("ID => " + context.ID().GetText() + "\n");
            printTab(cont);
            txtBoxTreeVisitor.AppendText("Posicion Memoria => " + context.ID().GetHashCode().ToString() + "\n");
            for (int i = 0; i < context.varDecl().Count; i++)
            {
                Visit(context.varDecl(i));
            }
            cont--;
            return null;
        }

        public override object VisitTypeVarDeclAST([NotNull] ParserProgra.TypeVarDeclASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("varDecl: \n");
            cont++;
            Visit(context.type());
            for (int i = 0; i < context.ID().Count; i++)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("ID => " + context.ID(i).GetText() + "\n");
                printTab(cont);
                txtBoxTreeVisitor.AppendText("Posicion Memoria => " + context.ID(i).GetHashCode().ToString() + "\n");
            }
            cont--;
            return null;
        }

        public override object VisitSubAST([NotNull] ParserProgra.SubASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("addlop: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenSub\n");
            cont--;
            return null;
        }

        public override object VisitDesignLabelAST([NotNull] ParserProgra.DesignLabelASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("designator: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("ID => " + context.ID(0).GetText() + "\n");
            for (int i = 1; i < context.ID().Count; i++)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("ID => " + context.ID(i).GetText() + "\n");
            }
            for (int i = 0; i < context.expr().Count; i++)
            {
                Visit(context.expr(i));
            }
            cont--;
            return null;
        }

        public override object VisitExprActParsAST([NotNull] ParserProgra.ExprActParsASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("ActPars: \n");
            cont++;
            for (int i = 0; i < context.expr().Count; i++)
            {
                Visit(context.expr(i));
            }
            cont--;
            return null;
        }

        public override object VisitStateBreakAST([NotNull] ParserProgra.StateBreakASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("statementBreak: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenBreak\n");
            cont--;
            return null;
        }

        public override object VisitStateBlockAST([NotNull] ParserProgra.StateBlockASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("StatementBlock: \n");
            cont++;
            Visit(context.block());
            cont--;
            return null;
        }

        public override object VisitDesignatorFactorAST([NotNull] ParserProgra.DesignatorFactorASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("factor: \n");
            cont++;
            Visit(context.designator());
            if (context.actPars() != null)
            {
                Visit(context.actPars());
            }
            cont--;
            return null;
        }

        public override object VisitTermAddlopExprAST([NotNull] ParserProgra.TermAddlopExprASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("expr: \n");
            cont++;
            if (context.SUB() != null)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("TokenSub\n");
            }

            string tipo1 = (string)Visit(context.term(0));
            printTab(cont);
            if (tipo1 != null)
            {
                txtBoxTreeVisitor.AppendText("Tipo => " + tipo1 + "\n");
            }
            for (int i = 1; i < context.addlop().Count; i++)
            {
                Visit(context.addlop(i));
                Visit(context.term(i + 1));
                printTab(cont);
                string tipo = (string)Visit(context.term(i));
                if (tipo != null)
                {
                    txtBoxTreeVisitor.AppendText("Tipo => " + tipo + "\n");
                }
            }
            cont--;
            return null;
        }

        public override object VisitStatementBlockAST([NotNull] ParserProgra.StatementBlockASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("block: \n");
            cont++;
            for (int i = 0; i < context.statement().Count; i++)
            {
                Visit(context.statement(i));
            }
            cont--;
            return null;
        }

        public override object VisitFactorMulopFactorTermAST([NotNull] ParserProgra.FactorMulopFactorTermASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("term: \n");
            cont++;
            Visit(context.factor(0));
            for (int i = 0; i < context.mulop().Count; i++)
            {
                Visit(context.mulop(i));
                Visit(context.factor(i + 1));
            }
            cont--;
            return null;
        }

        public override object VisitEqualAST([NotNull] ParserProgra.EqualASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("relop: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenIfEqual\n");
            cont--;
            return null;
        }

        public override object VisitProgramTokenClassAST([NotNull] ParserProgra.ProgramTokenClassASTContext context)
        {
            txtBoxTreeVisitor.AppendText("program: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenClass\n");
            printTab(cont);
            txtBoxTreeVisitor.AppendText("ID => " + context.ID().GetText() + "\n");
            cont--;

            for (int i = 0; i < context.programAux().Count; i++)
            {
                Visit(context.programAux(i));
            }

            for (int i = 0; i < context.methodDecl().Count; i++)
            {
                cont++;
                Visit(context.methodDecl(i));
                cont--;
            }

            return null;
        }

        public override object VisitCondFactCondTermAST([NotNull] ParserProgra.CondFactCondTermASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("CondTerm: \n");
            cont++;
            Visit(context.condFact(0));

            for (int i = 0; i < context.AND().Count; i++)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("TokenAND\n");
                Visit(context.condFact(i + 1));
            }
            cont--;
            return null;
        }

        public override object VisitStatementForAST([NotNull] ParserProgra.StatementForASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("statementFor: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenFor\n");
            Visit(context.expr());
            if (context.conditionL != null)
            {
                Visit(context.condition());
            }
            for (int i = 0; i < context.statement().Count; i++)
            {
                Visit(context.statement(i));
            }
            cont--;
            return null;
        }

        public override object VisitDifferentAST([NotNull] ParserProgra.DifferentASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("relop: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenIfDifferent\n");
            cont--;
            return null;
        }

        public override object VisitStateReadAST([NotNull] ParserProgra.StateReadASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("statementRead: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenRead\n");
            Visit(context.designator());
            cont--;
            return null;
        }

        public override object VisitStateWriteAST([NotNull] ParserProgra.StateWriteASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("Statement Write: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenWrite\n");
            Visit(context.expr());

            if (context.Num() != null)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("NUM => " + context.Num().GetText() + "\n");
            }
            cont--;
            return null;
        }

        public override object VisitAddAST([NotNull] ParserProgra.AddASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("addlop: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenADD\n");
            cont--;
            return null;
        }

        public override object VisitMajorAST([NotNull] ParserProgra.MajorASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("relop: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenIfMajor\n");
            cont--;
            return null;
        }

        public override object VisitMulAST([NotNull] ParserProgra.MulASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("mulop: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenMul\n");
            cont--;
            return null;
        }

        public override object VisitStateWhileAST([NotNull] ParserProgra.StateWhileASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("StatementWhile: \n");
            cont++;
            Visit(context.condition());
            Visit(context.statement());
            cont--;
            return null;
        }

        public override object VisitMinorAST([NotNull] ParserProgra.MinorASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("relop: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenIfMinor\n");
            cont--;
            return null;
        }

        public override object VisitMajorEqualAST([NotNull] ParserProgra.MajorEqualASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("relop: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenIfMajorEqual\n");
            cont--;
            return null;
        }

        public override object VisitStatementIFAST([NotNull] ParserProgra.StatementIFASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("statementIF: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenIF\n");
            Visit(context.condition());
            Visit(context.statement(0));
            cont--;
            for (int i = 1; i < context.statement().Count; i++)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("TokenELSE\n");
                Visit(context.statement(i));
            }
            cont--;
            return null;
        }

        public override object VisitTypeFormVarMethodDeclAST([NotNull] ParserProgra.TypeFormVarMethodDeclASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("methodDecl: \n");
            cont++;
            if (context.typeL != null)
            {
                Visit(context.type());
            }
            else
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("TokenVoid\n");
            }
            printTab(cont);
            txtBoxTreeVisitor.AppendText("ID => " + context.ID().GetText() + "\n");
            printTab(cont);
            txtBoxTreeVisitor.AppendText("Posicion Memoria => " + context.ID().GetHashCode().ToString() + "\n");
            if (context.formParsL != null)
            {
                Visit(context.formPars());
            }
            for (int i = 0; i < context.varDecl().Count; i++)
            {
                Visit(context.varDecl(i));
            }
            Visit(context.block());
            cont--;
            return null;
        }

        public override object VisitTokenNewFactorAST([NotNull] ParserProgra.TokenNewFactorASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("factor: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenNew\n");
            printTab(cont);
            //txtBoxTreeVisitor.AppendText("ID => " + context.ID().GetText() + "\n");
            if (context.PCI() != null)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("Tipo => " + context.ID().GetText() + "[]");
                //txtBoxTreeVisitor.AppendText("Posicion Memoria => " + context.ID().GetHashCode().ToString() + "\n");
            }
            else if (context.PCI() == null)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("Tipo => " + context.ID().GetText());
                //txtBoxTreeVisitor.AppendText("Posicion Memoria => " + context.ID().GetHashCode().ToString()+ "\n");
            }
            if (context.expr() != null)
            {
                Visit(context.expr());
            }
            cont--;
            return null;
        }

        public override object VisitStateReturnAST([NotNull] ParserProgra.StateReturnASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("StatementReturn\n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenReturn\n");
            if (context.exprL != null)
            {
                Visit(context.expr());
            }
            cont--;
            return null;
        }

        public override object VisitTokenConstConstDeclAST([NotNull] ParserProgra.TokenConstConstDeclASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("constDecl: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenConst\n");
            Visit(context.type());
            printTab(cont);
            txtBoxTreeVisitor.AppendText("ID => " + context.ID().GetText() + "\n");
            printTab(cont);
            txtBoxTreeVisitor.AppendText("Posicion Memoria => " + context.ID().GetHashCode().ToString() + "\n");
            printTab(cont);
            txtBoxTreeVisitor.AppendText("TokenEqual\n");
            if (context.Num() != null)
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("NUM => " + context.Num().GetText() + "\n");
            }
            else
            {
                printTab(cont);
                txtBoxTreeVisitor.AppendText("CHAR => " + context.CHAR().GetText() + "\n");
            }
            cont--;
            return null;
        }

        public override object VisitCharFactorAST([NotNull] ParserProgra.CharFactorASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("factor: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("CHAR => " + context.CHAR().GetText() + "\n");
            printTab(cont);
            //txtBoxTreeVisitor.AppendText("Tipo => " + "char\n");
            //txtBoxTreeVisitor.AppendText("Posicion Memoria => " + context.CHAR().GetHashCode().ToString() + "\n");
            cont--;
            return null;
        }

        public override object VisitNumFactoAST([NotNull] ParserProgra.NumFactoASTContext context)
        {
            printTab(cont);
            txtBoxTreeVisitor.AppendText("factor: \n");
            cont++;
            printTab(cont);
            txtBoxTreeVisitor.AppendText("NUM => " + context.Num().GetText() + "\n");
            printTab(cont);
            //txtBoxTreeVisitor.AppendText("Tipo => " + "int\n");
            //txtBoxTreeVisitor.AppendText("Puntero Memoria =>" + context.Num().GetHashCode().ToString() + "\n");
            cont--;
            return null;
        }
    }
}
