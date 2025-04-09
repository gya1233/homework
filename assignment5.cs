using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagement
{
    public class OrderDetails
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public OrderDetails(string productName, int quantity, decimal price)
        {
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }

        public override bool Equals(object obj)
        {
            return obj is OrderDetails details &&
                   ProductName == details.ProductName &&
                   Quantity == details.Quantity &&
                   Price == details.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductName, Quantity, Price);
        }

        public override string ToString()
        {
            return $"{ProductName} x{Quantity} 单价：{Price:C} 小计：{Quantity * Price:C}";
        }
    }

    public class Order
    {
        public int OrderId { get; }
        public string Customer { get; set; }
        public decimal TotalAmount => Details.Sum(d => d.Quantity * d.Price);
        public List<OrderDetails> Details { get; }

        public Order(int orderId, string customer, List<OrderDetails> details)
        {
            if (details.Distinct().Count() != details.Count)
                throw new ArgumentException("订单明细存在重复项");

            OrderId = orderId;
            Customer = customer;
            Details = details;
        }

        public override bool Equals(object obj)
        {
            return obj is Order order &&
                   OrderId == order.OrderId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OrderId);
        }

        public override string ToString()
        {
            return $"订单号：{OrderId}\n客户：{Customer}\n总金额：{TotalAmount:C}\n明细：\n{string.Join("\n", Details)}";
        }
    }

    public class OrderService
    {
        private List<Order> orders = new List<Order>();

        public void AddOrder(Order order)
        {
            if (orders.Contains(order))
                throw new ArgumentException("订单已存在");
            orders.Add(order);
        }

        public void RemoveOrder(int orderId)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                throw new ArgumentException("订单不存在");
            orders.Remove(order);
        }

        public void ModifyOrder(Order newOrder)
        {
            var index = orders.FindIndex(o => o.OrderId == newOrder.OrderId);
            if (index == -1)
                throw new ArgumentException("订单不存在");
            orders[index] = newOrder;
        }

        public List<Order> QueryOrders(Func<Order, bool> predicate)
        {
            return orders.Where(predicate)
                .OrderBy(o => o.TotalAmount)
                .ToList();
        }

        public void SortOrders(Func<Order, object> keySelector = null)
        {
            orders = keySelector == null
                ? orders.OrderBy(o => o.OrderId).ToList()
                : orders.OrderBy(keySelector).ToList();
        }
    }

    class Program
    {
        static OrderService service = new OrderService();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n1. 添加订单\n2. 删除订单\n3. 修改订单\n4. 查询订单\n5. 退出");
                Console.Write("请选择操作：");
                switch (Console.ReadLine())
                {
                    case "1":
                        AddOrder();
                        break;
                    case "2":
                        RemoveOrder();
                        break;
                    case "3":
                        ModifyOrder();
                        break;
                    case "4":
                        QueryOrders();
                        break;
                    case "5":
                        return;
                }
            }
        }

        static void AddOrder()
        {
            try
            {
                Console.Write("订单号：");
                int id = int.Parse(Console.ReadLine());
                Console.Write("客户：");
                string customer = Console.ReadLine();

                var details = new List<OrderDetails>();
                while (true)
                {
                    Console.Write("商品名称（空结束）：");
                    string product = Console.ReadLine();
                    if (string.IsNullOrEmpty(product)) break;

                    Console.Write("数量：");
                    int qty = int.Parse(Console.ReadLine());
                    Console.Write("单价：");
                    decimal price = decimal.Parse(Console.ReadLine());

                    details.Add(new OrderDetails(product, qty, price));
                }

                service.AddOrder(new Order(id, customer, details));
                Console.WriteLine("添加成功");
            }
            catch (Exception e)
            {
                Console.WriteLine($"错误：{e.Message}");
            }
        }

        static void RemoveOrder()
        {
            try
            {
                Console.Write("输入要删除的订单号：");
                int id = int.Parse(Console.ReadLine());
                service.RemoveOrder(id);
                Console.WriteLine("删除成功");
            }
            catch (Exception e)
            {
                Console.WriteLine($"错误：{e.Message}");
            }
        }

        static void ModifyOrder()
        {
            try
            {
                Console.Write("输入要修改的订单号：");
                int id = int.Parse(Console.ReadLine());

                Console.Write("新客户：");
                string customer = Console.ReadLine();

                var details = new List<OrderDetails>();
                while (true)
                {
                    Console.Write("新商品名称（空结束）：");
                    string product = Console.ReadLine();
                    if (string.IsNullOrEmpty(product)) break;

                    Console.Write("新数量：");
                    int qty = int.Parse(Console.ReadLine());
                    Console.Write("新单价：");
                    decimal price = decimal.Parse(Console.ReadLine());

                    details.Add(new OrderDetails(product, qty, price));
                }

                service.ModifyOrder(new Order(id, customer, details));
                Console.WriteLine("修改成功");
            }
            catch (Exception e)
            {
                Console.WriteLine($"错误：{e.Message}");
            }
        }

        static void QueryOrders()
        {
            Console.WriteLine("查询条件：\n1. 订单号\n2. 商品名称\n3. 客户\n4. 金额范围");
            Console.Write("请选择：");
            var result = new List<Order>();

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("输入订单号：");
                    int id = int.Parse(Console.ReadLine());
                    result = service.QueryOrders(o => o.OrderId == id);
                    break;
                case "2":
                    Console.Write("输入商品名称：");
                    string product = Console.ReadLine();
                    result = service.QueryOrders(o => o.Details.Any(d => d.ProductName == product));
                    break;
                case "3":
                    Console.Write("输入客户：");
                    string customer = Console.ReadLine();
                    result = service.QueryOrders(o => o.Customer == customer);
                    break;
                case "4":
                    Console.Write("最低金额：");
                    decimal min = decimal.Parse(Console.ReadLine());
                    Console.Write("最高金额：");
                    decimal max = decimal.Parse(Console.ReadLine());
                    result = service.QueryOrders(o => o.TotalAmount >= min && o.TotalAmount <= max);
                    break;
            }

            Console.WriteLine($"找到 {result.Count} 条结果：");
            result.ForEach(o => Console.WriteLine(o + "\n"));
        }
    }
}