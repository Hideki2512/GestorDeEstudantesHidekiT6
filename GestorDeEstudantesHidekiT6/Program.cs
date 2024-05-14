using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestorDeEstudantesHidekiT6
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form_Login());

            Form_Login login_form = new Form_Login(); 
            if (login_form.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FormMenuPrincipal());
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
