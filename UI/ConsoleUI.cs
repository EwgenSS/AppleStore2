using System;
using AppleStore.Services;

namespace AppleStore.UI
{
    public class ConsoleUI
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;
        private readonly ProductDetailsService _productDetailsService;
        private readonly CustomerService _customerService;
        private readonly EmployeeService _employeeService;
        private readonly OrderService _orderService;
        private readonly OrderItemService _orderItemService;

        public ConsoleUI(
            CategoryService categoryService,
            ProductService productService,
            ProductDetailsService productDetailsService,
            CustomerService customerService,
            EmployeeService employeeService,
            OrderService orderService,
            OrderItemService orderItemService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _productDetailsService = productDetailsService;
            _customerService = customerService;
            _employeeService = employeeService;
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        public void Start()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Apple Store ===");
                Console.WriteLine("1. Управление категориями");
                Console.WriteLine("2. Управление продуктами");
                Console.WriteLine("3. Управление клиентами");
                Console.WriteLine("4. Управление сотрудниками");
                Console.WriteLine("5. Управление заказами");
                Console.WriteLine("6. Просмотр каталога продуктов"); // Новый пункт меню
                Console.WriteLine("0. Выход");
                Console.Write("\nВыберите действие: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            CategoryMenu();
                            break;
                        case 2:
                            ProductMenu();
                            break;
                        case 3:
                            CustomerMenu();
                            break;
                        case 4:
                            EmployeeMenu();
                            break;
                        case 5:
                            OrderMenu();
                            break;
                        case 6:
                            BrowseProductCatalog(); // Новая функция
                            break;
                        case 0:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        // Новый метод для просмотра каталога продуктов с возможностью выбора по номеру
        private void BrowseProductCatalog()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Каталог продуктов Apple ===");
                Console.WriteLine("Выберите категорию:");
                
                var categories = _categoryService.GetAllCategories();
                
                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i].Name}");
                }
                
                Console.WriteLine("0. Назад");
                Console.Write("\nВыберите категорию (введите номер): ");
                
                if (int.TryParse(Console.ReadLine(), out int categoryChoice))
                {
                    if (categoryChoice == 0)
                    {
                        back = true;
                    }
                    else if (categoryChoice > 0 && categoryChoice <= categories.Count)
                    {
                        var selectedCategory = categories[categoryChoice - 1];
                        BrowseProductsByCategory(selectedCategory.CategoryId);
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор категории. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        // Метод для просмотра продуктов в выбранной категории
        private void BrowseProductsByCategory(int categoryId)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                var category = _categoryService.GetCategoryById(categoryId);
                Console.WriteLine($"=== Продукты в категории: {category.Name} ===");
                
                var products = _productService.GetProductsByCategory(categoryId);
                
                if (products.Count == 0)
                {
                    Console.WriteLine("В данной категории нет продуктов.");
                    Console.WriteLine("Нажмите любую клавишу для возврата...");
                    Console.ReadKey();
                    back = true;
                    continue;
                }
                
                for (int i = 0; i < products.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {products[i].Name} - ${products[i].Price}");
                }
                
                Console.WriteLine("0. Назад");
                Console.Write("\nВыберите продукт для просмотра подробной информации (введите номер): ");
                
                if (int.TryParse(Console.ReadLine(), out int productChoice))
                {
                    if (productChoice == 0)
                    {
                        back = true;
                    }
                    else if (productChoice > 0 && productChoice <= products.Count)
                    {
                        var selectedProduct = products[productChoice - 1];
                        DisplayDetailedProductInfo(selectedProduct.ProductId);
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор продукта. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        // Метод для отображения подробной информации о продукте
        private void DisplayDetailedProductInfo(int productId)
        {
            Console.Clear();
            var product = _productService.GetProductById(productId);
            var details = _productDetailsService.GetProductDetailsById(productId);
            
            Console.WriteLine("=== Подробная информация о продукте ===");
            Console.WriteLine($"Название: {product.Name}");
            Console.WriteLine($"Категория: {_categoryService.GetCategoryById(product.CategoryId).Name}");
            Console.WriteLine($"Описание: {product.Description}");
            Console.WriteLine($"Цена: ${product.Price}");
            Console.WriteLine($"В наличии: {product.StockQuantity} шт.");
            
            if (details != null)
            {
                Console.WriteLine("\n=== Технические характеристики ===");
                Console.WriteLine($"Цвет: {details.Color}");
                Console.WriteLine($"Размер экрана: {details.ScreenSize}");
            }
            
            Console.WriteLine("\nНажмите любую клавишу для возврата к списку продуктов...");
            Console.ReadKey();
        }

        private void CategoryMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Управление категориями ===");
                Console.WriteLine("1. Просмотреть все категории");
                Console.WriteLine("2. Добавить категорию");
                Console.WriteLine("3. Обновить категорию");
                Console.WriteLine("4. Удалить категорию");
                Console.WriteLine("0. Назад");
                Console.Write("\nВыберите действие: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            _categoryService.DisplayAllCategories();
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                        case 2:
                            AddCategory();
                            break;
                        case 3:
                            UpdateCategory();
                            break;
                        case 4:
                            DeleteCategory();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void AddCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление категории ===");
            
            Console.Write("Введите название категории: ");
            string name = Console.ReadLine();
            
            Console.Write("Введите описание категории: ");
            string description = Console.ReadLine();
            
            var category = new Models.Category
            {
                Name = name,
                Description = description
            };
            
            _categoryService.AddCategory(category);
            
            Console.WriteLine("Категория успешно добавлена. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void UpdateCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Обновление категории ===");
            
            _categoryService.DisplayAllCategories();
            
            Console.Write("Введите ID категории для обновления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var category = _categoryService.GetCategoryById(id);
                if (category != null)
                {
                    Console.Write($"Введите новое название категории (текущее: {category.Name}): ");
                    string name = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        category.Name = name;
                    }
                    
                    Console.Write($"Введите новое описание категории (текущее: {category.Description}): ");
                    string description = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        category.Description = description;
                    }
                    
                    _categoryService.UpdateCategory(category);
                    Console.WriteLine("Категория успешно обновлена.");
                }
                else
                {
                    Console.WriteLine($"Категория с ID {id} не найдена.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void DeleteCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление категории ===");
            
            _categoryService.DisplayAllCategories();
            
            Console.Write("Введите ID категории для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var category = _categoryService.GetCategoryById(id);
                if (category != null)
                {
                    Console.Write($"Вы уверены, что хотите удалить категорию '{category.Name}'? (y/n): ");
                    string confirm = Console.ReadLine().ToLower();
                    
                    if (confirm == "y" || confirm == "yes")
                    {
                        _categoryService.DeleteCategory(id);
                        Console.WriteLine("Категория успешно удалена.");
                    }
                    else
                    {
                        Console.WriteLine("Удаление отменено.");
                    }
                }
                else
                {
                    Console.WriteLine($"Категория с ID {id} не найдена.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void ProductMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Управление продуктами ===");
                Console.WriteLine("1. Просмотреть все продукты");
                Console.WriteLine("2. Просмотреть продукты по категории");
                Console.WriteLine("3. Просмотреть детали продукта");
                Console.WriteLine("4. Добавить продукт");
                Console.WriteLine("5. Обновить продукт");
                Console.WriteLine("6. Удалить продукт");
                Console.WriteLine("7. Обновить количество товара");
                Console.WriteLine("0. Назад");
                Console.Write("\nВыберите действие: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            _productService.DisplayAllProducts();
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                        case 2:
                            DisplayProductsByCategory();
                            break;
                        case 3:
                            DisplayProductDetails();
                            break;
                        case 4:
                            AddProduct();
                            break;
                        case 5:
                            UpdateProduct();
                            break;
                        case 6:
                            DeleteProduct();
                            break;
                        case 7:
                            UpdateProductStock();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void DisplayProductsByCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Просмотр продуктов по категории ===");
            
            _categoryService.DisplayAllCategories();
            
            Console.Write("Введите ID категории: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _productService.DisplayProductsByCategory(id);
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void DisplayProductDetails()
        {
            Console.Clear();
            Console.WriteLine("=== Просмотр деталей продукта ===");
            
            _productService.DisplayAllProducts();
            
            Console.Write("Введите ID продукта: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var product = _productService.GetProductById(id);
                if (product != null)
                {
                    Console.WriteLine(product.GetProductInfo());
                    _productDetailsService.DisplayProductDetails(id);
                }
                else
                {
                    Console.WriteLine($"Продукт с ID {id} не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление продукта ===");
            
            _categoryService.DisplayAllCategories();
            
            Console.Write("Введите ID категории: ");
            if (!int.TryParse(Console.ReadLine(), out int categoryId))
            {
                Console.WriteLine("Неверный ввод ID категории.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return;
            }
            
            var category = _categoryService.GetCategoryById(categoryId);
            if (category == null)
            {
                Console.WriteLine($"Категория с ID {categoryId} не найдена.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Введите название продукта: ");
            string name = Console.ReadLine();
            
            Console.Write("Введите описание продукта: ");
            string description = Console.ReadLine();
            
            Console.Write("Введите цену продукта: ");
            if (!double.TryParse(Console.ReadLine(), out double price))
            {
                Console.WriteLine("Неверный ввод цены.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Введите количество на складе: ");
            if (!int.TryParse(Console.ReadLine(), out int stockQuantity))
            {
                Console.WriteLine("Неверный ввод количества.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return;
            }
            
            var product = new Models.Product
            {
                CategoryId = categoryId,
                Name = name,
                Description = description,
                Price = price,
                StockQuantity = stockQuantity,
                Category = category
            };
            
            _productService.AddProduct(product);
            
            // Добавляем детали продукта
            Console.WriteLine("\nДобавление деталей продукта:");
            
            Console.Write("Введите цвет продукта: ");
            string color = Console.ReadLine();
            
            Console.Write("Введите размер экрана: ");
            string screenSize = Console.ReadLine();
            
            var details = new Models.ProductDetails
            {
                ProductId = product.ProductId,
                Color = color,
                ScreenSize = screenSize,
                Product = product
            };
            
            _productDetailsService.AddProductDetails(details);
            
            Console.WriteLine("Продукт успешно добавлен. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void UpdateProduct()
        {
            Console.Clear();
            Console.WriteLine("=== Обновление продукта ===");
            
            _productService.DisplayAllProducts();
            
            Console.Write("Введите ID продукта для обновления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var product = _productService.GetProductById(id);
                if (product != null)
                {
                    _categoryService.DisplayAllCategories();
                    
                    Console.Write($"Введите новый ID категории (текущий: {product.CategoryId}): ");
                    string categoryIdStr = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(categoryIdStr) && int.TryParse(categoryIdStr, out int categoryId))
                    {
                        var category = _categoryService.GetCategoryById(categoryId);
                        if (category != null)
                        {
                            product.CategoryId = categoryId;
                            product.Category = category;
                        }
                        else
                        {
                            Console.WriteLine($"Категория с ID {categoryId} не найдена. Категория не изменена.");
                        }
                    }
                    
                    Console.Write($"Введите новое название продукта (текущее: {product.Name}): ");
                    string name = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        product.Name = name;
                    }
                    
                    Console.Write($"Введите новое описание продукта (текущее: {product.Description}): ");
                    string description = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        product.Description = description;
                    }
                    
                    Console.Write($"Введите новую цену продукта (текущая: {product.Price}): ");
                    string priceStr = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(priceStr) && double.TryParse(priceStr, out double price))
                    {
                        product.Price = price;
                    }
                    
                    Console.Write($"Введите новое количество на складе (текущее: {product.StockQuantity}): ");
                    string stockQuantityStr = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(stockQuantityStr) && int.TryParse(stockQuantityStr, out int stockQuantity))
                    {
                        product.StockQuantity = stockQuantity;
                    }
                    
                    _productService.UpdateProduct(product);
                    
                    // Обновляем детали продукта
                    var details = _productDetailsService.GetProductDetailsById(id);
                    if (details != null)
                    {
                        Console.WriteLine("\nОбновление деталей продукта:");
                        
                        Console.Write($"Введите новый цвет продукта (текущий: {details.Color}): ");
                        string color = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(color))
                        {
                            details.Color = color;
                        }
                        
                        Console.Write($"Введите новый размер экрана (текущий: {details.ScreenSize}): ");
                        string screenSize = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(screenSize))
                        {
                            details.ScreenSize = screenSize;
                        }
                        
                        _productDetailsService.UpdateProductDetails(details);
                    }
                    
                    Console.WriteLine("Продукт успешно обновлен.");
                }
                else
                {
                    Console.WriteLine($"Продукт с ID {id} не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void DeleteProduct()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление продукта ===");
            
            _productService.DisplayAllProducts();
            
            Console.Write("Введите ID продукта для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var product = _productService.GetProductById(id);
                if (product != null)
                {
                    Console.Write($"Вы уверены, что хотите удалить продукт '{product.Name}'? (y/n): ");
                    string confirm = Console.ReadLine().ToLower();
                    
                    if (confirm == "y" || confirm == "yes")
                    {
                        _productService.DeleteProduct(id);
                        Console.WriteLine("Продукт успешно удален.");
                    }
                    else
                    {
                        Console.WriteLine("Удаление отменено.");
                    }
                }
                else
                {
                    Console.WriteLine($"Продукт с ID {id} не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void UpdateProductStock()
        {
            Console.Clear();
            Console.WriteLine("=== Обновление количества товара ===");
            
            _productService.DisplayAllProducts();
            
            Console.Write("Введите ID продукта: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var product = _productService.GetProductById(id);
                if (product != null)
                {
                    Console.Write($"Введите новое количество товара (текущее: {product.StockQuantity}): ");
                    if (int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        _productService.UpdateStock(id, quantity);
                        Console.WriteLine("Количество товара успешно обновлено.");
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод количества.");
                    }
                }
                else
                {
                    Console.WriteLine($"Продукт с ID {id} не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void CustomerMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Управление клиентами ===");
                Console.WriteLine("1. Просмотреть всех клиентов");
                Console.WriteLine("2. Добавить клиента");
                Console.WriteLine("3. Обновить клиента");
                Console.WriteLine("4. Удалить клиента");
                Console.WriteLine("0. Назад");
                Console.Write("\nВыберите действие: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            _customerService.DisplayAllCustomers();
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                        case 2:
                            AddCustomer();
                            break;
                        case 3:
                            UpdateCustomer();
                            break;
                        case 4:
                            DeleteCustomer();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void AddCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление клиента ===");
            
            Console.Write("Введите ФИО клиента: ");
            string fullName = Console.ReadLine();
            
            Console.Write("Введите email клиента: ");
            string email = Console.ReadLine();
            
            Console.Write("Введите телефон клиента: ");
            string phone = Console.ReadLine();
            
            Console.Write("Введите адрес клиента: ");
            string address = Console.ReadLine();
            
            var customer = new Models.Customer
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                Address = address
            };
            
            _customerService.AddCustomer(customer);
            
            Console.WriteLine("Клиент успешно добавлен. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void UpdateCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== Обновление клиента ===");
            
            _customerService.DisplayAllCustomers();
            
            Console.Write("Введите ID клиента для обновления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = _customerService.GetCustomerById(id);
                if (customer != null)
                {
                    Console.Write($"Введите новое ФИО клиента (текущее: {customer.FullName}): ");
                    string fullName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(fullName))
                    {
                        customer.FullName = fullName;
                    }
                    
                    Console.Write($"Введите новый email клиента (текущий: {customer.Email}): ");
                    string email = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        customer.Email = email;
                    }
                    
                    Console.Write($"Введите новый телефон клиента (текущий: {customer.Phone}): ");
                    string phone = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(phone))
                    {
                        customer.Phone = phone;
                    }
                    
                    Console.Write($"Введите новый адрес клиента (текущий: {customer.Address}): ");
                    string address = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(address))
                    {
                        customer.Address = address;
                    }
                    
                    _customerService.UpdateCustomer(customer);
                    Console.WriteLine("Информация о клиенте успешно обновлена.");
                }
                else
                {
                    Console.WriteLine($"Клиент с ID {id} не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void DeleteCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление клиента ===");
            
            _customerService.DisplayAllCustomers();
            
            Console.Write("Введите ID клиента для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = _customerService.GetCustomerById(id);
                if (customer != null)
                {
                    Console.Write($"Вы уверены, что хотите удалить клиента '{customer.FullName}'? (y/n): ");
                    string confirm = Console.ReadLine().ToLower();
                    
                    if (confirm == "y" || confirm == "yes")
                    {
                        _customerService.DeleteCustomer(id);
                        Console.WriteLine("Клиент успешно удален.");
                    }
                    else
                    {
                        Console.WriteLine("Удаление отменено.");
                    }
                }
                else
                {
                    Console.WriteLine($"Клиент с ID {id} не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void EmployeeMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Управление сотрудниками ===");
                Console.WriteLine("1. Просмотреть всех сотрудников");
                Console.WriteLine("2. Добавить сотрудника");
                Console.WriteLine("3. Обновить сотрудника");
                Console.WriteLine("4. Удалить сотрудника");
                Console.WriteLine("0. Назад");
                Console.Write("\nВыберите действие: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            _employeeService.DisplayAllEmployees();
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                        case 2:
                            AddEmployee();
                            break;
                        case 3:
                            UpdateEmployee();
                            break;
                        case 4:
                            DeleteEmployee();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void AddEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление сотрудника ===");
            
            Console.Write("Введите ФИО сотрудника: ");
            string fullName = Console.ReadLine();
            
            Console.Write("Введите должность сотрудника: ");
            string position = Console.ReadLine();
            
            Console.Write("Введите email сотрудника: ");
            string email = Console.ReadLine();
            
            Console.Write("Введите телефон сотрудника: ");
            string phone = Console.ReadLine();
            
            var employee = new Models.Employee
            {
                FullName = fullName,
                Position = position,
                Email = email,
                Phone = phone
            };
            
            _employeeService.AddEmployee(employee);
            
            Console.WriteLine("Сотрудник успешно добавлен. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void UpdateEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== Обновление сотрудника ===");
            
            _employeeService.DisplayAllEmployees();
            
            Console.Write("Введите ID сотрудника для обновления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var employee = _employeeService.GetEmployeeById(id);
                if (employee != null)
                {
                    Console.Write($"Введите новое ФИО сотрудника (текущее: {employee.FullName}): ");
                    string fullName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(fullName))
                    {
                        employee.FullName = fullName;
                    }
                    
                    Console.Write($"Введите новую должность сотрудника (текущая: {employee.Position}): ");
                    string position = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(position))
                    {
                        employee.Position = position;
                    }
                    
                    Console.Write($"Введите новый email сотрудника (текущий: {employee.Email}): ");
                    string email = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        employee.Email = email;
                    }
                    
                    Console.Write($"Введите новый телефон сотрудника (текущий: {employee.Phone}): ");
                    string phone = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(phone))
                    {
                        employee.Phone = phone;
                    }
                    
                    _employeeService.UpdateEmployee(employee);
                    Console.WriteLine("Информация о сотруднике успешно обновлена.");
                }
                else
                {
                    Console.WriteLine($"Сотрудник с ID {id} не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void DeleteEmployee()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление сотрудника ===");
            
            _employeeService.DisplayAllEmployees();
            
            Console.Write("Введите ID сотрудника для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var employee = _employeeService.GetEmployeeById(id);
                if (employee != null)
                {
                    Console.Write($"Вы уверены, что хотите удалить сотрудника '{employee.FullName}'? (y/n): ");
                    string confirm = Console.ReadLine().ToLower();
                    
                    if (confirm == "y" || confirm == "yes")
                    {
                        _employeeService.DeleteEmployee(id);
                        Console.WriteLine("Сотрудник успешно удален.");
                    }
                    else
                    {
                        Console.WriteLine("Удаление отменено.");
                    }
                }
                else
                {
                    Console.WriteLine($"Сотрудник с ID {id} не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void OrderMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Управление заказами ===");
                Console.WriteLine("1. Просмотреть все заказы");
                Console.WriteLine("2. Создать новый заказ");
                Console.WriteLine("3. Просмотреть детали заказа");
                Console.WriteLine("4. Изменить статус заказа");
                Console.WriteLine("0. Назад");
                Console.Write("\nВыберите действие: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            _orderService.DisplayAllOrders();
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                        case 2:
                            CreateOrder();
                            break;
                        case 3:
                            DisplayOrderDetails();
                            break;
                        case 4:
                            UpdateOrderStatus();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void CreateOrder()
        {
            Console.Clear();
            Console.WriteLine("=== Создание нового заказа ===");
            
            // Выбор клиента
            _customerService.DisplayAllCustomers();
            
            Console.Write("Введите ID клиента: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Неверный ввод ID клиента.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return;
            }
            
            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
            {
                Console.WriteLine($"Клиент с ID {customerId} не найден.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return;
            }
            
            // Выбор сотрудника
            _employeeService.DisplayAllEmployees();
            
            Console.Write("Введите ID сотрудника: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.WriteLine("Неверный ввод ID сотрудника.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return;
            }
            
            var employee = _employeeService.GetEmployeeById(employeeId);
            if (employee == null)
            {
                Console.WriteLine($"Сотрудник с ID {employeeId} не найден.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return;
            }
            
            // Создание заказа
            var order = new Models.Order
            {
                CustomerId = customerId,
                EmployeeId = employeeId,
                OrderDate = DateTime.Now,
                TotalAmount = 0,
                Status = "Новый",
                Customer = customer,
                Employee = employee
            };
            
            _orderService.AddOrder(order);
            
            // Добавление товаров в заказ
            bool addingItems = true;
            while (addingItems)
            {
                Console.Clear();
                Console.WriteLine($"=== Добавление товаров в заказ #{order.OrderId} ===");
                
                _productService.DisplayAllProducts();
                
                Console.Write("Введите ID продукта (0 для завершения): ");
                if (!int.TryParse(Console.ReadLine(), out int productId))
                {
                    Console.WriteLine("Неверный ввод ID продукта.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    continue;
                }
                
                if (productId == 0)
                {
                    addingItems = false;
                    continue;
                }
                
                var product = _productService.GetProductById(productId);
                if (product == null)
                {
                    Console.WriteLine($"Продукт с ID {productId} не найден.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    continue;
                }
                
                Console.Write("Введите количество: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity))
                {
                    Console.WriteLine("Неверный ввод количества.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    continue;
                }
                
                if (quantity <= 0)
                {
                    Console.WriteLine("Количество должно быть больше 0.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    continue;
                }
                
                if (quantity > product.StockQuantity)
                {
                    Console.WriteLine($"Недостаточно товара на складе. Доступно: {product.StockQuantity}");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    continue;
                }
                
                var orderItem = new Models.OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    Order = order,
                    Product = product
                };
                
                _orderItemService.AddOrderItem(orderItem);
                
                // Обновляем количество товара на складе
                _productService.UpdateStock(productId, product.StockQuantity - quantity);
                
                Console.WriteLine("Товар успешно добавлен в заказ.");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            
            // Пересчитываем общую сумму заказа
            _orderService.CalculateTotal(order.OrderId);
            
            Console.WriteLine("Заказ успешно создан. Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void DisplayOrderDetails()
        {
            Console.Clear();
            Console.WriteLine("=== Просмотр деталей заказа ===");
            
            _orderService.DisplayAllOrders();
            
            Console.Write("Введите ID заказа: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var order = _orderService.GetOrderById(id);
                if (order != null)
                {
                    Console.WriteLine($"Заказ #{order.OrderId}");
                    Console.WriteLine($"Клиент: {order.Customer.FullName}");
                    Console.WriteLine($"Сотрудник: {order.Employee.FullName}");
                    Console.WriteLine($"Дата заказа: {order.OrderDate}");
                    Console.WriteLine($"Статус: {order.Status}");
                    Console.WriteLine($"Общая сумма: ${order.TotalAmount}");
                    
                    Console.WriteLine("\nТовары в заказе:");
                    _orderItemService.DisplayOrderItems(id);
                }
                else
                {
                    Console.WriteLine($"Заказ с ID {id} не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void UpdateOrderStatus()
        {
            Console.Clear();
            Console.WriteLine("=== Изменение статуса заказа ===");
            
            _orderService.DisplayAllOrders();
            
            Console.Write("Введите ID заказа: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var order = _orderService.GetOrderById(id);
                if (order != null)
                {
                    Console.WriteLine("Доступные статусы:");
                    Console.WriteLine("1. Новый");
                    Console.WriteLine("2. В обработке");
                    Console.WriteLine("3. Отправлен");
                    Console.WriteLine("4. Доставлен");
                    Console.WriteLine("5. Отменен");
                    
                    Console.Write($"Выберите новый статус (текущий: {order.Status}): ");
                    if (int.TryParse(Console.ReadLine(), out int statusChoice))
                    {
                        string newStatus = "";
                        switch (statusChoice)
                        {
                            case 1:
                                newStatus = "Новый";
                                break;
                            case 2:
                                newStatus = "В обработке";
                                break;
                            case 3:
                                newStatus = "Отправлен";
                                break;
                            case 4:
                                newStatus = "Доставлен";
                                break;
                            case 5:
                                newStatus = "Отменен";
                                break;
                            default:
                                Console.WriteLine("Неверный выбор статуса.");
                                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                                Console.ReadKey();
                                return;
                        }
                        
                        order.Status = newStatus;
                        _orderService.UpdateOrder(order);
                        Console.WriteLine("Статус заказа успешно обновлен.");
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод статуса.");
                    }
                }
                else
                {
                    Console.WriteLine($"Заказ с ID {id} не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
