using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Inventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HideEntryElements();

            ClearFields();

            RefreshGridView();
            PopulateDropdown();

            PrepareButtons("load");
            ShowPopulateButton();
            ShowPopulateInventoryButton();
            ShowListViewElements();
        }

        private void ShowPopulateButton()
        {
            if (CSql.CountRecords("tblTypes") > 0)
            {
                BtnPopulate.Visibility = Visibility.Hidden;
            }
            else
            {
                BtnPopulate.Visibility = Visibility.Visible;
            }

        }

        private void ShowPopulateInventoryButton()
        {
            if(CSql.CountRecords("tblItems") > 0)
            {
                BtnPopulateInventory.Visibility = Visibility.Hidden;
            } else
            {
                BtnPopulateInventory.Visibility = Visibility.Visible;
            }
        }

        private void RefreshGridView()
        {
            string sql = "SELECT * FROM viewInventory";
            DataTable dataTable = CSql.GetDataTable(sql);
            if (dataTable != null)
            {
                try
                {
                    lvInventory.ItemsSource = null;
                    lvInventory.Items.Clear();
                    List<Product> items = new List<Product>();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        items.Add(new Product()
                        {
                            Id = dataTable.Rows[i]["Id"].ToString(),
                            Item = dataTable.Rows[i]["Item"].ToString(),
                            TypeId = Convert.ToInt32(dataTable.Rows[i]["TypeId"]),
                            Type = dataTable.Rows[i]["Type"].ToString(),
                            SellIn = (Convert.ToInt32(dataTable.Rows[i]["TypeId"]) > 0) ? dataTable.Rows[i]["SellIn"].ToString() : "",
                            Quality = (Convert.ToInt32(dataTable.Rows[i]["TypeId"]) > 0) ? dataTable.Rows[i]["Quality"].ToString() : ""
                        });
                    }
                    lvInventory.ItemsSource = items;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace.ToString());
                }
            }
        }

        private string CheckValue(int typeId,int value)
        {
            if(typeId > 0)
            {
                return value.ToString();
            }
            return "";
        }

        private void ShowEntryElements()
        {
            lblEntry.Visibility = Visibility.Visible;
            txtItem.Visibility = Visibility.Visible;
            cmbType.Visibility = Visibility.Visible;
            txtSell.Visibility = Visibility.Visible;
            TxtQuality.Visibility = Visibility.Visible;
            lblItem.Visibility = Visibility.Visible;
            lblType.Visibility = Visibility.Visible;
            lblSell.Visibility = Visibility.Visible;
            lblQuality.Visibility = Visibility.Visible;
            lblError.Visibility = Visibility.Visible;
        }

        private void HideEntryElements()
        {
            lblEntry.Visibility = Visibility.Hidden;
            txtItem.Visibility = Visibility.Hidden;
            cmbType.Visibility = Visibility.Hidden;
            txtSell.Visibility = Visibility.Hidden;
            TxtQuality.Visibility = Visibility.Hidden;
            lblItem.Visibility = Visibility.Hidden;
            lblType.Visibility = Visibility.Hidden;
            lblSell.Visibility = Visibility.Hidden;
            lblQuality.Visibility = Visibility.Hidden;
            lblError.Visibility = Visibility.Hidden;
        }

        private void ShowListViewElements()
        {
            lvInventory.Visibility = Visibility.Visible;
        }

        private void PrepareButtons(string buttonCall)
        {
            switch (buttonCall)
            {
                case "load":
                    BtnAdd.Visibility = Visibility.Visible;
                    BtnClose.Visibility = Visibility.Visible;
                    BtnCancel.Visibility = Visibility.Hidden;
                    BtnSave.Visibility = Visibility.Hidden;
                    BtnInventory.Visibility = Visibility.Hidden;
                    BtnProgress.Visibility = Visibility.Visible;
                    BtnDelete.Visibility = Visibility.Hidden;
                    break;

                case "add":
                    BtnAdd.Visibility = Visibility.Hidden;
                    BtnCancel.Visibility = Visibility.Visible;
                    BtnSave.Visibility = Visibility.Visible;
                    BtnInventory.Visibility = Visibility.Hidden;
                    BtnProgress.Visibility = Visibility.Hidden;
                    BtnDelete.Visibility = Visibility.Hidden;
                    break;

                case "cancel":
                    BtnAdd.Visibility = Visibility.Visible;
                    BtnCancel.Visibility = Visibility.Hidden;
                    BtnSave.Visibility = Visibility.Hidden;
                    BtnInventory.Visibility = Visibility.Hidden;
                    BtnProgress.Visibility = Visibility.Visible;
                    BtnDelete.Visibility = Visibility.Hidden;
                    break;

                case "save":
                    break;

                case "inventory":
                    BtnAdd.Visibility = Visibility.Visible;
                    BtnInventory.Visibility = Visibility.Hidden;
                    BtnProgress.Visibility = Visibility.Visible;
                    BtnCancel.Visibility = Visibility.Hidden;
                    BtnSave.Visibility = Visibility.Hidden;
                    BtnDelete.Visibility = Visibility.Hidden;
                    break;

                case "delete":
                    BtnAdd.Visibility = Visibility.Visible;
                    BtnInventory.Visibility = Visibility.Hidden;
                    BtnProgress.Visibility = Visibility.Visible;
                    BtnCancel.Visibility = Visibility.Hidden;
                    BtnSave.Visibility = Visibility.Hidden;
                    BtnDelete.Visibility = Visibility.Hidden;
                    break;

                default:
                    break;

            }
        }

        private void ClearFields()
        {
            lblId.Content = "";
            txtItem.Text = "";
            cmbType.SelectedIndex = -1;
            txtSell.Text = "";
            TxtQuality.Text = "";
            lblEntry.Content = "";
        }

        private void PopulateDropdown()
        {
            string sql = "SELECT * FROM tblTypes WHERE TypeValue > 0 ORDER BY Type ASC";
            DataTable dataTable = CSql.GetDataTable(sql);
            if (dataTable != null)
            {
                try                   
                {
                    ClearDropDown();
                    cmbType.ItemsSource = dataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace.ToString());
                }
            }
        }

        private void PopulateError(string error)
        {
            lblError.Content = error;
        }

        private string CheckInput(string item, int sellin, int quality)
        {
            string error = "";
            if (item.Equals(""))
            {
                error = "Item Content Required; ";
            }
            if (sellin == -1)
            {
                error = $"{error}Invalid value for Sell In; ";
            }
            if (quality == -1)
            {
                error = $"{error}Invalid value for Quality";
            }
            return error;
        }

        private string CleanText(string textValue)
        {
            return textValue.Trim();
        }

        private int CheckValue(int value, int min, int max)
        {

            if (value >= min && value <= max)
            {
                return value;
            }
            return -1;
        }

        private int CheckValue(string textValue)
        {
            int value;
            try
            {
                value = Convert.ToInt32(textValue);
            }
            catch
            {
                value = -1;
            }
            return value;
        }

        private void UpdateRecord(string type,int value,int quality)
        {

        }

        private void TxtQuality_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]{1,2}");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //lblType.Content = cmbType.SelectedValue.ToString();
                //cmbType.SelectedItem.ToString(); cmbType.SelectedValuePath.ToString()
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
            }
        }

        private void txtSell_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]{1,2}");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void lvInventory_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PopulateDropdown();
            try
            {
                Product lvi = (Product)lvInventory.SelectedItems[0];
                lblId.Content = lvi.Id;
                txtItem.Text = lvi.Item;
                cmbType.Text = lvi.Type.ToString();
                TxtQuality.Text = lvi.Quality.ToString();
                txtSell.Text = lvi.SellIn.ToString();
                txtItem.Focus();

                ShowEntryElements();
                PrepareButtons("add");
                BtnDelete.Visibility = Visibility.Visible;
            }
            catch
            {
                ClearFields();
                RefreshGridView();
            }
        }

        private  void ClearDropDown()
        {
            try
            {
                cmbType.ItemsSource = null;
                cmbType.Items.Clear();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        private int GetSelectedID()
        {
            try
            {
                return Convert.ToInt32(lblId.Content);
            } catch
            {
                return 0;
            }
        }





        private void BtnPopulate_Click(object sender, RoutedEventArgs e)
        {
            //Populate Types Table with Data
            SqlConnection conn = CSql.GetConnection();
            if (conn != null)
            {
                try
                {
                    CSql.InsertTypeRecord(conn, "NO SUCH ITEM", 0);
                    CSql.InsertTypeRecord(conn, "Aged Brie", 1);
                    CSql.InsertTypeRecord(conn, "Frozen Item", 2);
                    CSql.InsertTypeRecord(conn, "Soap", 3);
                    CSql.InsertTypeRecord(conn, "Fresh Item", 4);
                    CSql.InsertTypeRecord(conn, "Christmas Crackers", 5);
                    CSql.InsertTypeRecord(conn, "INVALID ITEM", 6);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace.ToString());
                }
            }

            ShowPopulateButton();
            PopulateDropdown();
        }

        private void BtnPopulateInventory_Click(object sender, RoutedEventArgs e)
        {
            //Populate Items Table with Data
            SqlConnection conn = CSql.GetConnection();
            if (conn != null)
            {
                try
                {
                    CSql.InsertItemRecord(conn, "Cheese", 1, 1, 1);
                    CSql.InsertItemRecord(conn, "Bargain Crackers", 5, -1, 2);
                    CSql.InsertItemRecord(conn, "Luxury Crackers", 5, 9, 2);
                    CSql.InsertItemRecord(conn, "Dove", 3, 2, 2);
                    CSql.InsertItemRecord(conn, "Fish Fingers", 2, -1, 55);
                    CSql.InsertItemRecord(conn, "Ice Cream", 2, 2, 2);
                    CSql.InsertItemRecord(conn, "Washing Machine", 6, 2, 2);
                    CSql.InsertItemRecord(conn, "Bread", 4, 2, 2);
                    CSql.InsertItemRecord(conn, "Meat", 4, -1, 5);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace.ToString());
                }
            }

            ShowPopulateInventoryButton();
            RefreshGridView();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int id = GetSelectedID();                
            if(id > 0)
            {
                CSql.DeleteRecord("tblItems", id);
                RefreshGridView();
                ClearFields();
                PrepareButtons("delete");
                HideEntryElements();
            }

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = CSql.GetConnection();
            if (conn != null)
            {
                CSql.ClearTable(conn, "tblItems");
                CSql.ClearTable(conn, "tblTypes");
            }
            RefreshGridView();
            ClearFields();
            ClearDropDown();
            HideEntryElements();
            ShowPopulateButton();
            ShowPopulateInventoryButton();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            ShowEntryElements();
            PopulateDropdown();
            txtItem.Focus();
            PrepareButtons("add");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            HideEntryElements();
            PrepareButtons("cancel");
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string item = CleanText(txtItem.Text);
            int type = CheckValue(cmbType.SelectedValue.ToString());
            int sellin = CheckValue(txtSell.Text);
            int quality = CheckValue(CheckValue(TxtQuality.Text), 0, 55);
            string error = CheckInput(item, sellin, quality);
            if (error.Equals(""))
            {
                int id = GetSelectedID();
                if (id > 0)
                {
                    sql = $"UPDATE tblItems SET Item='{item}',Type='{type}',SellIn='{sellin}',Quality='{quality}' WHERE Id = '{id}';";
                }
                else
                {
                    sql = $"INSERT INTO tblItems ([Item],[Type],[SellIn],[Quality]) VALUES ('{item}','{type}','{sellin}','{quality}');";
                }
                CSql.ExecuteCommand(sql);
                PopulateError(error);
                ClearFields();
                PrepareButtons("save");
            }
            else
            {
                PopulateError(error);
            }
            RefreshGridView();
        }

        private void BtnInventory_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            PrepareButtons("inventory");
            HideEntryElements();
            RefreshGridView();
            ShowListViewElements();
        }

        private void BtnProgress_Click(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM tblItems";
            DataTable dataTable = CSql.GetDataTable(sql);
            if (dataTable != null)
            {
                SqlConnection conn = CSql.GetConnection();
                if (conn != null)
                {
                    CheckRecords(conn,dataTable);

                    RefreshGridView();
                    CSql.CloseConnection();
                }
            }
        }

        private void CheckRecords(SqlConnection conn,DataTable dataTable)
        {
            int id;
            int type;
            int sell;
            int quality;
            int qualityChange;
            foreach (DataRow row in dataTable.Rows)
            {
                id = CheckValue(row["Id"].ToString());
                if (id > 0)
                {
                    type = CheckValue(row["Type"].ToString());
                    quality = CheckValue(row["Quality"].ToString());
                    sell = CheckValue(row["SellIn"].ToString());

                    qualityChange = (sell < 0) ? -2 : -1;
                    switch (type)
                    {
                        case 1:
                            //Brie
                            sell -= 1;
                            quality = (quality >= 50) ? 50 : (quality + 1);
                            CSql.UpdateItemRecord(conn, type, sell, quality, id);
                            break;

                        case 2:
                            //Frozen Item
                            sell -= 1;
                            quality = (quality + qualityChange < 0) ? 0 : ((quality + qualityChange) > 50 ? 50 : (quality + qualityChange));
                            CSql.UpdateItemRecord(conn, sell, quality, id);
                            break;

                        case 3:
                            //soap
                            break;

                        case 4:
                            //fresh item
                            sell -= 1;
                            qualityChange *= 2;
                            quality = (quality + qualityChange < 0) ? 0 : ((quality + qualityChange) > 50 ? 50 : (quality + qualityChange));
                            CSql.UpdateItemRecord(conn, sell, quality, id);
                            break;

                        case 5:
                            //Christmas Crackers
                            sell -= 1;
                            if (sell <= 0)
                            {
                                quality = 0;
                            }
                            else if (sell <= 5)
                            {
                                quality += 3;
                            }
                            else if (sell <= 10)
                            {
                                quality += 2;
                            }
                            CSql.UpdateItemRecord(conn, sell, quality, id);
                            break;

                        case 6:
                            //Invalid Item
                            CSql.UpdateItemRecord(conn, 0, id);
                            break;

                    }

                }
            }
        }


        public class Product
        {
            public string Id { get; set; }
            public string Item { get; set; }
            public int TypeId { get; set; }
            public string Type { get; set; }
            public string SellIn { get; set; }
            public string Quality { get; set; }

        }

    }

}
