using UnityEngine;

public class InventoryInput : MonoBehaviour
{
	[SerializeField] GameObject characterPanelGameObject;
	[SerializeField] GameObject equipmentPanelGameObject;
    [SerializeField] GameObject HeroInfoPanelGameObject;
    [SerializeField] GameObject InGameMenuGameObject;
    [SerializeField] KeyCode[] toggleCharacterPanelKeys;
	[SerializeField] KeyCode[] toggleInventoryKeys;
    [SerializeField] KeyCode[] toggleHeroInfoKeys;
    [SerializeField] KeyCode[] toggleMainMenuKeys;
    [SerializeField] bool showAndHideMouse = false;

	void Update()
	{
		ToggleCharacterPanel();
		ToggleInventory();
		ToggleHeroInfo();
		ToggleInGameMenu();
    }

	private void ToggleCharacterPanel()
	{
		for (int i = 0; i < toggleCharacterPanelKeys.Length; i++)
		{
			if (Input.GetKeyDown(toggleCharacterPanelKeys[i]))
			{
				characterPanelGameObject.SetActive(!characterPanelGameObject.activeSelf);

				if (characterPanelGameObject.activeSelf)
				{
					equipmentPanelGameObject.SetActive(true);
					ShowMouseCursor();
				}
				else
				{
					HideMouseCursor();
				}

				break;
			}
		}
	}

	private void ToggleInventory()
	{
		for (int i = 0; i < toggleInventoryKeys.Length; i++)
		{
			if (Input.GetKeyDown(toggleInventoryKeys[i]))
			{
				if (!characterPanelGameObject.activeSelf)
				{
					characterPanelGameObject.SetActive(true);
					equipmentPanelGameObject.SetActive(false);
					ShowMouseCursor();
				}
				else if (equipmentPanelGameObject.activeSelf)
				{
					equipmentPanelGameObject.SetActive(false);
				}
				else
				{
					characterPanelGameObject.SetActive(false);
					HideMouseCursor();
				}
				break;
			}
		}
	}

    private void ToggleHeroInfo()
    {
        for (int i = 0; i < toggleHeroInfoKeys.Length; i++)
        {
            if (Input.GetKeyDown(toggleHeroInfoKeys[i]))
            {
                if (!HeroInfoPanelGameObject.activeSelf)
                {
                    HeroInfoPanelGameObject.SetActive(true);
                }
                else
                {
                    HeroInfoPanelGameObject.SetActive(false);
                }
                break;
            }
        }
    }

    private void ToggleInGameMenu()
    {
        for (int i = 0; i < toggleMainMenuKeys.Length; i++)
        {
            if (Input.GetKeyDown(toggleMainMenuKeys[i]))
            {
                if (!InGameMenuGameObject.activeSelf)
                {
                    InGameMenuGameObject.SetActive(true);
                }
                else
                {
                    InGameMenuGameObject.SetActive(false);
                }
                break;
            }
        }
    }

    public void ShowMouseCursor()
	{
		if (showAndHideMouse)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}

	public void HideMouseCursor()
	{
		if (showAndHideMouse)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public void ToggleEquipmentPanel()
	{
		equipmentPanelGameObject.SetActive(!equipmentPanelGameObject.activeSelf);
	}
}
