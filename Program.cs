using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public record Product(string Name, string Category, decimal Price, int Stock);

class Program
{
    static void Main()
    {
        Console.WriteLine("=== WEEK 3 PRODUCT INVENTORY SYSTEM ===\n");

        List<Product> products = new List<Product>
        {
            new Product("Laptop", "Electronics", 45000m, 5),
            new Product("Mouse", "Electronics", 800m, 15),
            new Product("Keyboard", "Electronics", 1500m, 10),
            new Product("Notebook", "School", 100m, 30),
            new Product("Backpack", "School", 1200m, 8)
        };

        Console.WriteLine("1. ORIGINAL PRODUCTS");
        DisplayProducts(products);

        Dictionary<string, Product> productDictionary =
            products.ToDictionary(p => p.Name, p => p);

        Console.WriteLine("\n2. DICTIONARY SEARCH");
        string searchName = "Laptop";

        if (productDictionary.TryGetValue(searchName, out Product? foundProduct))
        {
            Console.WriteLine(
                $"Found: {foundProduct.Name} | {foundProduct.Category} | " +
                $"P{foundProduct.Price} | Stock: {foundProduct.Stock}");
        }

        Console.WriteLine("\n3. LINQ SEARCH - Mouse");

        var searchedProducts = products
            .Where(p => p.Name.Contains("Mouse", StringComparison.OrdinalIgnoreCase));

        DisplayProducts(searchedProducts);

        Console.WriteLine("\n4. FILTERED PRODUCTS - Price >= P1000");

        var filteredProducts = products
            .Where(p => p.Price >= 1000m);

        DisplayProducts(filteredProducts);

        Console.WriteLine("\n5. SORTED PRODUCTS - Lowest to Highest Price");

        var sortedProducts = products
            .OrderBy(p => p.Price);

        DisplayProducts(sortedProducts);

        Console.WriteLine("\n6. GROUPED PRODUCTS BY CATEGORY");

        var groupedProducts = products.GroupBy(p => p.Category);

        foreach (var group in groupedProducts)
        {
            Console.WriteLine($"\n{group.Key}:");

            foreach (var product in group)
            {
                Console.WriteLine(
                    $"{product.Name} - P{product.Price} - Stock: {product.Stock}");
            }
        }

        decimal totalInventoryValue =
            products.Sum(p => p.Price * p.Stock);

        Console.WriteLine("\n7. TOTAL INVENTORY VALUE");
        Console.WriteLine($"P{totalInventoryValue}");

        Console.WriteLine("\n8. ADDING TWO VALID PRODUCTS");

        AddProduct(
            products,
            new Product("Headphones", "Electronics", 2500m, 12));

        AddProduct(
            products,
            new Product("Pen", "School", 25m, 50));

        Console.WriteLine("\n9. ADDING INVALID PRODUCT");

        AddProduct(
            products,
            new Product("Broken Product", "Electronics", -500m, -2));

        string jsonFile = "products.json";

        Console.WriteLine("\n10. JSON SAVE");

        try
        {
            string json = JsonSerializer.Serialize(
                products,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(jsonFile, json);

            Console.WriteLine("Products successfully saved to products.json.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"JSON save error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("JSON save operation finished.");
        }

        Console.WriteLine("\n11. JSON LOAD");

        try
        {
            string json = File.ReadAllText(jsonFile);

            List<Product>? loadedProducts =
                JsonSerializer.Deserialize<List<Product>>(json);

            if (loadedProducts != null)
            {
                Console.WriteLine("Products successfully loaded from JSON:");
                DisplayProducts(loadedProducts);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"JSON load error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("JSON load operation finished.");
        }

        Console.WriteLine("\n12. TEXT REPORT");

        try
        {
            List<string> report = new List<string>();

            report.Add("PRODUCT INVENTORY REPORT");
            report.Add("========================");

            foreach (Product product in products)
            {
                report.Add(
                    $"{product.Name} | {product.Category} | " +
                    $"P{product.Price} | Stock: {product.Stock}");
            }

            report.Add("========================");
            report.Add(
                $"Total Inventory Value: P{products.Sum(p => p.Price * p.Stock)}");

            File.WriteAllLines("InventoryReport.txt", report);

            Console.WriteLine("InventoryReport.txt successfully created.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Text file error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Text report operation finished.");
        }

        Console.WriteLine("\n13. EXCEPTION HANDLING");

        try
        {
            Console.WriteLine("Attempting to read a file...");

            string text = File.ReadAllText("InventoryReport.txt");

            Console.WriteLine("File read successfully.");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File not found: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Exception handling demonstration finished.");
        }

        Console.WriteLine("\n=== ACTIVITY COMPLETED ===");
    }

    static void AddProduct(List<Product> products, Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            Console.WriteLine("REJECTED: Product name cannot be empty.");
            return;
        }

        if (string.IsNullOrWhiteSpace(product.Category))
        {
            Console.WriteLine("REJECTED: Category cannot be empty.");
            return;
        }

        if (product.Price <= 0)
        {
            Console.WriteLine($"REJECTED: {product.Name} has an invalid price.");
            return;
        }

        if (product.Stock < 0)
        {
            Console.WriteLine($"REJECTED: {product.Name} has invalid stock.");
            return;
        }

        products.Add(product);
        Console.WriteLine($"ADDED: {product.Name}");
    }

    static void DisplayProducts(IEnumerable<Product> products)
    {
        foreach (Product product in products)
        {
            Console.WriteLine(
                $"{product.Name} | {product.Category} | " +
                $"P{product.Price} | Stock: {product.Stock}");
        }
    }
}