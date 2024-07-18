using System;
using System.Diagnostics;
using static ConsoleProjectTest.Program;

namespace ConsoleProjectTest
{
    internal class Program
    {


        //씬 이동
        public enum Scene
        {
            Select, Confirm, Town, Hunt, SHOP
        }

        //무기 타입
        public enum Weapon
        {
            Knife = 1, Axe, Bat
        }

        //직업 타입
        public enum Job
        {
            Adventure, Warrior, Mage, Rouge, Archer
        }
         
        //몬스터 타입
        public enum MonsterType
        {
            GreenSnail, BlueSnail, RedSnail
        }

        //게임 관련 데이터
        public struct GameData
        {
            public int monsterIndex;

            public bool running;
            public bool[,] townMap;

            public Scene scene;

        }

        //무기 정보
        public struct WeaponItem
        {
            public int dmg;
            public string name;
        }

        //아이템 정보
        public struct Item
        {
            public string itemName;
            public int itemCount;

        }

        //몬스터 정보
        public struct Monster
        {
            public string name;
            public int posX;
            public int posY;
            public int curHp;
            public int maxHp;
            public int getExp;
            public int monsterDmg;
            public bool monsterLife;
            public MonsterType type;
        }

        //플레이어 정보
        public struct Player
        {
            public string name;
            public int posX;
            public int posY;
            public int level;
            public int exp;
            public int curHp;
            public int maxHp;
            public int strAbility;
            public int dexAbility;
            public int intAbility;
            public int luckAbility;
            public int meso;
            public bool startItem;
            public bool isState;
            public int playerDmg;
            public bool playerLife;

            public Item[] itemList;
            public WeaponItem weaponItem;
            public Job curJob;
            public Weapon useWeapon;


        }

        //게임 데이터 변수
        static GameData gamedata;

        //플레이어 변수
        static Player player;

        //던전 입구 변수 
        static Player Dgate;

        //마을 입구 변수
        static Player townGate;

        //몬스터 관리 배열
        static Monster[] monster;

        static void Main(string[] args)
        {
            Random ran = new Random();


            Start();

            while (gamedata.running)
            {
                Run();
                MoveHuntMap();      
                MoveTownMap();
                MonsterBattle();
                LevelUp();

                if (player.isState)
                {
                    MonsterRandPos();
                }

            }


        }

        
        static void ConsoleSetting()
        {
            Console.Title = "MapleStory Console";

            Console.CursorVisible = false;
            Console.SetWindowSize(70, 55);
        }

        static void Start()
        {

            ConsoleSetting();

            gamedata = new GameData();
            player = new Player();
            Dgate = new Player();
            townGate = new Player();
            monster = new Monster[3];

            gamedata.running = true;
            
            
            TownMap();  //맵 배열 생성

            player.posX = 21;   //플레이어 X 좌표
            player.posY = 13;   //플레이어 Y 좌표

            Dgate.posX = 25;    //던전입구 x좌표
            Dgate.posY = 8;     //던전입구 y좌표

            townGate.posX = 25;     //마을입구 x 좌표
            townGate.posY = 20;     //마을입구 y 좌표

            player.level = 1;       //플레이어 시작 레벨 설정
            player.exp = 0;         //플레이어 경험치
            player.curHp = 100;     //플레이어 현재 체력
            player.maxHp = 100;     //플레이어 최대 체력

            MonsterInfo();      //몬스터 정보 생성

            Console.Clear();

            Console.WriteLine("\t\t\t     MapleStory Console");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\t\t계속하려면 아무키나 누르세요");
            Console.ReadKey(true);

        }

        static void Run()
        {
 
            switch (gamedata.scene)
            {
                case Scene.Select:
                    CharacterCreate();      //캐릭터 생성 진행
                    break;

                case Scene.Confirm:
                    ShowProfile();          //캐릭터 생성 완료 확인
                    break;

                case Scene.Town:
                    Town();                 //마을 랜더링
                    PlayerPrint();          //플레이어 랜더링
                    DGate();                //던전입구 랜더링
                    Move();                 //키 입력 확인
                    break;

                case Scene.Hunt:
                    HuntMap();              //던전 랜더링
                    MonsterPrint();         //몬스터 랜더링
                    PlayerPrint();          //플레이어 랜더링
                    TownGate();             //마을입구 랜더링
                    Move();                 //키 입력 확인
                    break;

            }

        }

