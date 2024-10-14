using Shared.Models;
using Shared.Services;
using System.Collections.ObjectModel;
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

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProductService _productService;
        private ObservableCollection<Product> _products = [];

        public MainWindow()
        {
            InitializeComponent();
            _productService = new ProductService();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var product = new Product
            {
                ProductName = Input_ProductName.Text,
                ProductPrice = Input_ProductPrice.Text
            };
            var response = _productService.AddProduct(product);

            Input_ProductName.Text = "";
            Input_ProductPrice.Text = "";


            MessageBox.Show(response.Message);

            UpdateListView();
        }

        private void UpdateListView()
        {
            _products.Clear();
            foreach (var product in _productService.GetProducts())
            {
                _products.Add(product);
            }

            LB_Products.ItemsSource = _products;
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}