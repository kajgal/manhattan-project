using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

delegate void AddMessage(string message);

// this form is where connection happens, async socket

namespace Mess
{
    public partial class ucLogin : UserControl
    {
        internal static Socket _socket;
        private static byte[] _buffer = new byte[1024];
        private static event AddMessage _AddMessage;
        public static List<string> onlineUsers;
        internal static string _username;
        internal static Dictionary<string, string> chatHistory = new Dictionary<string, string>();
        private ucMain ucmain = new ucMain () { Dock = DockStyle.Fill };

        public ucLogin()
        {
            InitializeComponent();
            _AddMessage = new AddMessage(OnAddMessage);
            this.BackColor = ColorTranslator.FromHtml("#f6f6f6");

        }

        // sign in and connection establishment
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            // establish connection

            // remember name
            _username = txtName.Text;
            ucMain._username = txtName.Text;

            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint target = new IPEndPoint(IPAddress.Parse("192.168.226.135"), 8080);

                AsyncCallback onConnect = new AsyncCallback(OnConnect);

                _socket.BeginConnect(target, onConnect, _socket);

                Byte[] messageToBytes = Encoding.ASCII.GetBytes(_username.ToCharArray());

                _socket.Send(messageToBytes, messageToBytes.Length, 0);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }

            mainForm.Instance.Content.Controls.Add(ucmain);
            mainForm.Instance.Content.Controls[0].SendToBack();
        }

        // onConnect async callback
        public void OnConnect(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;

            try
            {
                if (socket.Connected)
                    SetupReceiveCallback(socket);
                else
                    MessageBox.Show(this, "Unable to connect to remote machine", "Connection failure");
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        // onReceivedData async callback
        // here we basically speak with server, answer for his calls
        public void OnReceivedData(IAsyncResult ar)
        {
            Socket sock = (Socket)ar.AsyncState;

            try
            {
                int bytesReceived = sock.EndReceive(ar);
                
                if(bytesReceived > 0)
                {
                    string receivedMessage = Encoding.ASCII.GetString(_buffer, 0, bytesReceived).Trim();

                    // check if its first call to get current list of users
                    if (receivedMessage.Split(" ")[0] == "+{OnlineUsersListOnLoad}+")
                    {
                        onlineUsers = receivedMessage.Trim().Split(" ").ToList();

                        onlineUsers.Remove("+{OnlineUsersListOnLoad}+");
                        ucmain.updateList(onlineUsers);
                    }
                    // check if its call after sameone joined
                    else if (receivedMessage.Split(" ")[0] == "+{AddNewUser}+")
                    {
                        // if the user we add its first on the list it has never been created so
                        if (ucMain._onlineUsers == null)
                            ucMain._onlineUsers = new List<string>();

                        string userToAdd = receivedMessage.Split(" ")[1];
                        ucmain.addNewOnlineUser(userToAdd);
                    }
                    // check if its call after sameone disconnected
                    else if (receivedMessage.Split(" ")[0] == "+{RemoveDisconnectedUser}+")
                    {
                        string userToRemove = receivedMessage.Split(" ")[1];
                        ucmain.deleteDisconnectedUser(userToRemove);
                    }
                    else
                    {
                        Invoke(_AddMessage, new string[] { receivedMessage });
                    }

                    SetupReceiveCallback(sock);
                }
                else
                {
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // adding messages
        public void OnAddMessage(string message)
        {
            ucmain.updateChat(message);
        }

        public void SetupReceiveCallback(Socket sock)
        {
            try
            {
                AsyncCallback receiveData = new AsyncCallback(OnReceivedData);
                sock.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, receiveData, sock);
;            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
