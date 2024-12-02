using System.Collections;
using UnityEngine;

public class PanelsManager : MonoBehaviour
{
	public PanelInstance[] Pages;

	public PanelInstance currentPanel;

	private void Start()
	{
		for (int i = 0; i < Pages.Length; i++)
		{
			Pages[i].gameObject.AddComponent<PanelInstance>();
		}
	}

	private void Awake()
	{
		if (Pages.Length > 0)
		{
			OpenPanel(Pages[0]);
		}
	}

	public void CloseAllPanels()
	{
		if (Pages.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < Pages.Length; i++)
		{
			Animator component = Pages[i].GetComponent<Animator>();
			if (component != null && component.isActiveAndEnabled)
			{
				component.SetBool("Open", false);
			}
			if (Pages[i].isActiveAndEnabled)
			{
				StartCoroutine(DisablePanelDeleyed(Pages[i]));
			}
		}
	}

	public void DisableAllPanels()
	{
		if (Pages.Length > 0)
		{
			for (int i = 0; i < Pages.Length; i++)
			{
				Pages[i].gameObject.SetActive(false);
			}
		}
	}

	public void CloseAllPanelsInTheScene()
	{
		PanelsManager[] array = (PanelsManager[])Object.FindObjectsOfType(typeof(PanelsManager));
		if (array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].CloseAllPanels();
			}
		}
	}

	public void OpenPanelByName(string name)
	{
		PanelInstance panelInstance = null;
		for (int i = 0; i < Pages.Length; i++)
		{
			if (Pages[i].name == name)
			{
				panelInstance = Pages[i];
				break;
			}
		}
		if (!(panelInstance == null))
		{
			panelInstance.PanelBefore = currentPanel;
			currentPanel = panelInstance;
			CloseAllPanels();
			Animator component = panelInstance.GetComponent<Animator>();
			if ((bool)component && component.isActiveAndEnabled)
			{
				component.SetBool("Open", true);
			}
			panelInstance.gameObject.SetActive(true);
		}
	}

	public bool IsPanelOpened(string name)
	{
		for (int i = 0; i < Pages.Length; i++)
		{
			if (Pages[i].name == name)
			{
				return Pages[i].gameObject.activeSelf;
			}
		}
		return false;
	}

	public bool TogglePanelByName(string name)
	{
		PanelInstance panelInstance = null;
		for (int i = 0; i < Pages.Length; i++)
		{
			if (Pages[i].name == name)
			{
				panelInstance = Pages[i];
				break;
			}
		}
		if (panelInstance == null)
		{
			return false;
		}
		if (currentPanel == panelInstance)
		{
			ClosePanel(panelInstance);
			return false;
		}
		panelInstance.PanelBefore = currentPanel;
		currentPanel = panelInstance;
		CloseAllPanels();
		Animator component = panelInstance.GetComponent<Animator>();
		if ((bool)component && component.isActiveAndEnabled)
		{
			component.SetBool("Open", true);
		}
		panelInstance.gameObject.SetActive(true);
		return true;
	}

	public void ClosePanelByName(string name)
	{
		PanelInstance panelInstance = null;
		for (int i = 0; i < Pages.Length; i++)
		{
			if (Pages[i].name == name)
			{
				panelInstance = Pages[i];
				break;
			}
		}
		if (!(panelInstance == null))
		{
			currentPanel = null;
			Animator component = panelInstance.GetComponent<Animator>();
			if ((bool)component && component.isActiveAndEnabled)
			{
				component.SetBool("Open", false);
			}
			StartCoroutine(DisablePanelDeleyed(panelInstance));
		}
	}

	public void ClosePanel(PanelInstance page)
	{
		if (!(page == null))
		{
			currentPanel = null;
			Animator component = page.GetComponent<Animator>();
			if ((bool)component && component.isActiveAndEnabled)
			{
				component.SetBool("Open", false);
			}
			StartCoroutine(DisablePanelDeleyed(page));
		}
	}

	public void OpenPanel(PanelInstance page)
	{
		if (!(page == null))
		{
			page.PanelBefore = currentPanel;
			currentPanel = page;
			CloseAllPanels();
			Animator component = page.GetComponent<Animator>();
			if ((bool)component && component.isActiveAndEnabled)
			{
				component.SetBool("Open", true);
			}
			page.gameObject.SetActive(true);
		}
	}

	public void OpenPreviousPanel()
	{
		if ((bool)currentPanel && (bool)currentPanel.PanelBefore)
		{
			CloseAllPanels();
			Animator component = currentPanel.PanelBefore.GetComponent<Animator>();
			if ((bool)component && component.isActiveAndEnabled)
			{
				component.SetBool("Open", true);
			}
			currentPanel.PanelBefore.gameObject.SetActive(true);
			currentPanel = currentPanel.PanelBefore;
		}
	}

	public void OpenPanelByNameNoPreviousSave(string name)
	{
		PanelInstance panelInstance = null;
		for (int i = 0; i < Pages.Length; i++)
		{
			if (Pages[i].name == name)
			{
				panelInstance = Pages[i];
				break;
			}
		}
		if (!(panelInstance == null))
		{
			currentPanel = panelInstance;
			CloseAllPanels();
			Animator component = panelInstance.GetComponent<Animator>();
			if ((bool)component && component.isActiveAndEnabled)
			{
				component.SetBool("Open", true);
			}
			panelInstance.gameObject.SetActive(true);
		}
	}

	private IEnumerator DisablePanelDeleyed(PanelInstance page)
	{
		bool closedStateReached = false;
		bool wantToClose = true;
		Animator anim = page.GetComponent<Animator>();
		if ((bool)anim && anim.enabled)
		{
			while (!closedStateReached && wantToClose)
			{
				if (anim.isActiveAndEnabled && !anim.IsInTransition(0))
				{
					closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName("Closing");
				}
				yield return new WaitForEndOfFrame();
			}
			if (wantToClose)
			{
				anim.gameObject.SetActive(false);
			}
		}
		else
		{
			page.gameObject.SetActive(false);
		}
	}
}
