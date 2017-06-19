using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //IMobileServiceTableQuery<TodoItem> query = App.usersTable. ;
            
            List<TodoItem> list = await App.usersTable.ToListAsync();
            CompareMy cmp = new CompareMy();
            list.Sort(cmp);
            int size = list.Count();
            int toScreen = (size < 4 ? size : 4);
            var strBuilder = new StringBuilder();
            for (int i = 0; i < toScreen; i++)
            {
                strBuilder.Append(list[i].Nickname).Append(" ").Append(list[i].Time).Append("\n");
            }
            List.Text = strBuilder.ToString();
        }

        

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserId), null);
        }
    }

    
}
