using System;
using System.Linq;
using System.Windows.Forms;
using Freetime.Data.Services.Handles;

namespace Freetime.Data.Services.Host
{
    public partial class HostApp : Form
    {
        private Service Service { get; set; }
        
        private BasicHttpEndpointHandle BasicHttpEndpoint { get; set; }

        public HostApp()
        {
            InitializeComponent();
            gridSessions.AutoGenerateColumns = false;
        }

        private void HostApp_Load(object sender, EventArgs e)
        {
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnRestart.Enabled = true;
            btnStop.Enabled = true;
            btnStart.Enabled = false;

            Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
            btnRestart.Enabled = false;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Stop();
            Start();
        }

        private void Start()
        {
            Service = new Service();

            BasicHttpEndpoint = new BasicHttpEndpointHandle { Address = "http://192.168.175.123:8000/FreetimeDataServices" };
            
            Service.AddEndpointHandle(BasicHttpEndpoint);
            Service.Load();
            
            gridSessions.DataSource = Service.ServiceHosts.ToList();
            SetupGrid();
            
            Service.Start();            
        }

        private void Stop()
        {
            Service.Stop();
            Service = null;
            gridSessions.DataSource = null;
        }

        private void SetupGrid()
        {
            colName.DisplayIndex = 0;
            colService.DisplayIndex = 1;
            colContract.DisplayIndex = 2;
            colStatus.DisplayIndex = 3;
            colStartStop.DisplayIndex = 4;
            colStartStop.Visible = false;
        }

    }
}
