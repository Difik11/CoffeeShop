using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CoffeeShop.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace CoffeeShop
{
    public partial class AdminWindow : Window
    {
        private Employee _currentEmployee;

        public AdminWindow(Employee employee)
        {
            InitializeComponent();
            _currentEmployee = employee;
            LoadData();
            LoadFilters();
        }

        private void LoadData()
        {
            LoadProducts();
            LoadEmployees();
            LoadCoffeehouses();
            LoadReportFilters();
        }

        private void LoadProducts(string search = "", string category = "")
        {
            using (var db = new AppDbContext())
            {
                var query = db.Products
                    .Include(p => p.Coffeehouse)
                    .AsQueryable();

                foreach (var p in query)
                {
                    p.Category ??= string.Empty;
                    p.Unit ??= string.Empty;
                    p.Name ??= string.Empty;
                }

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(p => p.Name.Contains(search));

                if (!string.IsNullOrEmpty(category) && category != "Все")
                    query = query.Where(p => p.Category == category);

                ProductsGrid.ItemsSource = query.ToList();
            }
        }

        private void LoadEmployees()
        {
            using (var db = new AppDbContext())
            {
                EmployeesGrid.ItemsSource = db.Employees
                    .Include(e => e.Role)
                    .Include(e => e.Coffeehouse)
                    .ToList();
            }
        }

        private void LoadCoffeehouses()
        {
            using (var db = new AppDbContext())
            {
                CoffeehousesGrid.ItemsSource = db.Coffeehouses.ToList();
            }
        }

        private void LoadFilters()
        {
            using (var db = new AppDbContext())
            {
                var categories = db.Products
                    .Select(p => p.Category)
                    .Distinct()
                    .ToList();
                categories.Insert(0, "Все");
                CategoryFilter.ItemsSource = categories;
                CategoryFilter.SelectedIndex = 0;
            }
        }

        private void LoadReportFilters()
        {
            MonthComboBox.ItemsSource = new List<string>
            {
                "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
            };
            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;

            var years = Enumerable.Range(2023, 5).ToList();
            YearComboBox.ItemsSource = years;
            YearComboBox.SelectedItem = DateTime.Now.Year;
        }

        // Поиск и фильтрация товаров
        private void ProductSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string category = CategoryFilter.SelectedItem?.ToString() ?? "";
            LoadProducts(ProductSearchBox.Text, category);
        }

        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string category = CategoryFilter.SelectedItem?.ToString() ?? "";
            LoadProducts(ProductSearchBox.Text, category);
        }

        private void ResetProductFilter_Click(object sender, RoutedEventArgs e)
        {
            ProductSearchBox.Text = "";
            CategoryFilter.SelectedIndex = 0;
            LoadProducts();
        }

        // CRUD Товары
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProductDialog();
            if (dialog.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    db.Products.Add(dialog.Product);
                    db.SaveChanges();
                }
                LoadProducts();
                LoadFilters();
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selected)
            {
                var dialog = new ProductDialog(selected);
                if (dialog.ShowDialog() == true)
                {
                    using (var db = new AppDbContext())
                    {
                        db.Products.Update(dialog.Product);
                        db.SaveChanges();
                    }
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования");
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selected)
            {
                if (MessageBox.Show($"Удалить товар «{selected.Name}»?", "Подтверждение",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (var db = new AppDbContext())
                    {
                        db.Products.Remove(db.Products.Find(selected.Id));
                        db.SaveChanges();
                    }
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления");
            }
        }

        // CRUD Сотрудники
        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EmployeeDialog();
            if (dialog.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    db.Employees.Add(dialog.Employee);
                    db.SaveChanges();
                }
                LoadEmployees();
            }
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesGrid.SelectedItem is Employee selected)
            {
                var dialog = new EmployeeDialog(selected);
                if (dialog.ShowDialog() == true)
                {
                    using (var db = new AppDbContext())
                    {
                        db.Employees.Update(dialog.Employee);
                        db.SaveChanges();
                    }
                    LoadEmployees();
                }
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для редактирования");
            }
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesGrid.SelectedItem is Employee selected)
            {
                if (MessageBox.Show($"Удалить сотрудника «{selected.FullName}»?", "Подтверждение",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (var db = new AppDbContext())
                    {
                        db.Employees.Remove(db.Employees.Find(selected.Id));
                        db.SaveChanges();
                    }
                    LoadEmployees();
                }
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для удаления");
            }
        }

        // CRUD Кофейни
        private void AddCoffeehouse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CoffeehouseDialog();
            if (dialog.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    db.Coffeehouses.Add(dialog.Coffeehouse);
                    db.SaveChanges();
                }
                LoadCoffeehouses();
            }
        }

        private void EditCoffeehouse_Click(object sender, RoutedEventArgs e)
        {
            if (CoffeehousesGrid.SelectedItem is Coffeehouse selected)
            {
                var dialog = new CoffeehouseDialog(selected);
                if (dialog.ShowDialog() == true)
                {
                    using (var db = new AppDbContext())
                    {
                        db.Coffeehouses.Update(dialog.Coffeehouse);
                        db.SaveChanges();
                    }
                    LoadCoffeehouses();
                }
            }
            else
            {
                MessageBox.Show("Выберите кофейню для редактирования");
            }
        }

        private void DeleteCoffeehouse_Click(object sender, RoutedEventArgs e)
        {
            if (CoffeehousesGrid.SelectedItem is Coffeehouse selected)
            {
                if (MessageBox.Show($"Удалить кофейню «{selected.Name}»?", "Подтверждение",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (var db = new AppDbContext())
                    {
                        db.Coffeehouses.Remove(db.Coffeehouses.Find(selected.Id));
                        db.SaveChanges();
                    }
                    LoadCoffeehouses();
                }
            }
            else
            {
                MessageBox.Show("Выберите кофейню для удаления");
            }
        }

        // Выгрузка в Excel
        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (MonthComboBox.SelectedIndex < 0 || YearComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите месяц и год");
                return;
            }

            int month = MonthComboBox.SelectedIndex + 1;
            int year = (int)YearComboBox.SelectedItem;

            using (var db = new AppDbContext())
            {
                var sales = db.Sales
                    .Include(s => s.Product)
                    .Include(s => s.Coffeehouse)
                    .Where(s => s.SaleDate.Month == month && s.SaleDate.Year == year)
                    .ToList();

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var sheet = package.Workbook.Worksheets.Add("Продажи");

                    sheet.Cells[1, 1].Value = "Дата";
                    sheet.Cells[1, 2].Value = "Товар";
                    sheet.Cells[1, 3].Value = "Количество";
                    sheet.Cells[1, 4].Value = "Кофейня";

                    for (int i = 0; i < sales.Count; i++)
                    {
                        sheet.Cells[i + 2, 1].Value = sales[i].SaleDate.ToString("dd.MM.yyyy");
                        sheet.Cells[i + 2, 2].Value = sales[i].Product?.Name;
                        sheet.Cells[i + 2, 3].Value = sales[i].Quantity;
                        sheet.Cells[i + 2, 4].Value = sales[i].Coffeehouse?.Name;
                    }

                    sheet.Cells.AutoFitColumns();

                    var saveDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "Excel files (*.xlsx)|*.xlsx",
                        FileName = $"Отчёт_продажи_{month}_{year}.xlsx"
                    };

                    if (saveDialog.ShowDialog() == true)
                    {
                        package.SaveAs(new System.IO.FileInfo(saveDialog.FileName));
                        MessageBox.Show("Файл сохранён успешно!");
                    }
                }
            }
        }
    }
}
