using Shared.Models;
using Shared.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


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
            var productCategory = Input_ProductCategory.Text;

            var category = new Category { CategoryName = productCategory }; 

            if (_editingProduct != null)
            {
                var response = _productService.UpdateProduct(_editingProduct.Id, productName, productPrice, productCategory);

                MessageBox.Show(response.Message);

                _editingProduct = null!;
            }
            else
            {
                var newProduct = new Product
                {
                    ProductName = productName,
                    ProductPrice = productPrice,
                    Category = category
                };

                var response = _productService.AddProduct(newProduct);

                MessageBox.Show(response.Message);
            }

            Input_ProductName.Text = "";
            Input_ProductPrice.Text = "";
            Input_ProductCategory.Text = "";

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
                Input_ProductCategory.Text = product.Category.CategoryName;

                _editingProduct = product;
            }
 
        }





    }
}