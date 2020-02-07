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


namespace ClienteIPv6
{
    public partial class FrmCliente : Form
    {
        private int puerto = 9999;

        public FrmCliente()
        {
            InitializeComponent();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            byte[] mensaje;
            if (!Socket.OSSupportsIPv6)
            {
                MessageBox.Show("No hay soporte para IPv6");
                return;
            }
            IPAddress direccionRemota = IPAddress.Parse(txtDireccionServidor.Text);
            IPEndPoint ipServidor = new IPEndPoint(direccionRemota, puerto);
            Socket socket = new Socket(
                                    AddressFamily.InterNetworkV6,
                                    SocketType.Stream,
                                    ProtocolType.Tcp
                                    );

            socket.Connect(ipServidor);
            mensaje = Encoding.ASCII.GetBytes(txtMensaje.Text);
            socket.Send(mensaje);
            socket.Close();

        }
    }
}