        static ConsoleKey Input()
        {
            ConsoleKey pKey = Console.ReadKey(true).Key;

            return pKey;
        }

        /// <summary>
        /// 캐릭터 생성 함수
        /// </summary>
        static void CharacterCreate()
        {
            //hp, 레벨, 경험치, 메소, 현재 직업, 무기 데미지 설정 필요

            Console.Clear();
            //플레이어 닉네임 설정 함수 호출
            PlayerNickname();

            //시작 무기 설정 함수 호출
            PlayerStartWeapon();

            //능력치 랜덤 주사위 함수 호출
            PlayerStats();
        }

        /// <summary>
        /// Player 닉네임 설정
        /// </summary>
        static void PlayerNickname()
        {

            //닉네임 입력 체크
            do
            {
                Console.Clear();
                Console.Write("사용할 닉네임을 입력하세요. :");
                player.name = Console.ReadLine();


            } while (player.name == "");

        }

        /// <summary>
        /// 시작 무기 설정 함수
        /// </summary>
        static void PlayerStartWeapon()
        {
            int weapon;
            bool stringCheck;
            bool weaponCheck;
            ConsoleKey key;
            player.weaponItem = new WeaponItem();

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("----- 사용할 무기를 선택해주세요. -----\n");
            Console.WriteLine("1. 모험자의 칼");
            Console.WriteLine("2. 모험자의 도끼");
            Console.WriteLine("3. 모험자의 방망이");


            //1 ~ 3 무기 선택 체크
            do
            {
                Console.Write($"\n선택할 무기 :");
                stringCheck = int.TryParse(Console.ReadLine(), out weapon);

                weaponCheck = Enum.IsDefined(typeof(Weapon), weapon);

            } while (!weaponCheck || !stringCheck);

            //현재 내 무기에 선택한 무기 대입
            player.useWeapon = (Weapon)weapon;

            switch (player.useWeapon)
            {
                case Weapon.Knife:
                    player.weaponItem.name = "모험자의 칼";
                    player.weaponItem.dmg = 7;
                    Console.WriteLine($"\n{player.weaponItem.name}을 선택하셨습니다.\n");
                    Wait(1.5f);
                    break;

                case Weapon.Axe:
                    player.weaponItem.name = "모험자의 도끼";
                    player.weaponItem.dmg = 12;
                    Console.WriteLine($"\n{player.weaponItem.name}를 선택하셨습니다.\n");
                    Wait(1.5f);

                    break;

                case Weapon.Bat:
                    player.weaponItem.name = "모험자의 방망이";
                    player.weaponItem.dmg = 9;
                    Console.WriteLine($"\n{player.weaponItem.name}를 선택하셨습니다.\n");
                    Wait(1.5f);
                    break;
            }
        }

        /// <summary>
        /// 플레이어 능력치 랜덤 설정 함수
        /// </summary>
        static void PlayerStats()
        {
            Console.Clear();

            Random random = new Random(Environment.TickCount);
            ConsoleKey key;
            int allAbilityStats = 25;
            int abilityStatsTotal = 0;
            int[] allAbilityStatsArr = new int[4];

            do
            {
                while (true)
                {
                    {
                        //난수의 값들을 배열에 저장
                        allAbilityStatsArr[0] = random.Next(4, 12);
                        allAbilityStatsArr[1] = random.Next(4, 12);
                        allAbilityStatsArr[2] = random.Next(4, 12);
                        allAbilityStatsArr[3] = random.Next(4, 12);

                        for (int i = 0; i < allAbilityStatsArr.Length; i++)
                        {
                            //난수가 저장된 배열의 값들을 난수의 합산 변수에 저장
                            abilityStatsTotal += allAbilityStatsArr[i];
                        }

                        //난수의 합산이 저장된 변수가 능력치 총량 25와 같으면 반복문 탈출
                        if (abilityStatsTotal == allAbilityStats)
                        {
                            break;
                        }
                        else
                        {
                            //합산된 값이 25보다 작거나 크면 저장된 변수를 0으로 초기화
                            abilityStatsTotal = 0;
                        }
                    }
                }

                //각 능력치에 난수값 저장
                player.strAbility = allAbilityStatsArr[0];
                player.dexAbility = allAbilityStatsArr[1];
                player.intAbility = allAbilityStatsArr[2];
                player.luckAbility = allAbilityStatsArr[3];


                Console.WriteLine("---- 주사위 결과 ----\n");
                Console.WriteLine($"STR :{player.strAbility}");
                Console.WriteLine($"DEX :{player.dexAbility}");
                Console.WriteLine($"INT :{player.intAbility}");
                Console.WriteLine($"LUCK :{player.luckAbility}");

                Console.WriteLine("\n능력치를 다시 돌리겠습니까?\n");
                Console.WriteLine("[Y]   [N]");

                key = Input();

                //'n' or 'N' 을 누르면 스탯 확정
                if (key == ConsoleKey.N)
                {
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.Clear();
                }

            } while (true);


            //캐릭터 생성 확인 페이지 이동
            gamedata.scene = Scene.Confirm;

        }

