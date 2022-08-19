using MVVMDBdemo.Commands;
using MVVMDBdemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMDBdemo.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        NorthwindEntities db = null;
        private Product objmod = new Product();
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
      
       // private int productId { get; set; }
        public int ProductId
        {
            get { return objmod.ProductID; }
            set
            {
                objmod.ProductID = value;
                OnPropertyChanged("ProductId");
            }
        }
        private string productName { get; set; }
        public string ProductName
        {
            get { return objmod.ProductName; }
            set
            {
                objmod.ProductName = value;
                OnPropertyChanged("ProductName");
            }
        }
       // private decimal unitPrice;
        public decimal UnitPrice
        {
            get { return Convert.ToInt32(objmod.UnitPrice); }
            set
            {
                objmod.UnitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }
        public ObservableCollection<Product> productList;
        public ObservableCollection<Product> ProductList
        {
            get { return productList; }
            set { productList = value;
                 OnPropertyChanged("ProductList");
                }
        }

        public ProductViewModel()
        {
            db = new NorthwindEntities();
            ProductList = new ObservableCollection<Product>();

            loadcommand = new RelayCommand(LoadProducts);
            clearcommand = new RelayCommand(clearcommandMethod);
            searchcommand = new RelayCommand(SearchProduct, DisableSearch);
            addcommand = new RelayCommand(Addproduct, PropertyIsNotNull);
            updatecommand = new RelayCommand(UpdateProduct,PropertyIsNotNull);
            deletecommand = new RelayCommand(DeleteProduct, ProductIdlessThanZero);
        }
        public void LoadProducts(object obj)
        {
            var data = db.Products.ToList();
            ProductList = new ObservableCollection<Product>(data);
            if(ProductList.Count>0)
            {
                ProductId = Convert.ToInt32(ProductList[0].ProductID.ToString());
                ProductName = ProductList[0].ProductName;
                UnitPrice = Convert.ToDecimal(ProductList[0].UnitPrice.ToString());
            }
        }
        public void clearcommandMethod(object obj)
        {
            this.ProductId = 0;
            this.ProductName = null;
            this.UnitPrice = 0;
            this.ProductList.Clear();
        }

        public void SearchProduct(object obj)
        {
            var data = db.Products.Select(i => i.ProductID == ProductId);
            if (data!=null)
            {
              var query1 =db.Products.Where(e => e.ProductID == ProductId).Select(e => new { e.ProductID, e.ProductName, e.UnitPrice}).ToList();
               foreach(var i in query1)
                {
                    ProductId = i.ProductID;
                    ProductName = i.ProductName;
                    UnitPrice =Convert.ToDecimal(i.UnitPrice);
                }         
            }
            else
            {
                MessageBox.Show("Record not found");
            }
        }
        public void Addproduct(object obj)
        {
            objmod.ProductID = ProductId;
            objmod.ProductName = ProductName;
            objmod.UnitPrice = UnitPrice;
            var query = db.Products.Add(objmod);
           int i= db.SaveChanges();
            if (i > 0)
            {
                MessageBox.Show("Record added Successfully");
            }
        }

        public void UpdateProduct(object obj)
        {
             Product objmod = db.Products.SingleOrDefault(e => e.ProductID == ProductId);
            if (objmod != null)
            {
                objmod.ProductName = ProductName;
                objmod.UnitPrice = UnitPrice;
                db.SaveChanges();
                 MessageBox.Show("Record Updated Successfully");
            }
            else
            {
                MessageBox.Show("Record not found");
            }
        }

        public void DeleteProduct(object obj)
        {
            var data = db.Products.SingleOrDefault(e => e.ProductID == ProductId);
            if (data != null)
            {
                db.Products.Remove(data);
               int i= db.SaveChanges();
                if (i > 0)
                {
                    MessageBox.Show("Record deleted Successfully");
                }
            }
            else
            {
                MessageBox.Show("Record not found");
            }
        }

        public bool ProductIdlessThanZero(object obj)
        {
            if(ProductId>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool PropertyIsNotNull(object obj)
        {
            if(ProductName!=null && ProductId>0 && UnitPrice>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DisableSearch(object obj)
        {
            if(ProductId>0 && ProductName==string.Empty && UnitPrice==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private ICommand loadcommand;
        public ICommand LoadCommand
        {
            get { return loadcommand; }
        }

        private ICommand clearcommand;
        public ICommand ClearCommand
        {
            get { return clearcommand; }
        }

        private ICommand searchcommand;
        public ICommand SearchCommand
        {
            get { return searchcommand; }
        }

        private ICommand addcommand;
        public ICommand AddCommand
        {
            get { return addcommand; }
        }

        private ICommand updatecommand;
        public ICommand UpdateCommand
        {
            get { return updatecommand; }
        }

        private ICommand deletecommand;
        public ICommand DeleteCommand
        {
            get { return deletecommand; }
        }
    }
}
