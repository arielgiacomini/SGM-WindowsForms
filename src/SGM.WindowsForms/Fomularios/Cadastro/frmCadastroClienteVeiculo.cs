﻿using SGM.ApplicationServices.Application.Interface;
using SGM.Domain.Entities;
using SGM.Domain.Enumeration;
using SGM.Domain.Utils;
using SGM.WindowsForms.IoC;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SGM.WindowsForms
{
    public partial class FrmCadastroClienteVeiculo : FrmModeloDeFormularioDeCadastro
    {
        public int clienteId = 0;
        public string placaVeiculo = "";
        public int clienteVeiculoId = 0;
        private readonly IClienteApplication _clienteApplication;
        private readonly IVeiculoApplication _veiculoApplication;
        private readonly IClienteVeiculoApplication _clienteVeiculoApplication;

        public FrmCadastroClienteVeiculo(IClienteApplication clienteApplication, IVeiculoApplication veiculoApplication, IClienteVeiculoApplication clienteVeiculoApplication)
        {
            _clienteApplication = clienteApplication;
            _veiculoApplication = veiculoApplication;
            _clienteVeiculoApplication = clienteVeiculoApplication;
            InitializeComponent();
        }

        public void LimpaTela()
        {
            txtClienteVeiculoId.Clear();
            txtCorVeiculo.Clear();
            txtKmVeiculo.Clear();
            txtPlacaVeiculo.Clear();
            txtClienteId.Clear();
            txtCliente.Clear();
            txtTelefoneCliente.Clear();
            txtAnoModeloVeiculo.Clear();
            cboVeiculo.SelectedIndex = -1;
            cboMarcaVeiculo.SelectedIndex = -1;
            txtDataCadastro.Clear();
            txtDataAlteracao.Clear();
            checkBoxAtivo.Checked = false;
        }

        private void FrmCadastroClienteVeiculo_Load(object sender, EventArgs e)
        {
            LoadTela();

            if (clienteVeiculoId != 0)
            {
                var dadosVeiculoCliente = _clienteVeiculoApplication.GetVeiculoClienteByClienteVeiculoId(clienteVeiculoId);
                var dadosCliente = _clienteApplication.GetClienteById(dadosVeiculoCliente.ClienteId);
                var dadosVeiculo = _veiculoApplication.GetVeiculoByVeiculoId(dadosVeiculoCliente.VeiculoId);
                var dadosMarcaVeiculo = _veiculoApplication.GetMarcaByMarcaId(dadosVeiculo.MarcaId);

                PreencheInformacoesNaTela(dadosCliente, dadosVeiculoCliente, dadosVeiculo, dadosMarcaVeiculo);

                this.DisponibilizarBotoesTela(EnumControleTelas.AlterarExcluirCancelar);
                this.operacao = "alterar";
            }
            else if (clienteVeiculoId == 0 && clienteId != 0)
            {
                var dadosCliente = _clienteApplication.GetClienteById(clienteId);

                PreencheInformacoesNaTela(dadosCliente, new ClienteVeiculo(), new Veiculo(), new VeiculoMarca());

                this.DisponibilizarBotoesTela(EnumControleTelas.InserirLocalizar);
                this.operacao = "inserir";
            }
            else
            {
                this.DisponibilizarBotoesTela(EnumControleTelas.InserirLocalizar);
                this.operacao = "inserir";
            }
        }

        private void BtnInserir_Click(object sender, EventArgs e)
        {
            if (cboMarcaVeiculo.DataSource == null)
            {
                cboMarcaVeiculo.DataSource = _veiculoApplication.GetMarcasByAll();
                cboMarcaVeiculo.DisplayMember = "Marca";
                cboMarcaVeiculo.ValueMember = "MarcaId";
            }

            this.DisponibilizarBotoesTela(EnumControleTelas.SalvarCancelarExcluir);
            this.operacao = "inserir";
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DisponibilizarBotoesTela(EnumControleTelas.InserirLocalizar);
            this.LimpaTela();
        }

        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            this.DisponibilizarBotoesTela(EnumControleTelas.SalvarCancelarExcluir);
            this.operacao = "alterar";
            txtCliente.Enabled = false;
            txtClienteId.Enabled = false;
            txtTelefoneCliente.Enabled = false;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                int veiculoId = 0;

                if (txtClienteVeiculoId.Text != "")
                {
                    this.operacao = "alterar";
                }

                if (Convert.ToInt32(cboVeiculo.SelectedValue) == 0)
                {
                    Veiculo novoVeiculo = new Veiculo()
                    {
                        CodigoFipe = 0,
                        MarcaId = Convert.ToInt32(cboMarcaVeiculo.SelectedValue),
                        Modelo = cboVeiculo.Text,
                        VeiculoAtivo = true,
                        DataCadastro = DateTime.Now
                    };
                    try
                    {
                        veiculoId = _veiculoApplication.SalvarVeiculo(novoVeiculo);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("HOUVE ALGUM ERRO AO CADASTRAR O VEICULO: " + Convert.ToString(ex), "ERRO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                ClienteVeiculo clienteVeiculo = new ClienteVeiculo
                {
                    ClienteId = Convert.ToInt32(txtClienteId.Text),
                    VeiculoId = veiculoId == 0 ? Convert.ToInt32(cboVeiculo.SelectedValue) : veiculoId,
                    CorVeiculo = txtCorVeiculo.Text,
                    PlacaVeiculo = txtPlacaVeiculo.Text,
                    KmRodados = txtKmVeiculo.Text.Length == 0 ? 0 : Convert.ToInt32(txtKmVeiculo.Text),
                    AnoVeiculo = Convert.ToInt32(txtAnoModeloVeiculo.Text),
                    Ativo = checkBoxAtivo.Checked,
                    DataCadastro = DateTime.Now
                };

                if (this.operacao == "inserir")
                {
                    try
                    {
                        txtClienteVeiculoId.Text = Convert.ToString(_clienteVeiculoApplication.SalvarClienteVeiculo(clienteVeiculo));
                        MessageBox.Show("Cadastro inserido com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        AbrirPerguntaQualItemDesejaEfetuarParaCliente(Convert.ToInt32(txtClienteId.Text), Convert.ToInt32(txtClienteVeiculoId.Text), Convert.ToInt32(cboVeiculo.SelectedValue), Convert.ToString(txtPlacaVeiculo.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("HOUVE ALGUM ERRO AO CADASTRAR: " + Convert.ToString(ex), "ERRO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        clienteVeiculo.ClienteVeiculoId = Convert.ToInt32(txtClienteVeiculoId.Text);
                        clienteVeiculo.DataAlteracao = DateTime.Now;
                        _clienteVeiculoApplication.AtualizarClienteVeiculo(clienteVeiculo);
                        MessageBox.Show("Cadastro alterado com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        AbrirPerguntaQualItemDesejaEfetuarParaCliente(Convert.ToInt32(txtClienteId.Text), Convert.ToInt32(txtClienteVeiculoId.Text), Convert.ToInt32(cboVeiculo.SelectedValue), Convert.ToString(txtPlacaVeiculo.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("HOUVE ALGUM ERRO AO ALTERAR: " + Convert.ToString(ex), "ERRO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult d = MessageBox.Show("Deseja realmente excluir o registro?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (d.ToString() == "Yes")
                {
                    _clienteVeiculoApplication.InativarClienteVeiculo(Convert.ToInt32(txtClienteVeiculoId.Text));

                    this.LimpaTela();
                    this.DisponibilizarBotoesTela(EnumControleTelas.InserirLocalizar);

                    MessageBox.Show("Registro Excluído com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Impossível excluir o registro. \n O registro está sendo utilizado em outro local.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DisponibilizarBotoesTela(EnumControleTelas.AlterarExcluirCancelar);
            }
        }

        private void BtnLocalizar_Click(object sender, EventArgs e)
        {
            FrmConsultaClienteVeiculo formConsultaClienteVeiculo = FormResolve.Resolve<FrmConsultaClienteVeiculo>();
            formConsultaClienteVeiculo.ShowDialog();
            if (formConsultaClienteVeiculo.clienteVeiculoId != 0)
            {
                this.txtClienteId.Enabled = false;
                this.txtCliente.Enabled = false;
                this.txtTelefoneCliente.Enabled = false;

                if (cboMarcaVeiculo.DataSource == null)
                {
                    cboMarcaVeiculo.DataSource = _veiculoApplication.GetMarcasByAll();
                    cboMarcaVeiculo.DisplayMember = "Marca";
                    cboMarcaVeiculo.ValueMember = "MarcaId";
                }

                if (placaVeiculo != null && placaVeiculo != "")
                {
                    var dadosVeiculoCliente = _clienteVeiculoApplication.GetVeiculoClienteByPlaca(placaVeiculo);
                    var dadosCliente = _clienteApplication.GetClienteById(dadosVeiculoCliente.ClienteId);
                    var dadosVeiculo = _veiculoApplication.GetVeiculoByVeiculoId(dadosVeiculoCliente.VeiculoId);
                    var dadosMarcaVeiculo = _veiculoApplication.GetMarcaByMarcaId(dadosVeiculo.MarcaId);

                    PreencheInformacoesNaTela(dadosCliente, dadosVeiculoCliente, dadosVeiculo, dadosMarcaVeiculo);

                    this.DisponibilizarBotoesTela(EnumControleTelas.SalvarCancelarExcluir);
                    this.operacao = "alterar";
                }
            }
            else
            {
                this.LimpaTela();
                this.DisponibilizarBotoesTela(EnumControleTelas.InserirLocalizar);
            }

            formConsultaClienteVeiculo.Dispose();
        }

        private void BtnBuscarHistoricoCliente_Click(object sender, EventArgs e)
        {
            FrmConsultaServico consultaServico = FormResolve.Resolve<FrmConsultaServico>();
            consultaServico.servicoId = Convert.ToInt32(txtClienteId.Text);
            consultaServico.ShowDialog();
            consultaServico.Dispose();
            consultaServico.Close();
        }

        private void ComboBoxMarcaVeiculoAlteracaoValor(object sender, EventArgs e)
        {
            VeiculoMarca marcaVeiculo = (VeiculoMarca)cboMarcaVeiculo.SelectedItem;
            if (marcaVeiculo != null && marcaVeiculo.Marca != "Escolher Item")
            {
                this.txtAnoModeloVeiculo.Enabled = true;
            }
            else
            {
                txtAnoModeloVeiculo.Clear();
                cboVeiculo.SelectedIndex = -1;
            }
        }

        private void TextBoxAnoModeloSaidaCampo(object sender, EventArgs e)
        {
            var anoModeloFabricacao = txtAnoModeloVeiculo.Text;

            VeiculoMarca marcaVeiculo = (VeiculoMarca)cboMarcaVeiculo.SelectedItem;

            int marcaId = Convert.ToInt32(marcaVeiculo.MarcaId);

            if (!Util.VerificaSeEhNumero(anoModeloFabricacao))
            {
                MessageBox.Show("O Ano Modelo/Fabricação deve ser um número: ex.: 2020 ", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAnoModeloVeiculo.Clear();
            }

            else
            {
                cboVeiculo.DataSource = _veiculoApplication.GetVeiculosByMarcaId(marcaId);
                cboVeiculo.DisplayMember = "Modelo";
                cboVeiculo.ValueMember = "VeiculoId";
            }
        }

        public void PreencheInformacoesNaTela(Cliente cliente, ClienteVeiculo veiculoCliente, Veiculo veiculo, VeiculoMarca veiculoMarca)
        {
            if (cliente != null && cliente.ClienteId != 0)
            {
                txtClienteId.Text = cliente.ClienteId.ToString();
                txtCliente.Text = cliente.NomeCliente.ToString();
                txtTelefoneCliente.Text = cliente.TelefoneCelular.ToString();
            }

            if (veiculoCliente != null && veiculoCliente.ClienteVeiculoId != 0)
            {
                txtClienteVeiculoId.Text = veiculoCliente.ClienteVeiculoId.ToString();
                txtPlacaVeiculo.Text = veiculoCliente.PlacaVeiculo.ToString();
                txtKmVeiculo.Text = veiculoCliente.KmRodados.ToString();
                txtCorVeiculo.Text = veiculoCliente.CorVeiculo.ToString();
                txtAnoModeloVeiculo.Text = veiculoCliente.AnoVeiculo.ToString();
                checkBoxAtivo.Checked = veiculoCliente.Ativo;
                txtDataCadastro.Text = Util.ConvertHorarioOfServerToWorldReal(veiculoCliente.DataCadastro, 5).ToString();
                txtDataAlteracao.Text = veiculoCliente.DataAlteracao.HasValue ? Util.ConvertHorarioOfServerToWorldReal(veiculoCliente.DataAlteracao.Value, 5).ToString() : "";
            }

            if (veiculo != null && veiculo.VeiculoId != 0)
            {
                IList<Veiculo> veiculos = new List<Veiculo>();

                cboMarcaVeiculo.SelectedValue = veiculoMarca.MarcaId;

                if (cboMarcaVeiculo.SelectedIndex > 0)
                {
                    cboVeiculo.DataSource = _veiculoApplication.GetVeiculosByMarcaId(veiculoMarca.MarcaId);
                    cboVeiculo.DisplayMember = "Modelo";
                    cboVeiculo.ValueMember = "VeiculoId";
                    cboVeiculo.SelectedValue = veiculoCliente.VeiculoId;
                }

                veiculos.Add(veiculo);
                cboVeiculo.DataSource = veiculos;
                cboVeiculo.DisplayMember = "Modelo";
                cboVeiculo.ValueMember = "VeiculoId";
                cboVeiculo.SelectedValue = veiculo.VeiculoId;
            }

            if (veiculoMarca != null && veiculoMarca.MarcaId != 0)
            {
                cboMarcaVeiculo.DisplayMember = veiculoMarca.Marca.ToString();
            }
        }

        private void AbrirPerguntaQualItemDesejaEfetuarParaCliente(int clienteId, int clienteVeiculoId, int veiculoId, string placaVeiculo)
        {
            FrmGerarServicoOuOrcamento questionItemAbrir = new FrmGerarServicoOuOrcamento
            {
                clienteId = clienteId,
                veiculoId = veiculoId,
                placaVeiculo = placaVeiculo,
                clienteVeiculoId = clienteVeiculoId
            };

            questionItemAbrir.ShowDialog();
        }

        private void CboVeiculo_Leave(object sender, EventArgs e)
        {
            txtKmVeiculo.Focus();
        }

        private void LoadTela()
        {
            this.txtClienteId.Enabled = false;
            this.txtCliente.Enabled = false;
            this.txtTelefoneCliente.Enabled = false;
            this.txtAnoModeloVeiculo.Enabled = false;
            this.LimpaTela();
            this.DisponibilizarBotoesTela(EnumControleTelas.InserirLocalizar);

            if (cboMarcaVeiculo.DataSource == null)
            {
                cboMarcaVeiculo.DataSource = _veiculoApplication.GetMarcasByAll();
                cboMarcaVeiculo.DisplayMember = "Marca";
                cboMarcaVeiculo.ValueMember = "MarcaId";
            }
        }

        private void btnConsultaCliente_Click(object sender, EventArgs e)
        {
            frmConsultaCliente formConsultaCliente = FormResolve.Resolve<frmConsultaCliente>();
            formConsultaCliente.ShowDialog();
        }
    }
}