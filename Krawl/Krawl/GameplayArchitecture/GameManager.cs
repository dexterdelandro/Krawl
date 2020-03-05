using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Krawl.GameArchitecture;
using Krawl.GameplayArchitecture;
using Microsoft.Xna.Framework;



namespace Krawl
{
	public enum States {Menu, Round, P1Shop, P2Shop, BothShop, GameOver}
	static class GameManager
	{
		static private int round;
		static private bool p1ShopOpen;
		static private bool p2ShopOpen;
        static private States state = States.Menu;

		//info needed to create & upgrade enemies
		static public float totalArmorP1;
		static public float totalArmorP2;
		static public float totalDamageP1;
		static public float totalDamageP2;
		static public float totalMoveSpeedP1;
		static public float totalMoveSpeedP2;

		static public string p1Stats = "";
		static public string p2Stats = "";

		static private int numEnemiesP1;
		static private int numEnemiesP2;
		static public int numEnemiesStartP1 = 2;
		static public int numEnemiesStartP2 = 2;
		static public List<Enemy> EnemiesP1 = new List<Enemy>();
		static public List<Enemy> EnemiesP2 = new List<Enemy>();
		static public double[] enemyStatScalar = { 1.2, 1.3, 1.08, 1.05 };
		static private double buffedEnemyHp = 4;
		static private double buffedEnemyAttack = 10;
		static private double currentEnemyGold = 20;
		static private double buffedEnemyMovementSpeed = 0f;
		//static private int currentEnemyAttackSpeed;
		private const int ADD_ENEMIES_ROUND = 3;
		private const int NUM_ENEMIES_ADDED = 3;
		private const int MAX_ENEMIES = 20;
		private const float MAX_SECONDS_IN_SHOP = 15;
		private const float MAX_SECONDS_BETWEEN_WAVE = .7f;
		static private float secondsLeftInWave = 0f;
		static private bool createdRound = false;

		static public float SecondsLeftInShop;

		static private Random random = new Random();

		static private bool doneSpawning = false;
		static private Vector2 topLeftP1 = new Vector2(67,211);
		static private Vector2 topRightP1 = new Vector2(485,211);
		static private Vector2 bottomLeftP1 = new Vector2(67,700);
		static private Vector2 bottomRightP1 = new Vector2(485,700);
		static private Vector2 topLeftP2 = new Vector2(650,211);
		static private Vector2 topRightP2 = new Vector2(1112,211);
		static private Vector2 bottomLeftP2 = new Vector2(650,700);
		static private Vector2 bottomRightP2 = new Vector2(1112,700);

		static private Vector2 spawnPointP1;
		static private Vector2 spawnPointP2;

		static public Player p1;
		static public Player p2;

		static private double[] enemyStatArray = new double[4];

        static private bool roundRewardGiven = false;

        static public List<Item> items = new List<Item>();

		/// <summary>
		/// gets the current state of the game
		/// </summary>
		static public States State { get { return state; } set { state = value; } }

		/// <summary>
		/// gets the current round
		/// </summary>
		static public int Round { get { return round; } }

		/// <summary>
		/// gets or sets whether the shop door is open for player1
		/// </summary>
		static public bool P1ShopOpen { get { return p1ShopOpen; } set { p1ShopOpen = value; } }

		/// <summary>
		/// gets or sets whether the shop door is open for player2
		/// </summary>
		static public bool P2ShopOpen { get { return p2ShopOpen; } set { p2ShopOpen = value; } }
        
