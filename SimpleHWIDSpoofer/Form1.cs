using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SimpleHWIDSpoofer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Text = "Sadece HWID Spoofer";
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Button btnSpoof = new Button();
            btnSpoof.Text = "Spoof HWID";
            btnSpoof.Width = 200;
            btnSpoof.Height = 50;
            btnSpoof.Top = 40;
            btnSpoof.Left = 40;
            btnSpoof.BackColor = System.Drawing.Color.MediumPurple;
            btnSpoof.ForeColor = System.Drawing.Color.White;
            btnSpoof.FlatStyle = FlatStyle.Flat;
            btnSpoof.Click += BtnSpoof_Click;

            this.Controls.Add(btnSpoof);
            this.ClientSize = new System.Drawing.Size(280, 130);
        }

        private void BtnSpoof_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", true);
                if (key == null)
                {
                    MessageBox.Show("Registry yolu bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string oldGuid = key.GetValue("MachineGuid").ToString();
                string newGuid = Guid.NewGuid().ToString();

                // Masaüstü yolunu al
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string backupPath = System.IO.Path.Combine(desktopPath, "HWID_Yedek.txt");

                // Eski HWID'yi yedekle
                System.IO.File.WriteAllText(backupPath, $"Eski MachineGuid:\n{oldGuid}");

                // Yeni HWID ile değiştir
                key.SetValue("MachineGuid", newGuid);

                MessageBox.Show($"✅ HWID değiştirildi!\n\nEski: {oldGuid}\nYeni: {newGuid}\n\nYedek: Masaüstüne kaydedildi.",
                    "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
