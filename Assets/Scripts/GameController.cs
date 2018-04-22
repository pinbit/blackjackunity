using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Text PlayingfieldText;

	int[] DealerCards = new int [22];
	int[] PlayerCards = new int [22];

	int playerDrawCount = 2;
	int dealerDrawCount = 2;

	int[] cards = new int[43];

	public void DrawButton(){
		PlayerCards [playerDrawCount] = DrawCard ();
		playerDrawCount++;
		if (isBurst ()) {
			PlayingfieldText.text += "," + PlayerCards [playerDrawCount -1] + "バーストしました";
		} else {
			SetPlayfieldText ();
		}
	}

	public void HikanaiButtton(){
		StartCoroutine ("DealerGaHiku");
	}

	public void ResetButton(){
		Application.LoadLevel ("GamePlay");
	}

	IEnumerator DealerGaHiku(){
		int sumOfPlayerCards = 0;
		for (int i = 0; i < 22; i++) {
			sumOfPlayerCards += PlayerCards [i];
		}
		PlayingfieldText.text += "合計：" + sumOfPlayerCards;

		while (SumOfDealerCards () < 17 && SumOfDealerCards() < sumOfPlayerCards) {
			DealerCards [dealerDrawCount] = DrawCard ();
			dealerDrawCount++;
			SetPlayfieldText ();


			yield return new WaitForSeconds (0.3f);
		}

		int sumOfDealerCards = SumOfDealerCards ();

		if (sumOfDealerCards > 21 || SumOfDealerCards () < sumOfPlayerCards) {
			PlayingfieldText.text += "\nあなたの勝ち";
		} else {
			PlayingfieldText.text += "\nあなたの負け";
		}
	}

	int SumOfDealerCards(){
		int sum = 0;
		for (int i = 0; i < 22; i++) {
			sum += DealerCards [i];
		}
		return sum;
	}

	// Use this for initialization
	void Start () {

		ResetCards ();
		DealerAndPlayerDraw2Cards ();
		SetPlayfieldText ();
	}

	void ResetCards(){
		for (int i = 0; i < cards.Length; i++) {
			cards [i] = i % 13 + 1;

			if (cards [i] > 10) {
				cards [i] = 10;
			}
		}

		//shuffle cards
		for (int i=0;i<cards.Length;i++){
			int temp = cards [i];
			int rand = Random.Range (0, cards.Length);
			cards [i] = cards[rand];
			cards [rand] = temp;
		}
	}

	void DealerAndPlayerDraw2Cards(){
		DealerCards [0] = DrawCard ();
		DealerCards [1] = DrawCard ();
		PlayerCards [0] = DrawCard ();
		PlayerCards [1] = DrawCard ();
	}

	int DrawCard(){
		int number;

		do {
			number = Random.Range (0, cards.Length);
		} while(cards[number] == -1);

		int cardNumber = cards [number];
		cards [number] = -1;

		return cardNumber;
	}

	bool isBurst(){
		int sum = 0;
		for (int i = 0; i < 22; i++) {
			sum += PlayerCards [i];
		}
		if (sum > 21) {
			return true;
		}
		return false;
	}

	void SetPlayfieldText(){
		string text;
		int i = 1;

		if (DealerCards [2] == 0) {
			text = "Dealer:?";
			i = 1;
			while (DealerCards [i] != 0) {
				text += "," + DealerCards [i];
				i++;
			}
		} else {
			text = "Dealer:";

			i = 0;
			while (DealerCards [i] != 0) {
				text += "," + DealerCards [i];
				i++;
			}
		}
		text += "\nPlayer:";
		i = 0;

		while (PlayerCards [i] != 0) {
			text += "," + PlayerCards [i];
			i++;
		}
		i = 0;

		PlayingfieldText.text = text;
	}
}