		/// <summary>
		/// Checks whether or not conditinos are met to change the state, if they are met, it changes the state
		/// </summary>
		public static void UpdateState() {

			//will change when we get the menu created
			
			switch (state) {

				case States.Menu:
					//if button pressed -> state = States.Round;
					break;

				case States.Round:
					//SpawnEnemies();
					if (!createdRound)
					{
						NextRound();
						createdRound = true;
					}
					if (!doneSpawning) SpawnEnemies();
					if (p1.HP <= 0 || p2.HP <= 0) state = States.GameOver;
                    if (doneSpawning && EnemiesP1.Count <= 0)
                    {
                        RoomManager.ChangeDoorState(Players.Player1, DoorState.Open);
                        state = States.P1Shop;
                        p1.HP = p1.MaxHP;

                        Shop.LoadUpShop();
                    }
                    if (doneSpawning && EnemiesP2.Count <= 0)
                    {
                        RoomManager.ChangeDoorState(Players.Player2, DoorState.Open);
                        state = States.P2Shop;
                        p2.HP = p2.MaxHP;

                        Shop.LoadUpShop();
                    }

                    if (Input.GetKey(Microsoft.Xna.Framework.Input.Keys.Tab))
                        JoshWinsTheGameBuyCheating();
                    //Console.WriteLine(EnemiesP1.Count);
                    break;

				case States.P1Shop:
					//Console.WriteLine("PLAYER 1 IS IN SHOP!");
					if(!p1ShopOpen)p1ShopOpen = true;
					if (p2.HP <= 0) state = States.GameOver;
					if (doneSpawning && EnemiesP2.Count == 0) {
						state = States.BothShop;
						p2ShopOpen = true;
                        p2.HP = p2.MaxHP;
                        RoomManager.ChangeDoorState(Players.Player2, DoorState.Open);
                        SecondsLeftInShop = MAX_SECONDS_IN_SHOP;
					}
                    else if(!roundRewardGiven)
                    {
                        p1.Gold += 50;
                        roundRewardGiven = true;
                    }
					break;

				case States.P2Shop:
					//Console.WriteLine("PLAYER 2 IS IN SHOP!");
					if (!p2ShopOpen)p2ShopOpen = true;
					if (p1.HP <= 0) state = States.GameOver;
					if (doneSpawning && EnemiesP1.Count == 0) {
						state = States.BothShop;
						p1ShopOpen = true;
                        p1.HP = p1.MaxHP;
                        RoomManager.ChangeDoorState(Players.Player1, DoorState.Open);
                        SecondsLeftInShop = MAX_SECONDS_IN_SHOP;
					}
                    else if (!roundRewardGiven)
                    {
                        p2.Gold += 50;
                        roundRewardGiven = true;
                    }
                    break;

				case States.BothShop:
                    roundRewardGiven = false;
					//Console.WriteLine("SHOPPING TIME!");
					if (SecondsLeftInShop <= 0)
					{
						//Console.WriteLine("START THE FUCKING ROUND!");
						createdRound = false;
                        roundRewardGiven = false;
						state = States.Round;

                        RoomManager.ChangeDoorState(Players.Player1, DoorState.Closed);
                        RoomManager.ChangeDoorState(Players.Player2, DoorState.Closed);
                        p1.Position = new Vector2(250, 400);
                        p2.Position = new Vector2(800, 400);
                        break;
					}
					SecondsLeftInShop -= Time.DeltaTime;
					break;

				case States.GameOver:
					while(EnemiesP1.Count > 0)
                    {
                        EnemiesP1[0].Destroy();
                        EnemiesP1.RemoveAt(0);
                    }
                    while(EnemiesP2.Count > 0)
                    {
                        EnemiesP2[0].Destroy();
                        EnemiesP2.RemoveAt(0);
                    }
                    break;
			}
		}

		/// <summary>
		/// Creates the next round: creates new enemies, closes shop doors, resets player position
		/// </summary>
		private static void NextRound() {
			p1ShopOpen = false;
			p2ShopOpen = false;
			round++;
			doneSpawning = false;
			numEnemiesP1 = 0;
			numEnemiesP2 = 0;
			CreateEnemies();

		}

		/// <summary>
		/// Creates the new enemies: Every 3 rounds add new enemies, Every other round, increase stats of enemies
		/// </summary>
		private static void CreateEnemies()
		{

			if (/*numEnemiesStartP1 < MAX_ENEMIES &&*/ round % ADD_ENEMIES_ROUND == 0)
			{
				numEnemiesStartP1 += NUM_ENEMIES_ADDED;
			}
			if (/*numEnemiesStartP2 < MAX_ENEMIES &&*/ round % ADD_ENEMIES_ROUND == 0)
			{
				numEnemiesStartP2 += NUM_ENEMIES_ADDED;
			}

			//if not, then increase the hp and attack of the enemies by the scalar amount.
			
			{
				buffedEnemyHp = Math.Pow(enemyStatScalar[0], round - round / ADD_ENEMIES_ROUND);
				buffedEnemyAttack = Math.Pow(enemyStatScalar[1], round - round / ADD_ENEMIES_ROUND);
				currentEnemyGold = Math.Pow(enemyStatScalar[2], round - round / ADD_ENEMIES_ROUND);
				buffedEnemyMovementSpeed = Math.Pow(enemyStatScalar[3], round - round / ADD_ENEMIES_ROUND);
				UpdateStatArray();
			}
			secondsLeftInWave = 0;			
		}

