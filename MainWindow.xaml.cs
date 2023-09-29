using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyConvertor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         Root val = new Root();

        public MainWindow()
        {
            InitializeComponent();
            ClearControls();
            GetValue();
        }

        private async void GetValue()
        {
            val = await GetData<Root>("https://openexchangerates.org/api/latest.json?app_id=0967e237d09142ea9a9d6292c3ad01bb");
            BindCurrency();
        }
        public async Task<Root> GetData<T>(string url)
        {
            var myRoot = new Root();
            try
            {
                using(var client = new HttpClient()) 
                    //Provides a class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
                {
                    client.Timeout =TimeSpan.FromMinutes(1);
                    HttpResponseMessage response = await client.GetAsync(url);
                    
                    if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<Root>(responseString);

                        //MessageBox.Show("License: " + responseObject.license, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return responseObject;
                    }
                    return myRoot;
                }
            }
            catch (Exception)
            {
                return myRoot;
            }

        }

        #region Bind Currency From and To comboBox
        private void BindCurrency()
        {
            DataTable dtCurrency = new DataTable();
            // adding columns in the dataTable
            dtCurrency.Columns.Add("Text");
            dtCurrency.Columns.Add("Value");

            //adding rows in the DataTable
            dtCurrency.Rows.Add("--SELECT--", 0);
            dtCurrency.Rows.Add("UGX", val.rates.UGX);
            dtCurrency.Rows.Add("USD", val.rates.USD);
            dtCurrency.Rows.Add("EUR", val.rates.EUR);
            dtCurrency.Rows.Add("GBP", val.rates.GBP);
            dtCurrency.Rows.Add("RWF", val.rates.RWF);
            dtCurrency.Rows.Add("KEH", val.rates.KES);
            dtCurrency.Rows.Add("PLN", val.rates.PLN);
            dtCurrency.Rows.Add("TZS", val.rates.TZS);
            dtCurrency.Rows.Add("AED", val.rates.AED);
            dtCurrency.Rows.Add("CAD", val.rates.CAD);
            dtCurrency.Rows.Add("CHF", val.rates.CHF);
            dtCurrency.Rows.Add("CNY", val.rates.CNY);

            //for the from comb box
            cmbFromCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbFromCurrency.DisplayMemberPath = "Text";
            cmbFromCurrency.SelectedValuePath = "Value";
            cmbFromCurrency.SelectedIndex = 0;

            //for the to comb box
            cmbToCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedValuePath = "Value";
            cmbToCurrency.SelectedIndex = 0;

        }
        #endregion
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            //convertedValue variable to store currency converted value
            double convertedValue;

            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == string.Empty)
            {
                //if textBox is null or blank it will show this messageBox
                MessageBox.Show("Please enter the currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //after clicking ok on the message box the focus is set on amount textBox
                txtCurrency.Focus();
                return;
            }
            // else if the form currency is not selected
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
            {
                //show the  message
                MessageBox.Show("Please select the from currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //set focus to the from ComboBox
                cmbFromCurrency.Focus();
                return;
            }
            // else if the to currency is not selected
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0)
            {
                //show the  message
                MessageBox.Show("Please select the to currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //set focus to the from ComboBox
                cmbToCurrency.Focus();
                return;
            }

            // checking if the from and to comboBox selected values are the same
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                //amount textbox value is set to be the ConvertedValue
                //double.parse is converting txtCurrency string into a double
                convertedValue = double.Parse(txtCurrency.Text);
                //showing the convertedValue at the lblCurrency label and ToString("N3") is used to add 3 zeros after a period
                IbICurrency.Content = cmbToCurrency.Text + " " + convertedValue.ToString("N3");
            }
            else
            {
                convertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString())) * double.Parse(txtCurrency.Text)
                    / double.Parse(cmbFromCurrency.SelectedValue.ToString());
                // showing the convertedValue and converted currency name at the lblCurrency label
                IbICurrency.Content = cmbToCurrency.Text + " " + convertedValue.ToString("N3");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        /// <summary>
        /// NumberValidationTextBox method is used to restrict user input in a text box
        /// to numeric characters only by preventing the entry of non-numeric characters and ensuring 
        /// that the input adheres to the specified pattern.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0)
            {
                cmbFromCurrency.SelectedIndex = 0;
            }

            if (cmbToCurrency.Items.Count > 0)
            {
                cmbToCurrency.SelectedIndex = 0;
            }

            IbICurrency.Content = string.Empty;
            txtCurrency.Focus();

        }
    }
}
