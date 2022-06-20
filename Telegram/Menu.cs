public class Menu
{
    List<MenuItem> items = new List<MenuItem>();

    public MenuItem GetFirstMenuItem()
    {
        return items[0];
    }

    public MenuItem GetMenuItem(string answer)
    {
        string resultId = "";
        foreach (var item in items)
        {
            foreach (var optionAnswer in item.Options.Keys)
            {
                if (optionAnswer == answer)
                    resultId = item.Options[optionAnswer];
            }
        }

        foreach (var item in items)
        {
            if (item.Id == resultId)
                return item;
        }
        return new MenuItem() { Message = "пункт меню не найден" };
    }

    public void AddMenuItem(string id, string message)
    {
        items.Add(new MenuItem() { Id = id, Message = message });
    }

    public void AddMenuItemOption(string forMenuItemId, string toMenuItemId, string answer)
    {
        foreach (var item in items)
        {
            if (item.Id == forMenuItemId)
            {
                item.Options[answer] = toMenuItemId;
            }
        }
    }

}