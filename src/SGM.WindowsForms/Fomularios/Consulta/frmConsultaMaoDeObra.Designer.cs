﻿namespace SGM.WindowsForms
{
    partial class FrmConsultaMaoDeObra
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConsultaMaoDeObra));
            this.dgvConsultaMaoDeObra = new System.Windows.Forms.DataGridView();
            this.btnConsultaMaoDeObra = new System.Windows.Forms.Button();
            this.txtConsultaMaoDeObra = new System.Windows.Forms.TextBox();
            this.lblConsultaMaoDeObra = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsultaMaoDeObra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvConsultaMaoDeObra
            // 
            this.dgvConsultaMaoDeObra.AllowUserToAddRows = false;
            this.dgvConsultaMaoDeObra.AllowUserToDeleteRows = false;
            this.dgvConsultaMaoDeObra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConsultaMaoDeObra.Location = new System.Drawing.Point(12, 71);
            this.dgvConsultaMaoDeObra.Name = "dgvConsultaMaoDeObra";
            this.dgvConsultaMaoDeObra.ReadOnly = true;
            this.dgvConsultaMaoDeObra.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConsultaMaoDeObra.Size = new System.Drawing.Size(760, 472);
            this.dgvConsultaMaoDeObra.TabIndex = 15;
            this.dgvConsultaMaoDeObra.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvConsultaMaoDeObra_CellDoubleClick);
            // 
            // btnConsultaMaoDeObra
            // 
            this.btnConsultaMaoDeObra.Image = global::SGM.WindowsForms.Properties.Resources.LocalicarRedimensionado3;
            this.btnConsultaMaoDeObra.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConsultaMaoDeObra.Location = new System.Drawing.Point(612, 30);
            this.btnConsultaMaoDeObra.Name = "btnConsultaMaoDeObra";
            this.btnConsultaMaoDeObra.Size = new System.Drawing.Size(160, 27);
            this.btnConsultaMaoDeObra.TabIndex = 14;
            this.btnConsultaMaoDeObra.Text = "Consultar";
            this.btnConsultaMaoDeObra.UseVisualStyleBackColor = true;
            this.btnConsultaMaoDeObra.Click += new System.EventHandler(this.BtnConsultaMaoDeObra_Click);
            // 
            // txtConsultaMaoDeObra
            // 
            this.txtConsultaMaoDeObra.Location = new System.Drawing.Point(12, 34);
            this.txtConsultaMaoDeObra.Name = "txtConsultaMaoDeObra";
            this.txtConsultaMaoDeObra.Size = new System.Drawing.Size(594, 20);
            this.txtConsultaMaoDeObra.TabIndex = 13;
            // 
            // lblConsultaMaoDeObra
            // 
            this.lblConsultaMaoDeObra.AutoSize = true;
            this.lblConsultaMaoDeObra.Location = new System.Drawing.Point(13, 18);
            this.lblConsultaMaoDeObra.Name = "lblConsultaMaoDeObra";
            this.lblConsultaMaoDeObra.Size = new System.Drawing.Size(196, 13);
            this.lblConsultaMaoDeObra.TabIndex = 12;
            this.lblConsultaMaoDeObra.Text = "Digite o nome da Mão-deObra/Serviço: ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SGM.WindowsForms.Properties.Resources.LogotipoLoquinho;
            this.pictureBox1.Location = new System.Drawing.Point(313, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(83, 30);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 35;
            this.pictureBox1.TabStop = false;
            // 
            // frmConsultaMaoDeObra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dgvConsultaMaoDeObra);
            this.Controls.Add(this.btnConsultaMaoDeObra);
            this.Controls.Add(this.txtConsultaMaoDeObra);
            this.Controls.Add(this.lblConsultaMaoDeObra);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConsultaMaoDeObra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de Gerenciamento Mecânica - Consulta de Mão-de-Obra (Serviços)";
            this.Load += new System.EventHandler(this.FrmConsultaMaoDeObra_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsultaMaoDeObra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvConsultaMaoDeObra;
        private System.Windows.Forms.Button btnConsultaMaoDeObra;
        private System.Windows.Forms.TextBox txtConsultaMaoDeObra;
        private System.Windows.Forms.Label lblConsultaMaoDeObra;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}