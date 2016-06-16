//Parser utilizado para el analisis sintactico del archivo a compilar
//Todas las reglas del lenguaje MiniC# son declaradas aqui, utilizando como palabras reservadas los tokens declarados en el lexer
parser grammar ParserProgra;
options
{
	tokenVocab = LexerProgra;
}
//Reglas Sintacticas
program : TokenClass ID (programAux)* LI (methodDecl)* LD # programTokenClassAST;
programAux: (constDecl | varDecl | classDecl) #programAuxLabelAST;
constDecl : TokenConst type ID EQUAL (Num|CHAR)PyC # tokenConstConstDeclAST;
varDecl : type ID(C ID)*PyC # typeVarDeclAST;
classDecl : TokenClass ID LI (varDecl)* LD # tokenClassClassDeclAST;
methodDecl : (typeL=type | tVoid=TokenVoid)ID PI (formParsL=formPars | ) PD (varDecl)* block # typeFormVarMethodDeclAST;
formPars : type ID (C type ID)* # typeIDFormParsAST;
type : ID ( (pci=PCI pcd=PCD) | ) # idTypeAST;
statement : designator(EQUAL exprL=expr | PI (actParsL=actPars | ) PD | incL=Inc | decL=Dec)PyC # designatorStatementAST
		  |	TokenIf PI condition PD statement (TokenElse statement | ) # statementIFAST
		  | TokenFor PI  expr PyC (conditionL=condition | ) PyC (statement | ) PD statement # statementForAST
		  | TokenWhile PI condition PD statement # stateWhileAST
		  | TokenBreak PyC # stateBreakAST
		  | TokenReturn (exprL=expr | ) PyC # stateReturnAST
		  | TokenRead PI designator PD PyC # stateReadAST
		  | TokenWrite PI expr(C Num | ) PD PyC # stateWriteAST
		  | block # stateBlockAST
		  | PyC # statePyCAST;
block : LI (statement)* LD # statementBlockAST;
actPars : expr(C expr)* # exprActParsAST;
condition : condTerm ( OR condTerm)* # condTermConditionAST;
condTerm : condFact(AND condFact)* # condFactCondTermAST;
condFact : expr relop expr # exprRelopExprCondFactAST;
expr locals [string tipo]: (SUB | ) term (addlop term)* # termAddlopExprAST;
term : factor (mulop  factor)* # factorMulopFactorTermAST;
factor : designator((PI(actPars | )PD) | ) # designatorFactorAST
	   | Num # numFactoAST
	   | CHAR # charFactorAST
	   | BOOL # boolFactorAST
	   | TokenNew ID( PCI expr PCD | ) # tokenNewFactorAST
	   | PI expr PD # exprFactorAST;
designator : ID (P ID | PCI expr PCD)* # designLabelAST;
relop : IfEqual # equalAST | IfDifferent # differentAST| IfMajor # majorAST| IfMajorEqual # majorEqualAST | IfMinor # minorAST| IfMinorEqual # minorEqualAST;
addlop : ADD # addAST| SUB # subAST;
mulop : MUL # mulAST| DIV # divAST| MOD # modAST; 





