using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgendaSQL
{
    public partial class FrmPesquisa : Form
    {
        public string texto_pesquisa { get; set; }
        public bool cancelado { get; set; }
        public FrmPesquisa()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            cancelado = true;
            Hide();
        }

        private void FrmPesquisa_Load(object sender, EventArgs e)
        {
           
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (txt_pesquisa.Text == string.Empty)
                cancelado = true;
            else
                texto_pesquisa = txt_pesquisa.Text;
            
            Hide();
        }
    }
}
