namespace TxtRPG
{
    internal class Program
    {
        public class GameContext
        {
            public Start Start { get; set; }
            public Person Person { get; set; }
            public Inventory Inventory { get; set; }
            public Shop Shop { get; set; }
            
        }

        public class Item
        {
            public string Name { get; set; }
            public int Power { get; set; }
            public int Armor { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }

            public bool IsEquipped { get; set; } = false;
            public bool IsPurchased { get; set; } = false;
        }

        public class Start
        {

            public void Move(GameContext context)
            {
                int select = 0;
                string goStatus = "상태 보기";
                string goInven = "인벤토리";
                string goShop = "상점";
                string goOut = "게임 끝내기";

                while (true)
                {
                    Console.WriteLine("\n스파르타 마을에 오신 여러분 환영합니다.");
                    Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                    Console.WriteLine($"\n1. {goStatus}\n2. {goInven}\n3. {goShop}\n4. {goOut}");
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                    bool isValid = int.TryParse(Console.ReadLine(), out select);

                    if (!isValid || select < 1 || select > 4)
                    {
                        Console.WriteLine("\n잘못된 입력입니다.");
                        continue;
                    }

                    break;
                }

                switch (select)
                {
                    case 1:
                        Console.Clear();
                        context.Person.Status(context);
                        break;

                    case 2:
                        Console.Clear();
                        context.Inventory.listItem(context);
                        break;

                    case 3:
                        Console.Clear();
                        context.Shop.SaleItem(context);
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("게임을 종료합니다.");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public class Person
        {
            public int Level { get; set; }
            public string Name { get; set; }
            public string Job { get; set; }
            public int Power { get; set; }
            public int Armor { get; set; }
            public int Hp { get; set; }
            public int Gold { get; set; }

            public void Status(GameContext context)
            {
                int select = 0;

                int plusPower = context.Inventory.Items.Where(i => i.IsEquipped).Sum(i => i.Power);
                int plusArmor = context.Inventory.Items.Where(i => i.IsEquipped).Sum(i => i.Armor);

                while (true)
                {
                    Console.WriteLine("\n상태 보기");
                    Console.WriteLine("캐릭터의 정보가 표시됩니다.");

                    Console.WriteLine($"\nLv. {Level}");
                    Console.WriteLine($"이름: {Name} ({Job})");
                    Console.WriteLine($"공격력: {Power + plusPower} (+{plusPower})");
                    Console.WriteLine($"방어력: {Armor + plusArmor} (+{plusArmor})");
                    Console.WriteLine($"체력: {Hp}");
                    Console.WriteLine($"Gold: {Gold} G");

                    Console.WriteLine("\n0. 나가기");
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                    bool isValid = int.TryParse(Console.ReadLine(), out select);

                    if (!isValid || select != 0)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }

                    else
                    {
                        Console.Clear();
                        context.Start.Move(context);
                        break;
                    }
                }


            }
        }

        public class Inventory
        {
            public List<Item> Items { get; set; } = new List<Item>();

            public void listItem(GameContext context)
            {
                int select = 0;

                while (true)
                {
                    Console.WriteLine("\n인벤토리");
                    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                    Console.WriteLine("\n[아이템 목록]");

                    foreach (Item item in Items)
                    {
                        string equip = item.IsEquipped ? "[E]" : "";
                        Console.WriteLine($"- {equip} {item.Name} | {(item.Power > 0 ? $"공격력 +{item.Power}" : $"방어력 +{item.Armor}")} | {item.Description}");
                    }
                    Console.WriteLine("\n1. 장착 관리\n0. 나가기");
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                    bool isValid = int.TryParse(Console.ReadLine(), out select);

                    if (isValid && select == 1)
                    {
                        Console.Clear();
                        context.Inventory.Equip(context);
                        break;
                    }

                    else if (isValid && select == 0)
                    {
                        Console.Clear();
                        context.Start.Move(context);
                        break;
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }
                }
            }

            public void Equip(GameContext context)
            {
                int select = 0;

                while (true)
                {
                    Console.WriteLine("\n인벤토리 - 장착 관리");
                    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                    Console.WriteLine("\n[아이템 목록]");

                    for (int i = 0; i < Items.Count; i++)
                    {
                        Item item = Items[i];
                        string equip = item.IsEquipped ? "[E]" : "";
                        Console.WriteLine($"- {i + 1}. {equip}{item.Name} | {(item.Power > 0 ? $"공격력 +{item.Power}" : $"방어력 +{item.Armor}")} | {item.Description}");
                    }

                    Console.WriteLine("\n0. 나가기");
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                    bool isValid = int.TryParse(Console.ReadLine(), out select);

                    if (!isValid || select < 0 || select > Items.Count)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }

                    if (select == 0)
                    {
                        Console.Clear();
                        context.Inventory.listItem(context);
                        break;
                    }
                    Items[select - 1].IsEquipped = !Items[select - 1].IsEquipped;
                    Console.Clear();

                }
            }
        }

