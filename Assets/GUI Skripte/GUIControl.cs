using UnityEngine;
using System.Collections;
using System;

public class GUIControl : MonoBehaviour
{
	private Rect[] windowRect = {
		new Rect (0, 0, 128, 64),
		new Rect (0, 0, 128, 64),
		new Rect (0, 0, 128, 64),
		new Rect (0, 0, 128, 64)
	};
	private Rect[] windowCloseRect = {
		new Rect (350, 200, 20, 20),
		new Rect (350, 200, 20, 20),
		new Rect (350, 200, 20, 20),
		new Rect (350, 200, 20, 20)
	};
	private const int CLOSE_SIZE = 20;
	private int sizePlayerButtons = 50;
	private int activePlayer = 10;
	private bool[] activeGUIPlayer = {false,false,false,false};
	private bool[] playerRoundEnd = {false,false,false,false};
	private int[] hpPlayer = new int[4];
	private bool[] infectedPlayer = {true,false,false,false};
	private int[] apPlayer = {7,5,7,7};
	private int[][] inventory; //= {{1,2,0,0},{1,2,0,0},{1,2,0,0},{1,2,0,0}};
	private int inventoryActive = -1;
	private bool[,] inventoryUsed = {{false,false,false,false},{false,false,false,false},{false,false,false,false},{false,false,false,false}};
	private bool[] actionButtonsActive = {false,false};
	private Texture2D[] inventoryIcons;
	private string[] itemTypes = {"weapon","weapon","medi","protection"};
	private string[] itemNames;
	private bool[] hasGhostItem = {false,false,false,false};
	private int[] ghostNumber = {5,5,5,5};
	private int[] primaryWeapon = {5,5,5,5};
	private int[] primaryProtection = {5,5,5,5};
	private int[] inventoryPreSelected = {0,0,0,0};//in item properties speichern
	private bool attack = false;
	private int infectedDamage = 0;
	private int infectionDamage = 25;
	private float infectedTimer = 0.0f;
	private Texture2D whiteTex;

	public Texture2D rifle;
	public Texture2D baseball;
	public Texture2D medipack;
	public Texture2D ablegen;
	public Texture2D benutzen;
	public Texture2D aufnehmen;
	public Texture2D[] playerButtons;
	public Texture2D[] attackButtons;
	public Texture2D hpRaster;
	public Texture2D apRaster;
	public Texture2D[] closeButtons;
	public Texture2D schutzweste;
	public GUIStyle buttonRaw;
	public GUISkin skinPlayerToggle;
	public GUISkin skinWindow;
	public GUISkin skinCloseButton;
		
	private Texture2D framePrimary;
	private Texture2D frameSelected;
	private Texture2D frameProtection;
	/*
	 * 
	 * wenn aktiver spiel fenster zu macht --> meldung welt, dass spieler inaktiv wird 
	 * gibt dazu methode in behaviourLevel
	 * 
	 */
	private GameObject[] players;
	private PlayerActions[] playerReferences;
	private BehaviorLevel levelReference;
	void Start(){

		inventory = new int[4][];
		inventory [0] = new int[] {1,2,0,0};
		inventory [1] = new int[] {1,2,0,0};
		inventory [2] = new int[] {1,2,0,0};
		inventory [3] = new int[] {1,2,0,0};



		framePrimary = createFrameTexture (new Color (0, 1, 0));
		frameSelected = createFrameTexture (new Color (0,0, 1));
		frameProtection = createFrameTexture (new Color (1,0, 0));
		whiteTex = createBlankTexture (new Color (1, 1, 1, 1));



		/*inventoryIcons =  new Texture2D[4];
		inventoryIcons [0] = rifle;
		inventoryIcons [1] = baseball;
		inventoryIcons [2] = medipack;
		inventoryIcons [3] = schutzweste;*/
		levelReference = GameObject.Find ("Level").GetComponent<BehaviorLevel> ();

		Transform playerContainer = GameObject.Find ("Players").transform;
		players = new GameObject[playerContainer.childCount];
		playerReferences = new PlayerActions[playerContainer.childCount];
		for (int i = 0; i < playerContainer.childCount; i++)
		{
			players[i] = playerContainer.GetChild(i).gameObject;
			playerReferences[i] = players[i].GetComponent<PlayerActions>();
			apPlayer[i] = Statics.ActionPoints;
			hpPlayer[i] = Statics.HealthPoints;
		}
		getItemTextures ();
		primaryWeapon [0] = 0;

	}

