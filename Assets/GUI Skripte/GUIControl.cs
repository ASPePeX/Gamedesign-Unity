using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	private const int SIZE_PLAYER_BUTTONS = 50;
	private int activePlayer = 10;
	private bool[] activeGUIPlayer = {false,false,false,false};
	private bool[] playerRoundEnd = {false,false,false,false};
	private int[] hpPlayer = new int[4];
	private bool[] infectedPlayer = {false,false,false,false};
	private int[] apPlayer = {7,5,7,7};
	private int[][] inventory; //= {{1,2,0,0},{1,2,0,0},{1,2,0,0},{1,2,0,0}};
	private int inventoryActive = -1;
	private bool[,] inventoryUsed = {{false,false,false,false},{false,false,false,false},{false,false,false,false},{false,false,false,false}};
	private bool[] actionButtonsActive = {false,false};
	private Texture2D[] inventoryIcons;
	private string[] itemNames;
	private bool[] hasGhostItem = {false,false,false,false};
	private int[] ghostNumber = {5,5,5,5};
	private int[] primaryWeapon = {-1,-1,-1,-1};
	private int[] primaryProtection = {5,5,5,5};
	private int[] inventoryPreSelected = {1,1,1,1};//in item properties speichern
	private bool attack = false;
	private int[] infectedDamage = {0,0,0,0};
	private int infectionDamage = 100; //von gegner holen
	private float[] infectedTimer = {0.0f,0.0f,0.0f,0.0f};
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
	private GameObject[] players;
	private PlayerActions[] playerReferences;
	private BehaviorLevel levelReference;
	private GameObject[] items;
	private GameObject[] addAttack;

	/*
	 * Item ablegen zuruecknehmen
	 * 
	 * item types aus itemproperties auslesen on the fly
	 */

	void Start(){
		inventory = new int[4][];


		framePrimary = createFrameTexture (new Color (0, 1, 0));
		frameSelected = createFrameTexture (new Color (0,0, 1));
		frameProtection = createFrameTexture (new Color (1,0, 0));
		whiteTex = createBlankTexture (new Color (1, 1, 1, 1));



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

			inventory[i] = new int[] {0,0,0,0};
		}
		getItemTextures ();
		/*primaryWeapon [0] = 0;
		playerReferences [0].PrimaryWeapon = 0;*/

	}

	void getItemTextures(){
		//get items and the properties and save them in arrays
		items = GameObject.Find ("Level").GetComponent<AvailableItems>()._items;
		Texture2D[] itemIcons = new Texture2D[items.Length];
		itemNames = new string[items.Length];
		for (int i=0; i<items.Length; i++) {
			itemNames[i] = items[i].name;
			itemIcons[i] = getTexFromGameObject(items[i]);
		}
		//noch haesslich, spaeter komplett hier erstellen
		inventoryIcons =  new Texture2D[itemIcons.Length];
		inventoryIcons = itemIcons;
	}

	Texture2D getTexFromGameObject(GameObject obj){
		SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer> ();
		Sprite sprt = spriteRenderer.sprite;
		Texture2D tex = sprt.texture;
		return tex;
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

			int apUsed = levelReference.gameObject.GetComponent<ActionQueue>().SpentActionPointsForPlayer(players[i]);
			if(apUsed!=-1){
				apPlayer[i] -= apUsed;
			}

			hpPlayer[i] = playerReferences[i].HealthPoints;
			primaryWeapon[i] = playerReferences[i].PrimaryWeapon;

			List<GameObject> itemsPlayer = playerReferences[i].Items;
			inventory[i] = new int[] {0,0,0,0};
			for(int m=0;m<itemsPlayer.Count;m++){
				if(m<4 && itemsPlayer[m]!=null){
					inventory[i][m] = Array.IndexOf(itemNames,itemsPlayer[m].name)+1;
				}
			}

			//if player has picked an item
			if(playerReferences[i].GhostItem!=null/* && !hasGhostItem[i]*/){
				for(int j=0;j<itemNames.Length;j++){
					//Debug.Log(itemNames[j]);
					if(itemNames[j]==playerReferences[i].GhostItem.name){
						int index = Array.IndexOf(inventory[i],0);
						//Debug.Log(index);
						inventory[i][index] = j+1;
						//Debug.Log("index");
						//Debug.Log(j+1);
						//Debug.Log(i);
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


		for (int i = 0; i<windowRect.Length; i++) {


			Vector2 startPos = new Vector2(playerReferences[i].FinalPosition.x * 32,playerReferences[i].FinalPosition.y*32);//Pfusch
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
		Rect[] playerRects = {new Rect (0, Screen.height - SIZE_PLAYER_BUTTONS, SIZE_PLAYER_BUTTONS, SIZE_PLAYER_BUTTONS),
					new Rect (0, 0, SIZE_PLAYER_BUTTONS, SIZE_PLAYER_BUTTONS),
					new Rect (Screen.width - SIZE_PLAYER_BUTTONS, 0, SIZE_PLAYER_BUTTONS, SIZE_PLAYER_BUTTONS),
					new Rect (Screen.width - SIZE_PLAYER_BUTTONS, Screen.height - SIZE_PLAYER_BUTTONS, SIZE_PLAYER_BUTTONS, SIZE_PLAYER_BUTTONS)
				};
		Vector2[] pivotPoints = {
			new Vector2 (25, Screen.height - SIZE_PLAYER_BUTTONS / 2),
			new Vector2 (25, 25),
			new Vector2 (Screen.width - SIZE_PLAYER_BUTTONS / 2, 25),
			new Vector2 (Screen.width - SIZE_PLAYER_BUTTONS / 2, Screen.height - SIZE_PLAYER_BUTTONS / 2)
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
			float currentHPDamage = ((float)infectedDamage[windowID])/((float)hpWidth/100.0f);
			float hpDamage = hpPlayer[windowID]/100.0f - ((float)currentHPDamage/100.0f);
			if(hpDamage<0){
				hpDamage=0.0f;
			}
			Debug.Log(hpDamage);
			if(hpDamage<0.5f){
				fillColor = new Color(1,(2*hpDamage),0);
			} else if(hpDamage>0.5f){
				fillColor = new Color(2*(1-hpDamage),1,0);
			} else {
				fillColor = new Color(1,1,0);
			}
			//fillColor = new Color(1-hpDamage,hpDamage,0);
		} else {
			if(hpPlayer[windowID]<50){
				fillColor = new Color(1,(2*(hpPlayer[windowID]/100.0f)),0);
			} else if(hpPlayer[windowID]>50){
				fillColor = new Color(2*(1-(hpPlayer[windowID]/100.0f)),1,0);
			} else {
				fillColor = new Color(1,1,0);
			}
			//fillColor = new Color(1-(hpPlayer[windowID]/100.0f),(hpPlayer[windowID]/100.0f),0);
		}

		Texture2D fillTex = createBlankTexture (fillColor);
		GUI.DrawTexture (new Rect(hpPos.x,hpPos.y,hpWidth * (hpPlayer[windowID]/100.0f),hpHeight),fillTex);
		if (infectedPlayer [windowID]) {
			infectedTimer[windowID] += Time.deltaTime;
			if(infectedTimer[windowID]>0.2f){
				infectedDamage[windowID]++;
				infectedTimer[windowID] = 0.0f;
			}
			GUI.DrawTexture (new Rect((hpPos.x+hpWidth * (hpPlayer[windowID]/100.0f))-infectedDamage[windowID],hpPos.y,infectedDamage[windowID],hpHeight),whiteTex);
			float stopValue = ((float)hpWidth/100.0f)*(float)infectionDamage;
			if(infectedDamage[windowID]==Mathf.Round(stopValue)){
				infectedDamage[windowID]=0;
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
				if(attack){
					actionButtonsActive[0] = true;
				} else {//only toggle button if the action is not attack
					actionButtonsActive[0] = !actionButtonsActive[0];
				}
				actionButtonsActive[1] = false;
				if(actionButtonsActive[0]){
					if(attack){
						//notice world that attack number has decreased
						levelReference.gameObject.GetComponent<ActionQueue>().RemoveLastAction(addAttack[0],addAttack[1]);
						Debug.Log("notice world that attack number has decreased");

					}
					if(firstAction==ablegen){
						int id = inventory[windowID][inventoryActive];
						levelReference.StartDropItem(itemNames[id-1]);
					}
				}
			}
			if(inventoryActive!=-1 /*&& !items[inventory[windowID][inventoryActive]-1].GetComponent<ItemProperties>().isArmor*/){
				Texture secondAction = (attack) ? attackButtons[1] : benutzen;
				if (GUI.Button (new Rect (96, 0, 32, 32), secondAction)) {
					if(attack){
						actionButtonsActive[1] = true;
					} else { //only toggle button if the action is not attack
						actionButtonsActive[1] = !actionButtonsActive[1];
					}
					actionButtonsActive[0] = false;
					if(actionButtonsActive[1]){

						if(attack){//nicht optimal
							//notice world that attack number has increased
							if(players[windowID].GetComponent<PlayerActions>().ActionPoints - levelReference.gameObject.GetComponent<ActionQueue>().SpentActionPointsForPlayer(players[windowID]) >= players[windowID].GetComponent<PlayerActions>().Items[players[windowID].GetComponent<PlayerActions>().ActiveItem].GetComponent<ItemProperties>().UsageCostInActionpoints){
								Debug.Log("new attack");
								levelReference.gameObject.GetComponent<ActionQueue>().AddAction(addAttack[0],addAttack[1],addAttack[2]);
							}
						}
						int id = inventory[windowID][inventoryActive];
						if(secondAction==benutzen && !items[id-1].GetComponent<ItemProperties>().isWeapon){
							Debug.Log("vor start use item");
							levelReference.StartUseItem(itemNames[id-1]);
						} else if(items[id-1].GetComponent<ItemProperties>().isWeapon){
							levelReference.DrawMovement(playerReferences[windowID].FinalPosition,playerReferences[windowID].ActionPoints,playerReferences[windowID].LastAction);
							levelReference.UseItem = null;
							levelReference.DropItem = null;
						}
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
					//stop actions in world
					levelReference.DrawMovement(playerReferences[windowID].FinalPosition,playerReferences[windowID].ActionPoints,playerReferences[windowID].LastAction);
					levelReference.DropItem = null;
					levelReference.UseItem = null;
				}
			} else {
				int index = inventory [windowID][i] - 1;
				if(GUI.Button (new Rect (32 * i, 32, 32, 32), inventoryIcons [index]) && windowID==activePlayer && ghostNumber[windowID]!=i){
					if(inventoryUsed[windowID,i]){
						//dropped item click-->player gets the item back
						inventoryUsed[windowID,i] = false;
						Debug.Log("Item has been grabbed again->Remove from ActionQueue");
						int ind = inventory [windowID][i]-1;
						levelReference.gameObject.GetComponent<ActionQueue>().RemoveAction(players[windowID],items[ind]);
					} else {
						//valid item click
						if(items[inventory[windowID][i]-1].GetComponent<ItemProperties>().isWeapon){
							//say to player that main weapon has changed
							primaryWeapon[windowID] = i;
							playerReferences [windowID].PrimaryWeapon = i;
							playerReferences [windowID].ActiveItem = i;
							Debug.Log("Main Weapon Change");
						}
						Array.Clear(actionButtonsActive,0,actionButtonsActive.Length);
						if(inventoryActive==i){
							inventoryActive = -1;
							Debug.Log("display stopped");
							levelReference.DropItem = null;
							levelReference.UseItem = null;
							levelReference.DrawMovement(playerReferences[windowID].FinalPosition,playerReferences[windowID].ActionPoints,playerReferences[windowID].LastAction);
						} else {
							playerReferences [windowID].ActiveItem = i;
							inventoryActive = i;
							actionButtonsActive[inventoryPreSelected[index]] = true;
							if(inventoryPreSelected[index]==0){
								levelReference.StartDropItem(itemNames[index]);
							} else if(inventoryPreSelected[index]==1){
								if(!items[inventory[windowID][i]-1].GetComponent<ItemProperties>().isWeapon){
									levelReference.StartUseItem(itemNames[index]);
								} else {
									levelReference.DropItem = null;
									levelReference.UseItem = null;
									levelReference.DrawMovement(playerReferences[windowID].FinalPosition,playerReferences[windowID].ActionPoints,playerReferences[windowID].LastAction);
								}
							}
						}
						attack = false;

						if(items[inventory[windowID][i]-1].GetComponent<ItemProperties>().isArmor){
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


	public void dropActionFromWorld(){
		Debug.Log("drop success");
		//item ghosten && actionbuttons hide
		/*if (inventoryActive != -1) {
			inventoryUsed[activePlayer,inventoryActive] = true;
			inventoryActive = -1;
			Array.Clear(actionButtonsActive,0,actionButtonsActive.Length);
		}*/
		if(inventoryActive == primaryWeapon[activePlayer]){
			primaryWeapon[activePlayer] = -1;
			playerReferences[activePlayer].PrimaryWeapon = -1;
		}
		inventoryActive = -1;
		Array.Clear(actionButtonsActive,0,actionButtonsActive.Length);
	}

	public void useActionFromWorld(){
		Debug.Log("use success");
		if (inventoryActive != -1) {
			inventoryUsed[activePlayer,inventoryActive] = true;
			inventoryActive = -1;
			Array.Clear(actionButtonsActive,0,actionButtonsActive.Length);
		}
	}

	public void roundEnd(){
		Debug.Log ("GUI Round End");
		//variable zurücksetzen
		activePlayer = 10;
		Array.Clear (activeGUIPlayer,0,activeGUIPlayer.Length);
		inventoryActive = -1;
		for (int i=0; i<4; i++) {
			for(int j=0;j<4;j++){
				inventoryUsed[i,j] = false;
			}
		}
		Array.Clear (hasGhostItem,0,hasGhostItem.Length);
		for (int i=0; i<ghostNumber.Length; i++) {
			ghostNumber[i] = 5;
		}
	}

	public void receiveAttackFromWorld(GameObject from,GameObject item,GameObject to){
		attack = true;
		addAttack = new GameObject[3];
		addAttack [0] = from;
		addAttack [1] = item;
		addAttack [2] = to;

		inventoryActive = primaryWeapon [activePlayer];
	}

}