        /// <summary>
        /// 생성 캐릭터 확인 함수
        /// </summary>
        static void ShowProfile()
        {


            Console.WriteLine("\n\n----- 캐릭터 생성 확인 -----");
            Console.WriteLine($"닉네임 : {player.name}");
            Console.WriteLine($"무기 : {player.weaponItem.name}");
            Console.WriteLine($"STR :{player.strAbility}");
            Console.WriteLine($"DEX :{player.dexAbility}");
            Console.WriteLine($"INT :{player.intAbility}");
            Console.WriteLine($"LUCK :{player.luckAbility}");


            Console.WriteLine("\n\n\n");
            Console.WriteLine("이대로 캐릭터를 생성하시겠습니까?\n");
            Console.WriteLine("[Y]   [N]");
            //ConsoleKeyInfo key = Console.ReadKey(true);
            ConsoleKey key = Input();

            switch (key)
            {
                case ConsoleKey.Y:
                    player.itemList = new Item[5];
                    
                    StartItem();
                    PlayerDmg();
                    Wait(1);
                    
                    Console.WriteLine("마을로 이동합니다.");
                    Wait(2);

                    gamedata.scene = Scene.Town;

                    break;

                case ConsoleKey.N:
                    gamedata.scene = Scene.Select;
                    break;

                default:
                    gamedata.scene = Scene.Confirm;
                    break;
            }

        }

        static void Wait(float seconds)
        {
            Thread.Sleep((int)(seconds * 1000));
        }

        /// <summary>
        /// 던전에서 마을 이동 함수
        /// </summary>
        static void MoveTownMap()
        {
            if (player.posX == townGate.posX && player.posY == townGate.posY)
            {

                gamedata.scene = Scene.Town;
            }
        }

        /// <summary>
        /// 맵 테두리 배열
        /// </summary>
        static void TownMap()
        {
            gamedata.townMap = new bool[,]
            {
               {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false},
               {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false}
            };

        }

        static void Town()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\t===== 헤네시스 마을 =====\n\n");
            player.isState = true;

            for (int y = 0; y < gamedata.townMap.GetLength(0); y++)
            {
                Console.Write("\t\t");
                for (int x = 0; x < gamedata.townMap.GetLength(1); x++)
                {
                    if (gamedata.townMap[y, x])
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("#");
                    }
                }

                Console.WriteLine();
            }
            player.playerLife = true;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            //내정보 및 인벤토리 정보 하단 노출
            Console.WriteLine("=== 내 정보 ===");
            Console.WriteLine($"Lv : {player.level}");
            Console.WriteLine($"Hp : {player.curHp}");
            Console.WriteLine($"Exp : {player.exp}");
            Console.WriteLine($"닉네임 : {player.name}");
            Console.WriteLine($"착용 무기 : {player.weaponItem.name}");
            Console.WriteLine($"총 공격력 : {player.playerDmg}");
            Console.WriteLine($"STR : {player.strAbility}");
            Console.WriteLine($"DEX : {player.dexAbility}");
            Console.WriteLine($"INT : {player.intAbility}");
            Console.WriteLine($"LUCK : {player.luckAbility}");

            Console.WriteLine();
            Console.WriteLine($"보유 메소 : {player.meso}");

