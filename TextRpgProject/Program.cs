using System;
using System.Numerics;
using System.Text.Json;
using System.Transactions;
using System.IO;
using System.Text.Json;

namespace TextRpgProject
{
    //플레이어 클래스
    class Player
    {
        private int level = 1;
        public string nickName = null;
        private string job = "전사";
        private float damage = 10;
        private int defense = 5;
        public int health = 100;
        public int gold = 10000;
        public List<Item> inventoryList = new List<Item>(); //아이템 목록을 상점 목록으로 받기
        public List<Item> equippedList = new List<Item>(); //장착한 아이템 목록

        public void ShowStatus(Player player) // 상태보기
        {
            string getDamageText = damage == GetDamage() ? null : $"(+{GetDamage() - damage})";
            string getDefenseText = defense == GetDefense() ? null : $"(+{GetDefense() - defense})";

            Console.WriteLine("==================================");
            Thread.Sleep(500);
            Console.WriteLine("상태보기");
            Thread.Sleep(500);
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

            Thread.Sleep(100);
            Console.WriteLine($"Lv.{level.ToString("D2")}");    //레벨
            Thread.Sleep(100);
            Console.WriteLine($"{nickName} ( {job} )");     //닉네임, 직업
            Thread.Sleep(100);
            Console.WriteLine($"공격력 : {GetDamage().ToString("N0")} {getDamageText}");     //공격력
            Thread.Sleep(100);
            Console.WriteLine($"방어력 : {GetDefense()} {getDefenseText}");     //방어력
            Thread.Sleep(100);
            Console.WriteLine($"체력 : {health}");     //체력
            Thread.Sleep(100);
            Console.WriteLine($"Gold : {gold} G\n");     //골드
            Thread.Sleep(100);

            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            int inputNum = int.Parse(Console.ReadLine());

            switch (inputNum)
            {
                case 0:
                    Console.WriteLine("현재 창을 나갑니다.");
                    break;
                default:
                    Console.WriteLine("잘못된 번호입니다.");
                    break;
            }
        }

        public float GetDamage() //총 공격력 계산
        {
            float totalDamage = damage;
            foreach (Item item in equippedList)
            {
                totalDamage += item.damage; //장착한 아이템의 공격력 추가
            }
            return totalDamage;
        }

        public int GetDefense() //총 방어력 계산
        {
            int totalDefense = defense;
            foreach (Item item in equippedList)
            {
                totalDefense += item.defense; //장착한 아이템의 방어력 추가
            }
            return totalDefense;
        }

        public void ShowInventory(Player player) // 인벤토리
        {
            Console.WriteLine("==================================");
            Thread.Sleep(500);
            Console.WriteLine("인벤토리");
            Thread.Sleep(500);
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

            Thread.Sleep(500);
            Console.WriteLine("[아이템 목록]");

            //아이템 목록
            for (int i = 0; i < inventoryList.Count; i++)
            {
                string effectType = inventoryList[i].damage == 0 ? "방어력" : "공격력";
                int effect = inventoryList[i].damage == 0 ? inventoryList[i].defense : inventoryList[i].damage;
                // [E] 장착 여부 확인
                string equippedMark = equippedList.Contains(inventoryList[i]) ? " [E] " : "";
                Thread.Sleep(100);
                Console.WriteLine($"- {i + 1} {equippedMark}{inventoryList[i].itemName} | {effectType} +{effect} | {inventoryList[i].itemDescription} | {inventoryList[i].itemPrice} G"); //아이템 목록
            }

            Choice(player);
        }

        public void EquipItem(Item selectedItem) //아이템 장착
        {
            // 이미 장착되어 있다면 → 해제
            if (equippedList.Contains(selectedItem))
            {
                equippedList.Remove(selectedItem);
                Console.WriteLine($"{selectedItem.itemName}을(를) 장착 해제했습니다.");
            }
            else
            {
                // 같은 타입 아이템이 이미 장착되어 있다면 → 기존 것 해제
                Item? sameTypeEquipped = equippedList.FirstOrDefault(item => item.ItemType == selectedItem.ItemType);
                if (sameTypeEquipped != null)
                {
                    equippedList.Remove(sameTypeEquipped);
                    Console.WriteLine($"{sameTypeEquipped.itemName}을(를) 장착 해제했습니다.");
                }

                // 새 아이템 장착
                equippedList.Add(selectedItem);
                Console.WriteLine($"{selectedItem.itemName}을(를) 장착했습니다.");
            }
        }

