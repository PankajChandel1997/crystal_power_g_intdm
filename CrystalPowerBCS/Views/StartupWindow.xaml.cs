using CrystalPowerBCS.Commands;
using CrystalPowerBCS.Helpers;
using CrystalPowerBCS.ViewModels;
using CrystalPowerBCS.Views.Custom_Dailog;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Enums;
using Gurux.Common;
using Gurux.DLMS;
using Gurux.Serial;
using Hardcodet.Wpf.TaskbarNotification;
using Infrastructure.API;
using Infrastructure.DTOs;
using Infrastructure.Helpers;
using Notification.Wpf;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace CrystalPowerBCS.Views
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window, Helpers.IWindow

    {
        private TaskbarIcon tb;
        private HashSet<int> usedNumbers = new HashSet<int>();
        public WindowCloseMode WindowCloseMode { get; set; }
        public int BackCount;
        public string Password = "Crystal@123";
        MeterConnectionCommand meterConnection = new MeterConnectionCommand();
        public GXSerial serial = new GXSerial();
        public MeterMethodsCommand meterMethodsCommand;
        MeterConnectionViewModel meterConnectionModel = new MeterConnectionViewModel();
        public string SelectedPort;
        public ErrorHelper _errorHelper;
        public ConsumerDetailsDto consumerDetails = new ConsumerDetailsDto();
        public int projectCode = -1;
        private static readonly Regex timeRegex = new Regex(@"^(0[1-9]|1[0-2]):[0-5][0-9]:[0-5][0-9] (AM|PM)$", RegexOptions.Compiled);
        public bool IsAuthenticated { get; set; }

        public StartupWindow()
        {
            InitializeComponent();

            WpfSingleInstance.Make(Constants.CrystalPower, uniquePerUser: false);

            MinimizeToTrayHelper.Enable(this);
            _errorHelper = new ErrorHelper();
            AddMeterTypes();

            BackCount = 0;
            meterTypeComboBox.SelectedIndex = 0;
            FetechMeterDataComboBox.SelectedIndex = 0;
        }

        private void StartupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsAuthenticated)
            {
                LoginView.Visibility = Visibility.Collapsed;
                Options.Visibility = Visibility.Visible;
            }
        }
        private void AddMeterTypes()
        {
            connectMeterTypeComboBox.Items.Clear();

            foreach (ProjectCode meterType in Enum.GetValues(typeof(ProjectCode)))
            {
                var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(
                    typeof(ProjectCode).GetField(meterType.ToString()), typeof(DescriptionAttribute));

                // If the description attribute exists, add the description to the ComboBox
                if (descriptionAttribute != null)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = descriptionAttribute.Description;
                    connectMeterTypeComboBox.Items.Add(comboBoxItem);
                }
            }
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                StartUpWindowGrid.Opacity = 0.7;
                StartUpWindowGrid.IsEnabled = false;

                View_Button.Content = "Please wait...";
                View_Button.IsEnabled = false;
                await Task.Run(() => ChangeView());
                this.Hide();

            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void ChangeView()
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                var currentWindow = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                var currentHeight = currentWindow.ActualHeight;
                var currentWidth = currentWindow.ActualWidth;
                var left = currentWindow.Left;
                var top = currentWindow.Top;
                var windowSatate = currentWindow.WindowState;

                MainWindow mainWindow = new MainWindow();
                mainWindow.Height = currentHeight;
                mainWindow.Width = currentWidth;
                mainWindow.Left = left;
                mainWindow.Top = top;
                mainWindow.WindowState = windowSatate;

                mainWindow.ShowDialog();

            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private async void ShowFetchOptions(object sender, RoutedEventArgs e)
        {

            List<string> activePorts = await Task.Run(() => meterConnection.GetComPorts());

            if (activePorts != null && activePorts.Count > 0)
            {
                comPortsComboBox.Items.Clear();
                foreach (string activePort in activePorts)
                {
                    comPortsComboBox.Items.Add(activePort);
                }
                comPortsComboBox.SelectedIndex = 0;
            }
            else
            {
                NotificationManager notificationManager = new NotificationManager();

                notificationManager.Show(Constants.Notification, Constants.UnabletoaccessComPortPleaseCheckOpticalCable, NotificationType.Error, CloseOnClick: true);
            }

            Options.Visibility = Visibility.Collapsed;
            FetchOptions.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            BackCount = BackCount + 1;
        }

        //Loop through multiple projects to make connection
        private async void Connect(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowLoader();

                if (string.IsNullOrEmpty(SelectedPort))
                {
                    NotificationManager notificationManager = new NotificationManager();

                    notificationManager.Show(Constants.Notification, Constants.PleaseSelectComPort, NotificationType.Warning, CloseOnClick: true);

                    return;
                }

                if (string.IsNullOrEmpty(MeterKeys.Active_Authentication) && string.IsNullOrEmpty(MeterKeys.Active_Encryption) && string.IsNullOrEmpty(MeterKeys.Active_Password))
                {
                    string[] keyTypes = { "GUJRAT", "Demo", "MCarlo" };

                    foreach (var item in keyTypes)
                    {

                        if (item == ProjectCode.Demo.GetDescription())
                        {
                            projectCode = (int)ProjectCode.Demo;
                            MeterKeys.Active_Authentication = MeterKeys.Demo_Authentication;
                            MeterKeys.Active_Encryption = MeterKeys.Demo_Encryption;
                            MeterKeys.Active_Password = MeterKeys.Demo_Password;
                        }
                        else if (item == ProjectCode.Gujrat.GetDescription())
                        {
                            projectCode = (int)ProjectCode.Gujrat;
                            MeterKeys.Active_Authentication = MeterKeys.GUJRAT_Authentication;
                            MeterKeys.Active_Encryption = MeterKeys.GUJRAT_Encryption;
                            MeterKeys.Active_Password = MeterKeys.GUJRAT_Password;
                        }

                        else if (item == ProjectCode.MCarlo.GetDescription())
                        {
                            projectCode = (int)ProjectCode.MCarlo;
                            MeterKeys.Active_Authentication = MeterKeys.MCarlo_Authentication;
                            MeterKeys.Active_Encryption = MeterKeys.MCarlo_Encryption;
                            MeterKeys.Active_Password = MeterKeys.MCarlo_Password;
                        }

                        if (meterConnectionModel.SerialConnected != 1)
                        {
                            meterConnectionModel = await Task.Run(() => meterConnection.ConnectMeter(SelectedPort));
                        }

                        bool response = await ConnectMeterAsync(projectCode);
                        if (response)
                        {
                            FetchOptions.Visibility = Visibility.Collapsed;
                            FetchMeterData.Visibility = Visibility.Visible;

                            BackCount = BackCount + 1;

                            break;
                        }
                    }
                }
                else
                {
                    if (meterConnectionModel.SerialConnected != 1)
                    {
                        meterConnectionModel = await Task.Run(() => meterConnection.ConnectMeter(SelectedPort));
                    }

                    bool response = await ConnectMeterAsync(projectCode);
                    if (response)
                    {
                        FetchOptions.Visibility = Visibility.Collapsed;
                        FetchMeterData.Visibility = Visibility.Visible;

                        BackCount = BackCount + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("'" + meterConnectionModel.Port.ToUpper() + "' is denied"))
                {
                    NotificationManager notificationManager = new NotificationManager();

                    notificationManager.Show(Constants.Notification, Constants.PortisengagedPleasetryagain, NotificationType.Error, CloseOnClick: true);
                }
                _errorHelper.WriteLog(DateTime.UtcNow + " : StartUpWindow : Connect : Exception ==>" + ex.Message);

                return;
            }
            finally
            {
                hideLoader();
            }
        }

        //Meter Connect Functionality
        private async Task<bool> ConnectMeterAsync(int projectCode)
        {
            try
            {
                if (meterConnectionModel != null && meterConnectionModel.IsConnected == true)
                {
                    meterMethodsCommand = new MeterMethodsCommand(meterConnectionModel, this);

                    //Getting Current Time on Meter
                    string serialNumber = await Task.Run(() => meterMethodsCommand.GetMeterTime(consumerDetails));

                    if (!string.IsNullOrEmpty(serialNumber))
                    {
                        await Task.Run(() => meterMethodsCommand.OpenConnection());

                        NotificationManager notificationManager = new NotificationManager();

                        notificationManager.Show(Constants.Notification, Constants.MeterConnected + projectCode + ", Meter Serial No :" + serialNumber, NotificationType.Success, CloseOnClick: true);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(DateTime.UtcNow + " : StartUpWindow : Connect : Exception ==>" + ex.Message);

                return false;
            }
        }

        private void ShowLoader()
        {
            Spinner.IsLoading = true;
            StartUpWindowGrid.Opacity = 0.7;
            StartUpWindowGrid.IsEnabled = false;
        }

        private void hideLoader()
        {
            StartUpWindowGrid.Opacity = 1;
            Spinner.IsLoading = false;
            StartUpWindowGrid.IsEnabled = true;
        }


        private void CloseOkBtn(object sender, RoutedEventArgs e)
        {
            FetchedMeterData.IsOpen = false;
        }

        private void CloseFetchedPopup(object sender, RoutedEventArgs e)
        {
            FetchedMeterData.IsOpen = false;
        }


        private async void ShowFetchedPopup(object sender, RoutedEventArgs e)
        {
            ShowLoader();
            bool response = false;
            ComboBoxItem SelectedItem = (ComboBoxItem)FetechMeterDataComboBox.SelectedValue;
            string value = SelectedItem.Content.ToString();

            //Getting From and To Date For filter
            DateTime? startDate = FilterFromDatePicker.SelectedDate.HasValue ? Functions.ToIndiaDateTimeObject((DateTime)FilterFromDatePicker.SelectedDate).Date : null;
            DateTime? endDate = FilterToDatePicker.SelectedDate.HasValue ? Functions.ToIndiaDateTimeObject((DateTime)FilterToDatePicker.SelectedDate).Date.AddDays(1).AddMinutes(-1) : null;

            if (value == MeterDataValueType.All.GetDescription())
            {
                response = await Task.Run(() => meterMethodsCommand.ReadAllMeterData());
            }
            else if (value == MeterDataValueType.IP.GetDescription())
            {
                response = await Task.Run(() => meterMethodsCommand.ReadIPOnly());
            }
            else if (value == MeterDataValueType.BillingProfile.GetDescription())
            {
                response = await Task.Run(() => meterMethodsCommand.ReadBillingProfileOnly(startDate, endDate));
            }
            else if (value == MeterDataValueType.BlockLoadProfile.GetDescription())
            {
                response = await Task.Run(() => meterMethodsCommand.ReadBlockLoadProfileOnly(startDate, endDate));
            }
            else if (value == MeterDataValueType.DailyLoadProfile.GetDescription())
            {
                response = await Task.Run(() => meterMethodsCommand.ReadDailyLoadProfile(startDate, endDate));
            }
            else if (value == MeterDataValueType.EventOnly.GetDescription())
            {
                response = await Task.Run(() => meterMethodsCommand.ReadEventOnly(startDate, endDate));
            }
            else if (value == MeterDataValueType.AllWithoutLoadProfile.GetDescription())
            {
                response = await Task.Run(() => meterMethodsCommand.ReadAllWithoutLoadProfile());
            }
            else if (value == MeterDataValueType.SelfDiagnostic.GetDescription())
            {
                response = await Task.Run(() => meterMethodsCommand.ReadSelfDiagnosticOnly());
            }
            else if (value == MeterDataValueType.NamePlate.GetDescription())
            {
                response = await Task.Run(() => meterMethodsCommand.ReadSelfDiagnosticOnly());
            }
            else if (value == MeterDataValueType.TOD.GetDescription())
            {
                response = await Task.Run(() => meterMethodsCommand.ReadTOD());
            }

            //Getting Current Time on Meter
            if (response)
            {
                NotificationManager notificationManager = new NotificationManager();

                notificationManager.Show(Constants.Notification, Constants.OperationCompletedSuccessFully, NotificationType.Success, CloseOnClick: true);
            }
            else
            {
                NotificationManager notificationManager = new NotificationManager();

                notificationManager.Show(Constants.Notification, Constants.SomethingWentWrongPleasetryagain, NotificationType.Error, CloseOnClick: true);
            }
            hideLoader();
        }


        private void BackButtonClick(object sender, MouseButtonEventArgs e)
        {
            if (BackCount == 2)
            {
                FetchOptions.Visibility = Visibility.Visible;
                FetchMeterData.Visibility = Visibility.Collapsed;
                meterMethodsCommand.StopKeepAliveTimer();
                _ = meterMethodsCommand.CloseConnection();
                NotificationManager notificationManager = new NotificationManager();
                notificationManager.Show(Constants.Notification, Constants.Disconnected, NotificationType.Error, CloseOnClick: true);
            }
            else if (BackCount == 1)
            {
                Options.Visibility = Visibility.Visible;
                FetchOptions.Visibility = Visibility.Collapsed;

                BackButton.Visibility = Visibility.Hidden;
            }

            BackCount = BackCount - 1;

            setMethods1.Visibility = Visibility.Collapsed;
            setMethods2.Visibility = Visibility.Collapsed;
            setMethods3.Visibility = Visibility.Collapsed;
            setMethods4.Visibility = Visibility.Collapsed;

            showSetMethods.Visibility = Visibility.Visible;
            passwordBox.Password = "";
            showSetMethodsPasswordBox.Text = "";

        }
        private async Task DisconnectMeterAsync()
        {
            ShowLoader();
            await System.Windows.Application.Current.Dispatcher.Invoke(async () =>
            {
                meterConnectionModel = await System.Threading.Tasks.Task.Run(async () => await meterConnection.DisconnectMeter());

            }, System.Windows.Threading.DispatcherPriority.Send);

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {

                System.Threading.Tasks.Task.Run(() => meterMethodsCommand.CloseConnection()).Wait();

            }, System.Windows.Threading.DispatcherPriority.Send);

            FetchMeterData.Visibility = Visibility.Collapsed;
            Options.Visibility = Visibility.Visible;

            BackButton.Visibility = Visibility.Hidden;

            BackCount = 0;

            setMethods1.Visibility = Visibility.Collapsed;
            setMethods2.Visibility = Visibility.Collapsed;
            setMethods3.Visibility = Visibility.Collapsed;
            setMethods4.Visibility = Visibility.Collapsed;

            showSetMethods.Visibility = Visibility.Visible;
            passwordBox.Password = "";
            showSetMethodsPasswordBox.Text = "";

            NotificationManager notificationManager = new NotificationManager();

            notificationManager.Show(Constants.Notification, Constants.Disconnected, NotificationType.Error, CloseOnClick: true);

            hideLoader();
        }
        public void DisconnectMeter(object sender, RoutedEventArgs e)
        {
            _ = DisconnectMeterAsync();
        }

        private void ComboOptions(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem SelectedItem = (ComboBoxItem)meterTypeComboBox.SelectedValue;
            string value = SelectedItem.Content.ToString();
            if (value == "Crystal")
            {
                PassowrdOption.Visibility = Visibility.Collapsed;
            }
            else
            {
                PassowrdOption.Visibility = Visibility.Visible;
            }
        }

        private void ShowSetParameterPopup(object sender, RoutedEventArgs e)
        {
            SetParameterPopup.IsOpen = !SetParameterPopup.IsOpen;
            StartUpWindowGrid.Opacity = 0.4;
            StartUpWindowGrid.IsEnabled = false;
        }

        private void ShowConnectDisconnectPopup(object sender, RoutedEventArgs e)
        {
            ConnectDissconnectPopup.IsOpen = !ConnectDissconnectPopup.IsOpen;
            StartUpWindowGrid.Opacity = 0.4;
            StartUpWindowGrid.IsEnabled = false;
        }

        public void SaveBillingDate(object sender, RoutedEventArgs e)
        {
            NotificationManager notificationManager = new NotificationManager();

            if (BillingDatePicker.SelectedDate != null)
            {
                try
                {
                    DateTime billingDate = BillingDatePicker.SelectedDate.Value;
                    _ = meterMethodsCommand.SetBillingDate(billingDate);

                    SetParameterPopup.IsOpen = false;
                    StartUpWindowGrid.Opacity = 1;
                    StartUpWindowGrid.IsEnabled = true;
                    notificationManager.Show(Constants.Notification, Constants.BillingDateSetSuccessfull, NotificationType.Success, CloseOnClick: true);
                }
                catch (Exception ex)
                {
                    notificationManager.Show("Error", $"An error occurred: {ex.Message}", NotificationType.Error, CloseOnClick: true);
                }
            }
            else
            {
                SetParameterPopup.IsOpen = false;
                StartUpWindowGrid.Opacity = 1;
                StartUpWindowGrid.IsEnabled = true;
                notificationManager.Show("Warning", "Date can't be null", NotificationType.Warning, CloseOnClick: true);
            }
        }
        private void CloseSetParmPopup(object sender, RoutedEventArgs e)
        {
            SetParameterPopup.IsOpen = false;
            StartUpWindowGrid.Opacity = 1;
            StartUpWindowGrid.IsEnabled = true;
        }
        private void CloseConnectDisconnectPopup(object sender, RoutedEventArgs e)
        {
            ConnectDissconnectPopup.IsOpen = false;
            StartUpWindowGrid.Opacity = 1;
            StartUpWindowGrid.IsEnabled = true;
        }

        private void ComPortChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedPort = comPortsComboBox.SelectedValue?.ToString();
        }

        // Changing Keys based on Selected Meter Type
        private void connectMeterChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem SelectedItem = (ComboBoxItem)connectMeterTypeComboBox.SelectedValue;
            string value = SelectedItem.Content.ToString();
            //if (value == ProjectCode.APDCL1.GetDescription())
            //{
            //    MeterKeys.Active_Authentication = MeterKeys.Intelli_Authentication;
            //    MeterKeys.Active_Encryption = MeterKeys.Intelli_Encryption;
            //    MeterKeys.Active_Password = MeterKeys.Intelli_Password;
            //}
            //else if (value == ProjectCode.APDCL2.GetDescription())
            //{
            //    MeterKeys.Active_Authentication = MeterKeys.APDCL2_Authentication;
            //    MeterKeys.Active_Encryption = MeterKeys.APDCL2_Encryption;
            //    MeterKeys.Active_Password = MeterKeys.APDCL2_Password;
            //}
            if (value == ProjectCode.Demo.GetDescription())
            {
                MeterKeys.Active_Authentication = MeterKeys.Demo_Authentication;
                MeterKeys.Active_Encryption = MeterKeys.Demo_Encryption;
                MeterKeys.Active_Password = MeterKeys.Demo_Password;
            }

            else if (value == ProjectCode.MCarlo.GetDescription())
            {
                MeterKeys.Active_Authentication = MeterKeys.MCarlo_Authentication;
                MeterKeys.Active_Encryption = MeterKeys.MCarlo_Encryption;
                MeterKeys.Active_Password = MeterKeys.MCarlo_Password;
            }
            else
            {
                MeterKeys.Active_Authentication = MeterKeys.GUJRAT_Authentication;
                MeterKeys.Active_Encryption = MeterKeys.GUJRAT_Encryption;
                MeterKeys.Active_Password = MeterKeys.GUJRAT_Password;
            }

            //else
            //{
            //    MeterKeys.Active_Authentication = MeterKeys.Aparva_Authentication;
            //    MeterKeys.Active_Encryption = MeterKeys.Aparva_Encryption;
            //    MeterKeys.Active_Password = MeterKeys.Aparva_Password;
            //}
        }

        private void ShowHideDateRangeFilter(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem SelectedItem = (ComboBoxItem)FetechMeterDataComboBox.SelectedValue;
            string value = SelectedItem.Content.ToString();

            if (string.IsNullOrEmpty(value))
            {
                DateRangeFilter.Visibility = Visibility.Collapsed;
                FilterFromDatePicker.SelectedDate = null;
                FilterToDatePicker.SelectedDate = null;
            }

            if (value.Contains(Constants.FetchBlockLoadProfile))
            {
                DateRangeFilter.Visibility = Visibility.Visible;

                //Setting Default Date at start
                FilterFromDatePicker.SelectedDate = Functions.ToIndiaDateTimeObject(DateTime.UtcNow);
                FilterToDatePicker.SelectedDate = Functions.ToIndiaDateTimeObject(DateTime.UtcNow.AddDays(+1));
            }
            else
            {
                DateRangeFilter.Visibility = Visibility.Collapsed;
                FilterFromDatePicker.SelectedDate = null;
                FilterToDatePicker.SelectedDate = null;
            }
        }

        private void AuthenticateUser(object sender, RoutedEventArgs e)
        {
            AuthenticateUser();
        }
        private void AuthenticateUser()
        {
            NotificationManager notificationManager = new NotificationManager();

            var passwordBox = LoginMeterPasswordHidden;
            var VisiblePassword = LoginMeterPasswordVisible.Text;
            if (passwordBox.Password == Password || VisiblePassword == Password)
            {
                LoginView.Visibility = Visibility.Collapsed;
                Options.Visibility = Visibility.Visible;

                notificationManager.Show(Constants.Notification, Constants.Login, NotificationType.Success, CloseOnClick: true);
            }
            else
            {
                notificationManager.Show(Constants.Notification, Constants.IncorrectPassword, NotificationType.Warning, CloseOnClick: true);
            }
        }

        private void AuthenticateUser(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AuthenticateUser();
            }
        }

        private void AddCustomerDetails(object sender, RoutedEventArgs e)
        {
            CustomerDetailsPopup.IsOpen = true;
        }
        private void CloseCustomerPopup(object sender, RoutedEventArgs e)
        {
            consumerDetails = new ConsumerDetailsDto()
            {
                ConsumerNo = string.Empty,
                ConsumerName = string.Empty,
                ConsumerAddress = string.Empty
            };

            CustomerDetailsPopup.IsOpen = false;
        }

        private void ConsumerSubmit(object sender, RoutedEventArgs e)
        {
            consumerDetails = new ConsumerDetailsDto()
            {
                ConsumerNo = ConsumerNo.Text,
                ConsumerName = ConsumerName.Text,
                ConsumerAddress = ConsumerAddress.Text
            };

            CustomerDetailsPopup.IsOpen = false;
        }

        private void ShowPassword(object sender, MouseButtonEventArgs e)
        {
            LoginMeterPasswordHidden.Visibility = Visibility.Hidden;
            LoginMeterPasswordVisible.Text = LoginMeterPasswordHidden.Password;
            LoginMeterPasswordVisible.Visibility = Visibility.Visible;
            showPasswordIcon.Visibility = Visibility.Hidden;
            hidePasswordIcon.Visibility = Visibility.Visible;
        }

        private void HidePassword(object sender, MouseButtonEventArgs e)
        {
            LoginMeterPasswordHidden.Visibility = Visibility.Visible;
            LoginMeterPasswordHidden.Password = LoginMeterPasswordVisible.Text;
            LoginMeterPasswordVisible.Visibility = Visibility.Hidden;
            showPasswordIcon.Visibility = Visibility.Visible;
            hidePasswordIcon.Visibility = Visibility.Hidden;
        }

        private void ConnectMeterCommand(object sender, RoutedEventArgs e)
        {
            _= meterMethodsCommand.SendConnectCommand();
        }

        private void DisconnectMeterCommand(object sender, RoutedEventArgs e)
        {
            _= meterMethodsCommand.SendDisconnectCommand();
        }

        private async void MDResetSetPopup(object sender, RoutedEventArgs e)
        {
            NotificationManager notificationManager = new NotificationManager();

            var confirmationDialog = new ConfirmationDialogWindow();
            confirmationDialog.Owner = this; // Set the owner to the main window
            confirmationDialog.ShowDialog();

            if (confirmationDialog.DialogResult)
            {
                bool res = await meterMethodsCommand.MDResetSet();
                if (res)
                {
                    notificationManager.Show(Constants.Notification, "MD reset Successful", NotificationType.Success, CloseOnClick: true);
                }
                else
                {
                    notificationManager.Show(Constants.Notification, "Something went wrong please try again", NotificationType.Error, CloseOnClick: true);
                }
            }
        }

        private void ShowSetRTCPopup(object sender, RoutedEventArgs e)
        {
            SetRTCPopup.IsOpen = !SetRTCPopup.IsOpen;
            StartUpWindowGrid.Opacity = 0.4;
            StartUpWindowGrid.IsEnabled = false;
        }

        private void SetRTC(object sender, RoutedEventArgs e)
        {
            NotificationManager notificationManager = new NotificationManager();

            if (SetRTCDatePicker.SelectedDateTime != null)
            {
                try
                {
                    DateTime billingDate = SetRTCDatePicker.SelectedDateTime.Value;
                    _ = meterMethodsCommand.SetRTC(billingDate);

                    SetRTCPopup.IsOpen = false;
                    StartUpWindowGrid.Opacity = 1;
                    StartUpWindowGrid.IsEnabled = true;
                    notificationManager.Show(Constants.Notification, Constants.RTCDateSetSuccessfull, NotificationType.Success, CloseOnClick: true);
                }
                catch (Exception ex)
                {
                    notificationManager.Show("Error", $"An error occurred: {ex.Message}", NotificationType.Error, CloseOnClick: true);
                }
            }
            else
            {
                SetRTCPopup.IsOpen = false;
                StartUpWindowGrid.Opacity = 1;
                StartUpWindowGrid.IsEnabled = true;
                notificationManager.Show("Warning", "Date can't be null", NotificationType.Warning, CloseOnClick: true);
            }
        }

        private void CloseSetRTCPopup(object sender, RoutedEventArgs e)
        {
            SetRTCPopup.IsOpen = false;
            StartUpWindowGrid.Opacity = 1;
            StartUpWindowGrid.IsEnabled = true;
        }

        private void ShowSetTODPopup(object sender, RoutedEventArgs e)
        {
            SetTod.IsOpen = !SetTod.IsOpen;
            StartUpWindowGrid.Opacity = 0.4;
            StartUpWindowGrid.IsEnabled = false;
        }

        private async void SetTOD(object sender, RoutedEventArgs e)
        {
            ShowLoader();
            try
            {
                List<SetTodViewModel> setTod = new List<SetTodViewModel>();

                // Add checks for Id and Time not null or empty before adding to the model
                if (!string.IsNullOrWhiteSpace(Id1.Text) && Time1.SelectedDateTime != null)
                {
                    setTod.Add(new SetTodViewModel
                    {
                        Id = Convert.ToUInt16(Id1.Text),
                        TimeSpan = DateTime.Parse(Time1.SelectedDateTime.ToString()).TimeOfDay
                    });
                }

                if (!string.IsNullOrWhiteSpace(Id2.Text) && Time2.SelectedDateTime != null)
                {
                    setTod.Add(new SetTodViewModel
                    {
                        Id = Convert.ToUInt16(Id2.Text),
                        TimeSpan = DateTime.Parse(Time2.SelectedDateTime.ToString()).TimeOfDay
                    });
                }

                if (!string.IsNullOrWhiteSpace(Id3.Text) && Time3.SelectedDateTime != null)
                {
                    setTod.Add(new SetTodViewModel
                    {
                        Id = Convert.ToUInt16(Id3.Text),
                        TimeSpan = DateTime.Parse(Time3.SelectedDateTime.ToString()).TimeOfDay
                    });
                }

                if (!string.IsNullOrWhiteSpace(Id4.Text) && Time4.SelectedDateTime != null)
                {
                    setTod.Add(new SetTodViewModel
                    {
                        Id = Convert.ToUInt16(Id4.Text),
                        TimeSpan = DateTime.Parse(Time4.SelectedDateTime.ToString()).TimeOfDay
                    });
                }

                if (!string.IsNullOrWhiteSpace(Id5.Text) && Time5.SelectedDateTime != null)
                {
                    setTod.Add(new SetTodViewModel
                    {
                        Id = Convert.ToUInt16(Id5.Text),
                        TimeSpan = DateTime.Parse(Time5.SelectedDateTime.ToString()).TimeOfDay
                    });
                }

                if (!string.IsNullOrWhiteSpace(Id6.Text) && Time6.SelectedDateTime != null)
                {
                    setTod.Add(new SetTodViewModel
                    {
                        Id = Convert.ToUInt16(Id6.Text),
                        TimeSpan = DateTime.Parse(Time6.SelectedDateTime.ToString()).TimeOfDay
                    });
                }

                if (!string.IsNullOrWhiteSpace(Id7.Text) && Time7.SelectedDateTime != null)
                {
                    setTod.Add(new SetTodViewModel
                    {
                        Id = Convert.ToUInt16(Id7.Text),
                        TimeSpan = DateTime.Parse(Time7.SelectedDateTime.ToString()).TimeOfDay
                    });
                }

                if (!string.IsNullOrWhiteSpace(Id8.Text) && Time8.SelectedDateTime != null)
                {
                    setTod.Add(new SetTodViewModel
                    {
                        Id = Convert.ToUInt16(Id8.Text),
                        TimeSpan = DateTime.Parse(Time8.SelectedDateTime.ToString()).TimeOfDay
                    });
                }

                DateTime activationDate = ActivationDatePicker.SelectedDateTime ?? DateTime.UtcNow;

                bool res = await meterMethodsCommand.SetTOD(setTod, activationDate);

                CloseSetTodPopup(null, null);

                hideLoader();
            }
            catch (Exception ex)
            {
                hideLoader();
            }
        }

        private void TimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numbers, colons, space, and letters APM
            if (!char.IsDigit(e.Text, e.Text.Length - 1) &&
                e.Text != ":" &&
                e.Text != " " &&
                !"APMapm".Contains(e.Text))
            {
                e.Handled = true;
            }
        }

        private void TimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;
            string text = textBox.Text;

            if (!timeRegex.IsMatch(text))
            {
                textBox.Text = CorrectInput(text);
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void TimeTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; // Prevent space key except for a valid position
            }
        }

        private string CorrectInput(string input)
        {
            // This method will correct input to match the desired format as much as possible
            string result = input;
            result = Regex.Replace(result, @"[^0-9:AMPamp ]", "");
            result = result.ToUpper();
            return result;
        }

        private void CloseSetTodPopup(object sender, RoutedEventArgs e)
        {
            SetTod.IsOpen = false;
            StartUpWindowGrid.Opacity = 1;
            StartUpWindowGrid.IsEnabled = true;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            VerifyTodPassowrd();
        }

        private void AuthenticateTod(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                VerifyTodPassowrd();
            }
        }

        private void VerifyTodPassowrd()
        {
            string enteredPassword = passwordBox.Password;
            string textBoxPassowrd = showSetMethodsPasswordBox.Text;

            NotificationManager notificationManager = new NotificationManager();

            // Do something with the entered password, like validate it or perform an action.
            // For example:
            if (enteredPassword == "Kimbal@321" || textBoxPassowrd == "Kimbal@321")
            {
                // Password is correct, do something here.
                setMethods1.Visibility = Visibility.Visible;
                setMethods2.Visibility = Visibility.Visible;
                setMethods3.Visibility = Visibility.Visible;
                setMethods4.Visibility = Visibility.Visible;

                showSetMethods.Visibility = Visibility.Collapsed;

                notificationManager.Show(Constants.Notification, "Success", NotificationType.Success, CloseOnClick: true);
            }
            else
            {
                // Password is incorrect, handle the error here.
                setMethods1.Visibility = Visibility.Collapsed;
                setMethods2.Visibility = Visibility.Collapsed;
                setMethods3.Visibility = Visibility.Collapsed;
                setMethods4.Visibility = Visibility.Collapsed;

                showSetMethods.Visibility = Visibility.Visible;

                notificationManager.Show(Constants.Notification, "Incorrect Password", NotificationType.Error, CloseOnClick: true);
            }

            // Close the popup after submitting the password
            showSetOptions.IsOpen = false;
            StartUpWindowGrid.Opacity = 1;
            StartUpWindowGrid.IsEnabled = true;

            passwordBox.Password = "";
            showSetMethodsPasswordBox.Text = "";
        }

        private void ShowSetMethodPopup(object sender, RoutedEventArgs e)
        {
            showSetOptions.IsOpen = !showSetOptions.IsOpen;
            StartUpWindowGrid.Opacity = 0.4;
            StartUpWindowGrid.IsEnabled = false;
        }

        private void CloseShowSetParmPopup(object sender, RoutedEventArgs e)
        {
            showSetOptions.IsOpen = false;
            StartUpWindowGrid.Opacity = 1;
            StartUpWindowGrid.IsEnabled = true;
        }

        private void SetShowPassword(object sender, MouseButtonEventArgs e)
        {
            passwordBox.Visibility = Visibility.Hidden;
            showSetMethodsPasswordBox.Text = passwordBox.Password;
            showSetMethodsPasswordBox.Visibility = Visibility.Visible;
            setshowPasswordIcon.Visibility = Visibility.Hidden;
            sethidePasswordIcon.Visibility = Visibility.Visible;
        }

        private void SetHidePassword(object sender, MouseButtonEventArgs e)
        {
            passwordBox.Visibility = Visibility.Visible;
            passwordBox.Password = showSetMethodsPasswordBox.Text;
            showSetMethodsPasswordBox.Visibility = Visibility.Hidden;
            setshowPasswordIcon.Visibility = Visibility.Visible;
            sethidePasswordIcon.Visibility = Visibility.Hidden;
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow only numbers
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = (System.Windows.Controls.TextBox)sender;
            if (int.TryParse(textBox.Text, out int num))
            {
                // Remove previous number from usedNumbers
                if (int.TryParse(textBox.Tag?.ToString(), out int previousNumber))
                {
                    usedNumbers.Remove(previousNumber);
                }

                if (num < 1 || num > 8 || usedNumbers.Contains(num))
                {
                    textBox.Text = "";
                    textBox.Tag = null;
                }
                else
                {
                    usedNumbers.Add(num);
                    textBox.Tag = num;
                }
            }
            else
            {
                if (int.TryParse(textBox.Tag?.ToString(), out int previousNumber))
                {
                    usedNumbers.Remove(previousNumber);
                }
                textBox.Tag = null;
            }

            // Clear other TextBoxes if they contain the same number
            foreach (var item in new System.Windows.Controls.TextBox[] { Id1, Id2, Id3, Id4, Id5, Id6, Id7, Id8 })
            {
                if (item != textBox && item.Text == textBox.Text)
                {
                    item.Text = "";
                }
            }

        }
    }
}
