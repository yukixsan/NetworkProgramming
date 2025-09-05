using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;


namespace MovingObject
{

    public partial class Form1 : Form
    {
        // Drawing 
        Pen red = new Pen(Color.Red);
        Rectangle rect = new Rectangle(20, 20, 30, 30);
        SolidBrush fillBlue = new SolidBrush(Color.Blue);
        int slide = 10;

        // Network
        private Socket serverSocket;
        private List<Socket> clientSockets = new List<Socket>();

        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 50;
            timer1.Enabled = true;

            StartServer();
        }

        private void StartServer()
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, 3333)); // listen on port 3333
                serverSocket.Listen(10);
                serverSocket.BeginAccept(AcceptCallback, null);

                this.Text = "Server running on port 3333";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server start failed: " + ex.Message);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = serverSocket.EndAccept(ar);
                clientSockets.Add(client);

                string msg = $"Welcome Client #{clientSockets.Count}";
                byte[] data = Encoding.ASCII.GetBytes(msg);
                client.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);

                // Keep listening for new clients
                serverSocket.BeginAccept(AcceptCallback, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Accept failed: " + ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            back();

            rect.X += slide;
            Invalidate();

            // Broadcast object position to all clients
            ObjectPackage obj = new ObjectPackage(rect.X, rect.Y);
            byte[] data = obj.ToByteArray();

            List<Socket> deadClients = new List<Socket>();
            foreach (Socket client in clientSockets)
            {
                try
                {
                    client.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
                }
                catch
                {
                    deadClients.Add(client);
                }
            }

            foreach (var dead in deadClients)
                clientSockets.Remove(dead);
        }

        private void back()
        {
            if (rect.X >= this.Width - rect.Width * 2)
                slide = -10;
            else if (rect.X <= rect.Width / 2)
                slide = 10;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(red, rect);
            g.FillRectangle(fillBlue, rect);
        }
    }
}
