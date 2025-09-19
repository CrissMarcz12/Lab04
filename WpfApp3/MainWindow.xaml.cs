using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        private readonly string CNX =
            @"Server=localhost\SQLEXPRESS;Database=NeptunoDBB;User ID=userTecsupDB;Password=123456;Encrypt=True;TrustServerCertificate=True;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            var lista = new List<Producto>();

            using var cn = new SqlConnection(CNX);
            using var cmd = new SqlCommand("usp.ListadoProductos", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cn.Open();
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Producto
                {
                    IdProducto = dr.GetInt32(dr.GetOrdinal("IdProducto")),
                    NombreProducto = dr.GetString(dr.GetOrdinal("NombreProducto")),
                    IdProveedor = dr.GetInt32(dr.GetOrdinal("IdProveedor")),
                    IdCategoria = dr.GetInt32(dr.GetOrdinal("IdCategoria")),
                    CantidadPorUnidad = dr.IsDBNull(dr.GetOrdinal("CantidadPorUnidad")) ? "" : dr.GetString(dr.GetOrdinal("CantidadPorUnidad")),
                    PrecioUnidad = dr.GetDecimal(dr.GetOrdinal("PrecioUnidad")),
                    UnidadesEnExistencia = dr.GetInt16(dr.GetOrdinal("UnidadesEnExistencia")),
                    UnidadesEnPedido = dr.GetInt16(dr.GetOrdinal("UnidadesEnPedido")),
                    NivelNuevoPedido = dr.GetInt16(dr.GetOrdinal("NivelNuevoPedido")),

                    Suspendido = dr.GetInt16(dr.GetOrdinal("Suspendido")) != 0
                });
            }


            string filtro = txtBuscar.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(filtro))
            {
                dgProductos.ItemsSource = lista
                    .Where(p => p.NombreProducto.ToLower().Contains(filtro))
                    .ToList();
            }
            else
            {
                dgProductos.ItemsSource = lista;
            }
        }
    }

  
}