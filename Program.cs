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

        var products = new List<Product>
        {
            new Product("Laptop", "Electronics", 48000m, 5),
            new Product("Mouse", "Electronics", 750m, 20),
            new Product("Keyboard", "Electronics", 1500m, 10),
            new Product("Desk", "Furniture", 5000m, 4),
            new Product("Chair", "Furniture", 2500m, 8)
        };

        var productDictionary = products.ToDictionary(p => p.Name);

        Console.WriteLine("DICTIONARY LOOKUP:");
        if (productDictionary.TryGetValue("Laptop", out var foundProduct))
        {
            Console.WriteLine($"{foundProduct.Name} - {foundProduct.Price}");
        }

        Console.WriteLine("\nFILTERING:");
        var filtered = products.Where(p => p.Price > 1000m);

        foreach (var product in filtered)
        {
            Console.WriteLine($"{product.Name} - {product.Price}");
        }

        Console.WriteLine("\nSORTING:");
        var sorted = products.OrderBy(p => p.Price);

        foreach (var product in sorted)
        {
            Console.WriteLine($"{product.Name} - {product.Price}");
        }

        Console.WriteLine("\nSEARCHING:");
        var searched = products.FirstOrDefault(p => p.Name == "Mouse");

        if (searched != null)
        {
            Console.WriteLine($"Found: {searched.Name}");
        }

        Console.WriteLine("\nGROUPING:");
        var grouped = products.GroupBy(p => p.Category);

        foreach (var group in grouped)
        {
            Console.WriteLine(group.Key);

            foreach (var product in group)
            {
                Console.WriteLine($"- {product.Name}");
            }
        }

        Console.WriteLine("\nAGGREGATION:");
        Console.WriteLine($"Total products: {products.Count}");
        Console.WriteLine($"Total stock: {products.Sum(p => p.Stock)}");
        Console.WriteLine($"Average price: {products.Average(p => p.Price):0.00}");

        Console.WriteLine("\nADDING PRODUCTS:");

        AddProduct(products, new Product("Headphones", "Electronics", 2500m, 12));
        AddProduct(products, new Product("Monitor", "Electronics", 8500m, 6));
        AddProduct(products, new Product("", "Electronics", -100m, -5));

        try
        {
            string json = JsonSerializer.Serialize(
                products,
                new JsonSerializerOptions { WriteIndented = true }
            );

            File.WriteAllText("products.json", json);
            Console.WriteLine("\nJSON saved.");

            string loadedJson = File.ReadAllText("products.json");

            var loadedProducts =
                JsonSerializer.Deserialize<List<Product>>(loadedJson)
                ?? new List<Product>();

            Console.WriteLine($"JSON loaded: {loadedProducts.Count} products.");

            var report = loadedProducts.Select(
                p => $"{p.Name} | {p.Category} | {p.Price} | Stock: {p.Stock}"
            );

            File.WriteAllLines("report.txt", report);
            Console.WriteLine("Text report exported.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"File error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("File handling finished.");
        }

        Console.WriteLine("\nNEW LINQ QUERY:");

        var lowStockProducts = products
            .Where(p => p.Stock <= 5)
            .OrderBy(p => p.Stock);

        foreach (var product in lowStockProducts)
        {
            Console.WriteLine($"{product.Name} - Stock: {product.Stock}");
        }

        Console.WriteLine("\n=== PROGRAM COMPLETE ===");
    }

    static void AddProduct(List<Product> products, Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name) ||
            product.Price <= 0 ||
            product.Stock < 0)
        {
            Console.WriteLine("Invalid product rejected.");
            return;
        }

        products.Add(product);
        Console.WriteLine($"{product.Name} added successfully.");
    }
}