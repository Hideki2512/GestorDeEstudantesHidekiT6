﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestorDeEstudantesT6
{
    public partial class FormAtualizarApagarAlunos : Form
    {
        Estudante estudante = new Estudante();
        public FormAtualizarApagarAlunos()
        {
            InitializeComponent();
        }

        private void buttonEnviarFoto_Click(object sender, EventArgs e)
        {
            // exibe uma janela para procurar a imagem.
            OpenFileDialog selecionarImagem = new OpenFileDialog();

            selecionarImagem.Filter = "Selecione a foto (*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (selecionarImagem.ShowDialog() == DialogResult.OK)
            {
                pictureBoxFoto.Image = Image.FromFile(selecionarImagem.FileName);
            }
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            Estudante estudante = new Estudante();
          
            int id = Convert.ToInt32(textBoxId.Text);
            string nome = textBoxNome.Text;
            string sobrenome = textBoxSobrenome.Text;
            DateTime nascimento = dateTimePickerNascimento.Value;
            string telefone = textBoxTelefone.Text;
            string endereco = textBoxEndereco.Text;
            string genero = "Feminino";

            // Verifica se outro gênero está selecionado.
            if (radioButtonMasculino.Checked)
            {
                genero = "Masculino";
            }

            MemoryStream foto = new MemoryStream();
            int anoDeNascimento = dateTimePickerNascimento.Value.Year;
            int anoAtual = DateTime.Now.Year;

            if (((anoAtual - anoDeNascimento) < 10) ||
                ((anoAtual - anoDeNascimento) > 100))
            {
                MessageBox.Show("Precisa ter entre 10 e 100 anos.",
                    "Idade Inválida", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (Verificar())
            {
                pictureBoxFoto.Image.Save(foto,
                    pictureBoxFoto.Image.RawFormat);

                if (estudante.atualizarEstudante(id, nome, sobrenome, nascimento,
                    telefone, genero, endereco, foto))
                {
                    MessageBox.Show("Dados alterados!", "Sucesso!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Dados não alterados!", "Falha!",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Campos não preenchidos!", "Erro!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonApagar_Click(object sender, EventArgs e)
        {
            // Remove o estudante
            int id = Convert.ToInt32(textBoxId.Text);
            if (MessageBox.Show("Tem certea que deseja apagar esse aluno?",
                "Apagar Aluno", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (estudante.apagarEstudante(id))
                {
                    MessageBox.Show("Estudante removido!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    //Limpa as caixas de texto
                    textBoxId.Text = "";
                    textBoxNome.Text = "";
                }
                else
                {
                    MessageBox.Show("Estudante não removido!", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    textBoxId.Text = "";
                    textBoxNome.Text = "";
                }
            }
        }

        private void textBoxId_TextChanged(object sender, KeyPressEventArgs e)
        {
            
        }
        bool Verificar()
        {
            if ((textBoxNome.Text.Trim() == "") ||
                (textBoxSobrenome.Text.Trim() == "") ||
                (textBoxTelefone.Text.Trim() == "") ||
                (textBoxEndereco.Text.Trim() == "") ||
                (pictureBoxFoto.Image == null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                // Busca estudante pela ID
                // Já salva a ID convertida para INTEIRO
                int id = Convert.ToInt32(textBoxId.Text);
                MeuBancoDeDados meuBancoDeDados = new MeuBancoDeDados();

                MySqlCommand comando = new MySqlCommand("SELECT `id`, `nome`, `sobrenome`, `nascimento`, `genero`, `telefone`, `endereco`, `foto`, FROM `estudantes` WHERE `id`=" + id, meuBancoDeDados.getConexao);

                DataTable tabela = estudante.getEstudantes(comando);

                if (tabela.Rows.Count > 0)
                {
                    textBoxNome.Text = tabela.Rows[0]["nome"].ToString();
                    textBoxSobrenome.Text = tabela.Rows[0]["sobrenome"].ToString();
                    textBoxTelefone.Text = tabela.Rows[0]["telefone"].ToString();
                    textBoxEndereco.Text = tabela.Rows[0]["endereco"].ToString();
                    dateTimePickerNascimento.Value = (DateTime)tabela.Rows[0]["nascimento"];
                    if (tabela.Rows[0]["genero"].ToString() == "Feminino")
                    {
                        radioButtonFeminino.Checked = true;
                    }
                    else
                    {
                        radioButtonMasculino.Checked = true;
                    }
                }

                //A foto
                byte[] imagem = (byte[])tabela.Rows[0]["foto"];
                //"objeto" intermediário entre a foto que está na tabela
                //e a foto que está salva no banco de dados
                MemoryStream fotoDoAluno = new MemoryStream(imagem);
                //reconstrói a imagem com base em um "memory stream"
                pictureBoxFoto.Image = Image.FromStream(fotoDoAluno);
            }
            catch
            {
                MessageBox.Show("Digite uma ID válida!", "ID inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        private void FormAtualizarApagarAlunos_Load(object sender, EventArgs e)
        {

        }

        private void textBoxId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
