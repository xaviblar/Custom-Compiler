using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace prograCompi
{
    //Clase de tipo windows form, es el principal editor de la aplicacion 
    public partial class Form1 : Form
    {
        //Variables donde se guardan el numero de linea y columna del editor en donde se encuentra el cursor
        int numLinea;
        int IndInicioLinea;
        int numColumna;
        //Boolean error;

        //Listas de strings que representan las rutas de los archivos abiertos en el editor para propositos de modificacion y guardado del archivo
        List<String> rutas=new List<string>();

        //Lista de booleanos que representa si cada uno de los archivos abiertos en el editor ha sido modoficado
        List<Boolean> modificado=new List<bool>();
        RichTextBox globalTextBox;
        

        public Form1()
        {
            InitializeComponent();
            rutas.Add(null);
            modificado.Add(false);
           // globalTextBox = richTxtBoxEditor;
        }

        //Evento que actualiza el numero de linea y columna en el que se ubica el cursor al hacer click sobre el textbox
        private void richTxtBoxEditor_MouseClick(object sender, MouseEventArgs e)
        {
            numLinea =richTxtBoxEditor.GetLineFromCharIndex(richTxtBoxEditor.SelectionStart) + 1;
            this.labelNumFila.Text = numLinea.ToString();
            IndInicioLinea = Win32.SendMessage(richTxtBoxEditor.Handle, Win32.EM_LINEINDEX, -1, 0);
            numColumna = richTxtBoxEditor.SelectionStart - IndInicioLinea + 1;
            this.labelNumColumna.Text = numColumna.ToString();
        }

        //Evento que actualiza el numero de linea y columna en el que se ubica el cursor al modificar el texto del textbox
        private void richTxtBoxEditor_TextChanged(object sender, EventArgs e)
        {
            numLinea = richTxtBoxEditor.GetLineFromCharIndex(richTxtBoxEditor.SelectionStart) + 1;
            this.labelNumFila.Text = numLinea.ToString();
            IndInicioLinea = Win32.SendMessage(richTxtBoxEditor.Handle, Win32.EM_LINEINDEX, -1, 0);
            numColumna = richTxtBoxEditor.SelectionStart - IndInicioLinea + 1;
            this.labelNumColumna.Text = numColumna.ToString();
            if (modificado[tabControl1.SelectedIndex] == false)
            {
                modificado[tabControl1.SelectedIndex] = true;
                tabControl1.SelectedTab.Text += " *";
            }
        }

        //Evento que actualiza el numero de linea y columna en el que se ubica el cursor al presionar una tecla estando colocado sobre el editor
        private void richTxtBoxEditor_KeyDown(object sender, KeyEventArgs e)
        {
            numLinea = richTxtBoxEditor.GetLineFromCharIndex(richTxtBoxEditor.SelectionStart) + 1;
            this.labelNumFila.Text = numLinea.ToString();
            IndInicioLinea = Win32.SendMessage(richTxtBoxEditor.Handle, Win32.EM_LINEINDEX, -1, 0);
            numColumna = richTxtBoxEditor.SelectionStart - IndInicioLinea + 1;
            this.labelNumColumna.Text = numColumna.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Metodo que abre un componente de tipo OpenFileDialog y permite seleccionar un archivo guardado y cargar su contenido en el textbox del editor
        private void cargarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrirArchivoDialog = new OpenFileDialog();
            abrirArchivoDialog.Title = "Abrir Archivo";
            abrirArchivoDialog.InitialDirectory = @"C:\Users\Xavier\Desktop";
            string rutaArchivo="";
            if (abrirArchivoDialog.ShowDialog() == DialogResult.OK)
            {
                rutaArchivo=abrirArchivoDialog.FileName.ToString();
                rutas.Add(rutaArchivo);
                modificado.Add(false);
                string nombre = Path.GetFileName(rutaArchivo);
                string contenido = File.ReadAllText(rutaArchivo);
                TabPage nuevoTab = new TabPage(nombre);
                tabControl1.TabPages.Add(nuevoTab);
                RichTextBox nuevoRichTxtBox = new RichTextBox();
                nuevoRichTxtBox.Name = nombre + "richTxtBox";
                nuevoTab.Controls.Add(nuevoRichTxtBox);
                nuevoRichTxtBox.Location = new System.Drawing.Point(6, 6);
                nuevoRichTxtBox.Size = new System.Drawing.Size(738, 391);
                nuevoRichTxtBox.Text = contenido;
                nuevoRichTxtBox.MouseClick += (sender2, args) =>
                {
                    numLinea = nuevoRichTxtBox.GetLineFromCharIndex(nuevoRichTxtBox.SelectionStart) + 1;
                    this.labelNumFila.Text = numLinea.ToString();
                    IndInicioLinea = Win32.SendMessage(nuevoRichTxtBox.Handle, Win32.EM_LINEINDEX, -1, 0);
                    numColumna = nuevoRichTxtBox.SelectionStart - IndInicioLinea + 1;
                    this.labelNumColumna.Text = numColumna.ToString();
                };
                nuevoRichTxtBox.TextChanged += (sender2, args) =>
                {
                    numLinea = nuevoRichTxtBox.GetLineFromCharIndex(nuevoRichTxtBox.SelectionStart) + 1;
                    this.labelNumFila.Text = numLinea.ToString();
                    IndInicioLinea = Win32.SendMessage(nuevoRichTxtBox.Handle, Win32.EM_LINEINDEX, -1, 0);
                    numColumna = nuevoRichTxtBox.SelectionStart - IndInicioLinea + 1;
                    this.labelNumColumna.Text = numColumna.ToString();
                    if (modificado[tabControl1.SelectedIndex] == false)
                    {
                        modificado[tabControl1.SelectedIndex] = true;
                        tabControl1.SelectedTab.Text += " *";
                    }
                };
                nuevoRichTxtBox.KeyDown += (sender2, args) =>
                {
                    numLinea = nuevoRichTxtBox.GetLineFromCharIndex(nuevoRichTxtBox.SelectionStart) + 1;
                    this.labelNumFila.Text = numLinea.ToString();
                    IndInicioLinea = Win32.SendMessage(nuevoRichTxtBox.Handle, Win32.EM_LINEINDEX, -1, 0);
                    numColumna = nuevoRichTxtBox.SelectionStart - IndInicioLinea + 1;
                    this.labelNumColumna.Text = numColumna.ToString();
                };
                nuevoTab.Click += (sender2, args) =>
                {
                    globalTextBox = nuevoRichTxtBox;
                };
                nuevoTab.Enter += (sender2, args) =>
                {
                    globalTextBox = nuevoRichTxtBox;
                };
            }
        }

        //Metodo que añade un tabpage al editor de manera que el contenido del textbox pueda ser editado y posteriormente ser guardado en un nuevo archivo
        private void nuevoArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rutas.Add(null);
            modificado.Add(false);
            string nombre = "Archivo sin guardar";
            TabPage nuevoTab = new TabPage(nombre);
            tabControl1.TabPages.Add(nuevoTab);
            RichTextBox nuevoRichTxtBox = new RichTextBox();
            nuevoRichTxtBox.Name = nombre + "richTxtBox";
            nuevoTab.Controls.Add(nuevoRichTxtBox);
            nuevoRichTxtBox.Location = new System.Drawing.Point(6, 6);
            nuevoRichTxtBox.Size = new System.Drawing.Size(738, 391);
            nuevoRichTxtBox.MouseClick += (sender2, args) =>
            {
                numLinea = nuevoRichTxtBox.GetLineFromCharIndex(nuevoRichTxtBox.SelectionStart) + 1;
                this.labelNumFila.Text = numLinea.ToString();
                IndInicioLinea = Win32.SendMessage(nuevoRichTxtBox.Handle, Win32.EM_LINEINDEX, -1, 0);
                numColumna = nuevoRichTxtBox.SelectionStart - IndInicioLinea + 1;
                this.labelNumColumna.Text = numColumna.ToString();
            };
            nuevoRichTxtBox.TextChanged += (sender2, args) =>
            {
                numLinea = nuevoRichTxtBox.GetLineFromCharIndex(nuevoRichTxtBox.SelectionStart) + 1;
                this.labelNumFila.Text = numLinea.ToString();
                IndInicioLinea = Win32.SendMessage(nuevoRichTxtBox.Handle, Win32.EM_LINEINDEX, -1, 0);
                numColumna = nuevoRichTxtBox.SelectionStart - IndInicioLinea + 1;
                this.labelNumColumna.Text = numColumna.ToString();
                if (modificado[tabControl1.SelectedIndex] == false)
                {
                    modificado[tabControl1.SelectedIndex] = true;
                    tabControl1.SelectedTab.Text += " *";
                }
            };
            nuevoRichTxtBox.KeyDown += (sender2, args) =>
            {
                numLinea = nuevoRichTxtBox.GetLineFromCharIndex(nuevoRichTxtBox.SelectionStart) + 1;
                this.labelNumFila.Text = numLinea.ToString();
                IndInicioLinea = Win32.SendMessage(nuevoRichTxtBox.Handle, Win32.EM_LINEINDEX, -1, 0);
                numColumna = nuevoRichTxtBox.SelectionStart - IndInicioLinea + 1;
                this.labelNumColumna.Text = numColumna.ToString();
            };
            nuevoTab.Click += (sender2, args) =>
            {
                globalTextBox = nuevoRichTxtBox;
            };
            nuevoTab.Enter += (sender2, args) =>
            {
                globalTextBox = nuevoRichTxtBox;
            };
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Evento que se activa al presionar el boton de guardar en el editor, guarda el contenido del textbox en un archivo ya sea nuevo o existente
        private void Savebtn_Click(object sender, EventArgs e)
        {
            string contenido = globalTextBox.Text;
            string nombreTab;
            if (rutas[tabControl1.SelectedIndex]==null)
            {
                SaveFileDialog guardarDialog = new SaveFileDialog();
                guardarDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                guardarDialog.FilterIndex = 1;
                guardarDialog.RestoreDirectory = true;

                if (guardarDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(guardarDialog.FileName.ToString(), contenido);
                    rutas[tabControl1.SelectedIndex] = guardarDialog.FileName.ToString();
                    tabControl1.SelectedTab.Text = Path.GetFileName(rutas[tabControl1.SelectedIndex].ToString());
                    modificado[tabControl1.SelectedIndex] = false;
                    nombreTab = tabControl1.SelectedTab.Text;
                    nombreTab = nombreTab.TrimEnd('*');
                    tabControl1.SelectedTab.Text = nombreTab;
                }
            }
            else
            {
                File.WriteAllText(rutas[tabControl1.SelectedIndex].ToString(), contenido);
                modificado[tabControl1.SelectedIndex] = false;
                nombreTab = tabControl1.SelectedTab.Text;
                nombreTab = nombreTab.TrimEnd('*');
                tabControl1.SelectedTab.Text = nombreTab;
            }
        }
        //Evento que enlaza la variable de globalTextBox con el textbox en el que se encuentre editando
        private void tabPage1_Click(object sender, EventArgs e)
        {
            globalTextBox = richTxtBoxEditor;
        }
        //Evento que enlaza la variable de globalTextBox con el textbox en el que se encuentre editando
        private void tabPage1_Enter(object sender, EventArgs e)
        {
            globalTextBox = richTxtBoxEditor;
        }
        
        //Evento que se ejecuta al presionar el boton de cerrar pestaña, comprueba si se han realizado cambios en el contenido, de ser asi, pide una confirmacion antes de cerrar la pestaña,
        //de lo contrario la cierra de inmediato
        private void closeTabBtn_Click(object sender, EventArgs e)
        {
            if(modificado[tabControl1.SelectedIndex]==true)
            {
               DialogResult resultado= MessageBox.Show("El archivo tiene cambios sin guardar, esta seguro que desea cerrar?", "Atencion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
               if (resultado == DialogResult.Yes)
               {
                   rutas.RemoveAt(tabControl1.SelectedIndex);
                   tabControl1.TabPages.Remove(tabControl1.SelectedTab);
               }
            }
            else
            {
                rutas.RemoveAt(tabControl1.SelectedIndex);
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                tabControl1.SelectedIndex = 0;
            }
        }

        //Evento que se ejecuta al presionar el boton de compilar en el editor, comprueba que el archivo a compilar este guardado, carga el lexer y el parser declarados anteriormente asi como
        //la clase errorHandler para mostrar posibles errores y ejecuta el visitor del arbol.
        private void button1_Click(object sender, EventArgs e)
        {
            txtBoxCompilacion.Text = "";
            treeVisitors.txtBoxTreeVisitor.Text = "";
            analisisContextual.txtBoxAnalisisContextual.Text = "";
            errorHandler.msjError = "";
            if(rutas[tabControl1.SelectedIndex]==null)
            {
                MessageBox.Show("Debe guardar los cambios antes de compilar el archivo", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                StreamReader inputStream = new StreamReader(rutas[tabControl1.SelectedIndex]);
                AntlrInputStream input = new AntlrInputStream(inputStream.ReadToEnd());
                inputStream.Close();
                LexerProgra lexer = new LexerProgra(input);
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                ParserProgra parser = new ParserProgra(tokens);
                parser.AddErrorListener(new errorHandler());
                IParseTree tree = parser.program();
                printTree classVisit = new printTree();
                analisisContextual contextVisit = new analisisContextual();
                Boolean error = false;
                try
                {
                    contextVisit.Visit(tree);
                }
                catch
                {
                    txtBoxCompilacion.Text=("\nAnalisis Sintactico:\n");
                    txtBoxCompilacion.AppendText(errorHandler.msjError);
                    if (txtBoxCompilacion.Text.Equals("\nAnalisis Sintactico:\n") == false)
                    {
                        error = true;
                    }
                    txtBoxCompilacion.AppendText("\nAnalisis Contextual:\n");
                    txtBoxCompilacion.AppendText(analisisContextual.txtBoxAnalisisContextual.Text);
                    if (txtBoxCompilacion.Text.Equals("\nAnalisis Sintactico:\n\nAnalisis Contextual:\n") == false)
                    {
                        error = true;
                    }
                }
                txtBoxCompilacion.Text = ("\nAnalisis Sintactico:\n");
                txtBoxCompilacion.AppendText(errorHandler.msjError);
                if (txtBoxCompilacion.Text.Equals("\nAnalisis Sintactico:\n") == false)
                {
                    error = true;
                }
                txtBoxCompilacion.AppendText("\nAnalisis Contextual:\n");
                txtBoxCompilacion.AppendText(analisisContextual.txtBoxAnalisisContextual.Text);
                if (txtBoxCompilacion.Text.Equals("\nAnalisis Sintactico:\n\nAnalisis Contextual:\n") == false)
                {
                    error = true;
                }

                try
                {
                    classVisit.Visit(tree);
                }
                catch
                {
                    txtBoxCompilacion.AppendText("\nArbol AST decorado:\n");
                    txtBoxCompilacion.AppendText(printTree.txtBoxTreeVisitor.Text);
                }
                txtBoxCompilacion.AppendText("\nArbol AST decorado:\n");
                txtBoxCompilacion.AppendText(printTree.txtBoxTreeVisitor.Text);
                if(error == false)
                {
                    generacionIL generarCodigo = new generacionIL();
                    generarCodigo.Visit(tree);
                    generarCodigo.principal();
                }
            }
        }
    }
}
