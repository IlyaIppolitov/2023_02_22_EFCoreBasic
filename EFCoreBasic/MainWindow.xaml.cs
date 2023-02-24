using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace EFCoreBasic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void buttonFillDataInTables_Click(object sender, RoutedEventArgs e)
        {
            await using var db = new AppDbContext();
            // Проверка подключения к базе данных
            try
            {
                if (!await db.Database.CanConnectAsync())
                {
                    MessageBox.Show("База данных недоступна!");
                }
            }
            catch (OperationCanceledException ex) { 
                MessageBox.Show($"Ошибка подключения к базе данных {db.Database.ProviderName}: " + ex.Message.ToString());}

            // Заполнение данных
            for (int i = 0; i < 20; i++)
                await db.Students.AddAsync(new Student() { Name = Faker.Name.First(), Surname = Faker.Name.Last(), Birthday = RandomDay() });

            foreach (var student in db.Students)
                await db.Visitations.AddAsync(new Visitation() { StudentId = student.Id, Date = new DateOnly(2022, 02, 22) });

            foreach (var student in db.Students)
                if (student.Id %4 != 0)
                    await db.Visitations.AddAsync(new Visitation() { StudentId = student.Id, Date = DateOnly.FromDateTime(DateTime.Now) });
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) { MessageBox.Show("Ошибка! Данные были изменены с момента их загрузки в память!" + ex.Message); }
            catch (DbUpdateException ex) { MessageBox.Show("Ошибка сохранения в базу данных: " + ex.Message); }
        }

        private async void buttonDeleteDb_Click(object sender, RoutedEventArgs e)
        {
            await using var db = new AppDbContext();
            // Удаляем базу данных 
            try
            {
                await db.Database.EnsureDeletedAsync();
            }
            catch(OperationCanceledException ex) 
            {
                MessageBox.Show($"Ошибка удаления базы данных {db.Database.ProviderName}: " + ex.Message.ToString());
            }
        }

        private async void buttonCreateDb_Click(object sender, RoutedEventArgs e)
        {
            await using var db = new AppDbContext();
            // Создаём базу данных 
            try
            {
                await db.Database.EnsureCreatedAsync();
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show($"Ошибка создания базы данных {db.Database.ProviderName}: " + ex.Message.ToString());
            }
        }

        private async void buttonShowStudents_Click(object sender, RoutedEventArgs e)
        {
            await UpdateStudentsInDataGrid();
        }

        private Random gen = new Random();
        DateOnly RandomDay()
        {
            DateTime start = new DateTime(1980,1,1);
            int range = (new DateTime(1990,1,1) - start).Days;
            return DateOnly.FromDateTime(start.AddDays(gen.Next(range)));
        }

        private void buttonNewStudent_Click(object sender, RoutedEventArgs e)
        {

            NewStudentWindow newStudentWindow = new NewStudentWindow();
            newStudentWindow.NewStudentWindowClosed += newStWin_Closed;

            newStudentWindow.Owner = this;

            newStudentWindow.Show();
        }

        private async void newStWin_Closed (object sender, EventArgs e)
        {
            await UpdateStudentsInDataGrid();
        }

        private async Task UpdateStudentsInDataGrid()
        {
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
            try
            {
                var students = await db.Students.ToListAsync();
                studentsDataGrid.ItemsSource = students;
            }
            catch (ArgumentNullException ex) { MessageBox.Show("Отсутствует необходимая таблица для отображения\n" + ex.Message); }
        }

        private void buttonFixSelection_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция в разработке!");
        }

        private void buttonShowVisitsByDate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция в разработке!");
        }
    }
}
