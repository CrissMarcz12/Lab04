using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp3
{
    public partial class Window2 : Window
    {
        private readonly string CNX =
            @"Server=localhost\SQLEXPRESS;Database=NeptunoDBB;User ID=userTecsupDB;Password=123456;Encrypt=True;TrustServerCertificate=True;";

        public Window2()
        {
            InitializeComponent();
            CargarDetalles();   // al iniciar ya carga
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            CargarDetalles();   // al dar click vuelve a cargar
        }

        private void CargarDetalles()
        {
            var lista = new List<DetallePedidoRow>();

            using var cn = new SqlConnection(CNX);
            using var cmd = new SqlCommand("usp.ListadoDetallesPedidos", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@desde", SqlDbType.Date).Value = (object?)dpDesde.SelectedDate ?? DBNull.Value;
            cmd.Parameters.Add("@hasta", SqlDbType.Date).Value = (object?)dpHasta.SelectedDate ?? DBNull.Value;

            cn.Open();
            using var dr = cmd.ExecuteReader();

            int count = 0;
            while (dr.Read())
            {
                lista.Add(new DetallePedidoRow
                {
                    IdPedido = Convert.ToInt32(dr["IdPedido"]),
                    FechaPedido = Convert.ToDateTime(dr["FechaPedido"]),
                    IdProducto = Convert.ToInt32(dr["IdProducto"]),
                    PrecioUnidad = Convert.ToDecimal(dr["PrecioUnidad"]),
                    Cantidad = Convert.ToInt16(dr["Cantidad"]),
                    Descuento = Convert.ToDecimal(dr["Descuento"])
                });

                if (++count >= 20) break; // solo 20 filas
            }

            dgDetalles.ItemsSource = lista;
        }
    }

    public class DetallePedidoRow
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public int IdProducto { get; set; }
        public decimal PrecioUnidad { get; set; }
        public short Cantidad { get; set; }
        public decimal Descuento { get; set; }
    }
}