        public class Shop
        {
            public List<Item> Items { get; set; } = new List<Item>();

            public void SaleItem(GameContext context)
            {
                int select = 0;

                while (true)
                {
                    Console.WriteLine("\n상점");
                    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                    Console.WriteLine("\n[보유 골드]");
                    Console.WriteLine($"{context.Person.Gold} G");
                    Console.WriteLine("\n[아이템 목록]");

                    for (int i = 0; i < Items.Count; i++)
                    {
                        Item item = Items[i];
                        string status = item.IsPurchased ? "구매완료" : $"{item.Price} G";
                        Console.WriteLine($"- {item.Name} | {(item.Power > 0 ? $"공격력 +{item.Power}" : $"방어력 +{item.Armor}")} | {item.Description} | {status}");
                    }

                    Console.WriteLine("\n1. 아이템 구매\n0. 나가기");
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                    bool isValid = int.TryParse(Console.ReadLine(), out select);

                    if (isValid && select == 1)
                    {
                        Console.Clear();
                        context.Shop.BuyItem(context);
                        break;
                    }

                    else if (isValid && select == 0)
                    {
                        Console.Clear();
                        context.Start.Move(context);
                        break;
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }
                }
            }

            public void BuyItem(GameContext context)
            {
                int select = 0;

                while (true)
                {
                    Console.WriteLine("\n상점");
                    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                    Console.WriteLine("\n[보유 골드]");
                    Console.WriteLine($"{context.Person.Gold} G");
                    Console.WriteLine("\n[아이템 목록]");

                    for (int i = 0; i < Items.Count; i++)
                    {
                        Item item = Items[i];
                        string status = item.IsPurchased ? "구매완료" : $"{item.Price} G";
                        Console.WriteLine($"- {i + 1}. {item.Name} | {(item.Power > 0 ? $"공격력 +{item.Power}" : $"방어력 +{item.Armor}")} | {item.Description} | {status}");
                    }

                    Console.WriteLine("\n0. 나가기");
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

                    bool isValid = int.TryParse(Console.ReadLine(), out select);

                    if (!isValid || select < 0 || select > Items.Count)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }

                    if(select == 0)
                    {
                        Console.Clear();
                        context.Shop.SaleItem(context);
                        break;
                    }

                    Item selectedItem = Items[select - 1];
                    if (selectedItem.IsPurchased)
                    { 
                        Console.WriteLine("이미 구매한 아이템입니다.");
                        continue;
                    }
                    if (context.Person.Gold >= selectedItem.Price)
                    {
                        context.Person.Gold -= selectedItem.Price;
                        selectedItem.IsPurchased = true;
                        context.Inventory.Items.Add(selectedItem);
                        Console.WriteLine("구매를 완료했습니다.");
                    }
                    else
                    {
                        Console.WriteLine("Gold 가 부족합니다.");
                    }
                }
            }

            static void Main(string[] args)
            {
                GameContext gameContext = new GameContext()
                {
                    Person = new Person()
                    {
                        Level = 1,
                        Name = "Won",
                        Job = "전사",
                        Power = 10,
                        Armor = 5,
                        Hp = 100,
                        Gold = 1500
                    },
                    Start = new Start(),
                    Inventory = new Inventory(),
                    Shop = new Shop()
                };

                gameContext.Inventory.Items.Add(new Item()
                {
                    Name = "낡은 검",
                    Power = 1,
                    Description = "이름 그래도 낡은 검..."
                });

                gameContext.Inventory.Items.Add(new Item()
                {
                    Name = "낡은 갑옷",
                    Armor = 1,
                    Description = "이름 그래도 낡은 갑옷..."
                });

                gameContext.Shop.Items.Add(new Item()
                {
                    Name = "청동 검",
                    Power = 30,
                    Description = "청동으로 만들어진 검",
                    Price = 150
                });

                gameContext.Shop.Items.Add(new Item()
                {
                    Name = "청동 갑옷",
                    Armor = 20,
                    Description = "청동으로 만들어진 갑옷",
                    Price = 100
                });

                gameContext.Shop.Items.Add(new Item()
                {
                    Name = "스파르타 검",
                    Power = 150,
                    Description = "전설의 검",
                    Price = 300
                });

                gameContext.Shop.Items.Add(new Item()
                {
                    Name = "스파르타 갑옷",
                    Armor = 100,
                    Description = "전설의 갑옷",
                    Price = 200
                });

                gameContext.Shop.Items.Add(new Item()
                {
                    Name = "마우스",
                    Power = 999,
                    Description = "모든 것을 소멸시키는 무기",
                    Price = 800
                });

                gameContext.Shop.Items.Add(new Item()
                {
                    Name = "잠옷",
                    Armor = 999,
                    Description = "모든 것을 막아내는 잠옷",
                    Price = 650
                });

                gameContext.Start.Move(gameContext);
            }
        }
    }
}
