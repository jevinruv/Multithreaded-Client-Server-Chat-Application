using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class formPrivate : Form
    {
        TcpClient clientSocket = new TcpClient();
        String friend, myName;
        Thread ctThread;
        NetworkStream serverStream = default(NetworkStream);
        List<string> chat = new List<string>();


        public String getFriend()
        {
            return this.friend;
        }

        public String getHistory()
        {
            return history.Text;
        }

        public void setHistory(String message)
        {
            this.Invoke((MethodInvoker)delegate // To Write the Received data
            {
                history.Text = history.Text + Environment.NewLine + " >> " + message;
            });
        }


        public formPrivate(String friend, TcpClient c, String name)
        {
            InitializeComponent();
            clientSocket = c;
            this.friend = friend;
            this.myName = name;

            serverStream = clientSocket.GetStream();
            ctThread = new Thread(getMessage);
            ctThread.Start();

        }


        private void history_TextChanged(object sender, EventArgs e)
        {
            history.SelectionStart = history.TextLength;
            history.ScrollToCaret();
        }

        public Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        public byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!inputPrivate.Text.Equals(""))
                {
                    chat.Clear();
                    chat.Add("pChat");
                    chat.Add(friend);
                    chat.Add(myName);
                    chat.Add(inputPrivate.Text);

                    byte[] outStream = ObjectToByteArray(chat);

                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();

                    this.Invoke((MethodInvoker)delegate // To Write the Received data
                    {
                        history.Text = history.Text + Environment.NewLine + inputPrivate.Text;
                        inputPrivate.Text = "";
                    });
                }
            }
            catch (Exception er)
            {

            }
        }

        public void getMessage()
        {
            try
            {
                while (true)
                {
                    byte[] inStream = new byte[10025];
                    serverStream.Read(inStream, 0, inStream.Length);

                    //if (!SocketConnected(clientSocket))
                    //{
                    //    MessageBox.Show("You've been Disconnected");
                    //    ctThread.Abort();
                    //    clientSocket.Close();
                    //    //btnConnect.Enabled = true;
                    //}

                    List<string> parts = (List<string>)ByteArrayToObject(inStream);

                    if ((parts[2].Equals(friend)))
                    {
                        setHistory(parts[3]);
                    }

                    else if (parts[0].Equals('\0'))
                    {
                        setHistory("Client Left");
                        ctThread.Abort();
                        clientSocket.Close();
                        break;
                    }
                    parts.Clear();
                }
            }
            catch (Exception e)
            {
                ctThread.Abort();
                clientSocket.Close();
                //btnConnect.Enabled = true;
            }

        }

        bool SocketConnected(TcpClient s)
        {
            bool part1 = s.Client.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

    }
}
