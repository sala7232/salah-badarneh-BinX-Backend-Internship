var customers = new List<Customer>
{
    new() { Id = 1, Name = "Ahmad" },
    new() { Id = 2, Name = "Lina" },
    new() { Id = 3, Name = "Yousef" },
    new() { Id = 4, Name = "Sara" },
    new() { Id = 5, Name = "Omar" },
    new() { Id = 6, Name = "Dana" },
};

var orders = new List<Order>
{
    new() { Id = 101, CustomerId = 1, Amount = 150, Items = new() { new() { ProductName = "Keyboard", Price = 100 }, new() { ProductName = "Mouse", Price = 50 } } },
    new() { Id = 102, CustomerId = 1, Amount = 300, Items = new() { new() { ProductName = "Monitor", Price = 300 } } },
    new() { Id = 103, CustomerId = 2, Amount = 75,  Items = new() { new() { ProductName = "Mousepad", Price = 25 }, new() { ProductName = "USB Cable", Price = 50 } } },
    new() { Id = 104, CustomerId = 3, Amount = 500, Items = new() { new() { ProductName = "Laptop Stand", Price = 200 }, new() { ProductName = "Webcam", Price = 300 } } },
    new() { Id = 105, CustomerId = 4, Amount = 60,  Items = new() { new() { ProductName = "Notebook", Price = 20 }, new() { ProductName = "Pen Set", Price = 40 } } },
    new() { Id = 106, CustomerId = 5, Amount = 220, Items = new() { new() { ProductName = "Headset", Price = 220 } } },
};

var totalPerCustomer = orders
.GroupBy(o => o.CustomerId)
.Select(g => new { g.Key, Total = g.Sum(o => o.Amount) });

foreach (var c in totalPerCustomer)
Console.WriteLine($"Customer {c.Key}: {c.Total}");

var namedOrders = customers.Join(orders, c => c.Id, o => o.CustomerId, (c, o) => new { c.Name, o.Amount });
foreach (var n in namedOrders)
 Console.WriteLine($"{n.Name}: {n.Amount}");

var items = orders.SelectMany(o => o.Items);
foreach (var i in items)
 Console.WriteLine($"{i.ProductName}: {i.Price}");

var bigOrders = orders.Where(o => o.Amount > 100);
orders.Add(new Order { Id = 107, CustomerId = 6, Amount = 999 });

foreach (var o in bigOrders)
 Console.WriteLine($"Order {o.Id}: {o.Amount}");
// order 107 shows up here even though it was added after bigOrders was defined,
// since Where() only evaluates when the foreach runs, not when the query was written

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class LineItem
{
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public List<LineItem> Items { get; set; } = new();
}