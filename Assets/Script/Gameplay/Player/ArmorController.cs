using UnityEngine;
using System.Collections;

public class ArmorController : MonoBehaviour
{

    void Update()
    {
        if (GameState.shield <= 0 )
        {
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy") )
        {
            GameState.shield.Value -= 10;
            MyPlaneController.instance.UpdateShield(10);
        }
        if (other.CompareTag("dandich"))
        {
            GameState.shield.Value -= 15;
            MyPlaneController.instance.UpdateShield(15);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("dandich2"))
        {
            GameState.shield.Value -= 25;
            MyPlaneController.instance.UpdateShield(25);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("dandich3"))
        {
            GameState.shield.Value -= 30;
            MyPlaneController.instance.UpdateShield(30);
        }
       
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("dandich3"))
        {
            GameState.shield.Value -= 30;
            MyPlaneController.instance.UpdateShield(30);
        }
        
    }

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("dandich3")) {
			
			GameState.shield.Value -= 20;
            MyPlaneController.instance.UpdateShield(20);
        }
	}
}