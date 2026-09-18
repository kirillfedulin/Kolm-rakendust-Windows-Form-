using System;
using System.Drawing;
using System.Windows.Forms;

namespace ThreeAppsWinForms.MathGameApp;

public class MathGameForm : Form
{
    private MathQuestion _question;
    private int _score;
    private int _attempts;
    private int _timeLeft;
    private readonly System.Windows.Forms.Timer _timer;

    private readonly Label _questionLabel;
    private readonly TextBox _answerTextBox;
    private readonly Button _btnSubmit;
    private readonly Label _feedbackLabel;
    private readonly Label _scoreLabel;
    private readonly Label _timerLabel;
    private readonly ComboBox _difficultyCombo;

    public MathGameForm()
    {
        Text = "Matemaatiline äraarvamismäng";
        ClientSize = new Size(420, 300);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var difficultyLabel = new Label { Text = "Keerukus:", Left = 20, Top = 20, Width = 80 };
        _difficultyCombo = new ComboBox
        {
            Left = 100,
            Top = 17,
            Width = 180,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _difficultyCombo.Items.AddRange(new object[]
        {
            "Lihtne (+/-, kuni 10)",
            "Keskmine (+/-/*, kuni 50)",
            "Raske (+/-/*//, kuni 100)"
        });
        _difficultyCombo.SelectedIndex = 0;
        _difficultyCombo.SelectedIndexChanged += (s, e) => NewQuestion();

        _questionLabel = new Label
        {
            Left = 20,
            Top = 65,
            Width = 380,
            Height = 40,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };

        _answerTextBox = new TextBox
        {
            Left = 140,
            Top = 115,
            Width = 140,
            Font = new Font("Segoe UI", 12F),
            TextAlign = HorizontalAlignment.Center
        };
        _answerTextBox.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter) SubmitAnswer();
        };

        _btnSubmit = new Button { Text = "Vasta", Left = 160, Top = 155, Width = 100 };
        _btnSubmit.Click += (s, e) => SubmitAnswer();

        _feedbackLabel = new Label
        {
            Left = 20,
            Top = 195,
            Width = 380,
            Height = 24,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.DarkGreen
        };

        _scoreLabel = new Label { Left = 20, Top = 235, Width = 180, Text = "Kontrolli: 0 / 0" };
        _timerLabel = new Label
        {
            Left = 220,
            Top = 235,
            Width = 180,
            TextAlign = ContentAlignment.MiddleRight,
            Text = "Aega: 30"
        };

        Controls.Add(difficultyLabel);
        Controls.Add(_difficultyCombo);
        Controls.Add(_questionLabel);
        Controls.Add(_answerTextBox);
        Controls.Add(_btnSubmit);
        Controls.Add(_feedbackLabel);
        Controls.Add(_scoreLabel);
        Controls.Add(_timerLabel);

        _timer = new System.Windows.Forms.Timer { Interval = 1000 };
        _timer.Tick += Timer_Tick;

        _question = CreateQuestionForDifficulty();
        NewQuestion();
    }

    private int GetMaxNumber() => _difficultyCombo.SelectedIndex switch
    {
        0 => 10,
        1 => 50,
        _ => 100
    };

    private MathQuestion CreateQuestionForDifficulty()
    {
        int level = _difficultyCombo.SelectedIndex;
        return new MathQuestion(
            GetMaxNumber(),
            includeMultiplication: level >= 1,
            includeDivision: level == 2,
            includeSubtraction: true);
    }

    private void NewQuestion()
    {
        _question = CreateQuestionForDifficulty();
        _questionLabel.Text = _question.ToString();
        _answerTextBox.Text = string.Empty;
        _answerTextBox.Focus();
        _feedbackLabel.Text = string.Empty;

        _timeLeft = 30;
        _timerLabel.Text = $"Aega: {_timeLeft}";
        _timer.Stop();
        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _timeLeft--;
        _timerLabel.Text = $"Aega: {_timeLeft}";
        if (_timeLeft <= 0)
        {
            _timer.Stop();
            _attempts++;
            _feedbackLabel.ForeColor = Color.DarkRed;
            _feedbackLabel.Text = $"Aeg on läbi! Õige vastus: {_question.CorrectAnswer}";
            UpdateScoreLabel();
            NewQuestion();
        }
    }

    private void SubmitAnswer()
    {
        if (!int.TryParse(_answerTextBox.Text, out int userAnswer))
        {
            _feedbackLabel.ForeColor = Color.DarkRed;
            _feedbackLabel.Text = "Sisestage täisarv.";
            return;
        }

        _attempts++;
        if (_question.CheckAnswer(userAnswer))
        {
            _score++;
            _feedbackLabel.ForeColor = Color.DarkGreen;
            _feedbackLabel.Text = "Oige!";
        }
        else
        {
            _feedbackLabel.ForeColor = Color.DarkRed;
            _feedbackLabel.Text = $"Vale. Õige vastus: {_question.CorrectAnswer}";
        }

        UpdateScoreLabel();
        NewQuestion();
    }

    private void UpdateScoreLabel() => _scoreLabel.Text = $"Kontrolii: {_score} / {_attempts}";

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _timer.Stop();
        _timer.Dispose();
        base.OnFormClosed(e);
    }
}
