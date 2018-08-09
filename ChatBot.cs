
using System;

/**
 * список возможных состояний запроса к БД
 */

public enum QueryStatus
{
    BAD = 0,
    OK,
    NOT_STATE
}

namespace chatBot
{
    delegate void delActiveChoice();

    class ChatBot
    {
        delActiveChoice activeChoice;

        public ChatBot() { }

        public void setChoice(delActiveChoice choice)
        {
            activeChoice = choice;
            activeChoice();
        }

    }

    class TalkingBot : ChatBot
    {
        ChatBot chatBot;
        int currentChoice;

        //статус запроса к БД на предмет корректности запроса
        int queryStatus;

        public TalkingBot()
        {
            currentChoice = 0;
            queryStatus = (int)QueryStatus.NOT_STATE;
            chatBot = new ChatBot();
        }

        //выбор, сделанный пользователем из меню
        public int CurrentChoice
        {
            get { return currentChoice; }
            set { currentChoice = value; }
        }

        //стартовая точка бота
        public void beginChoice()
        {
            Console.WriteLine(" - Привет, я чат-бот компании \"название компании\"." +
                "\nЗдесь ты можешь узнать всю необходимую информацию, но для начала, давай познакомимся." +
                "\nНам важно знать каждого клиента лично!" +
                "\nВыбери удобный для тебя вариант:" +
                "\n1.Идентифицироваться" +
                "\n2.Без идентификации");

            CurrentChoice = Convert.ToInt32(Console.ReadLine());

            switch (CurrentChoice) 
            {
                case 1:
                    chatBot.setChoice(identificateChoice);
                    break;
                case 2:
                    chatBot.setChoice(notIdentificateChoice);
                    break;
            }
        }

        //приветствие пользователя с именем
        void identificateChoice()
        {
            sayHelloName();
            chatBot.setChoice(menuChoice);
        }

        //приветствие пользователя без имени
        void notIdentificateChoice()
        {
            sayHelloNotName();
            chatBot.setChoice(menuChoice);
        }

        void menuChoice()
        {
            Console.WriteLine(" - Что тебя интересует? Выбери необходимый вариант из списка ниже:" +
                "\n\n1 Получить консультацию" +
                "\n2 Сделать заказ" +
                "\n3 Обратиться в тех.поддержку" +
                "\n4 Оставить жалобу, отзыв, предложение" +
                "\n5 Узнать об акциях и скидках" +
                "\n6 Узнать о бонусных баллах" +
                "\n7 Подписаться на рассылку новостей" +
                "\n8 Закончить диалог" +
                "\n9 Вернуться в корзину.");
            CurrentChoice = Convert.ToInt32(Console.ReadLine());

            switch (CurrentChoice)
            {
                case 2:
                    chatBot.setChoice(orderChoice);
                    break;
            }
            
        }

        /*
         * сделать заказ
         */
        void orderChoice()
        {
            Console.WriteLine(" - Выбери вариант:" +
                "\n1.Готов заказать сразу" +
                "\n2.Показать категории");

            CurrentChoice = Convert.ToInt32(Console.ReadLine());

            switch (CurrentChoice)
            {
                case 1:
                    chatBot.setChoice(orderAtOnceChoice);
                    break;
                case 2:
                    chatBot.setChoice(orderCatalogChoice);
                    break;
            }
        }

        /**
         * заказ сразу
         */
        void orderAtOnceChoice()
        {
            /**
             * проверка состояния статуса запроса к БД 
             */
            switch (this.queryStatus) {
                case 3: //NOT_STATE, не установлено
                    Console.WriteLine("- Отлично. Напиши конкретно, что мы ищем!");
                    string query = "select * from items";
                    break;
                case 2: //BAD, некорректный запрос
                    Console.WriteLine(" - Извини, я не могу ничего найти по твоему запросу. Попробуй еще раз, или выбери из списка!");
                    break;
            }

            /**
             * Здесь - обращение к БДк - Категории, подкатегории, товары. Происходит выбор товара.
             * this.queryStatus = setQueryToBD(query);
             */

            /**
             * продолжение логики
             */

            queryStatus = (int)QueryStatus.OK;

            switch (this.queryStatus)
            {
                case 1: //OK
                    chatBot.setChoice(purchaseChoice);
                    break;
                case 0: //BAD
                    chatBot.setChoice(orderAtOnceChoice);
                    break;
            }
        }

        /**
         * остальные методы описания логики
         */

        void orderCatalogChoice() { }
        void orderItemChoice() { }
        void purchaseChoice() { }

        /**
         * общение с неидентифицированным пользователем
         */
        void sayHelloNotName() { }

        /**
         * общение с идентифицированным пользователем
         */
        void sayHelloName()
        {
            Console.WriteLine("Приятно познакомиться, {0}.", getName());
        }
        string getName()
        {
            string name = "%username%";
            return name;
        }

    }
}
