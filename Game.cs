using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageMagick;

public struct Gladiator
{
    public double health;
    public double armor;
    public double initHealth;

    public double Attack(Gladiator opponent)
    {
        Random rand = new Random();
        double damage = rand.Next(5, 60) * (1 - opponent.armor / 100);

        opponent.health -= damage;

        if (opponent.health <= 0)
        {

            opponent.health = 0;
        }

        return opponent.health;
    }
}

namespace GladiatorUI
{
    public partial class Game : Form
    {
        private string idlePath = @"..\..\Resources\idle.gif";
        private string attackGifPath = @"..\..\Resources\attack.gif";
        private string diedGifPath = @"..\..\Resources\die.gif";

        public Gladiator Gladiator1 { get; set; }
        public Gladiator Gladiator2 { get; set; }

        public Game()
        {
            this.FormClosing += formClosed;
            InitializeComponent();
            this.Load += Game_Load;
        }

        public void InitGladiators()
        {

        }

        private void Game_Load(object sender, EventArgs e)
        {
            UpdateLabels(Gladiator1, Gladiator2);

            if (!string.IsNullOrEmpty(idlePath))
            {
                rotateGif(idlePath, pictureBox2);
            }
            else
            {
                MessageBox.Show("Путь к GIF не задан.");
            }

            Battle(Gladiator1, Gladiator2);
        }

        private void rotateGif(string path, PictureBox pictureBox)
        {
            using (var collection = new MagickImageCollection())
            {
                collection.Read(path);

                // Отзеркалить каждую GIF
                foreach (var image in collection)
                {
                    image.Rotate(180);
                    image.Flip();
                }

                // Сохранить отзеркаленный GIF во временный файл
                string tempFilePath = System.IO.Path.GetTempFileName() + ".gif";
                collection.Write(tempFilePath);

                // Загрузить отзеркаленный GIF обратно в PictureBox
                pictureBox.Image = Image.FromFile(tempFilePath);
            }
        }

        private async Task SwitchToAttackImage(PictureBox pictureBox, int attacker)
        {
            if (attacker == 1)
            {
                pictureBox.Image = Image.FromFile(attackGifPath);
            }
            else
            {
                rotateGif(attackGifPath, pictureBox);
            }

            // костыль для анимации
            await Task.Delay(1800);

            if (pictureBox == pictureBox1)
            {
                pictureBox.Image = Image.FromFile(idlePath);
            }
            else if (pictureBox == pictureBox2)
            {
                rotateGif(idlePath, pictureBox);
            }
        }

        private async void Battle(Gladiator g1, Gladiator g2)
        {
            Random rand = new Random();
            int currentAttacker = 0;

            while (g1.health > 0 && g2.health > 0)
            {
                PictureBox attackerBox;
                PictureBox defenderBox;

                if (currentAttacker == 0)
                {
                    defenderBox = pictureBox2;
                    g2.health = g1.Attack(g2);
                    currentAttacker = 1;
                    attackerBox = pictureBox1;
                }
                else
                {
                    defenderBox = pictureBox1;
                    g1.health = g2.Attack(g1);
                    attackerBox = pictureBox2;
                    currentAttacker = 0;
                }

                await SwitchToAttackImage(attackerBox, currentAttacker);
                UpdateLabels(g1, g2);

                await Task.Delay(500);
            }

            if (g1.health <= 0 && g2.health <= 0)
            {
                MessageBox.Show("Ничья!");
            }
            else if (g1.health <= 0)
            {
                pictureBox1.Image = Image.FromFile(diedGifPath);
                MessageBox.Show("Гладиатор 2 победил!");
            }
            else
            {
                rotateGif(diedGifPath, pictureBox2);
                MessageBox.Show("Гладиатор 1 победил!");
            }
        }

        private void UpdateLabels(Gladiator g1, Gladiator g2)
        {
            hp1Label.Text = $"Здоровье: {g1.health:0}\nБроня: {g1.armor}";
            hp1.Value = (int)((g1.health / g1.initHealth) * 100);

            hp2Label.Text = $"Здоровье: {g2.health:0}\nБроня: {g2.armor}";
            hp2.Value = (int)((g2.health / g2.initHealth) * 100);
        }

        private void formClosed(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
