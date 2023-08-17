using MobileCommander.Interfaces;
using MobileCommander.Services;

namespace MobileCommander
{
    public partial class Form1 : Form
    {
        private readonly IAdbService _adbService;

        public Form1(IAdbService adbService)
        {
            _adbService = adbService;
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await _adbService.KillAllBackgroundTasksAsync();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var searchData = textBox1;

            var response = await _adbService.GetChromeSearchAsync(searchData.Text);

            textBox2.Text = response;
        }
    }
}