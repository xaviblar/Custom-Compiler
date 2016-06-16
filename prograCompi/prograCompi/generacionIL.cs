using Antlr4.Runtime.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace prograCompi
{
    class generacionIL : ParserPrograBaseVisitor<object>
    {
        static ILGenerator globalIL;
        static List<FieldBuilder> globalsArray = new List<FieldBuilder>();
        static List<TypeBuilder> classArray = new List<TypeBuilder>();
        static List<List<string>> classFields = new List<List<string>>();
        static List<List<FieldBuilder>> classLocals = new List<List<FieldBuilder>>();
        static List<MethodBuilder> methodArray = new List<MethodBuilder>();
        static List<List<string>> methodFields = new List<List<string>>();
        static List<List<LocalBuilder>> methodLocals = new List<List<LocalBuilder>>();
        List<string> ListaLocales = new List<string>();
        List<ConstructorBuilder> classCtrs = new List<ConstructorBuilder>();
        List<LocalBuilder> ListaLocalesMetodo = new List<LocalBuilder>();
        List<FieldBuilder> ListaLocalesClase = new List<FieldBuilder>();
        List<string> listaParametrosTemp = new List<string>();
        List<List<string>> listaParametrosNombres = new List<List<string>>();
        List<List<string>> listaParametrosTipos = new List<List<string>>();
        static ModuleBuilder globalModule;
        static TypeBuilder globalMainType;
        static TypeBuilder globalActualType;
        static MethodBuilder globalActualMethod;
        static string nombrePadre="algunPadre";
        Type globalMyType;

        public MethodInfo writeLineInt = typeof(Console).GetMethod(
                            "WriteLine",
                             new Type[] { typeof(int) });

        public MethodInfo writeLineChr = typeof(Console).GetMethod(
                            "WriteLine",
                             new Type[] { typeof(char) });

        public int buscarIndMetodo(string nombre)
        {
            for (int i = 0; i < methodArray.Count; i++)
            {
                if (methodArray.ElementAt(i).Name.Equals(nombre))
                {
                    return i;
                }
            }
            return -1;
        }

        public int buscarIndClase(string nombre)
        {
            for (int i = 0; i < classArray.Count; i++)
            {
                if (classArray.ElementAt(i).Name.Equals(nombre))
                {
                    return i;
                }
            }
            return -1;
        }

        public int buscarIndLocal(string nombre, int indMetodo)
        {
            for (int i = 0; i < methodFields.ElementAt(indMetodo).Count; i++)
            {
                if (methodFields.ElementAt(indMetodo).ElementAt(i).Equals(nombre) == true)
                {
                    return i;
                }
            }
            return -1;
        }

        public int buscarIndParametro(string nombre, int indMetodo)
        {
            for (int i = 0; i < listaParametrosNombres.ElementAt(indMetodo).Count; i++)
            {
                if (listaParametrosNombres.ElementAt(indMetodo).ElementAt(i).Equals(nombre) == true)
                {
                    return i;
                }
            }
            return -1;
        }

        public int buscarIndLocalClase(string nombre, int indClase)
        {
            for (int i = 0; i <classLocals.ElementAt(indClase).Count; i++)
            {
                if (classLocals.ElementAt(indClase).ElementAt(i).Name.Equals(nombre) == true)
                {
                    return i;
                }
            }
            return -1;
        }

        public FieldBuilder buscarGlobal(string nombre)
        {
            for(int i=0;i<globalsArray.Count;i++)
            {
                if(globalsArray.ElementAt(i).Name.Equals(nombre))
                {
                    return globalsArray.ElementAt(i);
                }
            }
            return null;
        }

        public int buscarIndGlobal(string nombre)
        {
            for (int i = 0; i < globalsArray.Count; i++)
            {
                if (globalsArray.ElementAt(i).Name.Equals(nombre))
                {
                    return i;
                }
            }
            return -1;
        }
        public TypeBuilder buscarClase(string nombre)
        {
            for(int i=0; i<classArray.Count;i++)
            {
                if(classArray.ElementAt(i).Name.Equals(nombre))
                {
                    return classArray.ElementAt(i);
                }
            }
            return null;
        }

        public MethodBuilder buscarMetodo(string nombre)
        {
            for (int i = 0; i < methodArray.Count; i++)
            {
                if (methodArray.ElementAt(i).Name.Equals(nombre))
                {
                    return methodArray.ElementAt(i);
                }
            }
            return null;
        }

        public override object VisitProgramAuxLabelAST([NotNull] ParserProgra.ProgramAuxLabelASTContext context)
        {
            nombrePadre = "programAux";
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
            nombrePadre = "algunPadre";
            return null;
        }

        public override object VisitCondTermConditionAST([NotNull] ParserProgra.CondTermConditionASTContext context)
        {
            string var = (string)Visit(context.condTerm(0));
            LocalBuilder x = globalIL.DeclareLocal(typeof(int));
            globalIL.Emit(OpCodes.Stloc, x);
            LocalBuilder z = globalIL.DeclareLocal(typeof(int));
            //globalIL.Emit(OpCodes.Stloc, z);
            if (var.Equals("=="))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Ceq);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(1))
                {
                    //globalIL.Emit(OpCodes.Stloc, z);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                //return "==";
            }
            else if (var.Equals("!="))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Ceq);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(0))
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    //globalIL.Emit(OpCodes.Stloc, z);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                //return "!=";
            }
            else if (var.Equals("<"))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Clt);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(1))
                {
                    //globalIL.Emit(OpCodes.Stloc, z);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                //return "<";
            }
            else if (var.Equals("<="))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Cgt);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(0))
                {

                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                //return "<=";
            }
            else if (var.Equals(">"))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Cgt);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(1))
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                //return ">";
            }
            else if (var.Equals(">="))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Clt);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(0))
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                //return ">=";
            }
            for (int i = 0; i < context.OR().Count; i++)
            {
                string var2 = (string)Visit(context.condTerm(i));
                LocalBuilder y = globalIL.DeclareLocal(typeof(int));
                globalIL.Emit(OpCodes.Stloc, y);
                if (var.Equals("=="))
                {
                    //globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Ceq);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(1))
                    {
                        //globalIL.Emit(OpCodes.Stloc, z);
                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.Or);

                    //return "==";
                }
                else if (var.Equals("!="))
                {
                    globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Ceq);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(0))
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                        //globalIL.Emit(OpCodes.Stloc, z);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.Or);
                    //return "!=";
                }
                else if (var.Equals("<"))
                {
                    globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Clt);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(1))
                    {
                        //globalIL.Emit(OpCodes.Stloc, z);
                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.Or);
                    //return "<";
                }
                else if (var.Equals("<="))
                {
                    globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Cgt);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(0))
                    {

                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.Or);
                    //return "<=";
                }
                else if (var.Equals(">"))
                {
                    globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Cgt);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(1))
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.Or);
                    //return ">";
                }
                else if (var.Equals(">="))
                {
                    globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Clt);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(0))
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.Or);
                }

            }
            return null;
        }


        public override object VisitMinorEqualAST([NotNull] ParserProgra.MinorEqualASTContext context)
        {
            return null;
        }

        public override object VisitTypeIDFormParsAST([NotNull] ParserProgra.TypeIDFormParsASTContext context)
        {
            List<string> tipos = new List<string>();
            string tipo;
            for (int i = 0; i < context.type().Count; i++)
            {
                tipo = (string)Visit(context.type(i));
                tipos.Add(tipo);
                listaParametrosTemp.Add(context.ID(i).GetText());
            }
            return tipos;
        }

        public override object VisitDivAST([NotNull] ParserProgra.DivASTContext context)
        {
            return null;
        }

        public override object VisitBoolFactorAST([NotNull] ParserProgra.BoolFactorASTContext context)
        {
            if (context.BOOL().GetText().Equals("true"))
            {

                globalIL.Emit(OpCodes.Ldc_I4, 1);
                return true;
            }
            else
            {
                globalIL.Emit(OpCodes.Ldc_I4, 0);
                return false;
            }
        }

        public override object VisitExprFactorAST([NotNull] ParserProgra.ExprFactorASTContext context)
        {
            return Visit(context.expr());
        }

        public override object VisitStatePyCAST([NotNull] ParserProgra.StatePyCASTContext context)
        {
            return null;
        }

        public override object VisitExprRelopExprCondFactAST([NotNull] ParserProgra.ExprRelopExprCondFactASTContext context)
        {
            Visit(context.expr(0));
            //Visit(context.relop());
            Visit(context.expr(1));
            if (context.relop().GetText().Equals("=="))
            {
                globalIL.Emit(OpCodes.Ceq);
                return "==";
            }
            else if (context.relop().GetText().Equals("!="))
            {
                globalIL.Emit(OpCodes.Ceq);
                return "!=";
            }
            else if (context.relop().GetText().Equals("<"))
            {
                globalIL.Emit(OpCodes.Clt);
                return "<";
            }
            else if (context.relop().GetText().Equals("<="))
            {
                globalIL.Emit(OpCodes.Cgt);
                return "<=";
            }
            else if (context.relop().GetText().Equals(">"))
            {
                globalIL.Emit(OpCodes.Cgt);
                return ">";
            }
            else if (context.relop().GetText().Equals(">="))
            {
                globalIL.Emit(OpCodes.Clt);
                return ">=";
            }
            return null;
        }

        public override object VisitModAST([NotNull] ParserProgra.ModASTContext context)
        {
            return null;
        }

        public override object VisitDesignatorStatementAST([NotNull] ParserProgra.DesignatorStatementASTContext context)
        {
            nombrePadre = "statement";
            MethodInfo writeLineMI = typeof(Console).GetMethod(
                            "WriteLine",
                             new Type[] { typeof(int) });
            if(context.PI() != null)
            {
                MethodBuilder metodo=buscarMetodo(context.designator().GetChild(0).GetText());
                if(context.actPars() != null)
                {
                    Visit(context.actPars());
                }     
                globalIL.Emit(OpCodes.Call, metodo);
            }
            else if(context.EQUAL()!= null)
            {
                string idDesig = (string)Visit(context.designator());
                
                if(idDesig.Contains("[") == true)
                {
                    Visit(context.expr());
                    globalIL.Emit(OpCodes.Stelem_I4);
                }
                else
                {
                    if(idDesig.Contains(".") == false)
                    {
                        int ind = buscarIndMetodo(globalActualMethod.Name);
                        if (methodFields.ElementAt(ind).Contains(idDesig) == true)
                        {
                            int ind2 = buscarIndLocal(idDesig, ind);
                            Visit(context.expr());
                            globalIL.Emit(OpCodes.Stloc, methodLocals.ElementAt(ind).ElementAt(ind2));
                        }
                        else if (listaParametrosNombres.ElementAt(ind).Contains(idDesig) == true)
                        {
                            int ind2 = buscarIndParametro(idDesig, ind);
                            Visit(context.expr());
                            globalIL.Emit(OpCodes.Starg, ind2);
                        }
                        else
                        {
                            int vGlobal = buscarIndGlobal(idDesig);
                            Visit(context.expr());
                            globalIL.Emit(OpCodes.Stsfld, globalsArray.ElementAt(vGlobal));
                        }
                    }
                    else
                    {
                        int ind = buscarIndMetodo(globalActualMethod.Name);
                        string[] words = idDesig.Split('.');
                        if (methodFields.ElementAt(ind).Contains(words[0]) == true)
                        {
                            int ind2 = buscarIndLocal(words[0], ind);
                            string nombreClase = (methodLocals.ElementAt(ind).ElementAt(ind2).LocalType).Name;
                            int ind3 = buscarIndClase(nombreClase);
                            int ind4 = buscarIndLocalClase(words[1], ind3);
                            globalIL.Emit(OpCodes.Ldloc, methodLocals.ElementAt(ind).ElementAt(ind2));
                            Visit(context.expr());
                            globalIL.Emit(OpCodes.Stfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                        }
                        else if (listaParametrosNombres.ElementAt(ind).Contains(words[0]) == true)
                        {
                            int ind2 = buscarIndParametro(words[0], ind);
                            string nombreClase = (listaParametrosTipos.ElementAt(ind).ElementAt(ind2));
                            int ind3 = buscarIndClase(nombreClase);
                            int ind4 = buscarIndLocalClase(words[1], ind3);
                            globalIL.Emit(OpCodes.Ldarg, ind2);
                            Visit(context.expr());
                            globalIL.Emit(OpCodes.Stfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                        }
                        else
                        {
                            int ind2 = buscarIndGlobal(words[0]);
                            string nombreClase = globalsArray.ElementAt(ind2).FieldType.Name;
                            int ind3 = buscarIndClase(nombreClase);
                            int ind4 = buscarIndLocalClase(words[1], ind3);
                            globalIL.Emit(OpCodes.Ldsfld, globalsArray.ElementAt(ind2));
                            Visit(context.expr());
                            globalIL.Emit(OpCodes.Stfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                        }
                    }
                }
            }
            else if(context.Inc()!=null)
            {
                string idDesig = (string)Visit(context.designator());
                if(idDesig.Contains("[") == true)
                {

                }
                else
                {
                    if(idDesig.Contains(".")==false)
                    {
                        int ind = buscarIndMetodo(globalActualMethod.Name);
                        if(methodFields.ElementAt(ind).Contains(idDesig) == true)
                        {
                            int ind2=buscarIndLocal(idDesig,ind);
                            globalIL.Emit(OpCodes.Ldloc, methodLocals.ElementAt(ind).ElementAt(ind2));
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Add);
                            globalIL.Emit(OpCodes.Stloc, methodLocals.ElementAt(ind).ElementAt(ind2));
                        }
                        else if(listaParametrosNombres.ElementAt(ind).Contains(idDesig) == true)
                        {
                            int ind2 = buscarIndParametro(idDesig, ind);
                            globalIL.Emit(OpCodes.Ldarg, ind2);
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Add);
                            globalIL.Emit(OpCodes.Starg, ind2);
                        }

                        else
                        {
                            int vGlobal = buscarIndGlobal(idDesig);
                            globalIL.Emit(OpCodes.Ldsfld, globalsArray.ElementAt(vGlobal));
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Add);
                            globalIL.Emit(OpCodes.Stsfld, globalsArray.ElementAt(vGlobal));
                        }
                    }
                    else
                    {
                        int ind = buscarIndMetodo(globalActualMethod.Name);
                        string[] words = idDesig.Split('.');
                        if (methodFields.ElementAt(ind).Contains(words[0]) == true)
                        {
                            int ind2 = buscarIndLocal(words[0], ind);
                            string nombreClase = (methodLocals.ElementAt(ind).ElementAt(ind2).LocalType).Name;
                            int ind3 = buscarIndClase(nombreClase);
                            int ind4 = buscarIndLocalClase(words[1], ind3);
                            globalIL.Emit(OpCodes.Ldloc, methodLocals.ElementAt(ind).ElementAt(ind2));
                            globalIL.Emit(OpCodes.Dup);
                            globalIL.Emit(OpCodes.Ldfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Add);
                            globalIL.Emit(OpCodes.Stfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                        }
                        else if (listaParametrosNombres.ElementAt(ind).Contains(words[0]) == true)
                        {
                            int ind2 = buscarIndParametro(words[0], ind);
                            string nombreClase = (listaParametrosTipos.ElementAt(ind).ElementAt(ind2));
                            int ind3 = buscarIndClase(nombreClase);
                            int ind4 = buscarIndLocalClase(words[1], ind3);
                            globalIL.Emit(OpCodes.Ldarg, ind2);
                            globalIL.Emit(OpCodes.Dup);
                            globalIL.Emit(OpCodes.Ldfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Add);
                            globalIL.Emit(OpCodes.Stfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                        }
                        else
                        {
                            int ind2 = buscarIndGlobal(words[0]);
                            string nombreClase = globalsArray.ElementAt(ind2).FieldType.Name;
                            int ind3 = buscarIndClase(nombreClase);
                            int ind4 = buscarIndLocalClase(words[1], ind3);
                            globalIL.Emit(OpCodes.Ldsfld, globalsArray.ElementAt(ind2));
                            globalIL.Emit(OpCodes.Dup);
                            globalIL.Emit(OpCodes.Ldfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Add);
                            globalIL.Emit(OpCodes.Stfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                        }
                    }
                }
            }
            else if(context.Dec()!=null)
            {
                string idDesig = (string)Visit(context.designator());
                if (idDesig.Contains("[") == true)
                {

                }
                else
                {
                    if (idDesig.Contains(".") == false)
                    {
                        int ind = buscarIndMetodo(globalActualMethod.Name);
                        if (methodFields.ElementAt(ind).Contains(idDesig) == true)
                        {
                            int ind2 = buscarIndLocal(idDesig, ind);
                            globalIL.Emit(OpCodes.Ldloc, methodLocals.ElementAt(ind).ElementAt(ind2));
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Sub);
                            globalIL.Emit(OpCodes.Stloc, methodLocals.ElementAt(ind).ElementAt(ind2));
                        }
                        else if (listaParametrosNombres.ElementAt(ind).Contains(idDesig) == true)
                        {
                            int ind2 = buscarIndParametro(idDesig, ind);
                            globalIL.Emit(OpCodes.Ldarg, ind2);
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Sub);
                            globalIL.Emit(OpCodes.Starg, ind2);
                        }
                        else
                        {
                            int vGlobal = buscarIndGlobal(idDesig);
                            globalIL.Emit(OpCodes.Ldsfld, globalsArray.ElementAt(vGlobal));
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Sub);
                            globalIL.Emit(OpCodes.Stsfld, globalsArray.ElementAt(vGlobal));
                        }
                    }
                    else
                    {
                        int ind = buscarIndMetodo(globalActualMethod.Name);
                        string[] words = idDesig.Split('.');
                        if (methodFields.ElementAt(ind).Contains(words[0]) == true)
                        {
                            int ind2 = buscarIndLocal(words[0], ind);
                            string nombreClase = (methodLocals.ElementAt(ind).ElementAt(ind2).LocalType).Name;
                            int ind3 = buscarIndClase(nombreClase);
                            int ind4 = buscarIndLocalClase(words[1], ind3);
                            globalIL.Emit(OpCodes.Ldloc, methodLocals.ElementAt(ind).ElementAt(ind2));
                            globalIL.Emit(OpCodes.Dup);
                            globalIL.Emit(OpCodes.Ldfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Sub);
                            globalIL.Emit(OpCodes.Stfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                        }
                        else if (listaParametrosNombres.ElementAt(ind).Contains(words[0]) == true)
                        {
                            int ind2 = buscarIndParametro(words[0], ind);
                            string nombreClase = (listaParametrosTipos.ElementAt(ind).ElementAt(ind2));
                            int ind3 = buscarIndClase(nombreClase);
                            int ind4 = buscarIndLocalClase(words[1], ind3);
                            globalIL.Emit(OpCodes.Ldarg, ind2);
                            globalIL.Emit(OpCodes.Dup);
                            globalIL.Emit(OpCodes.Ldfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Sub);
                            globalIL.Emit(OpCodes.Stfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                        }
                        else
                        {
                            int ind2 = buscarIndGlobal(words[0]);
                            string nombreClase = globalsArray.ElementAt(ind2).FieldType.Name;
                            int ind3 = buscarIndClase(nombreClase);
                            int ind4 = buscarIndLocalClase(words[1], ind3);
                            globalIL.Emit(OpCodes.Ldsfld, globalsArray.ElementAt(ind2));
                            globalIL.Emit(OpCodes.Dup);
                            globalIL.Emit(OpCodes.Ldfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                            globalIL.Emit(OpCodes.Ldc_I4, 1);
                            globalIL.Emit(OpCodes.Sub);
                            globalIL.Emit(OpCodes.Stfld, classLocals.ElementAt(ind3).ElementAt(ind4));
                        }
                    }
                }
            }
            return null;
        }

        public override object VisitIdTypeAST([NotNull] ParserProgra.IdTypeASTContext context)
        {
            if (context.pci == null && context.pcd == null)
            {
                return context.ID().GetText();
            }

            if (context.pci != null && context.pcd != null)
            {
                return (context.ID().GetText() + "[]");
            }
            return null;
        }

        public override object VisitTokenClassClassDeclAST([NotNull] ParserProgra.TokenClassClassDeclASTContext context)
        {
            nombrePadre = "classDecl";
            TypeBuilder classBldr= globalMainType.DefineNestedType(context.ID().GetText());
            globalActualType = classBldr;
            if(context.varDecl()==null)
            {
                List<string> listaLocales = new List<string>();
                classFields.Add(listaLocales);
                List<FieldBuilder> listaLocales2 = new List<FieldBuilder>();
                classLocals.Add(listaLocales2);
            }
            else
            {
                for (int i = 0; i < context.varDecl().Count; i++)
                {
                    Visit(context.varDecl(i));
                }
                List<string> lista2 = new List<string>();
                lista2.AddRange(ListaLocales);
                classFields.Add(lista2);
                ListaLocales.Clear();

                List<FieldBuilder> lista3 = new List<FieldBuilder>();
                lista3.AddRange(ListaLocalesClase);
                classLocals.Add(lista3);
                ListaLocalesClase.Clear();
            }
            ConstructorBuilder ctr= classBldr.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[0]);
            ILGenerator anotherIL = ctr.GetILGenerator();
            anotherIL.Emit(OpCodes.Ret);
            classCtrs.Add(ctr);
            classBldr.CreateType();
            classArray.Add(classBldr);
            globalActualType = null;
            nombrePadre = "algunPadre";
            return null;
        }

        public override object VisitTypeVarDeclAST([NotNull] ParserProgra.TypeVarDeclASTContext context)
        {
            string tipo;
            tipo=(string)Visit(context.type());

            if (tipo.Equals("int") == false && tipo.Equals("char") == false && tipo.Equals("boolean") == false && tipo.Equals("char[]") == false && tipo.Equals("boolean[]") == false && tipo.Equals("int[]") == false)
            {
                TypeBuilder tipoClase = buscarClase(tipo);
                if(nombrePadre.Equals("programAux") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        globalsArray.Add(globalMainType.DefineField(context.ID(i).GetText(), tipoClase, FieldAttributes.Public |
                                                                                                        FieldAttributes.Static));
                    }
                }
                else
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesMetodo.Add(globalIL.DeclareLocal(tipoClase));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
            }
            else if(tipo.Equals("int") == true)
            {
                if (nombrePadre.Equals("programAux") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        globalsArray.Add(globalMainType.DefineField(context.ID(i).GetText(), typeof(int), FieldAttributes.Public |
                                                                                                          FieldAttributes.Static));
                    }
                }
                else if(nombrePadre.Equals("classDecl") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesClase.Add(globalActualType.DefineField(context.ID(i).GetText(), typeof(int), FieldAttributes.Public |
                                                                                           FieldAttributes.Static));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
                else
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesMetodo.Add(globalIL.DeclareLocal(typeof(int)));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
            }
            else if (tipo.Equals("int[]") == true)
            {
                if (nombrePadre.Equals("programAux") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        globalsArray.Add(globalMainType.DefineField(context.ID(i).GetText(), typeof(int[]), FieldAttributes.Public |
                                                                                                            FieldAttributes.Static));
                    }
                }
                else if (nombrePadre.Equals("classDecl") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesClase.Add(globalActualType.DefineField(context.ID(i).GetText(), typeof(int[]), FieldAttributes.Public |
                                                                                             FieldAttributes.Static));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
                else
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesMetodo.Add(globalIL.DeclareLocal(typeof(int[])));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
            }
            else if (tipo.Equals("char") == true)
            {
                if (nombrePadre.Equals("programAux") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        globalsArray.Add(globalMainType.DefineField(context.ID(i).GetText(), typeof(char), FieldAttributes.Public |
                                                                                                           FieldAttributes.Static));
                    }
                }
                else if (nombrePadre.Equals("classDecl") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesClase.Add(globalActualType.DefineField(context.ID(i).GetText(), typeof(char), FieldAttributes.Public |
                                                                                            FieldAttributes.Static));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
                else
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesMetodo.Add(globalIL.DeclareLocal(typeof(char)));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
            }
            else if (tipo.Equals("char[]") == true)
            {
                if (nombrePadre.Equals("programAux") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        globalsArray.Add(globalMainType.DefineField(context.ID(i).GetText(), typeof(char[]), FieldAttributes.Public |
                                                                                                           FieldAttributes.Static));
                    }
                }
                else if (nombrePadre.Equals("classDecl") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesClase.Add(globalActualType.DefineField(context.ID(i).GetText(), typeof(char[]), FieldAttributes.Public |
                                                                                              FieldAttributes.Static));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
                else
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesMetodo.Add(globalIL.DeclareLocal(typeof(char[])));
                        ListaLocales.Add(context.ID(i).GetText());
                        
                    }
                }
            }
            else if (tipo.Equals("boolean") == true)
            {
                if (nombrePadre.Equals("programAux") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        globalsArray.Add(globalMainType.DefineField(context.ID(i).GetText(), typeof(Boolean), FieldAttributes.Public |
                                                                                                              FieldAttributes.Static));
                    }
                }
                else if (nombrePadre.Equals("classDecl") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesClase.Add(globalActualType.DefineField(context.ID(i).GetText(), typeof(Boolean), FieldAttributes.Public |
                                                                                               FieldAttributes.Static));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
                else
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesMetodo.Add(globalIL.DeclareLocal(typeof(Boolean)));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
            }
            else if (tipo.Equals("boolean[]") == true)
            {
                if (nombrePadre.Equals("programAux") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        globalsArray.Add(globalMainType.DefineField(context.ID(i).GetText(), typeof(Boolean[]), FieldAttributes.Public |
                                                                                                              FieldAttributes.Static));
                    }
                }
                else if (nombrePadre.Equals("classDecl") == true)
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesClase.Add(globalActualType.DefineField(context.ID(i).GetText(), typeof(Boolean[]), FieldAttributes.Public |
                                                                                                 FieldAttributes.Static));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
                else
                {
                    for (int i = 0; i < context.ID().Count; i++)
                    {
                        ListaLocalesMetodo.Add(globalIL.DeclareLocal(typeof(Boolean[])));
                        ListaLocales.Add(context.ID(i).GetText());
                    }
                }
            }
            return null;
        }

        public override object VisitSubAST([NotNull] ParserProgra.SubASTContext context)
        {
            return null;
        }

        public override object VisitDesignLabelAST([NotNull] ParserProgra.DesignLabelASTContext context)
        {
            if (context.P(0) != null)
            {
                return context.ID(0).GetText() + "." + context.ID(1).GetText();
            }
            else if (context.PCD(0) != null)
            {
                int ind = buscarIndMetodo(globalActualMethod.Name);
                if (methodFields.ElementAt(ind).Contains(context.ID(0).GetText()) == true)
                {
                    int ind2 = buscarIndLocal(context.ID(0).GetText(), ind);
                    globalIL.Emit(OpCodes.Ldloc, methodLocals.ElementAt(ind).ElementAt(ind2));
                }
                else if (listaParametrosNombres.ElementAt(ind).Contains(context.ID(0).GetText()) == true)
                {
                    int ind2 = buscarIndParametro(context.ID(0).GetText(), ind);
                    globalIL.Emit(OpCodes.Ldarg, ind2);
                }
                else
                {
                    int vGlobal = buscarIndGlobal(context.ID(0).GetText());
                    globalIL.Emit(OpCodes.Ldsfld, globalsArray.ElementAt(vGlobal));
                }
                Visit(context.expr(0));
                if(nombrePadre=="factor")
                {
                    globalIL.Emit(OpCodes.Ldelem_I4);
                }
                return context.ID(0).GetText() + "[]";
            }
            else
            {
                return context.ID(0).GetText();
            }
        }

        public override object VisitExprActParsAST([NotNull] ParserProgra.ExprActParsASTContext context)
        {
            for (int i = 0; i < context.expr().Count; i++)
            {
                Visit(context.expr(i));
            }
            return null;
        }

        public override object VisitStateBreakAST([NotNull] ParserProgra.StateBreakASTContext context)
        {
            return null;
        }

        public override object VisitStateBlockAST([NotNull] ParserProgra.StateBlockASTContext context)
        {
            Visit(context.block());
            return null;
        }

        public override object VisitDesignatorFactorAST([NotNull] ParserProgra.DesignatorFactorASTContext context)
        {
            nombrePadre = "factor";
            string idDesig=(string)Visit(context.designator());
            if (context.PI() != null)
            {
                MethodBuilder metodo = buscarMetodo(idDesig);
                if(context.actPars() != null)
                {
                    Visit(context.actPars());
                }
                globalIL.Emit(OpCodes.Call, metodo);
                if(metodo.ReturnType == typeof(int))
                {
                    return 1;
                }
                else
                {
                    return "noEntero";
                }

            }
            else
            {
                if(idDesig.Contains("[") == true)
                {
                    int ind = buscarIndMetodo(globalActualMethod.Name);
                    string[] words = idDesig.Split('[');
                    if (methodFields.ElementAt(ind).Contains(words[0]) == true)
                    {
                        int ind2 = buscarIndLocal(words[0], ind);
                        if(methodLocals.ElementAt(ind).ElementAt(ind2).LocalType == typeof(int[]))
                        {
                            return 1;
                        }
                        else
                        {
                            return "noEntero";
                        }
                    }
                    else if (listaParametrosNombres.ElementAt(ind).Contains(words[0]) == true)
                    {
                        int ind2 = buscarIndParametro(words[0], ind);
                        if(listaParametrosTipos.ElementAt(ind).ElementAt(ind2)=="int[]")
                        {
                            return 1;
                        }
                        else
                        {
                            return "noEntero";
                        }
                    }
                    else
                    {
                        int ind2 = buscarIndGlobal(words[0]);
                        if(globalsArray.ElementAt(ind2).FieldType == typeof(int[]))
                        {
                            return 1;
                        }
                        else
                        {
                            return "noEntero";
                        }
                    }
                }
                else if(idDesig.Contains(".") == true)
                {
                    int ind = buscarIndMetodo(globalActualMethod.Name);
                    string[] words = idDesig.Split('.');
                    if (methodFields.ElementAt(ind).Contains(words[0]) == true)
                    {
                        int ind2 = buscarIndLocal(words[0], ind);
                        string nombreClase = (methodLocals.ElementAt(ind).ElementAt(ind2).LocalType).Name;
                        int ind3 = buscarIndClase(nombreClase);
                        int ind4 = buscarIndLocalClase(words[1], ind3);
                        globalIL.Emit(OpCodes.Ldloc, methodLocals.ElementAt(ind).ElementAt(ind2));
                        globalIL.Emit(OpCodes.Ldfld, classLocals.ElementAt(ind3).ElementAt(ind4));

                        if (classLocals.ElementAt(ind3).ElementAt(ind4).FieldType == typeof(int))
                        {
                            return 1;
                        }
                        else
                        {
                            return "noEntero";
                        }
                        
                    }
                    else if (listaParametrosNombres.ElementAt(ind).Contains(words[0]) == true)
                    {
                        int ind2 = buscarIndParametro(words[0], ind);
                        string nombreClase = (listaParametrosTipos.ElementAt(ind).ElementAt(ind2));
                        int ind3 = buscarIndClase(nombreClase);
                        int ind4 = buscarIndLocalClase(words[1], ind3);
                        globalIL.Emit(OpCodes.Ldarg, ind2);
                        globalIL.Emit(OpCodes.Ldfld, classLocals.ElementAt(ind3).ElementAt(ind4));

                        if (classLocals.ElementAt(ind3).ElementAt(ind4).FieldType == typeof(int))
                        {
                            return 1;
                        }
                        else
                        {
                            return "noEntero";
                        }
                    }
                    else
                    {
                        int ind2 = buscarIndGlobal(words[0]);
                        string nombreClase = globalsArray.ElementAt(ind2).FieldType.Name;
                        int ind3 = buscarIndClase(nombreClase);
                        int ind4 = buscarIndLocalClase(words[1], ind3);
                        globalIL.Emit(OpCodes.Ldsfld, globalsArray.ElementAt(ind2));
                        globalIL.Emit(OpCodes.Ldfld, classLocals.ElementAt(ind3).ElementAt(ind4));

                        if (classLocals.ElementAt(ind3).ElementAt(ind4).FieldType == typeof(int))
                        {
                            return 1;
                        }
                        else
                        {
                            return "noEntero";
                        }
                    }
                }
                else
                {
                    int ind = buscarIndMetodo(globalActualMethod.Name);
                    if (methodFields.ElementAt(ind).Contains(idDesig) == true)
                    {
                        int ind2 = buscarIndLocal(idDesig, ind);
                        globalIL.Emit(OpCodes.Ldloc, methodLocals.ElementAt(ind).ElementAt(ind2));
                        if(methodLocals.ElementAt(ind).ElementAt(ind2).LocalType == typeof(int))
                        {
                            return 1;
                        }
                        else
                        {
                            return "noEntero";
                        }
                        
                    }
                    else if (listaParametrosNombres.ElementAt(ind).Contains(idDesig) == true)
                    {
                        int ind2 = buscarIndParametro(idDesig, ind);
                        globalIL.Emit(OpCodes.Ldarg, ind2);
                        if (listaParametrosTipos.ElementAt(ind).ElementAt(ind2)== "int")
                        {
                            return 1;
                        }
                        else
                        {
                            return "noEntero";
                        }
                    }
                    else
                    {
                        int vGlobal = buscarIndGlobal(idDesig);
                        globalIL.Emit(OpCodes.Ldsfld, globalsArray.ElementAt(vGlobal));
                        if (globalsArray.ElementAt(vGlobal).FieldType == typeof(int))
                        {
                            return 1;
                        }
                        else
                        {
                            return "noEntero";
                        }
                    }
                }
            }
        }

        public override object VisitTermAddlopExprAST([NotNull] ParserProgra.TermAddlopExprASTContext context)
        {
            Object type = Visit(context.term(0));
            if (type.GetType() == typeof(int))
            {
                if (context.SUB() != null)
                {
                    globalIL.Emit(OpCodes.Neg);
                }

                for (int i = 0; i < context.addlop().Count; i++)
                {
                    Visit(context.term(i + 1));
                    if (context.addlop(i).GetText().Equals("+"))
                    {
                        globalIL.Emit(OpCodes.Add);
                    }
                    else if (context.addlop(i).GetText().Equals("-"))
                    {
                        globalIL.Emit(OpCodes.Sub);
                    }
                }
                int x = 1;
                return x;

            }
            if (type.GetType() == typeof(char))
            {
                return Convert.ToChar(type);
            }
            else if (type.GetType() == typeof(Boolean))
            {
                return Convert.ToBoolean(type);
            }
            else
            {
                return "noEntero";
            }
        }

        public override object VisitStatementBlockAST([NotNull] ParserProgra.StatementBlockASTContext context)
        {
            for (int i = 0; i < context.statement().Count;i++ )
            {
                Visit(context.statement(i));
            }
            if(globalActualMethod.ReturnType == typeof(void))
            {
                globalIL.Emit(OpCodes.Ret);
            }
            
            return null;
        }

        public override object VisitFactorMulopFactorTermAST([NotNull] ParserProgra.FactorMulopFactorTermASTContext context)
        {
            Object var = Visit(context.factor(0));
            if (var.GetType() == typeof(int))
            {
                for (int i = 0; i < context.mulop().Count; i++)
                {
                    int num2 = (int)Visit(context.factor(i + 1));
                    if (context.mulop(i).GetText().Equals("*"))
                    {
                        globalIL.Emit(OpCodes.Mul);
                    }
                    else if (context.mulop(i).GetText().Equals("/"))
                    {
                        globalIL.Emit(OpCodes.Div);
                    }
                    else if (context.mulop(i).GetText().Equals("%"))
                    {
                        globalIL.Emit(OpCodes.Rem);
                    }
                }
                int x = 1;
                return x;
            }
            else if (var.GetType() == typeof(char))
            {
                return Convert.ToChar(var);
            }
            else if (var.GetType() == typeof(Boolean))
            {
                return Convert.ToBoolean(var);
            }
            else
            {
                return "NoEntero";
            }
            
        }

        public override object VisitEqualAST([NotNull] ParserProgra.EqualASTContext context)
        {
            return null;
        }

        public override object VisitProgramTokenClassAST([NotNull] ParserProgra.ProgramTokenClassASTContext context)
        {

            AppDomain currentDom = Thread.GetDomain();
            StringBuilder asmFileNameBldr = new StringBuilder();
            asmFileNameBldr.Append("bldr.exe");
            string asmFileName = asmFileNameBldr.ToString();

            AssemblyName myAsmName = new AssemblyName();
            myAsmName.Name = "asmBldr";

            AssemblyBuilder myAsmBldr = currentDom.DefineDynamicAssembly(
                               myAsmName,
                               AssemblyBuilderAccess.RunAndSave);

            ModuleBuilder myModuleBldr = myAsmBldr.DefineDynamicModule(asmFileName);
            globalModule = myModuleBldr;

            TypeBuilder myTypeBldr = globalModule.DefineType(context.ID().GetText());
            globalMainType = myTypeBldr;
            globalActualType = myTypeBldr;
            for (int i = 0; i < context.programAux().Count;i++ )
            {
                Visit(context.programAux(i));
            }

            for (int i = 0; i < context.methodDecl().Count; i++)
            {
                Visit(context.methodDecl(i));
            }
            MethodBuilder metodoMain = buscarMetodo("Main");
            globalMyType=myTypeBldr.CreateType();
            myAsmBldr.SetEntryPoint(metodoMain);
            myAsmBldr.Save(asmFileName);
            return null;
        }

        public override object VisitCondFactCondTermAST([NotNull] ParserProgra.CondFactCondTermASTContext context)
        {
            string var = (string)Visit(context.condFact(0));
            LocalBuilder x = globalIL.DeclareLocal(typeof(int));
            globalIL.Emit(OpCodes.Stloc, x);
            LocalBuilder z = globalIL.DeclareLocal(typeof(int));
            //globalIL.Emit(OpCodes.Stloc, z);
            if (var.Equals("=="))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Ceq);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(1))
                {
                    //globalIL.Emit(OpCodes.Stloc, z);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                return "==";
            }
            else if (var.Equals("!="))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Ceq);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(0))
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    //globalIL.Emit(OpCodes.Stloc, z);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                return "!=";
            }
            else if (var.Equals("<"))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Clt);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(1))
                {
                    //globalIL.Emit(OpCodes.Stloc, z);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                return "<";
            }
            else if (var.Equals("<="))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Cgt);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(0))
                {

                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                return "<=";
            }
            else if (var.Equals(">"))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Cgt);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(1))
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                return ">";
            }
            else if (var.Equals(">="))
            {
                globalIL.Emit(OpCodes.Ldloc, x);
                globalIL.Emit(OpCodes.Ldc_I4, 1);
                globalIL.Emit(OpCodes.Clt);
                globalIL.Emit(OpCodes.Stloc, x);
                if (x.Equals(0))
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                }
                else
                {
                    globalIL.Emit(OpCodes.Ldc_I4, 0);
                }
                return ">=";
            }
            for (int i = 0; i < context.AND().Count; i++)
            {
                string var2 = (string)Visit(context.condFact(i));
                LocalBuilder y = globalIL.DeclareLocal(typeof(int));
                globalIL.Emit(OpCodes.Stloc, y);
                if (var.Equals("=="))
                {
                    globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Ceq);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(1))
                    {
                        //globalIL.Emit(OpCodes.Stloc, z);
                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.And);

                    return "==";
                }
                else if (var.Equals("!="))
                {
                    globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Ceq);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(0))
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                        //globalIL.Emit(OpCodes.Stloc, z);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.And);
                    return "!=";
                }
                else if (var.Equals("<"))
                {
                    globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Clt);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(1))
                    {
                        //globalIL.Emit(OpCodes.Stloc, z);
                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.And);
                    return "<";
                }
                else if (var.Equals("<="))
                {
                    globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Cgt);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(0))
                    {

                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.And);
                    return "<=";
                }
                else if (var.Equals(">"))
                {
                    globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Cgt);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(1))
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.And);
                    return ">";
                }
                else if (var.Equals(">="))
                {
                    globalIL.Emit(OpCodes.Ldloc, x);
                    globalIL.Emit(OpCodes.Ldc_I4, 1);
                    globalIL.Emit(OpCodes.Clt);
                    globalIL.Emit(OpCodes.Stloc, y);
                    if (y.Equals(0))
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 1);
                    }
                    else
                    {
                        globalIL.Emit(OpCodes.Ldc_I4, 0);
                    }
                    globalIL.Emit(OpCodes.And);
                    return ">=";
                }

            }
            return null;
        }

        public override object VisitStatementForAST([NotNull] ParserProgra.StatementForASTContext context)
        {
            Visit(context.expr());
            Label l1 = globalIL.DefineLabel();
            if (context.conditionL != null)
            {
                Visit(context.condition());
                if (context.statement(0) != null)
                {
                    Visit(context.statement(0));
                }
                globalIL.Emit(OpCodes.Brfalse, l1);
            }
            if (context.statement(1) != null)
            {
                Visit(context.statement(1));
            }
            globalIL.MarkLabel(l1);
            return null;
        }

        public override object VisitDifferentAST([NotNull] ParserProgra.DifferentASTContext context)
        {
            return null;
        }

        public override object VisitStateReadAST([NotNull] ParserProgra.StateReadASTContext context)
        {
            Visit(context.designator());
            return null;
        }

        public override object VisitStateWriteAST([NotNull] ParserProgra.StateWriteASTContext context)
        {
            Object tipo = Visit(context.expr());
            if (tipo.GetType() == typeof(int))
            {
                globalIL.EmitCall(OpCodes.Call, writeLineInt, null);
            }
            else
            {
                globalIL.EmitCall(OpCodes.Call, writeLineChr, null);
            }
            
            return null;
        }

        public override object VisitAddAST([NotNull] ParserProgra.AddASTContext context)
        {
            return null;
        }

        public override object VisitMajorAST([NotNull] ParserProgra.MajorASTContext context)
        {
            return null;
        }

        public override object VisitMulAST([NotNull] ParserProgra.MulASTContext context)
        {
            return null;
        }

        public override object VisitStateWhileAST([NotNull] ParserProgra.StateWhileASTContext context)
        {
            Visit(context.condition());
            Label l1 = globalIL.DefineLabel();
            globalIL.Emit(OpCodes.Brfalse, l1);
            Visit(context.statement());
            if (context.statement().GetText().Equals("break;"))
            {

            }
            globalIL.MarkLabel(l1);
            return null;
        }

        public override object VisitMinorAST([NotNull] ParserProgra.MinorASTContext context)
        {
            return null;
        }

        public override object VisitMajorEqualAST([NotNull] ParserProgra.MajorEqualASTContext context)
        {
            return null;
        }

        public override object VisitStatementIFAST([NotNull] ParserProgra.StatementIFASTContext context)
        {
            Visit(context.condition());
            Label l1 = globalIL.DefineLabel();
            globalIL.Emit(OpCodes.Brfalse, l1);
            Visit(context.statement(0));
            globalIL.MarkLabel(l1);
            if (context.TokenElse() != null)
            {
                Visit(context.statement(1));
            }
            return null;
        }

        public override object VisitTypeFormVarMethodDeclAST([NotNull] ParserProgra.TypeFormVarMethodDeclASTContext context)
        {
             
            if(context.ID().GetText().Equals("Main") == true || context.ID().GetText().Equals("main")==true)
            {
                MethodBuilder methodBldr = globalMainType.DefineMethod("Main",
                                MethodAttributes.Public |
                                MethodAttributes.Static,
                                typeof(void),
                                null); 
                globalIL = methodBldr.GetILGenerator();
                methodArray.Add(methodBldr);
                List<string> parametros = new List<string>();
                listaParametrosNombres.Add(parametros);
                List<string> parametrosTipos = new List<string>();
                listaParametrosTipos.Add(parametrosTipos);
                globalActualMethod = methodBldr;
            }
            else if (context.formPars() != null)
            {
                List<string> lista = new List<string>();
                lista = (List<string>)Visit(context.formPars());
                List<string> lista2 = new List<string>();
                lista2.AddRange(lista);
                if (lista.Count > 0)
                {
                    Type[] arrayType = new Type[lista.Count];
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista.ElementAt(i).Equals("int"))
                        {
                            arrayType[i] = typeof(int);
                        }
                        else if (lista.ElementAt(i).Equals("int[]"))
                        {
                            arrayType[i] = typeof(int[]);
                        }
                        else if (lista.ElementAt(i).Equals("char"))
                        {
                            arrayType[i] = typeof(char);
                        }
                        else if (lista.ElementAt(i).Equals("char[]"))
                        {
                            arrayType[i] = typeof(char[]);
                        }
                        else if (lista.ElementAt(i).Equals("boolean"))
                        {
                            arrayType[i] = typeof(Boolean);
                        }
                        else
                        {
                            TypeBuilder tipo = buscarClase(lista.ElementAt(i));
                            arrayType[i] = tipo;
                        }
                    }
                    
                    string tipo2 = "";
                    if (context.type() != null)
                    {
                        tipo2 = (string)Visit(context.type());
                        if (tipo2.Equals("int"))
                        {
                            MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                                MethodAttributes.Public |
                                MethodAttributes.Static,
                                typeof(int),
                                arrayType); // Parametros de la funcion
                            globalIL = methodBldr.GetILGenerator();
                            methodArray.Add(methodBldr);
                            List<string> parametros = new List<string>();
                            parametros.AddRange(listaParametrosTemp);
                            listaParametrosNombres.Add(parametros);
                            listaParametrosTemp.Clear();
                            listaParametrosTipos.Add(lista2);
                            globalActualMethod = methodBldr;
                        }
                        else if (tipo2.Equals("int[]"))
                        {
                            MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                                MethodAttributes.Public |
                                MethodAttributes.Static,
                                typeof(int[]),
                                arrayType); // Parametros de la funcion
                            globalIL = methodBldr.GetILGenerator();
                            methodArray.Add(methodBldr);
                            List<string> parametros = new List<string>();
                            parametros.AddRange(listaParametrosTemp);
                            listaParametrosNombres.Add(parametros);
                            listaParametrosTemp.Clear();
                            listaParametrosTipos.Add(lista2);
                            globalActualMethod = methodBldr;
                        }
                        else if (tipo2.Equals("char"))
                        {
                            MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                                MethodAttributes.Public |
                                MethodAttributes.Static,
                                typeof(char),
                                arrayType); // Parametros de la funcion
                            globalIL = methodBldr.GetILGenerator();
                            methodArray.Add(methodBldr);
                            List<string> parametros = new List<string>();
                            parametros.AddRange(listaParametrosTemp);
                            listaParametrosNombres.Add(parametros);
                            listaParametrosTemp.Clear();
                            listaParametrosTipos.Add(lista2);
                            globalActualMethod = methodBldr;
                        }
                        else if (tipo2.Equals("char[]"))
                        {
                            MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                                MethodAttributes.Public |
                                MethodAttributes.Static,
                                typeof(char[]),
                                arrayType); // Parametros de la funcion
                            globalIL = methodBldr.GetILGenerator();
                            methodArray.Add(methodBldr);
                            List<string> parametros = new List<string>();
                            parametros.AddRange(listaParametrosTemp);
                            listaParametrosNombres.Add(parametros);
                            listaParametrosTemp.Clear();
                            listaParametrosTipos.Add(lista2);
                            globalActualMethod = methodBldr;
                        }
                        else if (tipo2.Equals("boolean"))
                        {
                            MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                                MethodAttributes.Public |
                                MethodAttributes.Static,
                                typeof(Boolean),
                                arrayType); // Parametros de la funcion
                            globalIL = methodBldr.GetILGenerator();
                            methodArray.Add(methodBldr);
                            List<string> parametros = new List<string>();
                            parametros.AddRange(listaParametrosTemp);
                            listaParametrosNombres.Add(parametros);
                            listaParametrosTemp.Clear();
                            listaParametrosTipos.Add(lista2);
                            globalActualMethod = methodBldr;
                        }

                        else
                        {
                            TypeBuilder tipo = buscarClase(tipo2);
                            MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                                MethodAttributes.Public |
                                MethodAttributes.Static,
                                tipo,
                                arrayType); // Parametros de la funcion
                            globalIL = methodBldr.GetILGenerator();
                            methodArray.Add(methodBldr);
                            List<string> parametros = new List<string>();
                            parametros.AddRange(listaParametrosTemp);
                            listaParametrosNombres.Add(parametros);
                            listaParametrosTemp.Clear();
                            listaParametrosTipos.Add(lista2);
                            globalActualMethod = methodBldr;
                        }

                    }
                    else if (context.TokenVoid() != null)
                    {
                        MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                            MethodAttributes.Public |
                            MethodAttributes.Static,
                            typeof(void),
                            arrayType); // Parametros de la funcion
                        globalIL = methodBldr.GetILGenerator();
                        methodArray.Add(methodBldr);
                        List<string> parametros = new List<string>();
                        parametros.AddRange(listaParametrosTemp);
                        listaParametrosNombres.Add(parametros);
                        listaParametrosTemp.Clear();
                        listaParametrosTipos.Add(lista2);
                        globalActualMethod = methodBldr;
                    }
                }
            }
            else
            {
                List<string> lista2 = new List<string>();
                List<string> parametros = new List<string>();
                string tipo2 = "";
                if (context.type() != null)
                {
                    tipo2 = (string)Visit(context.type());
                    if (tipo2.Equals("int"))
                    {
                        MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                            MethodAttributes.Public |
                            MethodAttributes.Static,
                            typeof(int),
                            null); // Parametros de la funcion
                        globalIL = methodBldr.GetILGenerator();
                        methodArray.Add(methodBldr);
                        listaParametrosNombres.Add(parametros);
                        listaParametrosTipos.Add(lista2);
                        globalActualMethod = methodBldr;
                    }
                    else if (tipo2.Equals("int[]"))
                    {
                        MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                            MethodAttributes.Public |
                            MethodAttributes.Static,
                            typeof(int[]),
                            null); // Parametros de la funcion
                        globalIL = methodBldr.GetILGenerator();
                        methodArray.Add(methodBldr);
                        listaParametrosNombres.Add(parametros);
                        listaParametrosTipos.Add(lista2);
                        globalActualMethod = methodBldr;
                    }
                    else if (tipo2.Equals("char"))
                    {
                        MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                            MethodAttributes.Public |
                            MethodAttributes.Static,
                            typeof(char),
                            null); // Parametros de la funcion
                        globalIL = methodBldr.GetILGenerator();
                        methodArray.Add(methodBldr);
                        listaParametrosNombres.Add(parametros);
                        listaParametrosTipos.Add(lista2);
                        globalActualMethod = methodBldr;
                    }
                    else if (tipo2.Equals("char[]"))
                    {
                        MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                            MethodAttributes.Public |
                            MethodAttributes.Static,
                            typeof(char[]),
                            null); // Parametros de la funcion
                        globalIL = methodBldr.GetILGenerator();
                        methodArray.Add(methodBldr);
                        listaParametrosNombres.Add(parametros);
                        listaParametrosTipos.Add(lista2);
                        globalActualMethod = methodBldr;
                    }
                    else if (tipo2.Equals("boolean"))
                    {
                        MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                            MethodAttributes.Public |
                            MethodAttributes.Static,
                            typeof(Boolean),
                            null); // Parametros de la funcion
                        globalIL = methodBldr.GetILGenerator();
                        methodArray.Add(methodBldr);
                        listaParametrosNombres.Add(parametros);
                        listaParametrosTipos.Add(lista2);
                        globalActualMethod = methodBldr;
                    }
                    else
                    {
                        TypeBuilder tipo = buscarClase(tipo2);
                        MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                            MethodAttributes.Public |
                            MethodAttributes.Static,
                            tipo,
                            null); // Parametros de la funcion
                        globalIL = methodBldr.GetILGenerator();
                        methodArray.Add(methodBldr);
                        listaParametrosNombres.Add(parametros);
                        listaParametrosTipos.Add(lista2);
                        globalActualMethod = methodBldr;
                    }
                }
                else if (context.TokenVoid() != null)
                {
                    MethodBuilder methodBldr = globalMainType.DefineMethod(context.ID().GetText(),
                        MethodAttributes.Public |
                        MethodAttributes.Static,
                        typeof(void),
                        null); // Parametros de la funcion
                    globalIL = methodBldr.GetILGenerator();
                    methodArray.Add(methodBldr);
                    listaParametrosNombres.Add(parametros);
                    listaParametrosTipos.Add(lista2);
                    globalActualMethod = methodBldr;
                }
            }
            if(context.varDecl()==null)
            {
                List<string> listaLocales = new List<string>();
                methodFields.Add(listaLocales);
                List<LocalBuilder> listalocales2 = new List<LocalBuilder>();
                methodLocals.Add(listalocales2);
                
            }
            else
            {
                for (int i = 0; i < context.varDecl().Count; i++)
                {
                    Visit(context.varDecl(i));
                }
                List<string> lista2 = new List<string>();
                lista2.AddRange(ListaLocales);
                methodFields.Add(lista2);
                ListaLocales.Clear();

                List<LocalBuilder> lista3 = new List<LocalBuilder>();
                lista3.AddRange(ListaLocalesMetodo);
                methodLocals.Add(lista3);
                ListaLocalesMetodo.Clear();
            }
            
            Visit(context.block());
            return null;
        }
        

        public override object VisitTokenNewFactorAST([NotNull] ParserProgra.TokenNewFactorASTContext context)
        {
            if (context.ID().GetText().Equals("int"))
            {
                Visit(context.expr());
                globalIL.Emit(OpCodes.Newarr, typeof(int));
            }
            else if (context.ID().GetText().Equals("char"))
            {
                Visit(context.expr());
                globalIL.Emit(OpCodes.Newarr, typeof(char));
            }
            else
            {
                int ind = buscarIndClase(context.ID().GetText());
                globalIL.Emit(OpCodes.Newobj, classCtrs.ElementAt(ind));
            }
   
            return "noEntero";
        }

        public override object VisitStateReturnAST([NotNull] ParserProgra.StateReturnASTContext context)
        {
            if (context.exprL != null)
            {
                Visit(context.expr());
            }
            globalIL.Emit(OpCodes.Ret);
            return null;
        }

        public override object VisitTokenConstConstDeclAST([NotNull] ParserProgra.TokenConstConstDeclASTContext context)
        {
            if (context.Num() != null)
            {
               globalsArray.Add(globalMainType.DefineField(context.ID().GetText(), typeof(int),
                                  FieldAttributes.Private | FieldAttributes.Static));
            }
            else
            {
                globalsArray.Add(globalMainType.DefineField(context.ID().GetText(), typeof(char),
                                  FieldAttributes.Private | FieldAttributes.Static));
            }
            return null;
        }

        public override object VisitCharFactorAST([NotNull] ParserProgra.CharFactorASTContext context)
        {
            string var = (string)context.CHAR().GetText();
            string[] cadena = var.Split('\'');
            char var2 = Convert.ToChar(cadena[1]);
            globalIL.Emit(OpCodes.Ldc_I4, (int)var2);
            return var2;
        }

        public override object VisitNumFactoAST([NotNull] ParserProgra.NumFactoASTContext context)
        {
            int num = Convert.ToInt32(context.Num().GetText());
            globalIL.Emit(OpCodes.Ldc_I4, num);
            return num;
        }

        public void principal()
        {
            object ptInstance = Activator.CreateInstance(globalMyType, null);

            globalMyType.InvokeMember("Main",
                     BindingFlags.InvokeMethod,
                     null,
                     ptInstance,
                     new object[0]);
        }
    }
}


