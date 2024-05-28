using DalsheBogaNet.Mvvm.Model;
using DalsheBogaNet.Mvvm.View;
using System.Collections.ObjectModel;
using System.Windows;

namespace DalsheBogaNet.Mvvm.ViewModel
{
    public class ListProductsVM : BaseVM
    {
        private ProductZapolnenie productZapolnenie;
        private string searchText = "";
        private ObservableCollection<Product> products;
        private ObservableCollection<Product> productsInfo;


        public VmCommand Create { get; set; }
        public VmCommand Edit { get; set; }
        public VmCommand Delete { get; set; }
        public VmCommand Sms { get; set; }


        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }

        public Product SelectedProduct { get; set; }
        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                products = value;
                Signal();
            }
        }
        public ObservableCollection<Product> ProductsInfo
        {
            get => products;
            set
            {
                products = value;
                Signal();
            }
        }


        public ListProductsVM()
        {
            string sql = "SELECT i.Tovar_ID, i.Name, i.Description, i.Price, i.Size, i.Breakable, i.Postavhik, i.Amount, i.Code, c.Time FROM izdelia i, Codes c";

            Products = new ObservableCollection<Product>(ProductZapolnenie.Instance.GetAllProducts(sql));


            Create = new VmCommand(() =>
            {
                MainVM.Instance.CurrentPage = new EditorProduct();
            });
            Sms = new VmCommand(() =>
            {
                MainVM.Instance.CurrentPage = new Sms();
            });
            Edit = new VmCommand(() =>
            {
                if (SelectedProduct == null)
                    return;
                else
                {
                    MainVM.Instance.CurrentPage = new EditorProduct( SelectedProduct);
                }
            });
            Delete = new VmCommand(() =>
            {
                if (SelectedProduct == null)
                    return;
                if (MessageBox.Show("Удалить изделие?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ProductZapolnenie.Instance.Remove(SelectedProduct);
                    Products.Remove(SelectedProduct);
                }
            });
        }
        private void Search()
        {
            Products = new ObservableCollection<Product>(
                ProductZapolnenie.Instance.Search(SearchText));

        }
        
    }
}
