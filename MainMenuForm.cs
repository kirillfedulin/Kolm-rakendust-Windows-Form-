using System;
using System.Drawing;
using System.Windows.Forms;
using ThreeAppsWinForms.MathGameApp;
using ThreeAppsWinForms.MemoryGameApp;

namespace ThreeAppsWinForms;

public class MainMenuForm : Form
{
    Label _titleLabel;
    Button _btnImageViewer;
    Button _btnMathGame;
    Button _btnMemoryGame;

    public MainMenuForm()
    {
        Text = "Kolm rakendust – peamenüü";
        ClientSize = new Size(420, 320);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        BackColor = Color.WhiteSmoke;

        _titleLabel = new Label
        {
            Text = "Valige rakendus",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 60
        };

        //_btnImageViewer = CreateMenuButton("Pildivaatur", 100);
        _btnMathGame = CreateMenuButton("Matemaatiline äraarvamismäng", 150);
        //_btnMemoryGame = CreateMenuButton("Sarnane pildiotsija mäng", 200);
        _btnMathGame.Click += (s, e) => OpenChildForm(new MathGameForm());

        //Controls.Add(_btnMemoryGame);
        Controls.Add(_btnMathGame);
        //Controls.Add(_btnImageViewer);
        Controls.Add(_titleLabel);
    }

    private static Button CreateMenuButton(string text, int top)
    {
        var button = new Button
        {
            Text = text,
            Left = 40,
            Top = top,
            Width = 340,
            Height = 40,
            Font = new Font("Segoe UI", 10F),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.SteelBlue,
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = 0;
        return button;
    }

    private static void OpenChildForm(Form form)
    {
        form.StartPosition = FormStartPosition.CenterScreen;
        using (form)
        {
            form.ShowDialog();
        }
    }
}
