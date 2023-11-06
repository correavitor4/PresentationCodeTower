using Microsoft.AspNetCore.Connections.Features;
using PresentationCodeTower.Models;
using System.Reflection.Metadata.Ecma335;

namespace PresentationCodeTower.Persistence
{
    public class FakeDb
    {
        private List<Product> Products { get; set; }
        private List<User> Users { get; set; }
        private List<Order> Orders { get; set; }

        public FakeDb()
        {
            this.Products = new List<Product>();
            this.Users = new List<User>();
            this.Orders = new List<Order>();
        }

        public Product AddProduct(Product product)
        {
            Products.Add(product);
            
            return product;
        }

        public void AddUser(User user)
        {
            this.Users.Add(user);
        }

        public Order AddOrder(Order order)
        {
            Orders.Add(order);
            return order;
        }

        public bool DoesUserExist(User user)
        {
            var users = GetUsers();
            return users.Any(u => u.Id == user.Id);
        }

        public bool AllProductsExist(List<Product> products)
        {
            foreach (var item in products)
            {
                if (!GetProducts().Any(p => p.Id == item.Id))
                {
                    return false;
                }
            }
            return true;
        }

        public List<Product> GetProducts()
        {
            return Products;
        }

        public List<User> GetUsers()
        {
            return this.Users;
        }

        public List<Order> GetOrders()
        {
            return this.Orders;
        }

        internal void populate()
        {
            //Populate users
            var user = new User("Edmundo");

            //Populate products
            var product = new Product("Estojo para lápis");
            var product2 = new Product("Caneta");
            var product3 = new Product("Borracha");

            //Populate orders
            var order = new Order(
                user, 
                new List<Product> { 
                    product, 
                    product2, 
                    product3 
                });

            // "Save" data
            this.AddUser(user);
            this.AddProduct(product);
            this.AddProduct(product2);
            this.AddProduct(product3);
            this.AddOrder(order);
        }

        internal Product? UpdateProduct(int id, Product product)
        {
            if (GetProducts().FirstOrDefault(product => product.Id == id) is null)
            {
                return null;
            }

            var productToUpdate = GetProducts().FirstOrDefault(product => product.Id == id) 
                ?? throw new NullReferenceException("Product not found");
            var productIndex = GetProducts().IndexOf(productToUpdate);
            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;
            GetProducts()[productIndex] = productToUpdate;

            return GetProducts().FirstOrDefault(product => product.Id == id);
        }

        internal Product? DeleteProduct(int id)
        {
            var productToDelete = GetProducts().FirstOrDefault(product => product.Id == id);
            if (productToDelete is null)
            {
                return null;
            }
            GetProducts().Remove(productToDelete);

            return productToDelete;
        }

        internal Order? UpdateOrder(int id, Order order)
        {
            if (!GetOrders().Any(o => o.Id == id))
            {
                throw new Exception("Order not found");
            }

            var orderToUpdate = GetOrders().FirstOrDefault(o => o.Id == id)
                ?? throw new Exception("Order not found");
            var orderIndex = GetOrders().IndexOf(orderToUpdate);
            orderToUpdate.Products = order.Products;
            orderToUpdate.User = order.User;
            GetOrders()[orderIndex] = orderToUpdate;

            return GetOrders().FirstOrDefault(o => o.Id == id);
        }

        internal void DeleteOrder(int id)
        {
            var orderToDelete = GetOrders().FirstOrDefault(o => o.Id == id);
            if (orderToDelete is null)
            {
                throw new Exception("Order not found");
            }

            this.Orders.Remove(orderToDelete);
        }
    }
}
