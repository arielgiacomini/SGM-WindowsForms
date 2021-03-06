﻿using SGM.ApplicationServices.Application.Interface;
using SGM.WindowsForms.IoC;
using System.Windows.Forms;

namespace SGM.WindowsForms.Fomularios.Login
{
    public partial class FrmLogin : Form
    {
        private readonly IColaboradorApplication _colaboradorApplication;

        public FrmLogin(IColaboradorApplication colaboradorApplication)
        {
            _colaboradorApplication = colaboradorApplication;
            InitializeComponent();
        }

        private void BtnEntrar_Click(object sender, System.EventArgs e)
        {
            var usuario = txtUsuario.Text;
            var senha = txtSenha.Text;

            var usuarioAutenticado = _colaboradorApplication.AutenticacaoLogin(usuario, senha);

            if (usuarioAutenticado)
            {
                this.Hide();
                FrmPrincipal formPrincipal = FormResolve.Resolve<FrmPrincipal>();
                formPrincipal.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário ou senha inválido", "Erro ao Autenticar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}