using AutoLotModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

namespace Suciu_Bogdan_Lab7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource customerViewSource;
        CollectionViewSource inventoryViewSource;
        CollectionViewSource customerOrdersViewSource;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource.Source = ctx.Customers.Local;
            ctx.Customers.Load();
            
            inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource.Source = ctx.Inventories.Local;
            ctx.Inventories.Load();

            customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            customerOrdersViewSource.Source = ctx.Orders.Local;
            ctx.Orders.Load();

            cmbCustomers.ItemsSource = ctx.Customers.Local;
            cmbCustomers.DisplayMemberPath = "FirstName";
            cmbCustomers.SelectedValuePath = "CustId";
            cmbInventory.ItemsSource = ctx.Inventories.Local;
            cmbInventory.DisplayMemberPath = "Make";
            cmbInventory.SelectedValuePath = "CarId";

            btnSave_Inventory.IsEnabled = false;
            btnCancel_Inventory.IsEnabled = false;
            btnCancel_Orders.IsEnabled = false;
            btnSave_Orders.IsEnabled = false;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    customer = new Customer()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();

                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
             
            }
            else
            if (action == ActionState.Edit)
            {
               
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                customerViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                customerViewSource.View.MoveCurrentTo(customer);

            }
            else
            if (action == ActionState.Delete)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();

                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";

            Keyboard.Focus(firstNameTextBox);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnSave.IsEnabled = true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            MessageBox.Show("Alegeti din lista un client, apoi apasati Save pentru a sterge");
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            btnNew.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnEdit.IsEnabled = false;
        }

        private void btnSave_Inventory_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Inventory entity
                    inventory = new Inventory()
                    {
                        Color = colorTextBox.Text.Trim(),
                        Make = makeTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Inventories.Add(inventory);
                    inventoryViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();

                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            if (action == ActionState.Edit)
            {

                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    inventory.Make = makeTextBox.Text.Trim();
                    inventory.Color = colorTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                inventoryViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                inventoryViewSource.View.MoveCurrentTo(inventory);

            }
            else
            if (action == ActionState.Delete)
            {
                try
                {
                    inventory = (Inventory)customerDataGrid.SelectedItem;
                    ctx.Inventories.Remove(inventory);
                    ctx.SaveChanges();

                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
            }
        }

        private void btnNew_Inventory_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew_Inventory.IsEnabled = true;
            btnDelete_Inventory.IsEnabled = false;
            btnEdit_Inventory.IsEnabled = false;
            btnSave_Inventory.IsEnabled = true;
            btnCancel_Inventory.IsEnabled = true;

            colorTextBox.Text = "";
            makeTextBox.Text = "";

            Keyboard.Focus(colorTextBox);
        }

        private void btnCancel_Inventory_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew_Inventory.IsEnabled = true;
            btnEdit_Inventory.IsEnabled = true;
            btnDelete_Inventory.IsEnabled = true;
            btnSave_Inventory.IsEnabled = false;
            btnCancel_Inventory.IsEnabled = false;
        }

        private void btnEdit_Inventory_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            btnNew_Inventory.IsEnabled = false;
            btnDelete_Inventory.IsEnabled = false;
            btnEdit_Inventory.IsEnabled = true;
            btnSave_Inventory.IsEnabled = true;
            btnCancel_Inventory.IsEnabled = true;
        }

        private void btnDelete_Inventory_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            btnNew_Inventory.IsEnabled = false;
            btnDelete_Inventory.IsEnabled = true;
            btnEdit_Inventory.IsEnabled = false;
            btnSave_Inventory.IsEnabled = true;
            btnCancel_Inventory.IsEnabled = true;
        }

        private void btnSave_Orders_Click(object sender, RoutedEventArgs e)
        {
            Order order = null;
            if (action == ActionState.New)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory inventory = (Inventory)cmbInventory.SelectedItem;
                    //instantiem Order entity
                    order = new Order()
                    {

                        CustId = customer.CustId,
                        CarId = inventory.CarId
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Orders.Add(order);
                    customerOrdersViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            if (action == ActionState.Edit)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory inventory = (Inventory)cmbInventory.SelectedItem;

                    order = (Order)ordersDataGrid.SelectedItem;
                    order.CustId = customer.CustId;
                    order.CarId = inventory.CarId;
                  
                    //salvam modificarile
                    ctx.SaveChanges();
                    customerOrdersViewSource.View.Refresh();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            if (action == ActionState.Delete)
            {
                try
                {
                    order = (Order)ordersDataGrid.SelectedItem;
                    ctx.Orders.Remove(order);
                    ctx.SaveChanges();

                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerOrdersViewSource.View.Refresh();
            }

        }

        private void btnNew_Orders_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew_Orders.IsEnabled = true;
            btnEdit_Orders.IsEnabled = false;
            btnDelete_Orders.IsEnabled = false;

            btnCancel_Orders.IsEnabled = true;
            btnSave_Orders.IsEnabled = true;
        }

        private void btnCancel_Orders_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew_Orders.IsEnabled = true;
            btnEdit_Orders.IsEnabled = true;
            btnDelete_Orders.IsEnabled = true;

            btnCancel_Orders.IsEnabled = false;
            btnSave_Orders.IsEnabled = false;
        }

        private void btnEdit_Orders_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            btnNew_Orders.IsEnabled = false;
            btnEdit_Orders.IsEnabled = true;
            btnDelete_Orders.IsEnabled = false;

            btnCancel_Orders.IsEnabled = true;
            btnSave_Orders.IsEnabled = true;
        }

        private void btnDelete_Orders_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            btnNew_Orders.IsEnabled = false;
            btnEdit_Orders.IsEnabled = false;
            btnDelete_Orders.IsEnabled = true;

            btnCancel_Orders.IsEnabled = true;
            btnSave_Orders.IsEnabled = true;
        }
    }
}
