using otokhi_winform.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace otokhi_winform
{
    public partial class Form1 : Form
    {
        private readonly string _url = "http://iis-remote.otokhi.com";
        private readonly string _token = "0fbe299ca1409b9dba8c5cbdbbb879b5ab5196c3"; // Replace with a secure token
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var apiService = new ApiService(_url);
            var result = await apiService.GetAsync($"api/sites/{_token}/stop/piisiit");
            this.listBox1.Items.Add(result);
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            var apiService = new ApiService(_url);
            var result = await apiService.GetAsync($"api/sites/{_token}/start/piisiit");
            this.listBox1.Items.Add(result);
        }
    }
}
