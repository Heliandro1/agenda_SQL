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
namespace AgendaSQL
{
    public partial class FrmResultados : Form
    {
        List<int> id_contactos_eliminar = new List<int>();
        int id_contacto;
        string item_pesquisa;
        public FrmResultados(string item_pesquisa = "")
        {
            this.item_pesquisa = item_pesquisa;
            InitializeComponent();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void FrmResultados_Load(object sender, EventArgs e)
        {
            ConstruirGrelha();
        }
        private void ConstruirGrelha()
        {
            SqlCeConnection ligacao = new SqlCeConnection($"Data source = {vars.base_dados}");
            ligacao.Open();
            string query = "SELECT * FROM contactos";
            if (item_pesquisa != "")
                query = "SELECT * FROM contactos " +
                    "WHERE nome LIKE @item OR telefone LIKE @item";
            SqlCeCommand comando = new SqlCeCommand();
            comando.Parameters.AddWithValue("@item", $"%{item_pesquisa}%");
            comando.CommandText = query;
            comando.Connection = ligacao;

            SqlCeDataAdapter adaptador = new SqlCeDataAdapter();
            adaptador.SelectCommand = comando;
            DataTable dados = new DataTable();
            adaptador.Fill(dados);

            dgvResultados.DataSource = dados;
            lbl_res.Text = $"Resultados: {dgvResultados.Rows.Count}";
            dgvResultados.Columns["id_contacto"].Visible = dgvResultados.Columns["atualizacao"].Visible =  false;
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            SqlCeConnection ligacao = new SqlCeConnection($"Data source = {vars.base_dados}");
            ligacao.Open();
            SqlCeCommand comando = new SqlCeCommand();
            if (MessageBox.Show($"Tem a certeza que pretende eliminar os {dgvResultados.SelectedRows.Count} registos selecionados", 
                "ATENÇÃO", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning) == DialogResult.No)
                return;
            foreach (var item in id_contactos_eliminar)
            {
                comando.CommandText = $"DELETE FROM contactos WHERE id_contacto = {item}";
                comando.Connection = ligacao;
                comando.ExecuteNonQuery();
            }
            id_contactos_eliminar.Clear();
            ligacao.Dispose();
            ligacao.Dispose();
            ConstruirGrelha();
        }

        private void btnVerTudo_Click(object sender, EventArgs e)
        {
            item_pesquisa = "";
            ConstruirGrelha();
        }
        
        private void dgvResultados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvResultados.Rows.Count >= 1)
            {
                FrmAdicionarEditar F = new FrmAdicionarEditar(id_contacto);
                Hide();
                if (F.ShowDialog() == DialogResult.Cancel)
                {
                    Show();
                    ConstruirGrelha();
                }
            }
           
        }

        private void dgvResultados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvResultados.Rows.Count >= 1)
            {
                foreach (var item in dgvResultados.SelectedRows)
                {
                  
                    id_contactos_eliminar.Add(Convert.ToInt16(dgvResultados.Rows[dgvResultados.Rows.IndexOf(dgvResultados.CurrentRow)].Cells["id_contacto"].Value));
                }
                
                id_contacto = Convert.ToInt16(dgvResultados.Rows[dgvResultados.Rows.IndexOf(dgvResultados.CurrentRow)].Cells["id_contacto"].Value);
                btnApagar.Enabled = true;
            }
        }
    }
}
