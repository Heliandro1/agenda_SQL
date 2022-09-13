using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.Globalization;
namespace AgendaSQL
{
    public partial class FrmAdicionarEditar : Form
    {
        int id_contacto;
        bool editar;
        public FrmAdicionarEditar(int id_contacto = -1)
        {
            InitializeComponent();
            this.id_contacto = id_contacto;
            editar = id_contacto == -1 ? false : true;
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            SqlCeConnection ligacao = new SqlCeConnection($"Data source = {vars.base_dados}");
            ligacao.Open();
            #region Verificações
            if (txt_nome.Text == string.Empty || txt_telefone.Text == string.Empty)
            {
                MessageBox.Show("Por favor Preenchar todos os campos!");
                return;
            }
            #endregion

            #region Novo Contacto
            if (!editar)
            {
                //buscar o id_contacto disponivel
                DataTable dados = new DataTable();
                SqlCeDataAdapter adaptador = new SqlCeDataAdapter("SELECT MAX(id_contacto) AS maxid FROM contactos", ligacao);
                adaptador.Fill(dados);
                //verfica se o valor é nulo
                if (DBNull.Value.Equals(dados.Rows[0][0]))
                    id_contacto = 0;
                else
                    id_contacto = Convert.ToInt16(dados.Rows[0][0]) + 1;
                //gravar o novo contacto na base de dados
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = ligacao;

                //parametros
                comando.Parameters.AddWithValue("@id_contacto", id_contacto);
                comando.Parameters.AddWithValue("@nome", txt_nome.Text);
                comando.Parameters.AddWithValue("@telefone", txt_telefone.Text);
                comando.Parameters.AddWithValue("@atualizacao", DateTime.Now);


                //verifica se já existe um contacto com o mesmo nome e telefone

                adaptador = new SqlCeDataAdapter();
                dados = new DataTable();
                comando.CommandText = "SELECT * FROM contactos WHERE nome = @nome AND telefone = @telefone";
                adaptador.SelectCommand = comando;
                adaptador.Fill(dados);
                if (dados.Rows.Count != 0)
                {
                    if (MessageBox.Show("Já existe um registo com o mesmo nome e telefone.\nDeseja gravar ainda assim?", 
                        "ATENÇÃO", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question) == DialogResult.No)
                        return;
                    
                }
                //texto da query
                comando.CommandText = "INSERT INTO contactos VALUES(" +
                    "@id_contacto, @nome, @telefone, @atualizacao)";
                comando.ExecuteNonQuery();
                comando.Dispose();
                ligacao.Dispose();
                MessageBox.Show("Contacto adicionado com sucesso");
                txt_telefone.Text = txt_nome.Text = string.Empty;
                txt_nome.Focus();
            }
            #endregion

            #region Editar Contacto
            else
            {
                //edita o contacto na base de dados
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = ligacao;

                //parametros
                comando.Parameters.AddWithValue("@id_contacto", id_contacto);
                comando.Parameters.AddWithValue("@nome", txt_nome.Text);
                comando.Parameters.AddWithValue("@telefone", txt_telefone.Text);
                comando.Parameters.AddWithValue("@atualizacao", DateTime.Now);

                //verifica se existe registo com mesmo nome com id diferente
                DataTable dados = new DataTable();
                comando.CommandText = "SELECT * FROM contactos WHERE nome = @nome AND id_contacto <> @id_contacto";
                SqlCeDataAdapter adaptador = new SqlCeDataAdapter();
                adaptador.SelectCommand = comando;
                adaptador.Fill(dados);
                if (dados.Rows.Count != 0)
                {
                    if (MessageBox.Show("Já existe um registo com o mesmo nome.\nDeseja gravar ainda assim?",
                       "ATENÇÃO",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }
                comando.CommandText = "UPDATE contactos SET "+
                                      "nome = @nome, "+
                                      "telefone = @telefone, "+
                                      "atualizacao = @atualizacao "+
                                      "WHERE id_contacto = @id_contacto";
                comando.ExecuteNonQuery();
                Hide();
            }
            
            #endregion
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void FrmAdicionarEditar_Load(object sender, EventArgs e)
        {
            if (editar)
            {
                ApresentaContacto();
            }
        }
        private void ApresentaContacto()
        {
            SqlCeConnection ligacao = new SqlCeConnection($"Data source = {vars.base_dados}");
            ligacao.Open();
            DataTable dados = new DataTable();
            SqlCeDataAdapter adaptador = new SqlCeDataAdapter($"SELECT * FROM contactos WHERE id_contacto = {id_contacto}", ligacao);
            adaptador.Fill(dados);
            adaptador.Dispose();
            txt_nome.Text = dados.Rows[0]["nome"].ToString();
            txt_telefone.Text = dados.Rows[0]["telefone"].ToString();
            ligacao.Dispose();
        }
    }
}
