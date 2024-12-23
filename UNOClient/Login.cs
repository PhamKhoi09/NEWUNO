﻿using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UnoOnline
{
    public partial class Login : Form
    {
        public string CurrentUsername { get; private set; }

        public Login()
        {
            InitializeComponent();
        }

        public  void Button_DangNhap_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ TextBox
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên tài khoản và mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Chuỗi kết nối từ App.config
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ledan\source\repos\UNOOnline-86abf2d04e39fcfa3954a6642e634bf2d23a7a27\UNOClient\Database1.mdf;Integrated Security=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn lấy mật khẩu đã lưu từ bảng TaiKhoan
                    string query = "SELECT MatKhau FROM TaiKhoan WHERE TenTaiKhoan = @Username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        // Lấy mật khẩu đã lưu
                        string storedPassword = cmd.ExecuteScalar()?.ToString();

                        if (storedPassword != null && PasswordHelper.VerifyPassword(password, storedPassword))
                        {
                            // Thông báo đăng nhập thành công
                            MessageBox.Show("Đăng nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Gán tên người dùng
                            CurrentUsername = username;
                            Program.player.Name = username;
                            // Chuyển sang form Menu.cs
                            Menu menuForm = new Menu();
                            this.Hide(); // Ẩn form Login
                            menuForm.Show(); // Hiển thị form Menu
                        }
                        else
                        {
                            MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đăng nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_DangKy_Click(object sender, EventArgs e)
        {
            // Mở form đăng ký
            Register registerForm = new Register();
            registerForm.ShowDialog();
        }

        private void ForgotPasswordButton_Click_Click(object sender, EventArgs e)
        {
            // Mở form lấy lại mật khẩu
            GetPassword forgotPasswordForm = new GetPassword();
            forgotPasswordForm.ShowDialog();
        }
    }
}
