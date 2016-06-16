namespace prograCompi
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.richTxtBoxEditor = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelColumna = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.labelNumColumna = new System.Windows.Forms.Label();
            this.labelNumFila = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoArchivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargarArchivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Savebtn = new System.Windows.Forms.Button();
            this.closeTabBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtBoxCompilacion = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTxtBoxEditor
            // 
            this.richTxtBoxEditor.Location = new System.Drawing.Point(6, 6);
            this.richTxtBoxEditor.Name = "richTxtBoxEditor";
            this.richTxtBoxEditor.Size = new System.Drawing.Size(738, 391);
            this.richTxtBoxEditor.TabIndex = 1;
            this.richTxtBoxEditor.Text = "";
            this.richTxtBoxEditor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.richTxtBoxEditor_MouseClick);
            this.richTxtBoxEditor.TextChanged += new System.EventHandler(this.richTxtBoxEditor_TextChanged);
            this.richTxtBoxEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTxtBoxEditor_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 480);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Linea:";
            // 
            // labelColumna
            // 
            this.labelColumna.AutoSize = true;
            this.labelColumna.Location = new System.Drawing.Point(77, 480);
            this.labelColumna.Name = "labelColumna";
            this.labelColumna.Size = new System.Drawing.Size(51, 13);
            this.labelColumna.TabIndex = 4;
            this.labelColumna.Text = "Columna:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 39);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(758, 438);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.richTxtBoxEditor);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(750, 410);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Archivo sin guardar";
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            this.tabPage1.Enter += new System.EventHandler(this.tabPage1_Enter);
            // 
            // labelNumColumna
            // 
            this.labelNumColumna.AutoSize = true;
            this.labelNumColumna.Location = new System.Drawing.Point(134, 480);
            this.labelNumColumna.Name = "labelNumColumna";
            this.labelNumColumna.Size = new System.Drawing.Size(10, 13);
            this.labelNumColumna.TabIndex = 6;
            this.labelNumColumna.Text = "-";
            // 
            // labelNumFila
            // 
            this.labelNumFila.AutoSize = true;
            this.labelNumFila.Location = new System.Drawing.Point(61, 480);
            this.labelNumFila.Name = "labelNumFila";
            this.labelNumFila.Size = new System.Drawing.Size(10, 13);
            this.labelNumFila.TabIndex = 5;
            this.labelNumFila.Text = "-";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(782, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoArchivoToolStripMenuItem,
            this.cargarArchivoToolStripMenuItem});
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.abrirToolStripMenuItem.Text = "Archivo";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // nuevoArchivoToolStripMenuItem
            // 
            this.nuevoArchivoToolStripMenuItem.Name = "nuevoArchivoToolStripMenuItem";
            this.nuevoArchivoToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.nuevoArchivoToolStripMenuItem.Text = "Nuevo Archivo";
            this.nuevoArchivoToolStripMenuItem.Click += new System.EventHandler(this.nuevoArchivoToolStripMenuItem_Click);
            // 
            // cargarArchivoToolStripMenuItem
            // 
            this.cargarArchivoToolStripMenuItem.Name = "cargarArchivoToolStripMenuItem";
            this.cargarArchivoToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.cargarArchivoToolStripMenuItem.Text = "Abrir Archivo";
            this.cargarArchivoToolStripMenuItem.Click += new System.EventHandler(this.cargarArchivoToolStripMenuItem_Click);
            // 
            // Savebtn
            // 
            this.Savebtn.BackgroundImage = global::prograCompi.Properties.Resources.save_24;
            this.Savebtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Savebtn.Location = new System.Drawing.Point(71, 0);
            this.Savebtn.Name = "Savebtn";
            this.Savebtn.Size = new System.Drawing.Size(27, 24);
            this.Savebtn.TabIndex = 9;
            this.toolTip1.SetToolTip(this.Savebtn, "Guardar cambios");
            this.Savebtn.UseVisualStyleBackColor = true;
            this.Savebtn.Click += new System.EventHandler(this.Savebtn_Click);
            // 
            // closeTabBtn
            // 
            this.closeTabBtn.BackgroundImage = global::prograCompi.Properties.Resources.icon_close_16px;
            this.closeTabBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.closeTabBtn.Location = new System.Drawing.Point(104, 0);
            this.closeTabBtn.Name = "closeTabBtn";
            this.closeTabBtn.Size = new System.Drawing.Size(21, 24);
            this.closeTabBtn.TabIndex = 8;
            this.toolTip1.SetToolTip(this.closeTabBtn, "Cerrar pestaña actual");
            this.closeTabBtn.UseVisualStyleBackColor = true;
            this.closeTabBtn.Click += new System.EventHandler(this.closeTabBtn_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::prograCompi.Properties.Resources.player_button_32x32_589c489d1e1e9c1239edc668968446ce;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(131, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 23);
            this.button1.TabIndex = 0;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.button1, "Compilar");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtBoxCompilacion
            // 
            this.txtBoxCompilacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCompilacion.Location = new System.Drawing.Point(22, 545);
            this.txtBoxCompilacion.Name = "txtBoxCompilacion";
            this.txtBoxCompilacion.ReadOnly = true;
            this.txtBoxCompilacion.Size = new System.Drawing.Size(738, 388);
            this.txtBoxCompilacion.TabIndex = 10;
            this.txtBoxCompilacion.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 513);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(229, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Resultados de compilacion:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 983);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBoxCompilacion);
            this.Controls.Add(this.Savebtn);
            this.Controls.Add(this.closeTabBtn);
            this.Controls.Add(this.labelNumColumna);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.labelNumFila);
            this.Controls.Add(this.labelColumna);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTxtBoxEditor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelColumna;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoArchivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cargarArchivoToolStripMenuItem;
        private System.Windows.Forms.Label labelNumColumna;
        private System.Windows.Forms.Label labelNumFila;
        private System.Windows.Forms.Button closeTabBtn;
        private System.Windows.Forms.Button Savebtn;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox txtBoxCompilacion;
        private System.Windows.Forms.Label label2;
    }
}