        private void Choice(Player player)
        {
            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            int inputNum = int.Parse(Console.ReadLine());

            switch (inputNum)
            {
                case 0:
                    Console.WriteLine("현재 창을 나갑니다.");
                    Console.WriteLine("==================================");
                    Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                    Console.Write(">> ");
                    break;
                case 1:
                    Console.WriteLine("아이템 번호를 입력해주세요.");
                    Console.Write(">> ");
                    int equipNum = int.Parse(Console.ReadLine()) - 1;
                    if (equipNum >= 0 && equipNum < inventoryList.Count)
                    {
                        Item SelectedItem = inventoryList[equipNum];
                        EquipItem(SelectedItem); //아이템 장착
                    }
                    break;
                default:
                    Console.WriteLine("잘못된 번호입니다.");
                    break;
            }

        }

        public void Sleep() //휴식하기
        {
            int beforeHealth = health; //휴식 전 체력
            int beforeGold = gold; //휴식 전 골드

            Console.WriteLine("==================================");
            Thread.Sleep(500);
            Console.WriteLine("휴식하기");
            Thread.Sleep(500);
            Console.WriteLine($"500 G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {this.gold} G)\n");

            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");

            Console.Write(">> ");
            int inputNum = int.Parse(Console.ReadLine());

            if (inputNum == 1 && this.gold >= 500)
            {
                Console.WriteLine($"{nickName}이/가 잠자리에 듭니다.");
                isSleeping();
                gold -= 500; //골드 차감
                health = 100; //체력 회복
                Console.WriteLine("휴식을 완료했습니다.");
                Console.WriteLine($"체력 {beforeHealth} -> {health}");
                Console.WriteLine($"Gold {beforeGold} G -> {gold} G");
            }
            else if (inputNum == 0)
            {
                Console.WriteLine("현재 창을 나갑니다.");
                Console.WriteLine("==================================");
            }
        }

        public void LevelUp() //레벨업
        {
            level++;
            damage += 0.5f;
            defense += 1;
        }

        private void isSleeping()
        {
            Random random = new Random();
            string[] sleepMessage1 =
                { "코가 삐뚤어지게 자는 중\n",
                "꿈나라에서 헤엄치는 중\n",
                "흠냐흠냐...쿨쿨쿨\n",
                "넘모 맛있게 숙면을 취하는 중\n" };

            Console.WriteLine("");
            for (int i = 0; i < sleepMessage1.Length; i++)
            {
                Console.WriteLine(sleepMessage1[random.Next(0, 4)]);
                Thread.Sleep(500);
                for (int j = 0; j < 3; j++)
                {
                    Console.WriteLine(".");
                    Thread.Sleep(500);
                }
            }
        }
    }

    //아이템 타입 열거형
    public enum ItemType
    {
        Armor,
        Weapon
    }

    //아이템 클래스
    class Item
    {
        public string itemName { get; private set; }
        public int itemPrice { get; private set; }
        public string itemDescription { get; private set; }
        public int damage { get; private set; }
        public int defense { get; private set; }
        public ItemType ItemType { get; private set; } //아이템 타입

        public Item(string name, int price, string description, int damage, int defense, ItemType itemType)
        {
            this.itemName = name;
            this.itemPrice = price;
            this.itemDescription = description;
            this.damage = damage;
            this.defense = defense;
            this.ItemType = itemType; //아이템 타입
        }
    }

    //상점 클래스
    class Shop
    {
        public List<Item> shopList = new List<Item>(); //아이템 목록을 상점 목록으로 받기


        public void ShowShop(Player player) //상점을 여는 기능
        {
            Console.WriteLine("==================================");
            Thread.Sleep(1000);
            Console.WriteLine("상점");
            Thread.Sleep(1000);
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Thread.Sleep(1000);
            Console.WriteLine("[보유골드]");
            Console.WriteLine($"{player.gold} G\n");

            Thread.Sleep(1000);
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < shopList.Count; i++)
            {
                int effect = shopList[i].damage == 0 ? shopList[i].defense : shopList[i].damage;
                string effectType = shopList[i].damage == 0 ? "방어력" : "공격력";
                bool isPurchased = player.inventoryList.Contains(shopList[i]);
                string isSoldOut = isPurchased ? " | [구매완료]" : "";
                Thread.Sleep(100);
                Console.WriteLine($"- {i + 1} {shopList[i].itemName}   | {effectType} +{effect}   | {shopList[i].itemDescription}   |  {shopList[i].itemPrice} G {isSoldOut} "); //아이템 목록
            }
            Choice(player);
        }