	void getItemTextures(){
		//get items and the properties and save them in arrays
		GameObject[] items = GameObject.Find ("Level").GetComponent<AvailableItems>()._items;
		Texture2D[] itemIcons = new Texture2D[items.Length];
		string[] itemType = new string[items.Length];
		itemNames = new string[items.Length];
		for (int i=0; i<items.Length; i++) {
			itemNames[i] = items[i].name;
			itemIcons[i] = getTexFromGameObject(items[i]);
			ItemProperties properties = items[i].GetComponent<ItemProperties>();
			if(properties.isWeapon){
				itemType[i] = "weapon";
			} else if(properties.canHeal && properties.doesDesinfect){
				itemType[i] = "dualpack";
			} else if(properties.canHeal){
				itemType[i] = "medi";
			} else if(properties.doesDesinfect){
				itemType[i] = "desinfection";
			} else if(properties.isArmor){
				itemType[i] = "protection";
			}
		}
		//noch haesslich, spaeter komplett hier erstellen
		inventoryIcons =  new Texture2D[itemIcons.Length];
		inventoryIcons = itemIcons;
		itemTypes = itemType;
	}

	Texture2D getTexFromGameObject(GameObject obj){
		SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer> ();
		Sprite sprt = spriteRenderer.sprite;
		Texture2D tex = sprt.texture;
		return tex;
	}

	String returnSelectedGUIActions(){
		//call from world
		return "";
	}

