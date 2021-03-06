﻿using SGM.ApplicationServices.Application.Interface;
using SGM.Domain.DataSources;
using SGM.Domain.Entities;
using SGM.Domain.Enumeration;
using SGM.Domain.Utils;
using SGM.WindowsForms.Fomularios.Modelo;
using SGM.WindowsForms.IoC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SGM.WindowsForms
{
    public partial class FrmGerarOrcamento : FrmModeloDeFormularioDeCadastro
    {
        private readonly IClienteApplication _clienteApplication;
        private readonly IOrcamentoApplication _orcamentoApplication;
        private readonly IClienteVeiculoApplication _clienteVeiculoApplication;
        private readonly IVeiculoApplication _veiculoApplication;
        private readonly IMaodeObraApplication _maoDeObraApplication;
        private readonly IPecaApplication _pecaApplication;

        public FrmGerarOrcamento(IClienteApplication clienteApplication, IOrcamentoApplication orcamentoApplication, IClienteVeiculoApplication clienteVeiculoApplication, IVeiculoApplication veiculoApplication, IMaodeObraApplication maodeObraApplication, IPecaApplication pecaApplication)
        {
            _clienteApplication = clienteApplication;
            _orcamentoApplication = orcamentoApplication;
            _clienteVeiculoApplication = clienteVeiculoApplication;
            _veiculoApplication = veiculoApplication;
            _maoDeObraApplication = maodeObraApplication;
            _pecaApplication = pecaApplication;
            InitializeComponent();
        }

        public int codigo = 0;
        public int clienteId = 0;
        public int clienteVeiculoId = 0;
        public int veiculoId = 0;
        public string placaVeiculo = "";
        public string nomeCliente = "";

        public void LimpaTela()
        {
            txtClienteId.Clear();
            txtConsultaCliente.Clear();
            txtClienteSelecionado.Clear();
            txtDescricao.Clear();
            txtOrcamentoId.Clear();
            txtPercentualDesconto.Clear();
            txtValorAdicional.Clear();
            txtValorDesconto.Clear();
            txtValorTotal.Clear();
            txtValorTotalMaodeObra.Clear();
            txtValorTotalPecas.Clear();
            txtValorPecaManual.Clear();
            txtValorMaoDeObraManual.Clear();
            checkInclusaoManual.Checked = false;

            for (int i = 0; i < dgvCliente.RowCount; i++)
            {
                dgvCliente.Rows[i].DataGridView.Columns.Clear();
            }

            for (int i = 0; i < dgvMaodeObra.RowCount; i++)
            {
                dgvMaodeObra.Rows[i].DataGridView.Columns.Clear();
            }

            for (int i = 0; i < dgvPeca.RowCount; i++)
            {
                dgvPeca.Rows[i].DataGridView.Columns.Clear();
            }

            lblQtdRegistrosMaoDeObra.Text = "Quantidade de Registros: ";
            lblQtdRegistrosPecas.Text = "Quantidade de Registros: ";
        }

        private void FrmGerarOrcamento_Load(object sender, EventArgs e)
        {
            CalcularOrcamento();

            if (clienteId != 0 || clienteVeiculoId != 0)
            {
                var cliente = _clienteApplication.GetClienteById(clienteId);

                var dataSource = new List<PesquisaClienteOrcamentoDataSource>();

                foreach (var clienteVeiculo in cliente.ClienteVeiculo)
                {
                    var veiculo = _veiculoApplication.GetVeiculoByVeiculoId(clienteVeiculo.VeiculoId);

                    var marca = _veiculoApplication.GetMarcaByMarcaId(veiculo.MarcaId);

                    dataSource.Add(new PesquisaClienteOrcamentoDataSource
                    {
                        ClienteId = cliente.ClienteId,
                        NomeCliente = cliente.NomeCliente,
                        PlacaVeiculo = clienteVeiculo.PlacaVeiculo,
                        MarcaModeloVeiculo = marca.Marca + " / " + veiculo.Modelo,
                        ClienteVeiculoId = clienteVeiculo.ClienteVeiculoId
                    });
                }

                dgvCliente.DataSource = dataSource;
                dgvCliente.Columns[0].HeaderText = "Código";
                dgvCliente.Columns[0].Width = 50;
                dgvCliente.Columns[1].HeaderText = "Cliente";
                dgvCliente.Columns[1].Width = 296;
                dgvCliente.Columns[2].HeaderText = "Placa Veículo";
                dgvCliente.Columns[2].Width = 120;
                dgvCliente.Columns[3].HeaderText = "Marca/Modelo";
                dgvCliente.Columns[3].Width = 232;
                dgvCliente.Columns[4].HeaderText = "ClienteVeiculoId";
                dgvCliente.Columns[4].Width = 50;
                dgvCliente.Columns[4].Visible = false;

                txtClienteId.Text = cliente.ClienteId.ToString();
                txtClienteVeiculoId.Text = clienteVeiculoId.ToString();
                txtClienteSelecionado.Text = cliente.NomeCliente.ToString();
                txtValorTotalMaodeObra.Text = Convert.ToDecimal("0").ToString("C");
                txtValorTotalPecas.Text = Convert.ToDecimal("0").ToString("C");
                txtValorMaoDeObraManual.Text = Convert.ToDecimal("0").ToString("C");
                txtValorPecaManual.Text = Convert.ToDecimal("0").ToString("C");
                txtValorAdicional.Text = Convert.ToDecimal("0").ToString("C");
                txtPercentualDesconto.Text = Convert.ToDecimal("0").ToString("P");
                txtValorDesconto.Text = Convert.ToDecimal("0").ToString("C");
                txtValorTotal.Text = Convert.ToDecimal("0").ToString("C");
                txtDescricao.Text = "PESQUISANDO";

                Orcamento orcamento = new Orcamento
                {
                    ClienteVeiculoId = Convert.ToInt32(txtClienteVeiculoId.Text),
                    Status = (int)EnumStatusOrcamento.IniciadoPendente,
                    DataCadastro = DateTime.Now,
                    Descricao = txtDescricao.Text
                };

                var orcamentoId = _orcamentoApplication.SalvarOrcamento(orcamento);

                txtOrcamentoId.Text = orcamentoId.ToString();

                this.operacao = "inserir";
                this.DisponibilizarBotoesTela(EnumControleTelas.SalvarCancelarExcluir);

                txtConsultaCliente.Enabled = false;
                btnConsultaCliente.Enabled = false;
                dgvCliente.Enabled = false;
            }

            txtValorTotalMaodeObra.Text = Convert.ToDecimal("0").ToString("C");
            txtValorTotalPecas.Text = Convert.ToDecimal("0").ToString("C");
            txtValorAdicional.Text = Convert.ToDecimal("0").ToString("C");
            txtPercentualDesconto.Text = Convert.ToDecimal("0").ToString("P");
            txtValorDesconto.Text = Convert.ToDecimal("0").ToString("C");
            txtValorTotal.Text = Convert.ToDecimal("0").ToString("C");
            txtDescricao.Text = "PESQUISANDO";
        }

        private void BtnConsultaCliente_Click(object sender, EventArgs e)
        {
            var cliente = _clienteApplication.GetClienteByLikePlacaOrNomeOrApelido(txtConsultaCliente.Text);

            var dataSource = new List<PesquisaClienteOrcamentoDataSource>();

            foreach (var clienteVeiculo in cliente.ClienteVeiculo)
            {
                var veiculo = _veiculoApplication.GetVeiculoByVeiculoId(clienteVeiculo.VeiculoId);

                var marca = _veiculoApplication.GetMarcaByMarcaId(veiculo.MarcaId);

                dataSource.Add(new PesquisaClienteOrcamentoDataSource
                {
                    ClienteId = cliente.ClienteId,
                    NomeCliente = cliente.NomeCliente,
                    PlacaVeiculo = clienteVeiculo.PlacaVeiculo,
                    MarcaModeloVeiculo = marca.Marca + " / " + veiculo.Modelo,
                    ClienteVeiculoId = clienteVeiculo.ClienteVeiculoId
                });
            }

            dgvCliente.DataSource = dataSource;
            dgvCliente.Columns[0].HeaderText = "Código";
            dgvCliente.Columns[0].Width = 50;
            dgvCliente.Columns[1].HeaderText = "Cliente";
            dgvCliente.Columns[1].Width = 296;
            dgvCliente.Columns[2].HeaderText = "Placa Veículo";
            dgvCliente.Columns[2].Width = 120;
            dgvCliente.Columns[3].HeaderText = "Marca/Modelo";
            dgvCliente.Columns[3].Width = 232;
            dgvCliente.Columns[4].HeaderText = "ClienteVeiculoId";
            dgvCliente.Columns[4].Width = 50;
            dgvCliente.Columns[4].Visible = false;
        }

        private void BtnInserir_Click(object sender, EventArgs e)
        {
            this.operacao = "inserir";
            this.DisponibilizarBotoesTela(EnumControleTelas.SalvarCancelarExcluir);

            txtValorAdicional.Enabled = true;
            txtPercentualDesconto.Enabled = true;
            txtClienteId.Text = Convert.ToString(1);
            txtClienteVeiculoId.Text = Convert.ToString(1);
            txtClienteSelecionado.Text = Convert.ToString("SEM CLIENTE");
            txtValorTotalMaodeObra.Text = Convert.ToDecimal("0").ToString("C");
            txtValorTotalPecas.Text = Convert.ToDecimal("0").ToString("C");
            txtValorMaoDeObraManual.Text = Convert.ToDecimal("0").ToString("C");
            txtValorPecaManual.Text = Convert.ToDecimal("0").ToString("C");
            txtValorAdicional.Text = Convert.ToDecimal("0").ToString("C");
            txtPercentualDesconto.Text = Convert.ToDecimal("0").ToString("P");
            txtValorDesconto.Text = Convert.ToDecimal("0").ToString("C");
            txtValorTotal.Text = Convert.ToDecimal("0").ToString("C");
            txtDescricao.Text = "PESQUISANDO";

            Orcamento orcamento = new Orcamento
            {
                ClienteVeiculoId = Convert.ToInt32(txtClienteVeiculoId.Text),
                Descricao = txtDescricao.Text,
                Status = (int)EnumStatusOrcamento.IniciadoPendente,
                DataCadastro = DateTime.Now,
                DataAlteracao = null,
                Ativo = true,
                OrcamentoMaodeObra = new List<OrcamentoMaodeObra>(),
                OrcamentoPeca = new List<OrcamentoPeca>()
            };

            FrmLoading loading = new FrmLoading();
            loading.Show();
            var orcamentoId = _orcamentoApplication.SalvarOrcamento(orcamento);

            txtOrcamentoId.Text = orcamentoId.ToString();
        }

        private void BtnAdicionarMaodeObra_Click(object sender, EventArgs e)
        {
            bool apagaDadosTemporario = true;

            if (txtClienteId.Text == "")
            {
                MessageBox.Show("Você precisa primeiro incluir um cliente acima!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                FrmConsultaMaoDeObra consultaMaodeObra = FormResolve.Resolve<FrmConsultaMaoDeObra>();
                consultaMaodeObra.ShowDialog();

                if (consultaMaodeObra.codigo != 0)
                {
                    OrcamentoMaodeObra orcamentoMaodeObra = new OrcamentoMaodeObra()
                    {
                        OrcamentoId = Convert.ToInt32(txtOrcamentoId.Text),
                        MaodeObraId = consultaMaodeObra.codigo
                    };

                    var Id = _orcamentoApplication.SalvarOrcamentoMaodeObra(orcamentoMaodeObra);

                    var orcamentoMaodeObraSalvo = _orcamentoApplication.GetOrcamentoMaodeObraByOrcamentoId(Convert.ToInt32(txtOrcamentoId.Text));

                    IList<PesquisaMaodeObraOrcamentoDataSource> maoDeObra = new List<PesquisaMaodeObraOrcamentoDataSource>();

                    foreach (var item in orcamentoMaodeObraSalvo)
                    {
                        var mao = _maoDeObraApplication.GetMaodeObraById(item.MaodeObraId);

                        maoDeObra.Add(new PesquisaMaodeObraOrcamentoDataSource
                        {
                            MaodeObraId = mao.MaodeObraId,
                            MaodeObra = mao.Descricao,
                            Valor = mao.Valor,
                            OrcamentoMaodeObraId = Id
                        });
                    }

                    dgvMaodeObra.DataSource = maoDeObra;
                    dgvMaodeObra.Columns[0].HeaderText = "Código";
                    dgvMaodeObra.Columns[0].Width = 50;
                    dgvMaodeObra.Columns[1].HeaderText = "Mão de Obra";
                    dgvMaodeObra.Columns[1].Width = 330;
                    dgvMaodeObra.Columns[2].HeaderText = "Valor";
                    dgvMaodeObra.Columns[2].Width = 70;
                    dgvMaodeObra.Columns[2].DefaultCellStyle.Format = "C2";
                    dgvMaodeObra.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvMaodeObra.Columns[3].HeaderText = "OrcamentoMaoDeObraId";
                    dgvMaodeObra.Columns[3].Width = 20;
                    dgvMaodeObra.Columns[3].Visible = false;
                }

                CalcularOrcamento(apagaDadosTemporario);
            }
        }

        private void BtnAdicionarPeca_Click(object sender, EventArgs e)
        {
            bool apagaDadosTemporario = true;

            if (txtClienteId.Text == "")
            {
                MessageBox.Show("Você precisa primeiro incluir um cliente acima!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                FrmConsultaPeca consultaPeca = FormResolve.Resolve<FrmConsultaPeca>();
                consultaPeca.ShowDialog();

                if (consultaPeca.codigo != 0)
                {
                    OrcamentoPeca orcamentoPeca = new OrcamentoPeca()
                    {
                        OrcamentoId = Convert.ToInt32(txtOrcamentoId.Text),
                        PecaId = consultaPeca.codigo
                    };

                    var Id = _orcamentoApplication.SalvarOrcamentoPeca(orcamentoPeca);

                    var orcamentoPecaSalvo = _orcamentoApplication.GetOrcamentoPecaByOrcamentoId(Convert.ToInt32(txtOrcamentoId.Text));

                    IList<PesquisaPecaOrcamentoDataSource> peca = new List<PesquisaPecaOrcamentoDataSource>();

                    foreach (var item in orcamentoPecaSalvo)
                    {
                        var mao = _pecaApplication.GetPecaByPecaId(item.PecaId);
                        peca.Add(new PesquisaPecaOrcamentoDataSource
                        {
                            PecaId = mao.PecaId,
                            Peca = mao.Descricao,
                            Valor = mao.Valor,
                            OrcamentoPecaId = Id
                        });
                    }

                    dgvPeca.DataSource = peca;
                    dgvPeca.Columns[0].HeaderText = "Código";
                    dgvPeca.Columns[0].Width = 50;
                    dgvPeca.Columns[1].HeaderText = "Peça";
                    dgvPeca.Columns[1].Width = 330;
                    dgvPeca.Columns[2].HeaderText = "Valor Integral";
                    dgvPeca.Columns[2].Width = 70;
                    dgvPeca.Columns[2].DefaultCellStyle.Format = "C2";
                    dgvPeca.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPeca.Columns[3].HeaderText = "OrcamentoPecaId";
                    dgvPeca.Columns[3].Width = 20;
                    dgvPeca.Columns[3].Visible = false;
                }

                CalcularOrcamento(apagaDadosTemporario);
            }
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var orcamentoSalvo = _orcamentoApplication.GetOrcamentoByOrcamentoId(Convert.ToInt32(txtOrcamentoId.Text));

                Orcamento orcamento = new Orcamento
                {
                    OrcamentoId = Convert.ToInt32(txtOrcamentoId.Text),
                    ClienteVeiculoId = Convert.ToInt32(txtClienteVeiculoId.Text),
                    ColaboradorId = 0,
                    Descricao = txtDescricao.Text,
                    ValorMaodeObra = txtValorTotalMaodeObra.Text == "" ? 0 : Convert.ToDecimal(txtValorTotalMaodeObra.Text.Replace("R$ ", ""))
                                   + txtValorMaoDeObraManual.Text == "" ? 0 : Convert.ToDecimal(txtValorMaoDeObraManual.Text.Replace("R$ ", "")),
                    ValorPeca = txtValorTotalPecas.Text == "" ? 0 : Convert.ToDecimal(txtValorTotalPecas.Text.Replace("R$ ", ""))
                              + txtValorPecaManual.Text == "" ? 0 : Convert.ToDecimal(txtValorPecaManual.Text.Replace("R$ ", "")),
                    ValorAdicional = txtValorAdicional.Text == "R$ 0,00" ? 0 : Convert.ToDecimal(txtValorAdicional.Text.Replace("R$ ", "")),
                    PercentualDesconto = txtPercentualDesconto.Text == "0,00%" ? 0 : (Convert.ToDecimal(txtPercentualDesconto.Text.Replace("%", "")) / 100),
                    ValorDesconto = Convert.ToDecimal(txtValorDesconto.Text.Replace("R$ ", "")),
                    ValorTotal = Convert.ToDecimal(txtValorTotal.Text.Replace("R$ ", "")),
                    Status = (int)EnumStatusOrcamento.ConcluidoSemGerarServico,
                    Ativo = true,
                    DataCadastro = orcamentoSalvo.DataCadastro,
                    DataAlteracao = DateTime.Now,
                    OrcamentoMaodeObra = new List<OrcamentoMaodeObra>(),
                    OrcamentoPeca = new List<OrcamentoPeca>()
                };

                _orcamentoApplication.AtualizarOrcamento(orcamento);

                MessageBox.Show("Cadastro alterado com sucesso! Número do Orçamento: " + orcamento.OrcamentoId.ToString(), "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dgvCliente.DataSource = null;
                dgvMaodeObra.DataSource = null;
                dgvPeca.DataSource = null;

                this.LimpaTela();
                this.DisponibilizarBotoesTela(EnumControleTelas.InserirLocalizar);
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.operacao = "cancelar";
            this.DisponibilizarBotoesTela(EnumControleTelas.InserirLocalizar);
            this.LimpaTela();
        }

        private void BtnLocalizar_Click(object sender, EventArgs e)
        {
            FrmConsultaOrcamento consultaHistoricoOrcamento = FormResolve.Resolve<FrmConsultaOrcamento>();
            consultaHistoricoOrcamento.ShowDialog();

            DialogResult res = MessageBox.Show("Deseja efetuar alguma alteração no Orçamento?", "Orçamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res.ToString() == "Yes")
            {
                if (consultaHistoricoOrcamento.orcamentoId != 0)
                {
                    var orcamento = _orcamentoApplication.GetOrcamentoByOrcamentoId(consultaHistoricoOrcamento.orcamentoId);
                    var clienteVeiculo = _clienteVeiculoApplication.GetVeiculoClienteByClienteVeiculoId(orcamento.ClienteVeiculoId);
                    var cliente = _clienteApplication.GetClienteById(clienteVeiculo.ClienteId);

                    txtOrcamentoId.Text = orcamento.OrcamentoId.ToString();
                    txtClienteId.Text = orcamento.ClienteVeiculoId.ToString();
                    txtClienteVeiculoId.Text = orcamento.ClienteVeiculoId.ToString();
                    txtValorTotalMaodeObra.Text = orcamento.ValorMaodeObra.ToString("C");
                    txtValorTotalPecas.Text = orcamento.ValorPeca.ToString("C");
                    txtValorAdicional.Text = orcamento.ValorAdicional.ToString("C");
                    txtPercentualDesconto.Text = orcamento.PercentualDesconto.ToString("P");
                    txtValorDesconto.Text = orcamento.ValorDesconto.ToString("C");
                    txtValorTotal.Text = orcamento.ValorTotal.ToString("C");
                    txtClienteSelecionado.Text = cliente.NomeCliente;
                    txtDescricao.Text = orcamento.Descricao.ToString();

                    var orcamentoMaodeObraSalvo = _orcamentoApplication.GetOrcamentoMaodeObraByOrcamentoId(orcamento.OrcamentoId);

                    IList<PesquisaMaodeObraOrcamentoDataSource> maoDeObra = new List<PesquisaMaodeObraOrcamentoDataSource>();

                    foreach (var item in orcamentoMaodeObraSalvo)
                    {
                        var mao = _maoDeObraApplication.GetMaodeObraById(item.MaodeObraId);

                        maoDeObra.Add(new PesquisaMaodeObraOrcamentoDataSource
                        {
                            MaodeObraId = mao.MaodeObraId,
                            MaodeObra = mao.Descricao,
                            Valor = mao.Valor,
                            OrcamentoMaodeObraId = item.Id
                        });
                    }

                    dgvMaodeObra.DataSource = maoDeObra;
                    dgvMaodeObra.Columns[0].HeaderText = "Código";
                    dgvMaodeObra.Columns[0].Width = 50;
                    dgvMaodeObra.Columns[1].HeaderText = "Mão de Obra";
                    dgvMaodeObra.Columns[1].Width = 300;
                    dgvMaodeObra.Columns[2].HeaderText = "Valor";
                    dgvMaodeObra.Columns[2].Width = 70;
                    dgvMaodeObra.Columns[2].DefaultCellStyle.Format = "C2";
                    dgvMaodeObra.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvMaodeObra.Columns[3].HeaderText = "OrcamentoMaoDeObraId";
                    dgvMaodeObra.Columns[3].Width = 20;
                    dgvMaodeObra.Columns[3].Visible = false;

                    var orcamentoPecaSalvo = _orcamentoApplication.GetOrcamentoPecaByOrcamentoId(Convert.ToInt32(txtOrcamentoId.Text));

                    IList<PesquisaPecaOrcamentoDataSource> peca = new List<PesquisaPecaOrcamentoDataSource>();

                    foreach (var item in orcamentoPecaSalvo)
                    {
                        var mao = _pecaApplication.GetPecaByPecaId(item.PecaId);
                        peca.Add(new PesquisaPecaOrcamentoDataSource
                        {
                            PecaId = mao.PecaId,
                            Peca = mao.Descricao,
                            Valor = mao.Valor,
                            OrcamentoPecaId = item.Id
                        });
                    }

                    dgvPeca.DataSource = peca;
                    dgvPeca.Columns[0].HeaderText = "Código";
                    dgvPeca.Columns[0].Width = 50;
                    dgvPeca.Columns[1].HeaderText = "Peça";
                    dgvPeca.Columns[1].Width = 300;
                    dgvPeca.Columns[2].HeaderText = "Valor Integral";
                    dgvPeca.Columns[2].Width = 70;
                    dgvPeca.Columns[2].DefaultCellStyle.Format = "C2";
                    dgvPeca.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPeca.Columns[3].HeaderText = "OrcamentoPecaId";
                    dgvPeca.Columns[3].Width = 20;
                    dgvPeca.Columns[3].Visible = false;

                    DisponibilizarBotoesTela(EnumControleTelas.AlterarExcluirCancelar);
                }
                else
                {
                    this.LimpaTela();
                    this.DisponibilizarBotoesTela(EnumControleTelas.InserirLocalizar);
                }
            }
            else
            {
                this.LimpaTela();
                this.DisponibilizarBotoesTela(EnumControleTelas.InserirLocalizar);
            }
            consultaHistoricoOrcamento.Dispose();
        }

        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            this.operacao = "alterar";
            this.DisponibilizarBotoesTela(EnumControleTelas.SalvarCancelarExcluir);
        }

        private void DgvCliente_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.codigo = Convert.ToInt32(dgvCliente.Rows[e.RowIndex].Cells[0].Value);
                this.nomeCliente = Convert.ToString(dgvCliente.Rows[e.RowIndex].Cells[1].Value);
                txtClienteSelecionado.Text = Convert.ToString(nomeCliente);
                txtClienteId.Text = Convert.ToString(codigo);
                txtClienteVeiculoId.Text = Convert.ToString(dgvCliente.Rows[e.RowIndex].Cells[4].Value);
                dgvCliente.CurrentRow.Selected = false;
            }
        }

        private void DgvMaodeObra_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool apagaDadosTemporario = true;

            if (e.RowIndex >= 0)
            {
                int maoDeObraId = Convert.ToInt32(dgvMaodeObra.Rows[e.RowIndex].Cells[0].Value);
                int orcamentoMaodeObraId = Convert.ToInt32(dgvMaodeObra.Rows[e.RowIndex].Cells[3].Value);

                DialogResult res = MessageBox.Show("Deseja realmente EXCLUIR este item?", "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res.ToString() == "Yes")
                {
                    OrcamentoMaodeObra orcamentoMaodeObra = new OrcamentoMaodeObra()
                    {
                        Id = orcamentoMaodeObraId,
                        OrcamentoId = Convert.ToInt32(txtOrcamentoId.Text),
                        MaodeObraId = maoDeObraId
                    };

                    _orcamentoApplication.DeletarOrcamentoMaodeObra(orcamentoMaodeObra);

                    var orcamentoMaodeObraSalvo = _orcamentoApplication.GetOrcamentoMaodeObraByOrcamentoId(Convert.ToInt32(txtOrcamentoId.Text));

                    IList<PesquisaMaodeObraOrcamentoDataSource> maoDeObra = new List<PesquisaMaodeObraOrcamentoDataSource>();

                    foreach (var item in orcamentoMaodeObraSalvo)
                    {
                        var mao = _maoDeObraApplication.GetMaodeObraById(item.MaodeObraId);
                        maoDeObra.Add(new PesquisaMaodeObraOrcamentoDataSource
                        {
                            MaodeObraId = mao.MaodeObraId,
                            MaodeObra = mao.Descricao,
                            Valor = mao.Valor,
                            OrcamentoMaodeObraId = item.Id
                        });
                    }

                    dgvMaodeObra.DataSource = maoDeObra;
                    dgvMaodeObra.Columns[0].HeaderText = "Código";
                    dgvMaodeObra.Columns[0].Width = 50;
                    dgvMaodeObra.Columns[1].HeaderText = "Mão de Obra";
                    dgvMaodeObra.Columns[1].Width = 330;
                    dgvMaodeObra.Columns[2].HeaderText = "Valor";
                    dgvMaodeObra.Columns[2].Width = 70;
                    dgvMaodeObra.Columns[2].DefaultCellStyle.Format = "C2";
                    dgvMaodeObra.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvMaodeObra.Columns[3].HeaderText = "OrcamentoMaoDeObraId";
                    dgvMaodeObra.Columns[3].Width = 20;
                    dgvMaodeObra.Columns[3].Visible = false;
                }

                CalcularOrcamento(apagaDadosTemporario);
            }
        }

        private void DgvPeca_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool apagaDadosTemporario = true;

            if (e.RowIndex >= 0)
            {
                int pecaId = Convert.ToInt32(dgvPeca.Rows[e.RowIndex].Cells[0].Value);
                int orcamentoPecaId = Convert.ToInt32(dgvPeca.Rows[e.RowIndex].Cells[3].Value);

                DialogResult res = MessageBox.Show("Deseja realmente EXCLUIR este item?", "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res.ToString() == "Yes")
                {
                    OrcamentoPeca orcamentoPeca = new OrcamentoPeca()
                    {
                        Id = orcamentoPecaId,
                        OrcamentoId = Convert.ToInt32(txtOrcamentoId.Text),
                        PecaId = pecaId
                    };

                    _orcamentoApplication.DeletarOrcamentoPeca(orcamentoPeca);

                    var orcamentoPecaSalvo = _orcamentoApplication.GetOrcamentoPecaByOrcamentoId(Convert.ToInt32(txtOrcamentoId.Text));

                    IList<PesquisaPecaOrcamentoDataSource> peca = new List<PesquisaPecaOrcamentoDataSource>();

                    foreach (var item in orcamentoPecaSalvo)
                    {
                        var pec = _pecaApplication.GetPecaByPecaId(item.PecaId);
                        peca.Add(new PesquisaPecaOrcamentoDataSource
                        {
                            PecaId = pec.PecaId,
                            Peca = pec.Descricao,
                            Valor = pec.Valor,
                            OrcamentoPecaId = item.Id
                        });
                    }

                    dgvPeca.DataSource = peca;
                    dgvPeca.Columns[0].HeaderText = "Código";
                    dgvPeca.Columns[0].Width = 50;
                    dgvPeca.Columns[1].HeaderText = "Peça";
                    dgvPeca.Columns[1].Width = 330;
                    dgvPeca.Columns[2].HeaderText = "Valor Integral";
                    dgvPeca.Columns[2].Width = 70;
                    dgvPeca.Columns[2].DefaultCellStyle.Format = "C2";
                    dgvPeca.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPeca.Columns[3].HeaderText = "OrcamentoMaoDeObraId";
                    dgvPeca.Columns[3].Width = 20;
                    dgvPeca.Columns[3].Visible = false;
                }

                CalcularOrcamento(apagaDadosTemporario);
            }
        }

        private void DgvMaodeObra_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            foreach (DataGridViewColumn coluna in dgvMaodeObra.Columns)
                coluna.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void DgvPeca_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            foreach (DataGridViewColumn coluna in dgvPeca.Columns)
                coluna.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void TxtValorTotalMaodeObra_Leave(object sender, EventArgs e)
        {
            txtValorTotalMaodeObra.Text = Util.TranslateValorEmStringDinheiro(txtValorTotalMaodeObra.Text);

            CalcularOrcamento(resetDadosTemporario: true);
        }

        private void TxtValorTotalPecas_Leave(object sender, EventArgs e)
        {
            txtValorTotalPecas.Text = Util.TranslateValorEmStringDinheiro(txtValorTotalPecas.Text);

            CalcularOrcamento(resetDadosTemporario: true);
        }

        private void TxtValorAdicional_Leave(object sender, EventArgs e)
        {
            txtValorAdicional.Text = Util.TranslateValorEmStringDinheiro(txtValorAdicional.Text);

            CalcularOrcamento(resetDadosTemporario: true);
        }

        private void TxtPercentualDesconto_Leave(object sender, EventArgs e)
        {
            CalcularOrcamento(resetDadosTemporario: true);
        }

        private void TxtValorMaoDeObraManual_Leave(object sender, EventArgs e)
        {
            txtValorMaoDeObraManual.Text = Util.TranslateValorEmStringDinheiro(txtValorMaoDeObraManual.Text);

            CalcularOrcamento(resetDadosTemporario: true);
        }

        private void TxtValorPecaManual_Leave(object sender, EventArgs e)
        {
            txtValorPecaManual.Text = Util.TranslateValorEmStringDinheiro(txtValorPecaManual.Text);

            CalcularOrcamento(resetDadosTemporario: true);
        }

        private void TxtValorTotalMaodeObra_Enter(object sender, EventArgs e)
        {
            txtValorTotalMaodeObra.Text = "";
        }

        private void TxtValorTotalPecas_Enter(object sender, EventArgs e)
        {
            txtValorTotalPecas.Text = "";
        }

        private void TxtValorAdicional_Enter(object sender, EventArgs e)
        {
            txtValorAdicional.Text = "";
        }

        private void TxtValorMaoDeObraManual_Enter(object sender, EventArgs e)
        {
            txtValorMaoDeObraManual.Text = "";
        }

        private void TxtValorPecaManual_Enter(object sender, EventArgs e)
        {
            txtValorPecaManual.Text = "";
        }

        private void TxtPercentualDesconto_Enter(object sender, EventArgs e)
        {
            txtPercentualDesconto.Text = "";
        }

        private void CalcularOrcamento(bool resetDadosTemporario = true)
        {
            lblQtdRegistrosMaoDeObra.Text = "Quantidade de Registros: " + this.dgvMaodeObra.Rows.Count.ToString();
            lblQtdRegistrosPecas.Text = "Quantidade de Registros: " + this.dgvPeca.Rows.Count.ToString();

            decimal tempValorMaodeObra = Util.TranslateStringEmDecimal(txtValorTotalMaodeObra.Text);
            decimal tempValorPeca = Util.TranslateStringEmDecimal(txtValorTotalPecas.Text);

            var valorMaodeObra = Convert.ToDecimal(dgvMaodeObra.Rows.Cast<DataGridViewRow>().Sum(i => Convert.ToDecimal(i.Cells["Valor"].Value)));
            var valorPeca = Convert.ToDecimal(dgvPeca.Rows.Cast<DataGridViewRow>().Sum(i => Convert.ToDecimal(i.Cells["Valor"].Value)));
            var valorManualMaodeObra = Util.TranslateStringEmDecimal(txtValorMaoDeObraManual.Text);
            var valorManualPeca = Util.TranslateStringEmDecimal(txtValorPecaManual.Text);
            var valorAdicional = Util.TranslateStringEmDecimal(txtValorAdicional.Text);
            var percentualDesconto = Util.TranslateStringEmDecimal(txtPercentualDesconto.Text, true);

            if (!checkInclusaoManual.Checked)
            {
                valorManualMaodeObra = 0;
                valorManualPeca = 0;
            }

            CalcularDesconto(percentualDesconto, valorMaodeObra, valorPeca, valorAdicional, valorManualMaodeObra, valorManualPeca);

            var valorDesconto = Util.TranslateStringEmDecimal(txtValorDesconto.Text);

            var valorTotal = ((valorMaodeObra + valorManualMaodeObra + valorPeca + valorManualPeca + valorAdicional) - valorDesconto);

            var vmM = Util.TranslateStringEmDecimal(txtValorMaoDeObraManual.Text);
            var vpM = Util.TranslateStringEmDecimal(txtValorPecaManual.Text);

            txtValorTotalMaodeObra.Text = valorMaodeObra.ToString("C");
            txtValorTotalPecas.Text = valorPeca.ToString("C");
            txtValorMaoDeObraManual.Text = vmM.ToString("C");
            txtValorPecaManual.Text = vpM.ToString("C");
            txtValorAdicional.Text = valorAdicional.ToString("C");
            txtPercentualDesconto.Text = percentualDesconto.ToString("P");
            txtValorDesconto.Text = valorDesconto.ToString("C");
            txtValorTotal.Text = valorTotal.ToString("C");
        }

        private void CalcularDesconto(decimal pd = 0, decimal vm = 0, decimal vp = 0, decimal va = 0, decimal vmM = 0, decimal vpM = 0)
        {
            decimal percentualDesconto = pd == 0 ? Util.TranslateStringEmDecimal(txtPercentualDesconto.Text, ehPercentual: true) : pd;
            decimal valorMaodeObra = vm == 0 ? Util.TranslateStringEmDecimal(txtValorTotalMaodeObra.Text) : vm;
            decimal valorPeca = vp == 0 ? Util.TranslateStringEmDecimal(txtValorTotalPecas.Text) : vp;
            decimal valorMaodeObraManual = vmM != 0 ? Util.TranslateStringEmDecimal(txtValorMaoDeObraManual.Text) : vmM;
            decimal valorPecaManual = vpM != 0 ? Util.TranslateStringEmDecimal(txtValorPecaManual.Text) : vpM;
            decimal valorAdicional = va == 0 ? Util.TranslateStringEmDecimal(txtValorAdicional.Text) : va;
            decimal valorTotal = (valorMaodeObra + valorMaodeObraManual) + (valorPeca + valorPecaManual) + valorAdicional;

            txtValorDesconto.Text = Convert.ToString(Convert.ToDecimal((valorTotal * percentualDesconto)).ToString("C"));
        }

        private void CheckInclusaoManual_CheckedChanged(object sender, EventArgs e)
        {
            if (checkInclusaoManual.Checked)
            {
                lblValorMaoDeObraManual.Visible = true;
                lblValorPecaManual.Visible = true;
                txtValorMaoDeObraManual.Visible = true;
                txtValorPecaManual.Visible = true;

                CalcularOrcamento();
            }
            else
            {
                lblValorMaoDeObraManual.Visible = false;
                lblValorPecaManual.Visible = false;
                txtValorMaoDeObraManual.Visible = false;
                txtValorPecaManual.Visible = false;

                CalcularOrcamento();
            }
        }
    }
}