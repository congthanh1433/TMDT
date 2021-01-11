using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class CartItem
    {
        public Product Shopping_product { get; set; }
        public int Shopping_quantity { get; set; }
    }
    public class Cart
    {
        readonly List<CartItem> items = new List<CartItem>();

        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }

       /* [DisplayName("Ảnh")]
        public string HinhAnh { get; set; }
        public int MaSach { get; set; }
        [DisplayName("Tên Sách")]
        public string ProductName { get; set; }
        [DisplayName("Đơn giá")]
        public int Gia { get; set; }
        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }
        [DisplayName("Thành tiền")]
        public int TongTien
        {
            get
            {
                return SoLuong * Gia;
            }
        }*/
        public void Add(Product _pro, int _quantity = 1)
        {
            var item = Items.FirstOrDefault(s => s.Shopping_product.ProductID == _pro.ProductID);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    Shopping_product = _pro,
                    Shopping_quantity = _quantity
                });
            }
            else
            {
                item.Shopping_quantity += _quantity;
            }
        }
        public void Update_Quantity_Shopping(int id, int _quantity)
        {
            var item = items.Find(s => s.Shopping_product.ProductID == id);
            if (item != null)
            {
                item.Shopping_quantity = _quantity;
            }
        }
        public double Total_Money()
        {
            var total = items.Sum(s => s.Shopping_product.Price * s.Shopping_quantity);
            return (double)total;
        }
        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s => s.Shopping_product.ProductID == id);
        }
        public int Total_Quantity_in_Cart()
        {
            return items.Sum(s => s.Shopping_quantity);
        }
        public void ClearCart()
        {
            items.Clear();
        }
    }
}