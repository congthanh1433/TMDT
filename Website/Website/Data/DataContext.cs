using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Website.Models;

namespace Website.Data
{
    public class DataContext:DbContext
    {
        public DataContext()
        {
            SqlConnectionStringBuilder sqlb = new SqlConnectionStringBuilder();
            sqlb.DataSource = "LAPTOP-9F47JF9E\\SQLEXPRESS";
            sqlb.InitialCatalog = "LaptopData";
            sqlb.IntegratedSecurity = true;
            this.Database.Connection.ConnectionString = sqlb.ConnectionString;

            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
 

        public System.Data.Entity.DbSet<Website.Models.PhanQuyen> PhanQuyens { get; set; }

        
    }
}