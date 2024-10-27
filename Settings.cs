using System;
using System.Windows.Forms;

namespace GladiatorUI
{
    public partial class Settings : Form
    {
        private double gladiator1Health;
        private double gladiator1Armor;
        private double gladiator2Health;
        private double gladiator2Armor;

        public Settings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game gameForm = new Game();

            gameForm.Gladiator1 = new Gladiator { health = gladiator1Health, armor = gladiator1Armor, initHealth = gladiator1Health };
            gameForm.Gladiator2 = new Gladiator { health = gladiator2Health, armor = gladiator2Armor, initHealth = gladiator2Health };

            this.Hide();
            gameForm.Show();
        }

        private void hpInput1_TextChanged(object sender, EventArgs e)
        {
            gladiator1Health = ValidateHealthInput(hpInput.Text);
        }

        private void hpInput2_TextChanged(object sender, EventArgs e)
        {
            gladiator2Health = ValidateHealthInput(hpInput2.Text);
        }

        private double ValidateHealthInput(string inputText)
        {
            double health;
            if (double.TryParse(inputText, out health))
            {
                if (health > 0)
                {
                    UpdateButtonState();
                    return health;
                }
                MessageBox.Show("Значение должно быть положительным!");
            }
            return 0;
        }

        private double ValidateArmorInput(string inputText)
        {
            double armor;
            if (double.TryParse(inputText, out armor))
            {
                if (armor < 0)
                {
                    MessageBox.Show("Значение должно быть неотрицательным!");
                }
                UpdateButtonState();
                return armor;
            }
            return 0;
        }

        private void UpdateButtonState()
        {
            button1.Enabled = IsValidInputs();
        }

        private bool IsValidInputs()
        {
            return gladiator1Health > 0 && gladiator2Health > 0 &&
                   (double.TryParse(armorInput1.Text, out double armor1) && armor1 >= 0) &&
                   (double.TryParse(armorInput2.Text, out double armor2) && armor2 >= 0);
        }

        private void armorInput1_TextChanged(object sender, EventArgs e)
        {
            gladiator1Armor = ValidateArmorInput(armorInput1.Text);
        }

        private void armorInput2_TextChanged(object sender, EventArgs e)
        {
            gladiator2Armor = ValidateArmorInput(armorInput2.Text);
        }
    }
}
