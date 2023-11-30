using UnityEngine;

public class InventoryInput : MonoBehaviour
{
	[SerializeField] GameObject characterPanelGameObject;
	[SerializeField] GameObject equipmentPanelGameObject;
    [SerializeField] GameObject HeroInfoPanelGameObject;
    [SerializeField] KeyCode[] toggleCharacterPanelKeys;
	[SerializeField] KeyCode[] toggleInventoryKeys;
    [SerializeField] KeyCode[] toggleHeroInfoKeys;
    [SerializeField] bool showAndHideMouse = true;

	void Update()
	{
		ToggleCharacterPanel();
		ToggleInventory();
		ToggleHeroInfo();

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
