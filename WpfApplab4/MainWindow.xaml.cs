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

        public List<Cliente> ObtenerClientes()
        {
            var lista = new List<Cliente>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("USP_ListarClientes", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Cliente
                    {
                        IdCliente = reader["IdCliente"].ToString(),
                        NombreCompañia = reader["NombreCompañia"].ToString(),
                        NombreContacto = reader["NombreContacto"].ToString(),
                        CargoContacto = reader["CargoContacto"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Ciudad = reader["Ciudad"].ToString(),
                        Region = reader["Region"].ToString(),
                        CodPostal = reader["CodPostal"].ToString(),
                        Pais = reader["Pais"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Fax = reader["Fax"].ToString(),
                        Activo = Convert.ToBoolean(reader["Activo"])
                    });
                }

                reader.Close();
            }

            return lista;
        }

        public void RegistrarCliente(Cliente cliente)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("USP_InsertarClientes", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                command.Parameters.AddWithValue("@NombreCompañia", cliente.NombreCompañia);
                command.Parameters.AddWithValue("@NombreContacto", cliente.NombreContacto);
                command.Parameters.AddWithValue("@CargoContacto", cliente.CargoContacto);
                command.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                command.Parameters.AddWithValue("@Ciudad", cliente.Ciudad);
                command.Parameters.AddWithValue("@Region", cliente.Region);
                command.Parameters.AddWithValue("@CodPostal", cliente.CodPostal);
                command.Parameters.AddWithValue("@Pais", cliente.Pais);
                command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                command.Parameters.AddWithValue("@Fax", cliente.Fax);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void Button_RegistrarCliente(object sender, RoutedEventArgs e)
        {
            try
            {
                var cliente = new Cliente
                {
                    IdCliente = txtIdCliente.Text,
                    NombreCompañia = txtNombreCompañia.Text,
                    NombreContacto = txtNombreContacto.Text,
                    CargoContacto = txtCargoContacto.Text,
                    Direccion = txtDireccion.Text,
                    Ciudad = txtCiudad.Text,
                    Region = txtRegion.Text,
                    CodPostal = txtCodPostal.Text,
                    Pais = txtPais.Text,
                    Telefono = txtTelefono.Text,
                    Fax = txtFax.Text
                };

                RegistrarCliente(cliente);
                MessageBox.Show("✅ Cliente registrado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                
                LimpiarCampos();
                dgall.ItemsSource = ObtenerClientes();

            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error al registrar cliente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ListarClientes(object sender, RoutedEventArgs e)
        {
            try
            {
                var clientes = ObtenerClientes();
                dgall.ItemsSource = clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error al listar clientes: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtIdCliente.Clear();
            txtNombreCompañia.Clear();
            txtNombreContacto.Clear();
            txtCargoContacto.Clear();
            txtDireccion.Clear();
            txtCiudad.Clear();
            txtRegion.Clear();
            txtCodPostal.Clear();
            txtPais.Clear();
            txtTelefono.Clear();
            txtFax.Clear();
        }
    }
}
