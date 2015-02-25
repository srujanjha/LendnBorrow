using LendnBorrow.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Contacts;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace LendnBorrow
{
    public sealed partial class PivotPage : Page
    {

        private readonly NavigationHelper navigationHelper;

        public PivotPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            add(false);
        }
        int n=1;
        private IList<Contact> contacts;
        private async void Contact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var contactPicker = new Windows.ApplicationModel.Contacts.ContactPicker();
                contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);
                this.contacts = await contactPicker.PickContactsAsync();
                add(true);
            }
            catch (Exception eq) { }
        }
        private void add(bool flag)
        {
            if (flag)
            {
                n += contacts.Count;
                foreach (var i in contacts)
                {
                    InputScope inputScope = new InputScope();
                    InputScopeName inputScopeName = new InputScopeName();
                    inputScopeName.NameValue = InputScopeNameValue.NumberFullWidth;
                    inputScope.Names.Add(inputScopeName);
            
                   // TextBox f = new TextBox() { Text = i.DisplayName };
                    TextBox txt1 = new TextBox() {  Text = i.DisplayName, FontSize = 25 };
                    TextBox txt2 = new TextBox() { PlaceholderText = "0", InputScope = inputScope };
                    Prices.Children.Add(txt2);
                    //TextBox f1 = new TextBox() { Text="0", InputScope=inputScope };
                    Stacks.Children.Add(txt1);
                    //Prices.Children.Add(f1);
                }
            }
            else
            {
                InputScope inputScope = new InputScope();
                    InputScopeName inputScopeName = new InputScopeName();
                    inputScopeName.NameValue = InputScopeNameValue.NumberFullWidth;
                    inputScope.Names.Add(inputScopeName);
                    n++;
                    TextBox f = new TextBox() { PlaceholderText = "Friend " + n };
                    Stacks.Children.Add(f);
                    TextBox f1 = new TextBox() { PlaceholderText = "0",InputScope = inputScope };
                    Prices.Children.Add(f1);
            }
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Stacks.Children.Count <= 1) return;
                if (Prices.Children.Count <= 1) return;
                Stacks.Children.RemoveAt(Stacks.Children.Count - 1);
                Prices.Children.RemoveAt(Prices.Children.Count - 1);
                n = Stacks.Children.Count - 1;
            }
            catch (Exception) { }
        }
        List<string> names=new List<string>();
        List<double> price=new List<double>();
        List<string> lent = new List<string>();
        List<double> price1 = new List<double>();
        List<string> borrowed = new List<string>();
        List<double> price2 = new List<double>();
        
        private async void fill()
        {
            names.Clear();
            price.Clear();
            borrowed.Clear();
            price1.Clear();
            lent.Clear();
            price2.Clear();
            Lent.Children.Clear();
            Borrowed.Children.Clear();
            Amount.Children.Clear();
            Amount1.Children.Clear();
            All.Children.Clear();
            Amount3.Children.Clear();
            try
            {
                StorageFolder localFolder = KnownFolders.DocumentsLibrary;
                StorageFile storageFile = await localFolder.CreateFileAsync("Transactions.csv", CreationCollisionOption.OpenIfExists);
                Stream readStream = await storageFile.OpenStreamForReadAsync();
                using (StreamReader reader = new StreamReader(readStream))
                {
                    n = Convert.ToInt16(reader.ReadLine());
                    for (int i = 0; i < n; i++)
                    {
                        string s = reader.ReadLine();
                        names.Add(s.Substring(0, s.IndexOf(",")));
                        price.Add(Convert.ToDouble(s.Substring(s.IndexOf(",") + 1)));
                        TextBlock o1 = new TextBlock() { Text=names[i],FontSize=20 };
                        TextBlock o2 = new TextBlock() { FontSize = 20 };
                        if (price[i] > 0) { 
                            lent.Add(names[i]); price2.Add(price[i]); o2.Text = price[i].ToString();
                            Lent.Children.Add(o1); Amount.Children.Add(o2);
                        }
                        else { borrowed.Add(names[i]); price1.Add(-price[i]); o2.Text = (-price[i]).ToString();
                        Borrowed.Children.Add(o1); Amount1.Children.Add(o2);
                        }
                        TextBlock o3 = new TextBlock() { Text = names[i], FontSize = 20 };
                        TextBlock o4 = new TextBlock() {Text=price[i].ToString(), FontSize = 20 };
                        All.Children.Add(o3); Amount3.Children.Add(o4);
                    }
                    total.Text = "Total: " + price.Sum();
                }

            }
            catch (Exception eq) { }
        }
        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            
                try
                {
                    Button bt = sender as Button;
                    for (int i = 1; i < Stacks.Children.Count; i++)
                    {
                        TextBox ob = (TextBox)Stacks.Children.ElementAt(i);
                        TextBox ob1 = (TextBox)Prices.Children.ElementAt(i);
                        double amt=0;
                        Double.TryParse(ob1.Text,out amt);
                        int k = names.IndexOf(ob.Text);
                        if (k != -1)
                        {
                            if (bt.Tag.Equals("1"))
                                price[k] += Convert.ToDouble(amt);
                            else price[k] -= Convert.ToDouble(amt);
                        }
                        else
                        {
                            names.Add(ob.Text); price.Add(Convert.ToDouble(amt));
                        }
                    }
                    for (int i = 0; i < price.Count; i++)
                    {
                        if (price[i] == 0)
                        { price.RemoveAt(i); names.RemoveAt(i);}
                    }
                    StorageFolder localFolder = KnownFolders.DocumentsLibrary;
                    StorageFile storageFile = await localFolder.CreateFileAsync("Transactions.csv", CreationCollisionOption.OpenIfExists);
                    Stream writeStream = await storageFile.OpenStreamForWriteAsync();
                    using (StreamWriter writer = new StreamWriter(writeStream))
                    {
                        writer.WriteLine(names.Count);
                        try
                        {
                            for (int i = 0; i < price.Count; i++)
                            {
                                writer.Write(names.ElementAt(i) + "," +price.ElementAt(i) + Environment.NewLine);
                            }
                        }
                        catch (Exception) { }
                    }
                    if (bt.Tag.Equals("1")) pivot.SelectedIndex = 1;
                    else pivot.SelectedIndex = 2;
                }
                catch (Exception eq) { }
                
        }
        
        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            fill();
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           fill();
        }
    }
}
