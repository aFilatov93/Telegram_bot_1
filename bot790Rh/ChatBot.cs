using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace bot790Rh
{
    public class ChatBot
    {
        public string q;            //вопрос 
        public string path;           //путь
        public string pathDrt;      //петь к базе с матами
        public string userAnswer;     //ответ пользователя (для обучения)

        //символы для удаления (для метода Trim)
        char[] trim = new[] 
        {
            '!','?','^','*','(',')',':','_',
            ';','.',',','>','<','\"','@','+',
            '#','№','$','%','&','-','='
        };

        List<string> samples = new List<string>();  //вопросы - ответы
        List<string> dirtyTalk = new List<string>();    //список матов
        public int flag = 2;   //переключатель учеба/работа

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pat"></param>
        public ChatBot(string pat, string drt)
        {
            path = pat;
            pathDrt = drt;
            try
            {
                samples.AddRange(File.ReadAllLines(path));
                dirtyTalk.AddRange(File.ReadAllLines(drt));
            }
            catch
            {

            }
        }

        private string Answer(string qw)
        {

            string ans = string.Empty;

            qw = qw.ToLower();
            qw = Trim(qw);

            for (int i = 0; i < samples.Count; i += 2)
            {
                if (qw == samples[i])
                {
                    ans = samples[i + 1];
                    break;
                }
            }

            return (ans);
        }

        /// <summary>
        /// Обучение
        /// </summary>
        public void TeachQ(string s)
        {
            //добавление вопроса
            samples.Add(s);
            //сохранение
            File.WriteAllLines(path, samples.ToArray());
        }
        
        public void TeachA(string s)
        {
            //ответ
            samples.Add(s);
            //сохранение
            File.WriteAllLines(path, samples.ToArray());
        }
        public bool temp = true;
        //генерация ответа
        public string Ans(string qw)
        {
                string ans = string.Empty; //ответ бота
                qw = qw.ToLower();  //перевод в нижний регистр
                q = Trim(qw);   //удаление симоволов
                ans = Answer(qw);

                if (!(AntiDirtyTalk(q)))
                {
                    if (ans == string.Empty)
                    {
                    	temp = false;
                        return "Вопрос запомнил. Теперь введи ответ.";
                    }
                    else return ans;
                }
                else return "Не ругайся! " + ans;
        }

        //удаление символов
        public string Trim(string str)
        {
            string strA = str;

            for (int i = 0; i < trim.Length; i++)
            {
                strA = strA.Replace(char.ToString(trim[i]), "");
            }

            return strA;
        }

        //детектор мата
        bool AntiDirtyTalk(string inputString)
        {
            //разделение полученой строки в слова
            string[] words = Trim(inputString).Split();

            //поиск матов
            foreach (string str in dirtyTalk)
            {
                foreach (string str2 in words)
                    if (str == str2)
                    {
                        //если мат найден
                        return true;
                    }
            }
            //если мат не найден
            return false;
        }
    }
}
