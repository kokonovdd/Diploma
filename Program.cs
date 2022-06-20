using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5303353761:AAHlIvNZ94_lE_tud_fs-iY5LDTQT3VH4Vk");

int year;
int month;
int day;
int hour;
int minute;
int second;


long chatId = 0;
string messageText = " ";
int messageId;
string firstName;
string lastName;
long id;
Message sentMessage;
string s = "Выберите пункт:"; // Ответы бота;
int level = 0; 

//----------------------//

year = int.Parse(DateTime.UtcNow.Year.ToString());
month = int.Parse(DateTime.UtcNow.Month.ToString());
day = int.Parse(DateTime.UtcNow.Day.ToString());
hour = int.Parse(DateTime.UtcNow.Hour.ToString());
minute = int.Parse(DateTime.UtcNow.Minute.ToString());
second = int.Parse(DateTime.UtcNow.Second.ToString());
Console.WriteLine("Data: " + year + "/" + month + "/" + day);
Console.WriteLine("Time: " + hour + ":" + minute + ":" + second);


using var cts = new CancellationTokenSource();

Menu menu = new Menu();

IUserService student = new UserService();

menu.AddMenuItem("M", "Меню");
menu.AddMenuItem("S1", "Мероприятия");
menu.AddMenuItem("S2", "Прачечная");
menu.AddMenuItem("S3", "Напишите номер блока");
menu.AddMenuItem("S4", "Общая информация");
menu.AddMenuItem("Q", "Задать вопрос");
menu.AddMenuItem("S11", "Мероприятие");
menu.AddMenuItem("S21", "Запись");

menu.AddMenuItemOption("M", "S1", "Мероприятия");
menu.AddMenuItemOption("M", "S2", "Прачечная");
menu.AddMenuItemOption("M", "S3", "Узнать старосту");
menu.AddMenuItemOption("M", "S4", "Общая информация");
menu.AddMenuItemOption("M", "Q", "Задать вопрос");
menu.AddMenuItemOption("S2", "M", "Меню");
menu.AddMenuItemOption("S3", "M", "Меню");
menu.AddMenuItemOption("S4", "M", "Меню");
menu.AddMenuItemOption("Q", "M", "Меню");

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { } // receive all update types
};

var events = EventService.GetAll();

botClient.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task SendMenuItem(long chatId, string prevAnswer, string textS, CancellationToken cancellationToken)
{
    MenuItem item;
    if (prevAnswer == "/start")
        item = menu.GetFirstMenuItem();
    else
        item = menu.GetMenuItem(prevAnswer);

    var rows = new List<KeyboardButton[]>();
    var cols = new List<KeyboardButton>();
    int i = 0;
    foreach (var Index in item.Options.Keys)
    {
        i++;
        cols.Add(new KeyboardButton("" + Index));
        if ((i) % 2 != 0) continue;
        rows.Add(cols.ToArray());
        cols = new List<KeyboardButton>();
    }

    if (cols.Count > 0) { rows.Add(cols.ToArray()); }

    var replyKeyboardMarkup = new ReplyKeyboardMarkup(rows.ToArray()) { ResizeKeyboard = true };

    if (prevAnswer == "Меню") textS = "Выберите пункт:";
    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: textS,
        replyMarkup: item.Options.Keys.Count > 0 ? replyKeyboardMarkup : new ReplyKeyboardRemove(),
        cancellationToken: cancellationToken);
}


//----------------------//


async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Type != UpdateType.Message)
        return;
    if (update.Message!.Type != MessageType.Text)
        return;

    messageId = update.Message.MessageId;
    firstName = update.Message.From.FirstName;
    lastName = update.Message.From.LastName;
    id = update.Message.From.Id;
    year = update.Message.Date.Year;
    month = update.Message.Date.Month;
    day = update.Message.Date.Day;
    hour = update.Message.Date.Hour;
    minute = update.Message.Date.Minute;
    second = update.Message.Date.Second;

    Console.WriteLine("\nData message --> " + year + "/" + month + "/" + day + " - " + hour + ":" + minute + ":" + second);

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId} from user:\n" + firstName + " - " + lastName + ".");



    chatId = update.Message.Chat.Id;
    messageText = update.Message.Text;

    switch (level)
    {
        case 0:
            switch (messageText)
            {
                case "Мероприятия":
                    s = "";
                    foreach (var event1 in events)
                    {
                        s = s + $"{event1.Event_Name}  {event1.Event_Date.ToLongDateString()}\n";
                        menu.AddMenuItem($"M{event1.Event_Id}", $"{event1.Event_Name}");
                        menu.AddMenuItemOption("S1", $"M{event1.Event_Id}", $"{event1.Event_Name}");
                        menu.AddMenuItemOption($"M{event1.Event_Id}", "M", "Меню");
                    }
                    menu.AddMenuItemOption("S1", "M", "Меню");
                    break;

                case "Прачечная":


                    break;

                case "Узнать старосту":
                    s = "Введите номер блока:";
                    level = 1;
                    break;

                case "Общая информация":
                    s ="Зав. общежитием: Бураковская Ольга Александровна, тел. 72–39–75\n" +
                    "адрес: г.Ижевск, ул.Майская, 23\n" +
                    "проезд:\nтроллейбусы № 1,4,7 до остановки «Майская»,\n" +
					"№ 2,14 — до остановки «Димитрова»,\n" +
                    "автобусы № 12, 14, 22, 24, 27, 50\n" +
                    "маршрутное такси № 53 до остановки «Димитрова»\n" +
                    "карта: www.igis.ru/build/27576\n\n" +
                    "После 23:00 не шуметь\n" +
                    "Белье меняют каждую среду в 421" +
                    "\n\nГрафик работы медпункта:\n9:00-18:00(Пн-Пт)\n9:00-17:00(Сб)\n";
                    break;

                case "Задать вопрос":
                    s = "Напишите вопрос:";
                    level = 2;
                    break;

                default:
                    foreach (var event1 in events)
                    {
                        if ($"{event1.Event_Name}" == messageText)
                        {
                            s = $"{event1.Event_Date.ToLongDateString()}\nРасположение: {event1.Event_Place}\n{event1.Event_Description}";
                            break;
                        }
                    }
                    break;
            }
            break;

        case 1:
            if (messageText != "Меню") 
            {
                bool result = int.TryParse(messageText , out var number);
                if (result == true)
                {
                    if (number < 17 && number > 14)
                    {
                        var b = student.EldestByBlock(number);
                        if ($"{b.User_Block}" != null)
                        {
                            s = $"Староста блока {b.User_Block} {b.User_Name}";
                        }
                        else
                            s = "Староста или блока не найден";
                    }
                    else
                        s = "Такого блока нет";
                }
                else
                    s = "Некорректное значение";
                sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: s,
                    cancellationToken: cancellationToken);
            }
            level = 0;
            messageText = "Меню";
            break;

        case 2:
            if (messageText != "Меню")
            {
                var result = PersonalService.Create(new PersonalQuestion() { personal_user = id, personal_quest = messageText });
                s = "Запрос создан.";
                sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: s,
                    cancellationToken: cancellationToken);
            }
            level = 0;
            messageText = "Меню";
            break;

        case 3:
            if (messageText != "Меню")
            {

            }
            level = 0;
            messageText = "Меню";
            break;

        default:
            break;
    }

    await SendMenuItem(chatId, messageText, s, cancellationToken);
}


Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
