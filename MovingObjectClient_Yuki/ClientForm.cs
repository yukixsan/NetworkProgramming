using MovingObject;
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace MovingObjectClient_Yuki
{
    public partial class ClientForm : Form
    {
        // Drawing
        Pen red = new Pen(Color.Red);
        Rectangle rect = new Rectangle(20, 20, 30, 30);
        SolidBrush fillBlue = new SolidBrush(Color.Blue);

        // Networking
        private Socket clientSocket;
        private byte[] buffer;

        public ClientForm()
        {
            InitializeComponent();
        }

        // Auto-connect when form is shown
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var endPoint = new IPEndPoint(IPAddress.Loopback, 3333);
                clientSocket.BeginConnect(endPoint, ConnectCallback, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);
                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ConnectCallback error: " + ex.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int received = clientSocket.EndReceive(ar);
                if (received == 0) return;

                string msg = Encoding.ASCII.GetString(buffer, 0, received);

                if (msg.StartsWith("Welcome"))
                {
                    
                    this.BeginInvoke((Action)(() => this.Text = msg));
                }
                else
                {
                    ObjectPackage obj = new ObjectPackage(buffer);
                    rect.X = obj.X;
                    rect.Y = obj.Y;
                    this.BeginInvoke((Action)(() => this.Invalidate()));
                }

                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                this.BeginInvoke((Action)(() => this.Text = "Disconnected: " + ex.Message));
            }
        }

        private void ClientForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(red, rect);
            g.FillRectangle(fillBlue, rect);
        }
    }
}
