using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlServerCe;
namespace AgendaSQL
{
    public static partial class vars
    {
        public static string versao = "v.1.0.0";
        public static string pasta_dados, base_dados;
        public static void Iniciar()
        {
            pasta_dados = $@"{ Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\AgendaSQL\";
            if (!Directory.Exists(pasta_dados))
                Directory.CreateDirectory(pasta_dados);
            base_dados = $"{pasta_dados}dados.sdf; Password = 'heliandrohFBI'";
            if (!File.Exists($"{pasta_dados}dados.sdf"))
                CriarBaseDados();
           
        }

        public static void CriarBaseDados()
        {
            SqlCeEngine motor = new SqlCeEngine($"Data source = {base_dados}");
            motor.CreateDatabase();

            SqlCeConnection ligacao = new SqlCeConnection();
            ligacao.ConnectionString = $"Data source = {base_dados}";
            ligacao.Open();
            SqlCeCommand comando = new SqlCeCommand();
            comando.CommandText =
                "CREATE TABLE contactos(" +
                "id_contacto            int not null primary key, " +
                "nome                   nvarchar(50), " +
                "telefone               nvarchar(20), " +
                "atualizacao            datetime)";
            comando.Connection = ligacao;
            comando.ExecuteNonQuery();
            comando.Dispose();
            ligacao.Dispose();
        }


    }
}
