// 新建 WinForms 项目 OrderManagementUI，并添加对 OrderManagement 的引用。

// 主窗口 MainForm.cs
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using OrderManagement;

namespace OrderManagementUI
{
    public partial class MainForm : Form
    {
        private OrderService service = new OrderService();
        private BindingSource orderBindingSource = new BindingSource();
        private BindingSource detailBindingSource = new BindingSource();

        public MainForm()
        {
            InitializeComponent();

            dgvOrders.AutoGenerateColumns = true;
            dgvDetails.AutoGenerateColumns = true;

            orderBindingSource.DataSource = typeof(Order);
            detailBindingSource.DataSource = orderBindingSource;
            detailBindingSource.DataMember = "Details";

            dgvOrders.DataSource = orderBindingSource;
            dgvDetails.DataSource = detailBindingSource;

            this.Load += (s, e) => LoadSampleData();

            btnAdd.Click += (s, e) =>
            {
                var form = new OrderEditForm(service);
                if (form.ShowDialog() == DialogResult.OK)
                    RefreshOrderList();
            };

            btnModify.Click += (s, e) =>
            {
                if (orderBindingSource.Current is Order selectedOrder)
                {
                    var form = new OrderEditForm(service, selectedOrder);
                    if (form.ShowDialog() == DialogResult.OK)
                        RefreshOrderList();
                }
            };

            btnDelete.Click += (s, e) =>
            {
                if (orderBindingSource.Current is Order selectedOrder)
                {
                    service.RemoveOrder(selectedOrder.OrderId);
                    RefreshOrderList();
                }
            };

            btnQuery.Click += (s, e) =>
            {
                string keyword = txtQuery.Text;
                var result = service.QueryOrders(o => o.Customer.Contains(keyword) || o.Details.Any(d => d.ProductName.Contains(keyword)));
                orderBindingSource.DataSource = new BindingList<Order>(result);
            };
        }

        private void LoadSampleData()
        {
            var order1 = new Order(1, "张三", new()
            {
                new OrderDetails("苹果", 3, 5m),
                new OrderDetails("香蕉", 2, 3.5m)
            });
            var order2 = new Order(2, "李四", new()
            {
                new OrderDetails("橙子", 1, 6m)
            });
            service.AddOrder(order1);
            service.AddOrder(order2);
            RefreshOrderList();
        }

        private void RefreshOrderList()
        {
            orderBindingSource.DataSource = new BindingList<Order>(service.QueryOrders(_ => true));
        }
    }
}

// 弹出窗口 OrderEditForm.cs
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OrderManagement;

namespace OrderManagementUI
{
    public partial class OrderEditForm : Form
    {
        private OrderService service;
        private Order editingOrder;

        public OrderEditForm(OrderService service, Order existingOrder = null)
        {
            InitializeComponent();
            this.service = service;
            this.editingOrder = existingOrder;

            if (existingOrder != null)
            {
                txtId.Text = existingOrder.OrderId.ToString();
                txtCustomer.Text = existingOrder.Customer;
                foreach (var d in existingOrder.Details)
                {
                    dgvDetails.Rows.Add(d.ProductName, d.Quantity, d.Price);
                }
                txtId.Enabled = false;
            }

            btnOK.Click += (s, e) =>
            {
                try
                {
                    int id = int.Parse(txtId.Text);
                    string customer = txtCustomer.Text;
                    var details = new List<OrderDetails>();
                    foreach (DataGridViewRow row in dgvDetails.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string name = row.Cells[0].Value?.ToString();
                        int qty = int.Parse(row.Cells[1].Value?.ToString());
                        decimal price = decimal.Parse(row.Cells[2].Value?.ToString());
                        details.Add(new OrderDetails(name, qty, price));
                    }

                    var newOrder = new Order(id, customer, details);
                    if (existingOrder == null)
                        service.AddOrder(newOrder);
                    else
                        service.ModifyOrder(newOrder);
                    DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误: " + ex.Message);
                }
            };
        }
    }
}
