﻿using BBL;
using DAL;
using Modelo;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmCadastroCliente : GUI.frmModeloDeFormularioDeCadastro
    {
        public frmCadastroCliente()
        {
            InitializeComponent();
        }

        public void LimpaTela()
        {
            txtClienteId.Clear();
            txtCliente.Clear();
            txtApelido.Clear();
            txtCPF.Clear();
            //cboSexo.Clear();
            //cboEstadoCivil.Clear();
            //dtpDataNascimento.Clear();
            txtEmail.Clear();
            txtTelefoneFixo.Clear();
            txtCelular.Clear();
            txtTelefoneOutros.Clear();
            txtCEP.Clear();
            txtEndereco.Clear();
            txtNumero.Clear();
            txtComplemento.Clear();
            txtBairro.Clear();
            txtCidade.Clear();
            txtUF.Clear();
        }

        private void frmCadastroCliente_Load(object sender, EventArgs e)
        {

        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            this.operacao = "inserir";
            this.alteraBotoes(2);
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            this.operacao = "alterar";
            this.alteraBotoes(2);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult d = MessageBox.Show("Deseja realmente excluir o registro?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (d.ToString() == "Yes")
                {
                    // objeto para gravar os dados no banco de dados
                    DALConexao cx = new DALConexao(DadosDaConexao.StringDeConexao);
                    BLLCliente bll = new BLLCliente(cx);
                    bll.Excluir(Convert.ToInt32(txtClienteId.Text));
                    MessageBox.Show("Registro Excluído com Sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.LimpaTela();
                    this.alteraBotoes(1);
                }
            }
            catch
            {
                MessageBox.Show("Impossível excluir o registro. \n O registro está sendo utilizado em outro local.", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.alteraBotoes(3);
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {

                // leitura dos dados
                ModeloCliente modelo = new ModeloCliente();
                modelo.CCliente = txtCliente.Text;
                modelo.CApelido = txtApelido.Text;
                modelo.CDocumentoCliente = txtCPF.Text;
                modelo.CSexo = cboSexo.Text;
                modelo.CEstadoCivil = cboEstadoCivil.Text;
                modelo.CDataNascimento = dtpDataNascimento.Value;
                modelo.CEmail = txtEmail.Text;
                modelo.CTelefoneFixo = txtTelefoneFixo.Text;
                modelo.CTelefoneCelular = txtCelular.Text;
                modelo.CTelefoneOutros = txtTelefoneOutros.Text;
                modelo.CLogradouroCEP = txtCEP.Text;
                modelo.CLogradouroNome = txtEndereco.Text;
                modelo.CLogradouroNumero = txtNumero.Text;
                modelo.CLogradouroComplemento = txtComplemento.Text;
                modelo.CLogradouroMunicipio = txtCidade.Text;
                modelo.CLogradouroBairro = txtBairro.Text;
                modelo.CLogradouroUF = txtUF.Text;

                // objeto para gravar os dados no banco de dados
                DALConexao cx = new DALConexao(DadosDaConexao.StringDeConexao);
                BLLCliente bll = new BLLCliente(cx);

                if (this.operacao == "inserir")
                {
                    // Cadastrar uma categoria
                    bll.Incluir(modelo);
                    MessageBox.Show("Cadastro inserido com sucesso! Cliente: " + modelo.CCliente.ToString(), "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Alterar uma categoria
                    modelo.CClienteId = Convert.ToInt32(txtClienteId.Text);
                    bll.Alterar(modelo);
                    MessageBox.Show("Cadastro alterado com sucesso! Cliente: " + modelo.CCliente.ToString(), "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult res = MessageBox.Show("Deseja incluir o veículo dele agora? \n Clicando em (Sim), será aberto uma lista de clientes você escolhe o cliente que você acabou de cadastrar \n e clicando duas vezes você automáticamente poderá cadastrar o veículo dele.", "Cadastro de Veículo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res.ToString() == "Yes")
                {

                    frmConsultaCliente c = new frmConsultaCliente();
                    c.ShowDialog();

                    if (c.codigo != 0)
                    {
                        frmCadastroClienteVeiculo g = new frmCadastroClienteVeiculo();
                        g.alteraBotoes(3);
                        g.InformacaoCliente = c.CellCliente;
                        g.ShowDialog();
                        g.Dispose();
                    }
                }

                this.LimpaTela();
                this.alteraBotoes(1);
                this.Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.operacao = "cancelar";
            this.alteraBotoes(1);
            this.LimpaTela();
        }

        private void btnLocalizar_Click(object sender, EventArgs e)
        {

            frmConsultaCliente c = new frmConsultaCliente();
            c.ShowDialog();
            if (c.codigo != 0)
            {
                // objeto para gravar os dados no banco de dados
                DALConexao cx = new DALConexao(DadosDaConexao.StringDeConexao);
                BLLCliente bll = new BLLCliente(cx);
                ModeloCliente modelo = bll.CarregaModeloCliente(c.codigo);
                txtClienteId.Text = Convert.ToString(modelo.CClienteId);
                txtCliente.Text = modelo.CCliente;
                txtApelido.Text = modelo.CApelido;
                txtCPF.Text = modelo.CDocumentoCliente;
                cboSexo.Text = modelo.CSexo;
                cboEstadoCivil.Text = modelo.CEstadoCivil;
                dtpDataNascimento.Value = Convert.ToDateTime(modelo.CDataNascimento);
                txtEmail.Text = modelo.CEmail;
                txtTelefoneFixo.Text = modelo.CTelefoneFixo;
                txtCelular.Text = modelo.CTelefoneCelular;
                txtTelefoneOutros.Text = modelo.CTelefoneOutros;
                txtCEP.Text = modelo.CLogradouroCEP;
                txtEndereco.Text = modelo.CLogradouroNome;
                txtNumero.Text = modelo.CLogradouroNumero;
                txtComplemento.Text = modelo.CLogradouroComplemento;
                txtCidade.Text = modelo.CLogradouroMunicipio;
                txtBairro.Text = modelo.CLogradouroBairro;
                txtUF.Text = modelo.CLogradouroUF;
                alteraBotoes(3);
            }
            else
            {
                this.LimpaTela();
                this.alteraBotoes(1);
            }
            c.Dispose(); //destrói o formulário de consulta, para não ocupar memória.
        }

        /* ABAIXO O EVENTO QUE VERIFICA SE JÁ EXISTE O CLIENTE NA BASE DE DADOS.*/
        private void txtCPF_Leave(object sender, EventArgs e)
        {
            if (this.operacao == "inserir")
            {
                int retorno = 0;
                // objeto para gravar os dados no banco de dados
                DALConexao cx = new DALConexao(DadosDaConexao.StringDeConexao);
                BLLCliente bll = new BLLCliente(cx);
                retorno = bll.VerificaCPFCliente(txtCPF.Text);

                if (retorno > 0)
                {
                    DialogResult res = MessageBox.Show("Esse CPF já existe em nossa base de dados. Deseja alterar o registro?", "Aviso IMPORTANTE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (res.ToString() == "Yes")
                    {
                        this.operacao = "alterar";
                        ModeloCliente modelo = bll.CarregaModeloCliente(retorno);
                        txtClienteId.Text = Convert.ToString(modelo.CClienteId);
                        txtCliente.Text = modelo.CCliente;
                        txtApelido.Text = modelo.CApelido;
                        txtCPF.Text = modelo.CDocumentoCliente;
                        cboSexo.Text = modelo.CSexo;
                        cboEstadoCivil.Text = modelo.CEstadoCivil;
                        dtpDataNascimento.Value = Convert.ToDateTime(modelo.CDataNascimento);
                        txtEmail.Text = modelo.CEmail;
                        txtTelefoneFixo.Text = modelo.CTelefoneFixo;
                        txtCelular.Text = modelo.CTelefoneCelular;
                        txtTelefoneOutros.Text = modelo.CTelefoneOutros;
                        txtCEP.Text = modelo.CLogradouroCEP;
                        txtEndereco.Text = modelo.CLogradouroNome;
                        txtNumero.Text = modelo.CLogradouroNumero;
                        txtComplemento.Text = modelo.CLogradouroComplemento;
                        txtCidade.Text = modelo.CLogradouroMunicipio;
                        txtBairro.Text = modelo.CLogradouroBairro;
                        txtUF.Text = modelo.CLogradouroUF;
                        //alteraBotoes(3);
                    }

                }
            }
        }
    }
}