        public void BuyItem(Player player, Item ShopList) //물건 구매 기능
        {
            if (player.gold >= ShopList.itemPrice && !player.inventoryList.Contains(ShopList)) //골드가 충분한지 확인
            {
                player.gold -= ShopList.itemPrice; //골드 차감
                Console.WriteLine($"{ShopList.itemName}을(를) 구매하였습니다.");
                player.inventoryList.Add(ShopList); //아이템 획득
            }
            else if (player.inventoryList.Contains(ShopList))
            {
                Console.WriteLine($"{ShopList.itemName}은(는) 이미 구매한 아이템입니다.");
            }
            else
            {
                Console.WriteLine("골드가 부족합니다.");
            }
        }

        public void SellItem(Player player, Item shopList) //물건 판매 기능
        {
            if (player.inventoryList.Contains(shopList))
            {
                player.inventoryList.Remove(shopList); //아이템 판매
                player.gold += shopList.itemPrice * 85 / 100; //골드 획득
                Console.WriteLine($"{shopList.itemName}을(를) 판매하였습니다.");
            }
            else
            {
                Console.WriteLine("잘못된 번호입니다.");
            }
        }

        private void Choice(Player player)
        {
            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            int inputNum = int.Parse(Console.ReadLine());

            switch (inputNum)
            {
                case 0:
                    Console.WriteLine("현재 창을 나갑니다.");
                    Console.WriteLine("==================================");
                    Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                    Console.Write(">> ");
                    break;
                case 1:
                    Console.WriteLine("구매할 아이템 번호를 입력해주세요.");
                    Console.Write(">> ");
                    int itemNum = int.Parse(Console.ReadLine()) - 1;
                    if (itemNum >= 0 && itemNum < shopList.Count)
                    {
                        BuyItem(player, shopList[itemNum]);
                    }
                    else
                    {
                        Console.WriteLine("잘못된 번호입니다.");
                    }
                    break;
                case 2:
                    Console.WriteLine("판매할 아이템 번호를 입력해주세요.");
                    Console.Write(">> ");
                    itemNum = int.Parse(Console.ReadLine()) - 1;
                    SellItem(player, shopList[itemNum]); //판매할 아이템 번호를 입력해주세요.
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
    }

    //던전 클래스
    class Dengeon()
    {
        private int easyDungeonDefense = 5;
        private int normalDungeonDefense = 11;
        private int hardDungeonDefense = 17;

        private int easyGold = 1000;
        private int normalGold = 1700;
        private int hardGold = 2500;

        private string easyName = "쉬운 던전";
        private string normalName = "일반 던전";
        private string hardName = "어려운 던전";
        public void EnterDengeon(Player player)
        {
            Console.WriteLine("==================================");
            Thread.Sleep(1000);
            Console.WriteLine("던전입장");
            Thread.Sleep(1000);
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
            Thread.Sleep(1000);
            Console.WriteLine($"1. {easyName}\t 방어력 {easyDungeonDefense} 이상 권장");
            Thread.Sleep(100);
            Console.WriteLine($"2. {normalName}\t 방어력 {normalDungeonDefense} 이상 권장");
            Thread.Sleep(100);
            Console.WriteLine($"3. {hardName}\t 방어력 {hardDungeonDefense} 이상 권장");
            Thread.Sleep(100);
            Console.WriteLine("0. 나가기");

            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            int dengeonNum = int.Parse(Console.ReadLine());

            switch (dengeonNum)
            {
                case 0:
                    Console.WriteLine("현재 창을 나갑니다.");
                    Console.WriteLine("==================================");
                    break;
                case 1:
                    EasyDengeon(player, easyName, easyDungeonDefense, easyGold);
                    break;
                case 2:
                    NormalDengeon(player, normalName, normalDungeonDefense, normalGold);
                    break;
                case 3:
                    HardDengeon(player, hardName, hardDungeonDefense, hardGold);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }

        private void EasyDengeon(Player player, string easyName, int dengeonDefence, int dengeonGold)
        {
            if (player.GetDefense() >= dengeonDefence)
            {
                SuccessDengeon(player, easyName, dengeonDefence, dengeonGold);
            }
            else
            {
                FailDengeon(player, easyName, dengeonDefence, dengeonGold);
            }
        }

        private void NormalDengeon(Player player, string noramlName, int dengeonDefence, int dengeonGold)
        {
            if (player.GetDefense() >= dengeonDefence)
            {
                SuccessDengeon(player, noramlName, dengeonDefence, dengeonGold);
            }
            else
            {
                FailDengeon(player, noramlName, dengeonDefence, dengeonGold);
            }
        }

        private void HardDengeon(Player player, string hardName, int dengeonDefence, int dengeonGold)
        {
            if (player.GetDefense() >= dengeonDefence)
            {
                SuccessDengeon(player, hardName, dengeonDefence, dengeonGold);
            }
            else
            {
                FailDengeon(player, hardName, dengeonDefence, dengeonGold);
            }
        }

        private void SuccessDengeon(Player player, string dengeonName, int dungeonDefense, int dengeonGold)
        {
            int beforeHealth = player.health; //던전 입장 전 체력
            int beforeGold = player.gold; //던전 입장 전 골드

            Random random = new Random();

            int dengeonDamage = random.Next(20, 36);
            dengeonDamage -= player.GetDefense() - dungeonDefense;

            int goldPercent = random.Next((int)player.GetDamage(), (int)player.GetDamage() * 2);
            dengeonGold += hardGold * goldPercent / 100; //던전에서 얻는 골드

            player.health -= dengeonDamage; //체력 감소
            player.gold += dengeonGold; //골드 증가

            player.LevelUp();

            player.health = player.health < 0 ? 0 : player.health; //체력이 -로 안되게

            Fighting();
            Console.WriteLine("던전클리어");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine($"{dengeonName}을 클리어 하였습니다.\n");
            Console.WriteLine("[탐험결과]");
            Console.WriteLine($"체력 {beforeHealth} -> {player.health}");
            Console.WriteLine($"Gold {beforeGold} G -> {player.gold} G");

            GetOutOfHere(); //던전 나가기
        }

        private void FailDengeon(Player player, string dengeonName, int dungeonDefense, int dengeonGold)
        {
            Random random = new Random();
            int Possibility = random.Next(1, 11); //1~10
            if (Possibility <= 4)
            {
                int dengeonDamage = random.Next(20, 36);
                dengeonDamage += player.GetDefense() - dungeonDefense;
                int beforeHealth = player.health; //던전 입장 전 체력
                int beforeGold = player.gold; //던전 입장 전 골드
                player.health = beforeHealth / 2; //체력 감소

                player.health = player.health < 0 ? 0 : player.health; //체력이 -로 안되게

                Fighting();
                Console.WriteLine("던전 실패");
                Console.WriteLine("아쉽네요...");
                Console.WriteLine($"{dengeonName}을 실패했습니다..\n");
                Console.WriteLine("[탐험결과]");
                Console.WriteLine($"체력 {beforeHealth} -> {player.health}");
                Console.WriteLine($"Gold {beforeGold} G -> {player.gold} G");

                GetOutOfHere(); //던전 나가기
            }
            else
            {
                int beforeHealth = player.health; //던전 입장 전 체력
                int beforeGold = player.gold; //던전 입장 전 골드

                int dengeonDamage = random.Next(20, 36);
                dengeonDamage -= player.GetDefense() - dungeonDefense;

                int goldPercent = random.Next((int)player.GetDamage(), (int)player.GetDamage() * 2);
                dengeonGold += dengeonGold * goldPercent / 100; //던전에서 얻는 골드

                player.health -= dengeonDamage; //체력 감소
                player.gold += dengeonGold; //골드 증가

                player.LevelUp();

                player.health = player.health < 0 ? 0 : player.health; //체력이 -로 안되게

                Fighting();
                Console.WriteLine("던전클리어");
                Console.WriteLine("축하합니다!!");
                Console.WriteLine($"{dengeonName}을 클리어 하였습니다.\n");
                Console.WriteLine("[탐험결과]");
                Console.WriteLine($"체력 {beforeHealth} -> {player.health}");
                Console.WriteLine($"Gold {beforeGold} G -> {player.gold} G");

                GetOutOfHere(); //던전 나가기 
            }
        }

        private void GetOutOfHere()
        {
            Console.WriteLine("0. 나가기");

            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            int getOutNum = int.Parse(Console.ReadLine());
            if (getOutNum == 0)
            {
                Console.WriteLine("던전을 떠납니다.");
                Console.WriteLine("==================================");
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        private void Fighting()
        {
            Random random = new Random();

            string[] fightMessage1 =
                { "목숨을 바쳐 싸우는 중\n",
                "열심히 무기를 휘두르는 중\n",
                "상대방을 처치하는 중\n",
                "전투 결과를 기다리는 중\n" };

            Console.WriteLine("");
            for (int i = 0; i < fightMessage1.Length; i++)
            {
                Console.WriteLine(fightMessage1[random.Next(0, 4)]);
                Thread.Sleep(500);
                for (int j = 0; j < 3; j++)
                {
                    Console.WriteLine(".");
                    Thread.Sleep(500);
                }
            }
        }
    }

    internal class RPGWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("시작하시려면 아무 키나 누르세요.");
            Console.ReadKey(true); //키 입력 대기

            Player user = new Player(); //플레이어 생성
            Shop shop = new Shop(); //상점 생성
            Dengeon dengeon = new Dengeon(); //던전 생성
            SetItem(shop); //아이템 세팅
            
            Console.WriteLine("Developed by Kiahn Lee");
            ILikeDrawing(); //rpg 그림~
            SetNickName(); //닉네임 설정

            //첫 입장 인사
            Thread.Sleep(1000);
            Console.WriteLine("스파르타 마을에 오신 것을 환영합니다.");
            Thread.Sleep(1000);
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");

            //메인메뉴 반복
            while (user.health > 0)
            {
                Thread.Sleep(1000);

                Console.WriteLine("\n1. 상태보기 \n2. 인벤토리 \n3. 상점 \n4. 던전입장 \n5. 휴식하기\n\n0. 게임종료");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                int inputNum = int.Parse(Console.ReadLine());

                switch (inputNum)
                {
                    case 1:
                        user.ShowStatus(user);  //상태 창으로
                        break;
                    case 2:
                        user.ShowInventory(user);   //인벤토리로
                        break;
                    case 3:
                        shop.ShowShop(user);    //상점으로
                        break;
                    case 4:
                        dengeon.EnterDengeon(user); //던전 입력 창
                        break;
                    case 5:
                        user.Sleep(); //휴식
                        break;
                }
                if (inputNum < 0 || inputNum > 5)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                else if (inputNum == 0)
                {
                    Console.WriteLine("게임을 종료합니다...");
                    break;
                }
            }

            if (user.health <= 0) //체력이 0이하일 경우
            {
                Console.WriteLine($"{user.nickName}이/가 사망했습니다.\n게임이 종료되었습니다.");
            }

            //초기 아이템 세팅 -> 상점에 아이템 목록 추가
            void SetItem(Shop shop)
            {
                List<Item> itemList = new List<Item>(); //아이템 목록 생성

                Item apprenticeArmor = new Item("수련자 갑옷", 1000, "수련에 도움을 주는 갑옷입니다.", 0, 5, ItemType.Armor);
                Item ironArmor = new Item("무쇠갑옷", 1200, "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 9, ItemType.Armor);
                Item spartaArmor = new Item("스파르타의 갑옷", 3500, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 0, 15, ItemType.Armor);
                Item rustySword = new Item("낡은 검", 600, "쉽게 볼 수 있는 낡은 검 입니다.", 2, 0, ItemType.Weapon);
                Item bronzeAxe = new Item("청동 도끼", 1500, "어디선가 사용됐던거 같은 도끼입니다.", 5, 0, ItemType.Weapon);
                Item spartaSpear = new Item("스파르타의 창", 3500, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 7, 0, ItemType.Weapon);
                Item reservistVest = new Item("예비군 아저씨의 전투조끼", 2000, "예비군 센빠이의 짬이 느껴지는 전투조끼입니다.", 0, 20, ItemType.Armor);

                itemList.Add(apprenticeArmor); //아이템 목록에 추가
                itemList.Add(ironArmor); //아이템 목록에 추가
                itemList.Add(spartaArmor); //아이템 목록에 추가
                itemList.Add(rustySword); //아이템 목록에 추가
                itemList.Add(bronzeAxe); //아이템 목록에 추가
                itemList.Add(spartaSpear); //아이템 목록에 추가
                itemList.Add(reservistVest); //아이템 목록에 추가

                shop.shopList = itemList; //상점에 아이템 목록 추가
            }

            //닉네임 설정 메서드
            void SetNickName()
            {
                //닉네임 입력
                do
                {
                    Console.WriteLine("당신의 이름을 입력해주세요.");
                    Console.Write(">> ");
                    user.nickName = Console.ReadLine();

                    if (user.nickName == null || user.nickName == "")
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                    else if (user.nickName != null)
                    {
                        Console.WriteLine($"안녕하세요. {user.nickName}님.\n");
                        break;
                    }
                }
                while (user.nickName != null);
            }

            void ILikeDrawing()
            {
                string[] rpgArt = new string[]
          {
            "================================",
            "RRRRR   PPPPP   GGGGG",
            "R   R   P   P   G     ",
            "R   R   P   P   G     ",
            "RRRRR   PPPP    G  GG ",
            "R R     P       G   G ",
            "R  R    P       G   G ",
            "R   R   P        GGGG ",
            "================================"
          };

                foreach (string line in rpgArt)
                {
                    Console.WriteLine(line);
                    Thread.Sleep(300); // 300ms = 0.3초 지연
                }
            }
        }
    }
}
