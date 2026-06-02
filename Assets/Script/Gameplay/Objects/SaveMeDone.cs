using UnityEngine;

public class SaveMeDone : MonoBehaviour {

	public void ClickResume()
    {
        if(PlayerSettingsService.IsSoundEnabled())
        {
            AudioListener.pause = false;
        }
        gameObject.SetActive(false);
        GameState.isMoveMap = true;
        Time.timeScale = 1;
        GameState.isGamePaused = false;
    }
}