		private static void SpawnEnemies() {
            Console.WriteLine(numEnemiesStartP1 + " : " + numEnemiesStartP2);

			secondsLeftInWave += Time.DeltaTime;
			if (!doneSpawning && secondsLeftInWave > MAX_SECONDS_BETWEEN_WAVE)
			{
                string enemyToSpawn = GetRandomEnemy();

                secondsLeftInWave = 0;
				Enemy tempEnemyP1;
				Enemy tempEnemyP2;
				spawnPointP1 = topLeftP1;
				spawnPointP2 = topRightP2;
				if (MAX_ENEMIES > numEnemiesP1 && numEnemiesP1 <= numEnemiesStartP1)
				{
					//picks the corner
					switch (random.Next(1, 5))
					{
						case 1:
							spawnPointP1 = topLeftP1;
							break;

						case 2:
							spawnPointP1 = topRightP1;
							break;

						case 3:
							spawnPointP1 = bottomLeftP1;
							break;

						case 4:
							spawnPointP1 = bottomRightP1;
							break;
					}
					//Console.WriteLine("SPAWNING P1");
					tempEnemyP1 = Enemy.CreateEnemyGameObjectFromBlueprint(enemyToSpawn, spawnPointP1, p1);
					tempEnemyP1.MaxHP = (int)Math.Round(Math.Pow(enemyStatScalar[0], round - round / ADD_ENEMIES_ROUND) * tempEnemyP1.MaxHP);
					tempEnemyP1.HP = tempEnemyP1.MaxHP;
					tempEnemyP1.Damage = (int)Math.Round(Math.Pow(enemyStatScalar[1], round - round / ADD_ENEMIES_ROUND) * tempEnemyP1.Damage);
					tempEnemyP1.Gold = (int)Math.Round(Math.Pow(enemyStatScalar[2], round - round / ADD_ENEMIES_ROUND) * tempEnemyP1.Gold);

					tempEnemyP1.MovementSpeed = (int)Math.Round(Math.Pow(enemyStatScalar[3], round - round / ADD_ENEMIES_ROUND) * tempEnemyP1.MovementSpeed);
					//Console.WriteLine(tempEnemyP1.MaxHP+"//"+tempEnemyP1.Damage+"//"+tempEnemyP1.Gold+"//"+tempEnemyP1.MovementSpeed);
					EnemiesP1.Add(tempEnemyP1);
					numEnemiesP1++;
				}
				if (MAX_ENEMIES > numEnemiesP2 && numEnemiesP2 <= numEnemiesStartP2)
				{
					switch (random.Next(1, 5))
					{
						case 1:
							spawnPointP2 = topLeftP2;
							break;

						case 2:
							spawnPointP2 = topRightP2;
							break;

						case 3:
							spawnPointP2 = bottomLeftP2;
							break;

						case 4:
							spawnPointP2 = bottomRightP2;
							break;
					}
					//Console.WriteLine("SPAWNING P2");
					tempEnemyP2 = Enemy.CreateEnemyGameObjectFromBlueprint(enemyToSpawn, spawnPointP2, p2);
					EnemiesP2.Add(tempEnemyP2);
					tempEnemyP2.MaxHP = (int)Math.Round(Math.Pow(enemyStatScalar[0], round - round / ADD_ENEMIES_ROUND) * tempEnemyP2.MaxHP);
					tempEnemyP2.HP = tempEnemyP2.MaxHP;
					tempEnemyP2.Damage = (int)Math.Round(Math.Pow(enemyStatScalar[1], round - round / ADD_ENEMIES_ROUND) * tempEnemyP2.Damage);
					tempEnemyP2.Gold = (int)Math.Round(Math.Pow(enemyStatScalar[2], round - round / ADD_ENEMIES_ROUND) * tempEnemyP2.Gold);
					tempEnemyP2.MovementSpeed = (int)Math.Round(Math.Pow(enemyStatScalar[3], round - round / ADD_ENEMIES_ROUND) * tempEnemyP2.MovementSpeed);
					numEnemiesP2++;
				}
				if (numEnemiesP1 >= numEnemiesStartP1 && numEnemiesP2 >= numEnemiesStartP2)
                {
					//Console.WriteLine("done spawning");
					doneSpawning = true;
				}
			}
		}

		private static void UpdateStatArray() {
			enemyStatArray[0] = (int)buffedEnemyHp;
			enemyStatArray[1] = (int)buffedEnemyAttack;
			enemyStatArray[2] = (int)currentEnemyGold;
			enemyStatArray[3] = (int)buffedEnemyMovementSpeed;
		}

        public static string GetRandomEnemy()
        {
            switch (random.Next(4))
            {
                case 0:
                    return "Grockel";
                case 1:
                    return "Chort";
                case 2:
                    return "Swampy";
                case 3:
                    return "Zombie";
                default:
                    return "Grockel";
            }
        }

        public static void JoshWinsTheGameBuyCheating()
        {
            switch (random.Next(1, 5))
            {
                case 1:
                    spawnPointP2 = topLeftP2;
                    break;

                case 2:
                    spawnPointP2 = topRightP2;
                    break;

                case 3:
                    spawnPointP2 = bottomLeftP2;
                    break;

                case 4:
                    spawnPointP2 = bottomRightP2;
                    break;
            }
            //Console.WriteLine("SPAWNING P2");
            Enemy tempEnemyP2 = Enemy.CreateEnemyGameObjectFromBlueprint("Chort", spawnPointP2, p2);
            EnemiesP2.Add(tempEnemyP2);
            numEnemiesP2++;
        }
	}
}
