using Shared.Interfaces;
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
        private Product _editingProduct = null!; 




        public MainWindow()
        {
            InitializeComponent();
            var filePath = "file.json";
            var fileService = new FileService(filePath);
            _productService = new ProductService(fileService);
            UpdateListBox();

        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var productName = Input_ProductName.Text;
            var productPrice = Input_ProductPrice.Text;

            if (_editingProduct != null)
            {
                var response = _productService.UpdateProduct(_editingProduct.Id, productName, productPrice);

                MessageBox.Show(response.Message);

                _editingProduct = null!;
            }
            else
            {
                var newProduct = new Product
                {
                    ProductName = productName,
                    ProductPrice = productPrice,
                };

                var response = _productService.AddProduct(newProduct);

                MessageBox.Show(response.Message);
            }

            Input_ProductName.Text = "";
            Input_ProductPrice.Text = "";

            UpdateListBox();
        }

        private void UpdateListBox()
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
            var button = sender as Button;
            var product = button!.DataContext as Product;

            if (product != null)
            {
                _productService.RemoveProduct(product.Id);

                UpdateListBox();
            }   
        }



        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Product;

            if (product != null)
            {
                Input_ProductName.Text = product.ProductName;
                Input_ProductPrice.Text = product.ProductPrice;

                _editingProduct = product;
            }
 
        }





    }
}