	void OnGUI ()
	{

		//Poll player data
		bool someoneActive = false;
		for (int i=0; i<players.Length; i++) {
			if(players[i].GetComponent<PlayerActions>().Active){
				if(activePlayer!=i){
					inventoryActive = -1;
				}
				activePlayer = i;
				activeGUIPlayer[i] = true;
				someoneActive = true;
			}	
			
			apPlayer[i] = playerReferences[i].ActionPoints;
			hpPlayer[i] = playerReferences[i].HealthPoints;
			//if player has picked an item
			if(playerReferences[i].GhostItem!=null && !hasGhostItem[i]){
				for(int j=0;j<itemNames.Length;j++){
					Debug.Log(itemNames[j]);
					if(itemNames[j]==playerReferences[i].GhostItem.name){
						//for(int m=0;m<inventory[i]
						int index = Array.IndexOf(inventory[i],0);
						/*for(int k=0;k<inventory[i].Length;k++){
							if(inventory[i][k]==0){
								index = k;
								break;
							}
						}*/
						inventory[i][index] = j+1;
						Debug.Log("index");
						Debug.Log(j+1);
						Debug.Log(i);
						hasGhostItem[i]=true;
						ghostNumber[i] = index;
						break;
					}
				}
			}
		}
		if (!someoneActive) {
			activePlayer = 10;	
		}



		/* START Simulation der Welt */
		if(GUI.Button(new Rect(100,0,100,50),"Ablegen") && actionButtonsActive[0]){
			//item is dropped
			inventoryUsed[activePlayer,inventoryActive] = true;
			inventoryActive = -1;
			Array.Clear(actionButtonsActive,0,actionButtonsActive.Length);
		}
		if (GUI.Button (new Rect (200, 0, 100, 50), "Angreifen") && actionButtonsActive [1] &&  itemTypes[inventory[activePlayer][inventoryActive]-1] == "weapon") {
			attack = true;
		}
		/* END Simulation der Welt */

		for (int i = 0; i<windowRect.Length; i++) {


			Vector2 startPos = new Vector2(playerReferences[i].StartPosition.x * 32,playerReferences[i].StartPosition.y*32);//Pfusch
			if (activeGUIPlayer [i]) {
				int angle = 0;
				if(i>0&&i<3){
					angle =180;
				}
				if(windowRect[i].x==0&&windowRect[i].y==0){
					if(angle==180){
						windowRect[i].x = -startPos.x;
						windowRect[i].y = -startPos.y;
					} else {
						windowRect[i].x = startPos.x;
						windowRect[i].y = startPos.y;
					}
				}
				GUI.skin = skinWindow;
				GUIUtility.RotateAroundPivot (angle, new Vector2(0,0));
				windowRect [i] = GUI.Window (i, windowRect [i], WindowGUIControl, "");
				GUIUtility.RotateAroundPivot (-angle, new Vector2(0,0));
				GUI.skin = null;
			}
		}

		//create the close buttons as gui windows
		for(int i=0;i<windowCloseRect.Length;i++){
			if (activeGUIPlayer [i]) {
				if(i==1||i==2){
					windowCloseRect[i].x = -windowRect[i].x-CLOSE_SIZE/2+2;
					windowCloseRect[i].y = -windowRect[i].y-CLOSE_SIZE/2+2;
				} else {
					windowCloseRect[i].x = windowRect[i].x-CLOSE_SIZE/2;
					windowCloseRect[i].y = windowRect[i].y-CLOSE_SIZE/2;
				}
				GUI.skin = skinCloseButton;
				windowCloseRect [i] = GUI.Window (i+4, windowCloseRect [i], WindowCloseControl,"");
				GUI.skin = null;
			}
		}

		GUI.skin = skinPlayerToggle;
		//player corner buttons
		Rect[] playerRects = {new Rect (0, Screen.height - sizePlayerButtons, sizePlayerButtons, sizePlayerButtons),
					new Rect (0, 0, sizePlayerButtons, sizePlayerButtons),
					new Rect (Screen.width - sizePlayerButtons, 0, sizePlayerButtons, sizePlayerButtons),
					new Rect (Screen.width - sizePlayerButtons, Screen.height - sizePlayerButtons, sizePlayerButtons, sizePlayerButtons)
				};
		Vector2[] pivotPoints = {
			new Vector2 (25, Screen.height - sizePlayerButtons / 2),
			new Vector2 (25, 25),
			new Vector2 (Screen.width - sizePlayerButtons / 2, 25),
			new Vector2 (Screen.width - sizePlayerButtons / 2, Screen.height - sizePlayerButtons / 2)
		};
			
		for (int i = 0; i<playerRects.Length; i++) {
			GUIUtility.RotateAroundPivot ((90 * i) - 90, pivotPoints [i]);
			activeGUIPlayer [i] = GUI.Toggle (playerRects [i], activeGUIPlayer [i], playerButtons [i]);
			GUIUtility.RotateAroundPivot (-((90 * i)) + 90, pivotPoints [i]);
		}

		GUI.skin = null;

	}

	void WindowCloseControl(int windowID){
		int offsetID = windowID-4;
		if(GUI.Button(new Rect(0,0,CLOSE_SIZE,CLOSE_SIZE),closeButtons[offsetID],buttonRaw)){
			playerEnd(offsetID);
		}
		GUI.BringWindowToFront(windowID);
	}

