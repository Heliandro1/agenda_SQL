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
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
            lbl_versao.Text = vars.versao;
        }

        private void FrmMenu_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja Sair da Aplicação", "Sair", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            Application.Exit();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            FrmAdicionarEditar F = new FrmAdicionarEditar();
            Hide();
            if (F.ShowDialog() == DialogResult.Cancel)
                Show();
            
        }

        private void btnVerTudo_Click(object sender, EventArgs e)
        {
            FrmResultados F = new FrmResultados();
            Hide();
            if (F.ShowDialog() == DialogResult.Cancel)
                Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmPesquisa F = new FrmPesquisa();
            Hide();
            if (F.ShowDialog() == DialogResult.Cancel)
                Show();
            if (F.cancelado == true)
            {
                F.Dispose();
                return;
            }
            string query = "SELECT * FROM contactos"+
                "WHERE nome LIKE '%" ;
            FrmResultados FF = new FrmResultados(F.texto_pesquisa);
            Hide();
            if (FF.ShowDialog() == DialogResult.Cancel)
                Show();
        }

        private void btnResetar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ATENÇÃO: Deseja eliminar todos os contactos da base de dados",
                "ATENÇÃO",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.No)
                return;
            SqlCeConnection ligacao = new SqlCeConnection($"Data source = {vars.base_dados}");
            ligacao.Open();
            SqlCeCommand comando = new SqlCeCommand("DELETE FROM contactos", ligacao);
            comando.ExecuteNonQuery();
            ligacao.Dispose();
            comando.Dispose();
            MessageBox.Show("Dados eliminados com sucesso");

        }
    }
}
