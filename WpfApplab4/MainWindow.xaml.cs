using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace WpfApplab4
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly string connectionString = "Data Source=127.0.0.1,1433; Initial Catalog=Neptuno; User ID=SA; Password=12Groudon34@; TrustServerCertificate=True;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_BuscarPedidosPorFecha(object sender, RoutedEventArgs e)
        {
            if (dpInicio.SelectedDate == null || dpFin.SelectedDate == null)
            {
                MessageBox.Show("Por favor selecciona ambas fechas.");
                return;
            }

            try
            {
                DateTime fechaInicio = dpInicio.SelectedDate.Value;
                DateTime fechaFin = dpFin.SelectedDate.Value;

                var detalles = ObtenerDetallesPedidosPorFechas(fechaInicio, fechaFin);
                dgall.ItemsSource = detalles;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener detalles de pedidos: " + ex.Message);
            }
        }

        private void Button_Buscar_proveedor(object sender, RoutedEventArgs e)
        {
            try
            {
                // Asume que buscarás por contacto y ciudad separados por coma
                var valores = txtBuscar.Text.Split(',');

                string nombreContacto = valores.Length > 0 ? valores[0].Trim() : "";
                string ciudad = valores.Length > 1 ? valores[1].Trim() : "";

                var resultado = BuscarProveedores(nombreContacto, ciudad);
                dgall.ItemsSource = resultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la búsqueda: " + ex.Message);
            }
        }

        private void Button_Productos(object sender, RoutedEventArgs e)
        {
            try
            {
                var productos = ObtenerProductos();
                dgall.ItemsSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener productos: " + ex.Message);
            }
        }


        private void Button_Pedidos(object sender, RoutedEventArgs e)
        {
            try
            {
                var pedidos = ObtenerPedidos();
                dgall.ItemsSource = pedidos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener pedidos: " + ex.Message);
            }
        }

        private void Button_Proveedor(object sender, RoutedEventArgs e)
        {
            try
            {
                var proveedores = ObtenerProveedores();
                dgall.ItemsSource = proveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener proveedores: " + ex.Message);
            }
        }
        public List<Proveedor> ObtenerProveedores()
        {
            var lista = new List<Proveedor>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("ListarProveedores", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Proveedor
                    {
                        IdProveedor = Convert.ToInt32(reader["idProveedor"]),
                        NombreCompañia = reader["nombreCompañia"].ToString(),
                        NombreContacto = reader["nombrecontacto"].ToString(),
                        CargoContacto = reader["cargocontacto"].ToString(),
                        Direccion = reader["direccion"].ToString(),
                        Ciudad = reader["ciudad"].ToString(),
                        Region = reader["region"].ToString(),
                        CodPostal = reader["codPostal"].ToString(),
                        Pais = reader["pais"].ToString(),
                        Telefono = reader["telefono"].ToString(),
                        Fax = reader["fax"].ToString(),
                        PaginaPrincipal = reader["paginaprincipal"].ToString()
                    });
                }

                reader.Close();
            }

            return lista;
        }

        public List<Producto> ObtenerProductos()
        {
            var lista = new List<Producto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("ListarProductos", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Producto
                    {
                        IdProducto = Convert.ToInt32(reader["idproducto"]),
                        NombreProducto = reader["nombreProducto"].ToString(),
                        IdProveedor = Convert.ToInt32(reader["idProveedor"]),
                        IdCategoria = Convert.ToInt32(reader["idCategoria"]),
                        CantidadPorUnidad = reader["cantidadPorUnidad"].ToString(),
                        PrecioUnidad = Convert.ToDecimal(reader["precioUnidad"]),
                        UnidadesEnExistencia = Convert.ToInt16(reader["unidadesEnExistencia"]),
                        UnidadesEnPedido = Convert.ToInt16(reader["unidadesEnPedido"]),
                        NivelNuevoPedido = Convert.ToInt16(reader["nivelNuevoPedido"]),
                        Suspendido = Convert.ToBoolean(reader["suspendido"]),
                        CategoriaProducto = reader["categoriaProducto"].ToString()
                    });
                }

                reader.Close();
            }

            return lista;
        }


        public List<Pedido> ObtenerPedidos()
        {
            var lista = new List<Pedido>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("ListarPedidos", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Pedido
                    {
                        IdPedido = Convert.ToInt32(reader["IdPedido"]),
                        IdCliente = reader["IdCliente"].ToString(),
                        IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                        FechaPedido = Convert.ToDateTime(reader["FechaPedido"]),
                        FechaEntrega = reader["FechaEntrega"] == DBNull.Value ? null : (DateTime?)reader["FechaEntrega"],
                        FechaEnvio = reader["FechaEnvio"] == DBNull.Value ? null : (DateTime?)reader["FechaEnvio"],
                        FormaEnvio = reader["FormaEnvio"].ToString(),
                        Cargo = Convert.ToDecimal(reader["Cargo"]),
                        Destinatario = reader["Destinatario"].ToString(),
                        DireccionDestinatario = reader["DireccionDestinatario"].ToString(),
                        CiudadDestinatario = reader["CiudadDestinatario"].ToString(),
                        RegionDestinatario = reader["RegionDestinatario"].ToString(),
                        CodPostalDestinatario = reader["CodPostalDestinatario"].ToString(),
                        PaisDestinatario = reader["PaisDestinatario"].ToString()
                    });
                }

                reader.Close();
            }

            return lista;
        }
        public List<Proveedor> BuscarProveedores(string nombreContacto, string ciudad)
        {
            var lista = new List<Proveedor>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("BuscarProveedores", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@NombreContacto", nombreContacto);
                command.Parameters.AddWithValue("@Ciudad", ciudad);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Proveedor
                    {
                        IdProveedor = Convert.ToInt32(reader["idProveedor"]),
                        NombreCompañia = reader["nombreCompañia"].ToString(),
                        NombreContacto = reader["nombrecontacto"].ToString(),
                        CargoContacto = reader["cargocontacto"].ToString(),
                        Direccion = reader["direccion"].ToString(),
                        Ciudad = reader["ciudad"].ToString(),
                        Region = reader["region"].ToString(),
                        CodPostal = reader["codPostal"].ToString(),
                        Pais = reader["pais"].ToString(),
                        Telefono = reader["telefono"].ToString(),
                        Fax = reader["fax"].ToString(),
                        PaginaPrincipal = reader["paginaprincipal"].ToString()
                    });
                }

                reader.Close();
            }

            return lista;
        }
        public List<DetallePedido> ObtenerDetallesPedidosPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<DetallePedido>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("ListarDetallesPedidosPorFechas", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@FechaFin", fechaFin);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new DetallePedido
                    {
                        IdPedido = Convert.ToInt32(reader["IdPedido"]),
                        FechaPedido = Convert.ToDateTime(reader["FechaPedido"]),
                        IdProducto = Convert.ToInt32(reader["IdProducto"]),
                        PrecioUnidad = Convert.ToDecimal(reader["PrecioUnidad"]),
                        Cantidad = Convert.ToInt32(reader["Cantidad"]),
                        Descuento = float.Parse(reader["Descuento"].ToString())
                    });
                }

                reader.Close();
            }

            return lista;
        }

    }
}

