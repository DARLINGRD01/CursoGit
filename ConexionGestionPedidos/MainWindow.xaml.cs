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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {  
              
        SqlConnection miConnectionSql;

        public MainWindow()
        {
            InitializeComponent();
     

            string miconetion = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            miConnectionSql = new SqlConnection(miconetion);

            MuestraClientes();
            MuestratodosPedidos();

        }
        private void MuestraClientes()
        {
            string consulta = "SELECT * FROM  Cliente";

            //pruega de git 
            // numero 11

            SqlDataAdapter myadapter = new SqlDataAdapter(consulta, miConnectionSql);
            
                {
                using (myadapter)

                {
                    DataTable clientesTabla = new DataTable();

                    myadapter.Fill(clientesTabla);

                    listaClientes.DisplayMemberPath = "name";
                    listaClientes.SelectedValuePath = "Id";
                    listaClientes.ItemsSource = clientesTabla.DefaultView;             
                }


            }
           
        }

        //private void listaClientes_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{         
        //}

        private void listaClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            MostrarPedidos();
             //MessageBox.Show("has checho clip");

        }


        private void MuestratodosPedidos()
        {

            string consulta = "select *, concat (cCliente,' ',fechaPedido,' ',formaPago) AS INFOCOMPLETA from Pedido";

            SqlDataAdapter miadactador = new SqlDataAdapter(consulta, miConnectionSql);

                using (miadactador)
                 {
                DataTable pedidosTabla = new DataTable();

                miadactador.Fill(pedidosTabla);

                todosPedidos.DisplayMemberPath = "INFOCOMPLETA";
                todosPedidos.SelectedValuePath = "id";
                todosPedidos.ItemsSource = pedidosTabla.DefaultView;
            
                }
       
        }



        private void MostrarPedidos()
        {           
            string consulta = "SELECT * FROM PEDIDO P INNER JOIN CLIENTE  C  ON C.id = P.cCliente" +
                " WHERE C.id = @ClienteId";

            SqlCommand micomand = new SqlCommand(consulta, miConnectionSql);

            SqlDataAdapter myadapter = new SqlDataAdapter(micomand);

            using (myadapter)
                 
            {
                micomand.Parameters.AddWithValue("@ClienteId", listaClientes.SelectedValue);

                DataTable pedidoTabla = new DataTable();
                myadapter.Fill(pedidoTabla);

                listaPedidos.DisplayMemberPath = "fechaPedido";
                listaPedidos.SelectedValuePath = "Id";
                listaPedidos.ItemsSource = pedidoTabla.DefaultView;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //MessageBox.Show(todosPedidos.SelectedValue.ToString()) ;

                string consulta = "DELETE FROM PEDIDO " +
                " WHERE Id=@pedidoId";

                SqlCommand sqlCommand = new SqlCommand(consulta, miConnectionSql);

                miConnectionSql.Open();

                sqlCommand.Parameters.AddWithValue("@pedidoId", todosPedidos.SelectedValue);

                 
                
                if (todosPedidos.SelectedValue == null)
                {
                    MessageBox.Show(todosPedidos.ToString());
                }
                sqlCommand.ExecuteNonQuery();
                miConnectionSql.Close();
                MuestratodosPedidos();

            }              
            catch (Exception ex)
            {
                throw new Exception(ex.Message);//, todosPedidos.ToString());
            }

        }
    }
}
