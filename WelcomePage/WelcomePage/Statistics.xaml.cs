using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

using System.Threading.Tasks;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WelcomePage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated sto within a Frame.
    /// </summary>
    /// 

    public sealed partial class Statistics : Page
    {

        // There will be Azure module
        //  private MobileServiceCollection<ToDoItem, ToDoItem> items;

        public Statistics()
        {
            this.InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserId), null);
        }
    }
}
