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

using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp3
{
    public partial class Window1 : Window
    {
        private readonly string CNX =
            @"Server=localhost\SQLEXPRESS;Database=NeptunoDBB;User ID=userTecsupDB;Password=123456;Encrypt=True;TrustServerCertificate=True;";

        public Window1()
        {
            InitializeComponent();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            var lista = new List<Proveedor>();

            using var cn = new SqlConnection(CNX);
            using var cmd = new SqlCommand("usp.ListadoProveedores_NC", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nombrecontacto", txtNombre.Text.Trim());
            cmd.Parameters.AddWithValue("@ciudad", txtCiudad.Text.Trim());

            cn.Open();
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Proveedor
                {
                    IdProveedor = dr.GetInt32(dr.GetOrdinal("IdProveedor")),
                    NombreCompañia = dr.GetString(dr.GetOrdinal("NombreCompañia")),
                    NombreContacto = dr.GetString(dr.GetOrdinal("NombreContacto")),
                    CargoContacto = dr.IsDBNull(dr.GetOrdinal("CargoContacto")) ? "" : dr.GetString(dr.GetOrdinal("CargoContacto")),
                    Direccion = dr.IsDBNull(dr.GetOrdinal("Direccion")) ? "" : dr.GetString(dr.GetOrdinal("Direccion")),
                    Ciudad = dr.GetString(dr.GetOrdinal("Ciudad")),
                    Pais = dr.GetString(dr.GetOrdinal("Pais")),
                    Telefono = dr.GetString(dr.GetOrdinal("Telefono"))
                });
            }

            dgProveedores.ItemsSource = lista;
        }
    }

    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string NombreCompañia { get; set; }
        public string NombreContacto { get; set; }
        public string CargoContacto { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }
    }
}