	void WindowGUIControl (int windowID)
	{	
		GUI.skin = skinWindow;

		//health and actionbar size
		int hpWidth = 57;
		int hpHeight = 8;
		Vector2 hpPos = new Vector2 (3,7);
		//HP Background
		GUI.DrawTexture (new Rect(hpPos.x,hpPos.y,hpWidth,hpHeight),whiteTex);
		//HP Fill color
		Color fillColor;
		if (infectedPlayer [windowID]) {
			float currentHPDamage = ((float)infectedDamage)/((float)hpWidth/100.0f);
			float hpDamage = hpPlayer[windowID]/100.0f - ((float)currentHPDamage/100.0f);
			if(hpDamage<0){
				hpDamage=0.0f;
			}
			fillColor = new Color(1-hpDamage,hpDamage,0);
		} else {
			fillColor = new Color(1-(hpPlayer[windowID]/100.0f),(hpPlayer[windowID]/100.0f),0);
		}

		Texture2D fillTex = createBlankTexture (fillColor);
		GUI.DrawTexture (new Rect(hpPos.x,hpPos.y,hpWidth * (hpPlayer[windowID]/100.0f),hpHeight),fillTex);
		if (infectedPlayer [windowID]) {
			infectedTimer += Time.deltaTime;
			if(infectedTimer>0.2f){
				infectedDamage++;
				infectedTimer = 0.0f;
			}
			GUI.DrawTexture (new Rect((hpPos.x+hpWidth * (hpPlayer[windowID]/100.0f))-infectedDamage,hpPos.y,infectedDamage,hpHeight),whiteTex);
			float stopValue = ((float)hpWidth/100.0f)*(float)infectionDamage;
			if(infectedDamage==Mathf.Round(stopValue)){
				infectedDamage=0;
			}
		}
		//HP Grid
		GUI.DrawTexture (new Rect(hpPos.x,hpPos.y,hpWidth,hpHeight),hpRaster);

		//8 pixel width per action point
		int apPartWidth = 8;
		Vector2 apPos = new Vector2 (3,19);
		//AP Background
		GUI.DrawTexture (new Rect(apPos.x,apPos.y,hpWidth,hpHeight),whiteTex);
		//AP Fill color
		GUI.DrawTexture (new Rect(apPos.x,apPos.y,apPartWidth * apPlayer[windowID],hpHeight),createBlankTexture (new Color (0, 0, 1, 0.5f)));
		//AP Grid
		GUI.DrawTexture (new Rect(apPos.x,apPos.y,hpWidth,hpHeight),apRaster);

		//Display the Action Buttons and Inventory Items

		//Action Buttons
		if(inventoryActive!=-1 && activePlayer==windowID){
			Texture firstAction = (attack) ? attackButtons[0] : ablegen;
			if (GUI.Button (new Rect (64, 0, 32, 32), firstAction)) {
				actionButtonsActive[0] = !actionButtonsActive[0];
				actionButtonsActive[1] = false;
				if(attack){
					//notice world that attack number has decreased
					Debug.Log("notice world that attack number has decreased");
				}
			}
			if(inventoryActive!=-1&&itemTypes[inventory[windowID][inventoryActive]-1]!="protection"){
				Texture secondAction = (attack) ? attackButtons[1] : benutzen;
				if (GUI.Button (new Rect (96, 0, 32, 32), secondAction)) {
					actionButtonsActive[1] = !actionButtonsActive[1];
					actionButtonsActive[0] = false;
					if(attack){
						//notice world that attack number has increased
						Debug.Log("notice world that attack number has increased");
					}
				}
			}
			if(actionButtonsActive[0]){
				GUI.DrawTexture(new Rect (64, 0, 32, 32),frameSelected);
			} else if(actionButtonsActive[1]){
				GUI.DrawTexture(new Rect (96, 0, 32, 32),frameSelected);
			}
		}
		
		//Inventory Items
		for (int i=0; i<4; i++) {
			if(inventoryUsed[windowID,i] || windowID!=activePlayer || ghostNumber[windowID]==i){
				GUI.color = new Color(1,1,1,0.5f);
			} else {
				GUI.color = new Color(1,1,1,1);
			}
			if (inventory [windowID][i] == 0) {
				if(GUI.Button (new Rect (32 * i, 32, 32, 32),"")){
					inventoryActive = -1;
					Array.Clear(actionButtonsActive,0,actionButtonsActive.Length);
					attack = false;
				}
			} else {
				int index = inventory [windowID][i] - 1;
				int active = (inventoryActive==i) ? 1 : 0;
				if(GUI.Button (new Rect (32 * i, 32, 32, 32), inventoryIcons [index]) && windowID==activePlayer && ghostNumber[windowID]!=i){
					if(inventoryUsed[windowID,i]){
						//dropped item click-->player gets the item back
						inventoryUsed[windowID,i] = false;
						Debug.Log("Item has been grabbed again");
						/* 
						 * Notice world that item has been grabbed again
						 */
					} else {
						//valid item click

						if(itemTypes[inventory[windowID][i]-1]=="weapon"){
							primaryWeapon[windowID] = i;
							Debug.Log("Main Weapon Change");
							//say to player that main weapon has changed
						}
						Array.Clear(actionButtonsActive,0,actionButtonsActive.Length);
						if(inventoryActive==i){
							inventoryActive = -1;
						} else {
							inventoryActive = i;
							actionButtonsActive[inventoryPreSelected[index]] = true;
						}
						/*bool origin = inventoryActive[i];
						Array.Clear(inventoryActive,0,inventoryActive.Length);
						inventoryActive[i] = !origin;*/
						attack = false;

						if(itemTypes[inventory[windowID][i]-1]=="protection"){
							primaryProtection[windowID] = i;
						}
					}
				}
				if(primaryWeapon[windowID] == i){
					GUI.DrawTexture(new Rect (32 * i + 2, 34, 28, 28),framePrimary);
				}
				if(primaryProtection[windowID] == i){
					GUI.DrawTexture(new Rect (32 * i + 2, 34, 28, 28),frameProtection);
				}
				if(inventoryActive==i && windowID==activePlayer){
					GUI.DrawTexture(new Rect (32 * i, 32, 32, 32),frameSelected);
				}
			}

		}
		//make window draggable over the whole size
		GUI.DragWindow (new Rect (0, 0, 128, 64));
		GUI.skin = null;
	}

