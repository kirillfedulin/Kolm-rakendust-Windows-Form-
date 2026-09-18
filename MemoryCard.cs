using System.Drawing;
using System.Windows.Forms;

namespace ThreeAppsWinForms.MemoryGameApp;

public class MemoryCard
{
    public string Symbol { get; }
    public bool IsMatched { get; private set; }
    public bool IsFlipped { get; private set; }
    public Button UiButton { get; }

    public MemoryCard(string symbol, Button uiButton)
    {
        Symbol = symbol;
        UiButton = uiButton;
    }

    public void Flip()
    {
        IsFlipped = true;
        UiButton.Text = Symbol;
        UiButton.BackColor = Color.LightYellow;
    }

    public void Hide()
    {
        IsFlipped = false;
        UiButton.Text = string.Empty;
        UiButton.BackColor = SystemColors.Control;
    }

    public void MarkMatched()
    {
        IsMatched = true;
        UiButton.Enabled = false;
        UiButton.BackColor = Color.LightGreen;
    }
}
