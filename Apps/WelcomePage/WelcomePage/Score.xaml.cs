using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WelcomePage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Score : Page
    {
        public Score()
        {
            this.InitializeComponent();

            //int minuites = Convert.ToInt32 (App.CurrentTime / 60);
            //int seconds = Convert.ToInt32 (App.CurrentTime % 60);
            //var strBuilder = new StringBuilder();
            //strBuilder.Append(minuites.ToString()).Append(":").Append(seconds.ToString());
            //string timeToShow = strBuilder.ToString();
            Time.Text = TimeUtils.CurrentTimeToShow();
            //this.AddUser();
            //this.GetTime();
        }
        //private async void AddUser()
        //{
        //     TodoItem todoItem = new TodoItem { Nickname = App.CurrentNick, Time = App.CurrentTime };
        //     await InsertTodoItem(todoItem);
        //}

        //private async void GetTime() 
        //{
        //    //var usersCurrent = 
        //    //    await App.usersTable.Where(u => u.Nickname == App.CurrentNick).ToListAsync();
        //    //IMobileServiceTableQuery<TodoItem> query = App.usersTable.Where(u => u.Nickname == App.CurrentNick);
        //    try
        //    {
        //        //var list = await query.ToListAsync();
        //        //var user = list.First();
        //        //double time = user.Time;
        //        //Time.Text = time.ToString();


        //        //var names = await App.usersTable.Where(u => u.Nickname == App.CurrentNick)
        //        //                                .Select(u => u.Time)
        //        //                                .ToEnumerableAsync();
        //        //double time = names.FirstOrDefault();
        //        //Time.Text = time.ToString();


        //        // Danny
        //        var dataUser = App.users.FirstOrDefault(user => user.Nickname == App.CurrentNick && user.Time == App.CurrentTime);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Exception Message: {0}", ex.Message);
        //    }
        //}

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserId), null);
        }

        //private async Task InsertTodoItem(TodoItem todoItem)
        //{
        //    // This code inserts a new TodoItem into the database. After the operation completes
        //    // and the mobile app backend has assigned an id, the item is added to the CollectionView.
        //    await App.usersTable.InsertAsync(todoItem);
        //    //App.users.Add(todoItem);
        //}
    }
}
