﻿using DalsheBogaNet.Mvvm.Model;
using DalsheBogaNet.Mvvm.ViewModel;
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

namespace DalsheBogaNet.Mvvm.View
{
    /// <summary>
    /// Логика взаимодействия для EditorProduct.xaml
    /// </summary>
    public partial class EditorProduct : Page
    {
        public EditorProduct()
        {
            InitializeComponent();
        }
        public EditorProduct(Product selectedProduct) : this()
        {
            ((EditorProductVM)DataContext).SetEditProduct(selectedProduct);
        }
    }
}
