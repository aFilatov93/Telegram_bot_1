/*
 * Created by SharpDevelop.
 * User: afila
 * Date: 11.06.2018
 * Time: 20:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;

namespace bot790Rh
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		private static Telegram.Bot.TelegramBotClient BOT;
		
		public MainForm()
		{

			InitializeComponent();		

		}
		
		void Button1Click(object sender, EventArgs e)
		{			
			BOT = new Telegram.Bot.TelegramBotClient("565638539:AAEBhReNYzHgTMFvhiC-OYECXjlLY4g3Xto");	
			BOT.OnMessage += BotOnMessageReceived;
			BOT.StartReceiving( new UpdateType[] {UpdateType.Message} );
  			button1.Enabled = false;			
		}
		public static int flag = 1;
		private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
		{
			Telegram.Bot.Types.Message msg = messageEventArgs.Message;
			
			//создание экземпляра бота для чата
			var chatBot = new ChatBot("answersBase.txt", "dirtyBase.txt");
			
			if (msg == null || msg.Type != MessageType.Text) return;
			
			String Answer = "";
			switch (flag)
			{
				case 1:
					switch (msg.Text)
					{
						 case "/start": Answer = "/stone - твой камень\r\n/scissors - твои ножницы\r\n/paper - твоя бумага\r\n/baba - показать смачную бабу";break;
						 case "/stone": Answer = "А у меня бумага - ты проиграл"; break;
						 case "/scissors": Answer = "А у меня камень - ты проиграл"; break;
						 case "/paper": Answer = "А у меня ножницы - ты проиграл"; break;
						 case "/baba": Answer = "Вот тебе баба - http://holidaycalls.ru/wp-content/uploads/2012/07/%D0%91%D0%B0%D0%B1%D0%B0-%D0%AF%D0%B3%D0%B0-243x300.png"; break;
						 default: Answer = chatBot.Ans(msg.Text); 
						 if (chatBot.temp == false)
						 {
						 	flag = 3;
						 	chatBot.TeachQ(msg.Text);
						 }
						 break;
					}
					break;
			
			    case 2:
					
					chatBot.TeachQ(msg.Text);
					Answer = "Вопрос запомнил. Теперь введи ответ.";
					flag = 3;
					break;
				
				case 3:
					chatBot.TeachA(msg.Text);
					flag = 1;
					chatBot.temp = true;
					Answer = "Запомнил!";
					break;
			}
			await BOT.SendTextMessageAsync(msg.Chat.Id,Answer);	
			}
		
		static void chatBot_GetStr(string obj)
        {
            Console.WriteLine(obj);
        }
		
	}
}
