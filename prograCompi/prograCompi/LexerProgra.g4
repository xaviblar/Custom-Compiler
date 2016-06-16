//Lexer utilizado para el analisis sintactico del archivo a compilar
//Todas las posibles palabras reservadas del lenguaje MiniC# (Tokens) estan declaradas aqui
lexer grammar LexerProgra;
	//Fragmento de token que identifica el uso de cualquier letra ya sea mayuscula o minuscula
	fragment
	Letter: [A-Z] | [a-z];
	//Fragmento de token que identifica el uso de cualquier numero del 0 al 9
	fragment
	Digit: [0-9];
	//Fragmento de token que identifica el booleano true
	fragment
	TRUE : 'true';
	//Fragmento de token que identifica el booleano false
	fragment
	FALSE : 'false';
	//Tokens utilizados en el lenguaje MiniC#
	Num : [1-9][0-9]*|'0';
	TokenClass : 'class';
	TokenBreak : 'break';
	TokenConst : 'const';
	TokenElse : 'else';
	TokenIf : 'if';
	TokenNew : 'new';
	TokenRead : 'read';
	TokenReturn : 'return';
	TokenVoid : 'void';
	TokenWhile : 'while';
	TokenFor : 'for';
	TokenWrite : 'write';
	EQUAL : '=';
	MUL : '*';
	DIV : '/';
	ADD : '+';
	SUB : '-';
	MOD : '%';
	PI : '(';
	PD : ')'; 
	PCI : '[';
	PCD : ']';
	LI : '{';
	LD : '}'; 
	Comilla: '"';
	ID: Letter(Letter|Digit)*;
	PyC : ';';
	C : ',';
	P : '.';
	DP : ':';
	ComillaSimple : '\'';
	Dolar : '$';
	Sharp : '#';
	Underscore : '_';
	LComment : '//';
	OComment : '/*';
	CComment : '*/';
	AND : '&&';
	OR : '||';
	Inc : '++';
	Dec : '--';
	QSym : '?';
	ASym : '@';
	ESym : '!';
	IfEqual : '==';
	IfDifferent : '!=';
	IfMajorEqual : '>=';
	IfMinorEqual : '<=';
	IfMajor : '>';
	IfMinor : '<';
	BOOL : (TRUE | FALSE);
	FLOAT : ([0-9][0-9]*|'0')P([0-9][0-9]*);
	CHAR : ComillaSimple(Letter|Digit|'!'|'#'|'"'|'$'|'%'|'&'|'\''|PI|PD|'*'|'+'|'-'|'/'|':'|';'|'<'|'='|'>'|'?'|'@'|'\\n'|'\\r')ComillaSimple;
	COMMENT
	:   ( '//' ~[\r\n]* '\r'? '\n'
		| '/*' .*? '*/'
		) -> channel(HIDDEN)
	;

	WS
		: (' ' | '\r' | '\n' | '\t')->skip
		;	