using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Mysqlx.Connection;

namespace GestorDeEstudantesHidekiT6
{
    internal class MeuBancoDeDados
    {
        private MySqlConnection conexao =
            new MySqlConnection("datasources=localhost;port=3306;username=root;password=;database=sga_estudantes_bd_t6");
    }
}
