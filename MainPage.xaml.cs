using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using ImportTradeFiles.Classes;
using System.Windows.Browser;

namespace ImportTradeFiles
{
    public partial class MainPage : UserControl
    {
        #region Variables

        OpenFileDialog _dlg = new OpenFileDialog();
        bool _multipleFilesSelected = false;
        List<ITF> _multipleFilesList = new List<ITF>();
        List<ITF> _bindList = new List<ITF>();
        List<ITF> _delList = new List<ITF>();
        List<ITF> _concatList = new List<ITF>();
        ITF i = new ITF();
        #endregion

        public MainPage()
        {
            InitializeComponent();
            HtmlPage.RegisterScriptableObject("SilverlightPlugin", this); //Register the Scriptable Member for the communication between
                                                                         //  Sivlerlight and Javascript
        }

       
        #region Methods

        public List<string[]> parseCSV(FileStream path) // Parse CSV data and return a List of array of strings
        {
            List<string[]> _parsedData = new List<string[]>();

            try
            {
                using (StreamReader _readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = _readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        _parsedData.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


            return _parsedData;

           
        }

        public List<ITF> ConvertToITF(List<string[]> _list)  // Add the CSV data to the Object of ITF Class
        {
            var i = new List<ITF>();

            foreach (string[] _item in _list)
            {
                i.Add(new ITF()
                {
                    TRNO = _item[0].ToString(),
                    Account = _item[1].ToString(),
                    Quantity = _item[2].ToString(),
                    SecuritySymbol = _item[3].ToString(),
                    Price = _item[4].ToString(),
                    TimeLimit = _item[5].ToString(),
                    SpecialCondition = _item[6].ToString(),
                    DivRei = _item[7].ToString(),
                    BidPrice = _item[8].ToString(),
                    PriceTime = _item[9].ToString(),
                    EstimatedOrder = _item[10].ToString(),
                    MessageCode = _item[11].ToString(),
                    SwapToFunds = _item[12].ToString(),
                    TransFee = _item[13].ToString(),
                    AccountName = _item[14].ToString(),
                    NewMoney = _item[15].ToString()

                });
            }
            return i;

        }

        #endregion 
       
        #region Events

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
           
            _dlg.Filter = "csv|*.csv";

            if (_multipleFilesSelected == false)

                _dlg.Multiselect = false;
            else 
                _dlg.Multiselect = true;


                if ((bool)_dlg.ShowDialog())
                {
                    dgImportCsv.ItemsSource = null;
                    _bindList.Clear();
                    DateTime _date = DateTime.Now;
                    foreach (FileInfo file in (_dlg.Files).AsEnumerable().Reverse()) //Loop Through the List of Multiple Files Selected
                    {
                        FileStream a = file.OpenRead();
                       
                        if (_multipleFilesSelected == true && (_dlg.Files.Count()>1)) // Display Selected file name in the Textbox
                            txtSelectedFileName.Text = _dlg.Files.Count().ToString() + " " + "Files selected";
                        else
                            txtSelectedFileName.Text = _dlg.File.Name;
                       
                           
                            if (_multipleFilesSelected == false)
                                _bindList = ConvertToITF(parseCSV(a));//Call the convert to ITF function to convert array of strings to ITF object
                            else
                                _bindList = ConvertToITF(parseCSV(a)).Concat(_bindList).ToList();
                               
                    }
                   
                    dgImportCsv.ItemsSource = _bindList; //Bind List to the Datagrid
                    
                   
                    DateTime _date1 = DateTime.Now;
                    TimeSpan _dateT = (_date1 - _date);
                    _dateT.ToString();
                    tblTimeTaken.Text = _dateT.ToString();

                }
            }
        
        private void hypEdit_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton hyp = sender as HyperlinkButton;
            ITF _selectedRow = hyp.DataContext as ITF;

            i = _selectedRow;// Assign it to i to access in the scriptable member Method
            
            _delList.Add(_selectedRow);

            List<string> _sendData = new List<string>();
            _sendData.Add(_selectedRow.TRNO);
            _sendData.Add(_selectedRow.Account);
            _sendData.Add(_selectedRow.Quantity);
            _sendData.Add(_selectedRow.SecuritySymbol);
            _sendData.Add(_selectedRow.Price);
            _sendData.Add(_selectedRow.TimeLimit);
            _sendData.Add(_selectedRow.SpecialCondition);
            _sendData.Add(_selectedRow.DivRei);
            _sendData.Add(_selectedRow.BidPrice);
            _sendData.Add(_selectedRow.PriceTime);
            _sendData.Add(_selectedRow.EstimatedOrder);
            _sendData.Add(_selectedRow.MessageCode);
            _sendData.Add(_selectedRow.SwapToFunds);
            _sendData.Add(_selectedRow.TransFee);
            _sendData.Add(_selectedRow.AccountName);
            _sendData.Add(_selectedRow.NewMoney);

            HtmlPage.Window.Invoke("ShowOverlay", _sendData); // Call the Javascript function to show the Overlay
            
        }
        
        [ScriptableMember]
        public void EditedData(string Sec, string quan)
        {

            _concatList.Add(new ITF()
                {
                    TRNO = i.TRNO,
                    Account = i.Account,
                    Quantity = quan,
                    SecuritySymbol = Sec,
                    Price = i.Price,
                    TimeLimit = i.TimeLimit,
                    SpecialCondition = i.SpecialCondition,
                    DivRei = i.DivRei,
                    BidPrice = i.BidPrice,
                    PriceTime = i.PriceTime,
                    EstimatedOrder = i.EstimatedOrder,
                    MessageCode = i.MessageCode,
                    SwapToFunds = i.SwapToFunds,
                    TransFee = i.TransFee,
                    AccountName = i.AccountName,
                    NewMoney = i.NewMoney
                });

            _bindList.AddRange(_concatList);
            dgImportCsv.ItemsSource = null;
            dgImportCsv.ItemsSource = _bindList;
            _concatList.Clear();

            foreach (var item in _delList)
            {
                _bindList.Remove(item); // Remove the selected row from the datagrid

            }
            _delList.Clear();

        }

        private void chkMutipleFiles_Checked(object sender, RoutedEventArgs e)
        {
            _dlg.Multiselect = true;
            _multipleFilesSelected = true;
           
        }

        private void chkMutipleFiles_Unchecked(object sender, RoutedEventArgs e)
        {
            _dlg.Multiselect = false;
            _multipleFilesSelected = false;
        }

        #endregion