	private void playerEnd(int id){
		activeGUIPlayer [id] = false;		
		playerRoundEnd [id] = true;
		if(activePlayer==id){
			levelReference.EndRoundForActivePlayer ();
		}
		if(Array.IndexOf(playerRoundEnd,false)==-1){
			//Round End-->Notice Worl
			Debug.Log("Round End-->Notice World");
		}
	}

	private Texture2D createBlankTexture(Color c){
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,c);
		texture.Apply();
		return texture;
	}

	private Texture2D createFrameTexture(Color c){
		int width = 2;
		Texture2D tex = new Texture2D (32, 32);
		for (int i=0; i<32; i++) {
			for(int j=0;j<32;j++){
				Color texColor = new Color(0,0,0,0);
				if(i<width||j<width||i>32-width-1||j>32-width-1){
					texColor = c;
				}
				tex.SetPixel(i,j,texColor);
			}	
		}
		tex.Apply();
		return tex;
	}

	private Texture2D createHPGradient(){
		Texture2D texture = new Texture2D(100, 1);
		Color start = new Color (1, 0, 0);
		for (int i=0; i<100; i++) {
			texture.SetPixel(i,0,start);
			start.r -= 0.01f;
			start.g += 0.01f;
		}
		texture.Apply();
		return texture;
	}

	private void PollNewPlayerData(){
		//infos der spieler neu pollen
	}

	private void dropActionFromWorld(bool success){
		if (success) {
			//item ghosten && actionbuttons hide
		}
	}

	private void attackActionFromWorld(bool success){
		if (success) {
			//show + & - actionbuttons
		}
	}

	private void walkStepFromActivePlayer(int apLoss){
		apPlayer[activePlayer] -= apLoss;
		if (apPlayer [activePlayer]<0) {
			//walk action fail-->notice world
		}
	}

	private void setNewActivePlayer(int newPlayer){
		//davor noch überprüfen ob aktuell aktiver spieler schon durch ist
		activePlayer = newPlayer;
	}

}