            for (int i = 0; i < player.itemList.Length; i++)
            {
                if (player.itemList[i].itemName == null || player.itemList[i].itemCount == 0)
                {
                    Console.WriteLine($"{i + 1}번 인벤토리 :없음");
                }
                else
                {
                    Console.WriteLine($"{i + 1}번 인벤토리 :{player.itemList[i].itemName} {player.itemList[i].itemCount}개");
                }

            }


        }

        /// <summary>
        /// 플레이어 공격 계산 함수
        /// </summary>
        static void PlayerDmg()
        { 
            player.playerDmg =  player.strAbility + (int)(player.weaponItem.dmg * 0.3);
        }


        /// <summary>
        /// 던전으로 이동 시키는 함수
        /// </summary>
        static void MoveHuntMap()
        {


            if (player.posX == Dgate.posX && player.posY == Dgate.posY)
            {
                MonsterRandIndex();
                gamedata.scene = Scene.Hunt;

            }

        }


        /// <summary>
        /// 사냥터 그려주는 함수
        /// </summary>
        static void HuntMap()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\t===== 헤네시스 사냥터 1 =====\n\n");
            player.isState = false;

            for (int y = 0; y < gamedata.townMap.GetLength(0); y++)
            {
                Console.Write("\t\t");
                for (int x = 0; x < gamedata.townMap.GetLength(1); x++)
                {
                    if (gamedata.townMap[y, x])
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("#");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            //내정보 및 인벤토리 정보를 하단 노출
            Console.WriteLine("=== 내 정보 ===");
            Console.WriteLine($"Lv : {player.level}");
            Console.WriteLine($"Hp : {player.curHp}");
            Console.WriteLine($"Exp : {player.exp}");
            Console.WriteLine($"닉네임 : {player.name}");
            Console.WriteLine($"착용 무기 : {player.weaponItem.name}");
            Console.WriteLine($"총 공격력 : {player.playerDmg}");
            Console.WriteLine($"STR : {player.strAbility + (player.weaponItem.dmg * 0.3)} ( + 무기 데미지)");
            Console.WriteLine($"DEX : {player.dexAbility}");
            Console.WriteLine($"INT : {player.intAbility}");
            Console.WriteLine($"LUCK : {player.luckAbility}");

            Console.WriteLine();
            Console.WriteLine($"보유 메소 : {player.meso}");
            for (int i = 0; i < player.itemList.Length; i++)
            {
                if (player.itemList[i].itemName == null || player.itemList[i].itemCount == 0)
                {
                    Console.WriteLine($"{i + 1}번 인벤토리 :없음");
                }
                else
                {
                    Console.WriteLine($"{i + 1}번 인벤토리 :{player.itemList[i].itemName} {player.itemList[i].itemCount}개");
                }

            }

        }

        static void MonsterBattle()
        {
            ConsoleKey key;
            bool isFight;

            if (player.posX == monster[gamedata.monsterIndex].posX && player.posY == monster[gamedata.monsterIndex].posY && !player.isState)
            {
                Console.WriteLine($"{monster[gamedata.monsterIndex].name}몬스터랑 만났어요 싸우겠어요?");
                Console.Write("[Y] or [N] : ");
                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Y:
                        isFight = true;
                        Console.WriteLine("몬스터랑 싸워요!");

                        do
                        {
                            //몬스터의 피가 0보다 크고 플레이어가 살아있으면 공격
                            if (monster[gamedata.monsterIndex].curHp > 0 && player.playerLife)
                            {
                                monster[gamedata.monsterIndex].curHp -= player.playerDmg;
                                
                                //피격후 몬스터 체력이 0보다 아래로 떨어지면 승리
                                if(monster[gamedata.monsterIndex].curHp < 0)
                                {
                                    monster[gamedata.monsterIndex].monsterLife = false;
                                    player.isState = true;
                                    Console.WriteLine("몬스터가 죽었습니다.");
                                    Console.WriteLine("플레이어 승리 !");
                                    monster[gamedata.monsterIndex].curHp = monster[gamedata.monsterIndex].maxHp;
                                    player.exp += monster[gamedata.monsterIndex].getExp;
                                    player.meso += 15;
                                    isFight = false;

                                }
                                  
                            }

                            //플레이어의 피가 0보다 크고 몬스터가 살아있으면 공격
                            if (player.curHp > 0 && monster[gamedata.monsterIndex].monsterLife)
                            { 
                                player.curHp -= monster[gamedata.monsterIndex].monsterDmg;
                                
                                if(player.curHp <= 60)
                                {
                                    Console.WriteLine($"현재 체력이 {player.curHp} 만큼 남았습니다.");
                                    Console.WriteLine("계속 싸우시겠습니까 ? [Y]  [N]");
                                    key = Console.ReadKey(true).Key;
                                     
                                    switch (key)
                                    {
                                        case ConsoleKey.Y:
                                            isFight = true;
                                            break;

                                        case ConsoleKey.N:
                                            monster[gamedata.monsterIndex].curHp = monster[gamedata.monsterIndex].maxHp;
                                            isFight = false;
                                            break;
                                    }

                                }

                                if(player.curHp < 0)
                                {
                                    player.playerLife = false;
                                    isFight = false;
                                    monster[gamedata.monsterIndex].curHp = monster[gamedata.monsterIndex].maxHp;
                                    player.curHp = 30;
                                    Console.WriteLine("당신은 죽었습니다.");
                                    Wait(1);

                                    gamedata.scene = Scene.Town;
                                }

                            }
           

                            Wait(1);

                        } while (isFight);
 
                        break;

                    case ConsoleKey.N:
                        Console.WriteLine("싸움에서 도망갔어요");
                        Wait(1);
                        break;
                }
                 
            } 
        }

        /// <summary>
        /// 플레이어 그려주는 함수
        /// </summary>
        static void PlayerPrint()
        {
            Console.SetCursorPosition(player.posX, player.posY);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("A");
            Console.ResetColor();
        }

        /// <summary>
        /// 마을에서 던전으로 들어가는 입구를 그려주는 함수
        /// </summary>
        static void DGate()
        {
            Console.SetCursorPosition(Dgate.posX, Dgate.posY);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("D");
            Console.ResetColor();
        }

        /// <summary>
        /// 몬스터의 정보 함수
        /// </summary>
        static void MonsterInfo()
        {
            monster[0].type = (MonsterType)0;
            monster[0].name = "초록 달팽이";
            monster[0].curHp = 20;
            monster[0].maxHp = 20;
            monster[0].monsterDmg = 30;
            monster[0].monsterLife = true;
            monster[0].getExp = 30;

            monster[1].type = (MonsterType)1;
            monster[1].name = "파란 달팽이";
            monster[1].curHp = 30;
            monster[1].maxHp = 30;
            monster[1].monsterDmg = 30;
            monster[1].monsterLife = true;
            monster[1].getExp = 40;


            monster[2].type = (MonsterType)2;
            monster[2].name = "빨간 달팽이";
            monster[2].curHp = 50;
            monster[2].maxHp = 50;
            monster[2].monsterDmg = 30;
            monster[2].monsterLife = true;
            monster[2].getExp = 50;
        }


        /// <summary>
        /// 몬스터의 생성 위치를 랜덤으로 잡아주는 함수
        /// </summary>
        static void MonsterRandPos()
        {
            Random random = new Random();
            int posX = 0;
            int posY = 0;

            do
            {
                posX = random.Next(17, 38);
                posY = random.Next(7, 21);

                if (!(Dgate.posX == posX && townGate.posX == posX) && !(Dgate.posY == posY && townGate.posY == posY))
                {
                    monster[0].posX = posX;
                    monster[0].posY = posY;

                    monster[1].posX = posX;
                    monster[1].posY = posY;

                    monster[2].posX = posX;
                    monster[2].posY = posY;
                }

            } while ((Dgate.posX == posX && townGate.posX == posX) && (Dgate.posY == posY && townGate.posY == posY));
        }

        /// <summary>
        /// 몬스터 랜덤 생성을 위한 인덱스 랜덤 함수
        /// </summary>
        static void MonsterRandIndex()
        {
            Random rand = new Random();
            gamedata.monsterIndex = rand.Next(0, 3);

        }

        /// <summary>
        /// 몬스터 별로 다르게 표시하는 함수
        /// </summary>
        static void MonsterPrint()
        {


            if (monster[gamedata.monsterIndex].type == (MonsterType)0)
            {
                Console.SetCursorPosition(monster[gamedata.monsterIndex].posX, monster[gamedata.monsterIndex].posY);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("M");
                Console.ResetColor();
            }

            else if (monster[gamedata.monsterIndex].type == (MonsterType)1)
            {
                Console.SetCursorPosition(monster[gamedata.monsterIndex].posX, monster[gamedata.monsterIndex].posY);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("M");
                Console.ResetColor();
            }
            else
            {
                Console.SetCursorPosition(monster[gamedata.monsterIndex].posX, monster[gamedata.monsterIndex].posY);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("M");
                Console.ResetColor();

            }

        }


        /// <summary>
        /// 사냥터에서 마을로 이동하는 문을 그려주는 함수
        /// </summary>
        static void TownGate()
        {
            Console.SetCursorPosition(townGate.posX, townGate.posY);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("T");
            Console.ResetColor();
        }



        /// <summary>
        /// 레벨업 함수
        /// </summary>
        static void LevelUp()
        {
            int gar = 0;

            if(player.exp >= 100)
            {
                gar = player.exp % 100;
                player.level++;
                player.exp = gar;
            }

        }


        /// <summary>
        /// 체력 포션 사용 함수
        /// </summary>
        static void HealthPotion()
        {
             
            if(player.itemList[0].itemCount > 0 )
            {
               

                player.curHp += 30;
                player.itemList[0].itemCount--;
                Console.WriteLine("체력 30 회복");

                if (player.curHp >= player.maxHp)
                {
                    player.curHp = player.maxHp;
                }
            }
            else
            {
                Console.WriteLine("물약이 없습니다.");
            }

             
        }

        /// <summary>
        /// 공격력 포션 사용하는 함수
        /// </summary>
        static void PowerPotion()
        {
            if (player.itemList[1].itemCount > 0)
            {

                player.playerDmg += 1;
                player.itemList[1].itemCount--;

                Console.WriteLine("공격력 1증가");


            }
            else
            {
                Console.WriteLine("물약이 없습니다.");
            }
             

        }

        /// <summary>
        /// 플레이어 움직임 및 포션 조작 함수
        /// </summary>
        static void Move()
        {
            ConsoleKey key = Input();

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    MoveUp();
                    break;

                case ConsoleKey.DownArrow:

                    MoveDown();
                    break;

                case ConsoleKey.LeftArrow:
                    MoveLeft();
                    break;

                case ConsoleKey.RightArrow:
                    MoveRight();
                    break;

                //현재 공격력 증가 버튼
                case ConsoleKey.Z:
                    player.playerDmg++;
                    break;


                //현재 체력 증가 버튼 
                case ConsoleKey.X:
                    player.curHp++;
                    break;


                //Q 버튼 누를 시 체력 포션 사용 
                case ConsoleKey.Q:
                    HealthPotion();
                    break;

                //W 버튼 누를 시 공격력 포션 사용
                case ConsoleKey.W:
                    PowerPotion();
                    break;


                default:
                    break;

            }
        }
        
        static void MoveUp()
        {
            //좌표값으로 고정
            if (player.posY > 7)
            {
                player.posY = player.posY - 1;
            }

        }

        static void MoveDown()
        {
            if (player.posY < 21)
            {
                player.posY = player.posY + 1;
            }

        }

        static void MoveLeft()
        {
            if (player.posX > 17)
            {
                player.posX = player.posX - 1;
            }


        }
        static void MoveRight()
        {
            if (player.posX < 38)
            {
                player.posX = player.posX + 1;
            }

        }

        /// <summary>
        /// 캐릭터 생성 시 초반 아이템 지급하는 함수
        /// </summary>
        static void StartItem()
        {

            if (!player.startItem)
            {

                Item startHpPotion = new Item();
                startHpPotion.itemName = "시작의 체력 포션";
                startHpPotion.itemCount = 5;

                Item startStrPotion = new Item();
                startStrPotion.itemName = "시작의 힘 포션";
                startStrPotion.itemCount = 5;

                player.itemList[0] = startHpPotion;
                player.itemList[1] = startStrPotion;

                player.meso = 5000;
                player.startItem = false;

                Console.WriteLine($"\n{player.itemList[0].itemName}을 {player.itemList[0].itemCount}개 지급 받으셨습니다.\n");
                Console.WriteLine($"{player.itemList[1].itemName}을 {player.itemList[1].itemCount}개 지급 받으셨습니다.\n");
                Console.WriteLine($"{player.meso} 메소를 지급받으셨습니다.\n");
            }

        }

    }
}
