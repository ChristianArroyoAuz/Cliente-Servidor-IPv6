using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServidorIPv6
{
    public partial class FrmServidorIPv6 : Form
    {
        static public int puerto = 9999;

        private delegate void actualizarTexto(string texto);

        public FrmServidorIPv6()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread hiloTrabajador = new Thread(new ThreadStart(Tarea));
            hiloTrabajador.Start();

            this.Text = IPAddress.IPv6Loopback.ToString();
        }

        private void Tarea()
        {
            Socket socketEscucha;
            Socket socketCliente;

            IPEndPoint ipLocal = new IPEndPoint(IPAddress.IPv6Loopback, puerto);
            byte[] bytesRecibidos = new byte[Byte.MaxValue];
            int cantidadBytesRecibidos;
            if (!Socket.OSSupportsIPv6)
            {
                MessageBox.Show("No hay soporte para IPv6");
                return;
            }
            socketEscucha = new Socket(
                            AddressFamily.InterNetworkV6,
                            SocketType.Stream,
                            ProtocolType.Tcp
                            );

            socketEscucha.Bind(ipLocal);
            socketEscucha.Listen(0);

            socketCliente = socketEscucha.Accept();
            while (true)
            {
                cantidadBytesRecibidos = socketCliente.Receive(bytesRecibidos);
                if (cantidadBytesRecibidos <= 0)
                    break;

                txtDatos.Invoke(new actualizarTexto(ActualizarTexto), new object[] { Encoding.ASCII.GetString(bytesRecibidos) });
            }
            txtDatos.Invoke(new actualizarTexto(ActualizarTexto), new object[] { " \n" });

            socketCliente.Close();
            socketEscucha.Close();
        }

        private void ActualizarTexto(string texto)
        {
            txtDatos.Text += texto;
        }
    }
}