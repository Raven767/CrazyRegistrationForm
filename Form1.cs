using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace КП
{
    public partial class MainForm : Form
    {
        private Random random = new Random();
        private List<string> previousLogins = new List<string>();
        private bool isFlashlightOn = false;
        private SoundPlayer soundPlayer = new SoundPlayer();

        public MainForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // 1. Базовая форма регистрации
            this.Text = "СУПЕР-СЛОЖНАЯ РЕГИСТРАЦИЯ 3000";
            this.BackColor = Color.HotPink;
            this.ClientSize = new Size(800, 600);

            // 5. Странный дизайн
            this.BackgroundImage = Properties.Resources.JLabL4n1ZQg;
            this.BackgroundImageLayout = ImageLayout.Tile;

            // 3. Поля с ограничениями и подсказками
            var lblLogin = new Label { Text = "Ваш ЛОГИН (только цифры и буквы K, Y, Z):", Location = new Point(50, 50), ForeColor = Color.Lime, Size = new Size(250, 20) };
            var txtLogin = new TextBox { Location = new Point(50, 70), Width = 200, Tag = "login" };
            txtLogin.TextChanged += ValidateLogin;

            // 6. Странный способ ввода пароля
            var lblPassword = new Label { Text = "Пароль (используйте ползунок):", Location = new Point(50, 120), ForeColor = Color.Yellow, Size = new Size(180, 20) };
            var trackPassword = new TrackBar { Location = new Point(50, 140), Minimum = 0, Maximum = 10000, TickFrequency = 100 };

            var txtPassword = new TextBox { Location = new Point(50, 180), Width = 200, ReadOnly = true, Tag = "password" };

            trackPassword.Scroll += (s, e) => txtPassword.Text = $"P@ssw0rd{trackPassword.Value}";
            // 7. Странная капча
            var lblCaptcha = new Label { Text = "Капча: Сколько будет 2+2?", Location = new Point(50, 230), Size = new Size(150, 20) };
            var txtCaptcha = new TextBox { Location = new Point(50, 250), Width = 200, Tag = "captcha" };

            // 8. Подвижные элементы
            var btnSubmitReal = new Button { Text = "Отправить", Location = new Point(400, 400), Width = 100 };
            var btnSubmitFake = new Button { Text = "Отправить", Location = new Point(550, 400), Width = 100 };

            // 9. Угадай кнопку
            btnSubmitReal.Click += SubmitForm;
            btnSubmitFake.Click += (s, e) => {
                btnSubmitFake.Location = new Point(random.Next(300, 500), random.Next(350, 450));
                MessageBox.Show("Не та кнопка! Попробуйте другую!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            // 10. Надоедливые баннеры
            var annoyingLabel = new Label { Text = "КУПИТЕ НАШ КУРС!", Location = new Point(300, 50), ForeColor = Color.Red, Font = new Font("Arial", 16) };
            this.Controls.Add(annoyingLabel);

            // Таймер для движения баннера
            var timer = new System.Windows.Forms.Timer { Interval = 3000 };
            timer.Tick += (s, e) => {
                annoyingLabel.Location = new Point(random.Next(0, 600), random.Next(0, 200));
                annoyingLabel.ForeColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            };
            timer.Start();

            // 11. Фонарик
            var btnFlashlight = new Button { Text = "Включить фонарик", Location = new Point(50, 350), Size = new Size(150, 20) };
            btnFlashlight.Click += (s, e) => {
                isFlashlightOn = !isFlashlightOn;
                this.Invalidate();
            };

            // 12. Генератор пароля
            var btnGeneratePassword = new Button { Text = "Сгенерировать пароль", Location = new Point(50, 300) };
            btnGeneratePassword.Click += (s, e) => {
                string chars = "!@#$%^&*()_+-=[]{};':\"\\|,.<>/?";
                string password = "";
                for (int i = 0; i < 12; i++)
                {
                    password += chars[random.Next(chars.Length)];
                }
                txtPassword.Text = password;
                MessageBox.Show($"Ваш новый пароль: {password}\nЗапомните его и введите вручную!", "Генератор пароля");
            };

            // 13. Игра для разблокировки
            var gamePanel = new Panel { Location = new Point(400, 100), Size = new Size(300, 200), BackColor = Color.Black };
            var gameLabel = new Label { Text = "Поймайте 5 красных квадратов!", ForeColor = Color.White, Dock = DockStyle.Top };
            gamePanel.Controls.Add(gameLabel);

            int squaresCaught = 0;
            for (int i = 0; i < 10; i++)
            {
                var square = new Button
                {
                    Size = new Size(20, 20),
                    BackColor = random.Next(2) == 0 ? Color.Red : Color.Blue,
                    Location = new Point(random.Next(0, 280), random.Next(20, 180))
                };
                square.Click += (s, e) => {
                    if ((s as Button).BackColor == Color.Red)
                    {
                        squaresCaught++;
                        (s as Button).Visible = false;
                        if (squaresCaught >= 5)
                        {
                            gameLabel.Text = "Успех! Форма разблокирована!";
                            txtLogin.Enabled = true;
                            txtPassword.Enabled = true;
                        }
                    }
                    else
                    {
                        PlayErrorSound();
                    }
                };
                gamePanel.Controls.Add(square);
            }

            // 16. Нерабочий функционал
            var btnHelp = new Button { Text = "Помощь", Location = new Point(700, 10) };
            btnHelp.Click += (s, e) => {
                var helpForm = new Form { Text = "Помощь", Size = new Size(300, 200) };
                helpForm.Controls.Add(new Label
                {
                    Text = "Для получения помощи позвоните по номеру:\n+7 (XXX) XXX-XX-XX",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                });
                helpForm.ShowDialog();
            };

            // Добавляем все элементы на форму
            this.Controls.AddRange(new Control[] {
                lblLogin, txtLogin,
                lblPassword, trackPassword, txtPassword,
                lblCaptcha, txtCaptcha,
                btnSubmitReal, btnSubmitFake,
                btnFlashlight,
                btnGeneratePassword,
                gamePanel,
                btnHelp
            });

            // 4. Сохранение данных при ошибках
            this.FormClosing += (s, e) => {
                if (!string.IsNullOrEmpty(txtLogin.Text))
                    previousLogins.Add(txtLogin.Text);
            };
        }

        private void ValidateLogin(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            string validChars = "0123456789KYZkyz";

            // 3.1 Подсказки при вводе
            if (textBox.Text.Length > 0 && !validChars.Contains(textBox.Text[textBox.Text.Length - 1].ToString()))
            {
                PlayErrorSound();
                textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
                textBox.SelectionStart = textBox.Text.Length;

                var toolTip = new ToolTip();
                toolTip.Show("Можно вводить только цифры и буквы K,Y,Z!", textBox, 0, -20, 2000);
            }
        }

        private void SubmitForm(object sender, EventArgs e)
        {

            // 7. Проверка капчи
            if (this.txtCaptcha.Text != "4") // Теперь обращаемся к полю класса
            {
                MessageBox.Show("Неверная капча! Попробуйте еще раз.", "Ошибка");
                PlayErrorSound();
                return;
            }


            // 12. Проверка пароля
            if (txtPassword.Text.Length < 8)
            {
                MessageBox.Show("Пароль должен содержать минимум 8 символов!", "Ошибка");
                return;
            }

            // Если все проверки пройдены
            MessageBox.Show("Регистрация успешна!\nНо ваш аккаунт будет удален через 5 минут.", "Успех");
            this.Close();
        }

        private void PlayErrorSound()
        {
            try
            {
                //soundPlayer.Stream = Properties.Resources.error_sound;
                soundPlayer.Play();
            }
            catch { }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 11. Эффект фонарика
            if (isFlashlightOn)
            {
                using (var brush = new SolidBrush(Color.FromArgb(200, Color.Black)))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);

                    Point cursorPos = this.PointToClient(Cursor.Position);
                    int radius = 100;
                    e.Graphics.ExcludeClip(new Rectangle(cursorPos.X - radius, cursorPos.Y - radius, radius * 2, radius * 2));
                    e.Graphics.Clear(Color.Black);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isFlashlightOn)
                this.Invalidate();
        }
    }
}