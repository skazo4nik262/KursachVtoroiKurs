﻿using DalsheBogaNet.Mvvm.Model;
using DalsheBogaNet.Mvvm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalsheBogaNet.Mvvm.ViewModel
{
    internal class EditorProductVM : BaseVM
    {
        private Product product = new();

        public Product Product
        {
            get => product;
            set
            {
                product = value;
                Signal();
            }
            
        }
        public VmCommand Save {  get; set; }

        public EditorProductVM()
        {
            Save = new VmCommand(() =>
            {
                if (Product.ID == 0)
                { 
                    ProductZapolnenie.Instance.AddProduct(Product);
                }
                else
                {
                    ProductZapolnenie.Instance.UpdateProduct(Product);
                }

                MainVM.Instance.CurrentPage = new ListProducts();
            });
        }

        internal void SetEditProduct(Product selectedProduct)
        {
            Product = selectedProduct;
        }
    }
}
