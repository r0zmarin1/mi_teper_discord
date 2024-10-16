using System;
using System.Collections.Generic;

namespace mi_teper_discord;

public partial class Chat
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public string Author { get; set; } = null!;

    public override string ToString()
    {
        return $"Пользователь {Author} отправил/а: {Text}";
    }
}
