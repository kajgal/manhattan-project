using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// this form is box for every client side form, instance of materialize

namespace Mess
{
    public partial class mainForm : MaterialForm
    {

        private static mainForm _instance;
        public static mainForm Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new mainForm();
                return _instance;
            }
        }

        public mainForm()
        {
            InitializeComponent();
            MaterialSkinManager manager = MaterialSkinManager.Instance;
            manager.AddFormToManage(this);
            manager.Theme = MaterialSkinManager.Themes.LIGHT;
            manager.ColorScheme = new ColorScheme(Primary.Blue400, Primary.Blue500,Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
        }

        public Panel Content
        {
            get
            {
                return MainContainer;
            }
            set
            {
                MainContainer = value;
            }
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            _instance = this;
            MainContainer.Controls.Add(new ucLogin() {Dock=DockStyle.Fill });
            this.BackColor = ColorTranslator.FromHtml("#f6f6f6");
        }

        // close connection on exit
        private void mainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ucLogin._socket != null)
            {
                // exit request send to server
                string exitMessage = ":exit";
                Byte[] messageToBytes = Encoding.ASCII.GetBytes(exitMessage);
                ucLogin._socket.Send(messageToBytes, messageToBytes.Length, 0);

                // close
                ucLogin._socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                ucLogin._socket.Close();
            }
        }
    }
}
