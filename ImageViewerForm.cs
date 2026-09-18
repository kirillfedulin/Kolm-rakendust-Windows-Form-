using System;
using System.Drawing;
using System.Windows.Forms;

namespace ThreeAppsWinForms.ImageViewerApp;

public class ImageViewerForm : Form
{
    ImageManager _manager = new();

    PictureBox _pictureBox;
    Button _btnOpen;
    Button _btnPrevious;
    Button _btnNext;
    Button _btnZoomIn;
    Button _btnZoomOut;
    Button _btnRotate;
    Label _statusLabel;

    public ImageViewerForm()
    {
        Text = "Pildivaatur";
        ClientSize = new Size(800, 600);
        MinimumSize = new Size(500, 400);

        _pictureBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.CenterImage,
            BackColor = Color.Black
        };

        _statusLabel = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 26,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(6, 0, 0, 0),
            Text = "Pilte ei laaditud"
        };

        var toolPanel = new Panel { Dock = DockStyle.Top, Height = 44 };

        _btnOpen = CreateToolButton("Avatud...", 10);
        _btnPrevious = CreateToolButton("< tagasi", 130);
        _btnNext = CreateToolButton("Edasi >", 220);
        _btnZoomIn = CreateToolButton("Suurenda (+)", 320);
        _btnZoomOut = CreateToolButton("Vähenda (-)", 440);
        _btnRotate = CreateToolButton("Pööra 90°", 560);

        toolPanel.Controls.Add(_btnOpen);
        toolPanel.Controls.Add(_btnPrevious);
        toolPanel.Controls.Add(_btnNext);
        toolPanel.Controls.Add(_btnZoomIn);
        toolPanel.Controls.Add(_btnZoomOut);
        toolPanel.Controls.Add(_btnRotate);

        _btnOpen.Click += BtnOpen_Click;
        _btnPrevious.Click += (s, e) => { _manager.Previous(); RefreshImage(); };
        _btnNext.Click += (s, e) => { _manager.Next(); RefreshImage(); };
        _btnZoomIn.Click += (s, e) => { _manager.ZoomIn(); RefreshImage(); };
        _btnZoomOut.Click += (s, e) => { _manager.ZoomOut(); RefreshImage(); };
        _btnRotate.Click += (s, e) => { _manager.Rotate90(); RefreshImage(); };

        Controls.Add(_pictureBox);
        Controls.Add(toolPanel);
        Controls.Add(_statusLabel);
    }

    private static Button CreateToolButton(string text, int left)
    {
        return new Button
        {
            Text = text,
            Left = left,
            Top = 6,
            Width = 110,
            Height = 32
        };
    }

    private void BtnOpen_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "Pildid (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|Koik failid (*.*)|*.*",
            Multiselect = true,
            Title = "Valige pilt"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        foreach (var file in dialog.FileNames)
        {
            _manager.AddImage(file);
        }

        RefreshImage();
    }

    private void RefreshImage()
    {
        var previous = _pictureBox.Image;
        _pictureBox.Image = _manager.GetTransformedImage();
        previous?.Dispose();

        _statusLabel.Text = _manager.HasImages
            ? $"Pilt {_manager.CurrentIndex + 1} alates {_manager.Count} — skaala {_manager.ZoomFactor:P0}"
            : "Pilte ei laaditud";
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _pictureBox.Image?.Dispose();
        base.OnFormClosed(e);
    }
}
