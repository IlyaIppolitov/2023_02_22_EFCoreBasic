using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace EFCoreBasic
{
    /// <summary>
    /// Логика взаимодействия для NewStudent.xaml
    /// </summary>
    public partial class NewStudentWindow : Window
    {
        public event EventHandler MyEvent;

        protected void OnMyEvent()
        {
            if (this.MyEvent != null)
                this.MyEvent(this, EventArgs.Empty);
        }

        public NewStudentWindow()
        {
            InitializeComponent();
        }

        private async void buttonCreateSt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textboxName.Text))
            {
                MessageBox.Show("Заполните поле Имя!");
                return;
            }
            if (string.IsNullOrWhiteSpace(textboxSurname.Text))
            {
                MessageBox.Show("Заполните поле Фамилия!");
                return;
            }
            if (!calendar.SelectedDate.HasValue)
            {
                MessageBox.Show("Введите дату!");
                return;
            }
            if ((DateTime.Now - calendar.SelectedDate.Value).TotalDays < 365 * 12 )
            {
                MessageBox.Show("Слишком юн, чтобы учиться у нас!");
                return;
            }

            await using var db = new AppDbContext();
            // Проверка подключения к базе данных
            try
            {
                if (!await db.Database.CanConnectAsync())
                {
                    MessageBox.Show("База данных недоступна!");
                }
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных {db.Database.ProviderName}: " + ex.Message.ToString());
            }

            // Добавление данных
            await db.Students.AddAsync(new Student() { Name = textboxName.Text, Surname = textboxSurname.Text, Birthday = DateOnly.FromDateTime(calendar.SelectedDate.Value) }); ;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) { MessageBox.Show("Ошибка! Данные были изменены с момента их загрузки в память!" + ex.Message); }
            catch (DbUpdateException ex) { MessageBox.Show("Ошибка сохранения в базу данных: " + ex.Message); }

            Close();
        }

        public event EventHandler NewStudentWindowClosed;

        private void Window_Closed(object sender, EventArgs e)
        {
            if (NewStudentWindowClosed != null)
            {
                NewStudentWindowClosed(this, EventArgs.Empty);
            }
        }
    }
}
