using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePanelButton : MonoBehaviour {

	public void TogglePanel(GameObject panel)
    {
				//Changes the panel's active state. If it's not active, open it, and vice versa.
        panel.SetActive(!panel.activeSelf);
    }
}
