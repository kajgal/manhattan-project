using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// the most important form, here we we have online users list, we start chatting and sends file

namespace Mess
{
    public partial class ucMain : UserControl
    {
        [DllImport("user32.dll")]
        static public extern bool ShowScrollBar(System.IntPtr hWnd, int wBar, bool bShow);
        public static string _username = null;
        internal static List<string> _onlineUsers = null;
        private int usersCount = 0;
        private int statusClicks = 0;
        private BindingSource bs = new BindingSource();
        internal static string _passedMessage = String.Empty;
        internal static Dictionary<string, string> _currentSessionChat = new Dictionary<string, string>();
        internal static Dictionary<string, formSend> _chatForms = new Dictionary<string, formSend>();
        private FileInfo _File;

        Bitmap userOnline = Properties.Resources.good;
        Bitmap userBrb = Properties.Resources.bad;
        Bitmap userBusy = Properties.Resources.soso;

        public ucMain()
        {
            InitializeComponent();
        }

        private void ucMain_Load(object sender, EventArgs e)
        {
            lblUsername.Text = _username;
            lblUsersCount.Text += usersCount;
            this.ActiveControl = null;
            panelBar.BackColor = ColorTranslator.FromHtml("#42a5f5");
            sendFile.Visible = false;
            listViewSetup();
        }

        // setup list of users
        internal void listViewSetup()
        {
            ImageList statusIcons = new ImageList();
            statusIcons.ImageSize = new Size(32, 32);

            String[] paths = { };
            paths = Directory.GetFiles("C:/Users/Kajetan/source/repos/Mess/Resources/statusImages");

            try
            {
                foreach (String path in paths)
                {
                    statusIcons.Images.Add(Image.FromFile(path));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            listUsers.SmallImageList = statusIcons;
        }

        // on update request from server
        internal void updateList(List<string> onlineUsers)
        {
            // listBox
            _onlineUsers = onlineUsers;

            usersCount = _onlineUsers.Count;

            // listView
            foreach (string user in _onlineUsers)
            {
                listUsers.Items.Add(user, 0);
            }
        }

        // on add new user request from server
        internal void addNewOnlineUser(string user)
        {
            // listBox
            _onlineUsers.Add(user);

            usersCount++;
            refreshUsersCount();

            // listView
            listUsers.Items.Add(user, 0);

            bs.ResetBindings(false);
        }

        // on remove disconnected user request from server
        internal void deleteDisconnectedUser(string user)
        {
            // listBox
            _onlineUsers.Remove(user);

            usersCount--;
            refreshUsersCount();

            // listView
            listUsers.Items.Remove(listUsers.FindItemWithText(user));

            bs.ResetBindings(false);
        }

        // updating chat, very important function, pop outs chat with user when its closed, and updates when is opened
        internal void updateChat(string passedMessage)
        {
            // mesage is like Person: message
            
            // so we get person out of this message
            string person = passedMessage.Split(" ")[0].Replace(":","");

            // and message content
            string message = passedMessage + Environment.NewLine;

            //if its first message we creat history for that chat
            if(!_currentSessionChat.ContainsKey(person))
            {
                _currentSessionChat.Add(person, message);
            }
            // if there is already chat with that user we add new message
            else
            {
                string currentHistory = _currentSessionChat[person];
                currentHistory += message;
                _currentSessionChat[person] = currentHistory;
            }

            // when message comes we check if there is opened chat with the sender
            if(!_chatForms.ContainsKey(person))
            {
                // form settings
                formSend popoutForm = new formSend();
                popoutForm.Text = person;
                // add form to opened forms
                _chatForms.Add(person, popoutForm);
                // check if there is chat history with the sender
                if(_currentSessionChat.ContainsKey(person))
                {
                    popoutForm.getChatHistory(_currentSessionChat[person]);
                }
                // show form
                popoutForm.Show();
            }
            // chat is opened so we find that opened form and refresh its chat
            else
            {
                _chatForms[person].refreshChat(message);
            }
        }

        // open chat on double click
        private void listUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string userToChat = listUsers.SelectedItems[0].Text;

            // open chat form only if there isn't opened any with that user
            if(!_chatForms.ContainsKey(userToChat))
            {
                // form settings
                formSend openedForm = new formSend();
                openedForm.Text = userToChat;
                // adding to opened form dictionary
                _chatForms.Add(userToChat, openedForm);
                // check if there is chat history with that user and fill it when is
                if(_currentSessionChat.ContainsKey(userToChat))
                {
                    openedForm.getChatHistory(_currentSessionChat[userToChat]);
                }
                // showing form
                openedForm.Show();
            }
        }

        // maybe in future
        private void picStatus_Click(object sender, EventArgs e)
        {
            statusClicks += 1;
            if (statusClicks % 3 == 1) {
                picStatus.Image = userBrb;
            }
            else if(statusClicks % 3 == 2)
            {
                picStatus.Image = userBusy;
            } 
            else
            {
                picStatus.Image = userOnline;
            }
        }

        // refreshing online users
        private void refreshUsersCount()
        {
            lblUsersCount.Text = $"Online Users: {usersCount}";
        }

        // adding file and creation of while that we want to send, getting icon
        private void addFile_Click(object sender, EventArgs e)
        {
            // adding file to send
            var fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileToSend = fileDialog.FileName;

                ImageList imageList = new ImageList();

                _File = new FileInfo(fileDialog.FileName);

                Icon iconOrDefault = SystemIcons.WinLogo;

                if (!imageList.Images.ContainsKey(_File.Extension))
                {
                    iconOrDefault = System.Drawing.Icon.ExtractAssociatedIcon(_File.FullName);
                    fileIcon.Image = Bitmap.FromHicon(iconOrDefault.Handle);
                    filePath.Text = fileDialog.FileName;
                }

                sendFile.Visible = true;
            }
        }

        // very important function, whole logic of sending file
        private void sendFile_Click(object sender, EventArgs e)
        {
            // first thing is to send telegram to server so server will listen for file

            int chunkSize = 1024;
            byte[] chunk = new byte[chunkSize];
            int SendPackage;

            // read file 

            FileStream reader = new FileStream(_File.FullName, FileMode.Open, FileAccess.Read);

            BinaryReader binReader = new BinaryReader(reader);

            int bytesToRead = (int)reader.Length;


            string fileName = _File.Name;

            string telegram = "+{FILETRANSFER}+ " + fileName;

            Byte[] messageToBytes = Encoding.ASCII.GetBytes(telegram.ToCharArray());

            ucLogin._socket.Send(messageToBytes, messageToBytes.Length, 0);

            // sleep for a while
            Thread.Sleep(3);

            // buy more time

            MessageBox.Show($"Your file {_File.Name} has been sent to server with success!");

            // actually sending the file

            string fileSize = bytesToRead.ToString();

            byte[] stop = Encoding.ASCII.GetBytes(fileSize.ToCharArray());

            ucLogin._socket.Send(stop);

            Thread.Sleep(5);

            // it has to be done in chunks instead of Socket.File() because server didn't know when sending is done and couldn't exit the recv loop

            while (bytesToRead > 0)
            {
                chunk = binReader.ReadBytes(chunkSize);
                bytesToRead -= chunkSize;
                SendPackage = ucLogin._socket.Send(chunk);
            }

            binReader.Close();


            // clear transfer section for another usage
            filePath.Text = String.Empty;
            fileIcon.Image = null;
            sendFile.Visible = false;
        }
    }
}
