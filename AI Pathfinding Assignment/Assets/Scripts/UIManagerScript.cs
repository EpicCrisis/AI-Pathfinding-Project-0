using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{

	[Header ("===GameObject Adjust===")]
	//public GameObject[] UIButtons;
	public GameObject PlayButton;
	public GameObject StopButton;
	public GameObject StepButton;

	public GameObject MoreSizeButton;
	public GameObject LessSizeButton;

	[Header ("===Slider Adjust===")]
	//public Slider[] UISliders;
	public Slider mapSlider;
	public Slider timeSlider;

    [Header("===Text Adjust===")]
    public Text mapSizeText;
    public Text updateIntervalText;

	public static UIManagerScript instance = null;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

    void Start()
    {
        mapSizeText.text = "Map Size: " + GridManager.instance.gridSizeX + "x" + GridManager.instance.gridSizeY;
        updateIntervalText.text = "Time Step: " + Mathf.Round(GridManager.instance.updateInterval * 1000.0f) + "ms";

		if (GridManager.instance.gridSizeX >= GridManager.instance.maxGridSizeX
			&& GridManager.instance.gridSizeY >= GridManager.instance.maxGridSizeY) {

			MoreSizeButton.SetActive (false);
		}  else if (GridManager.instance.gridSizeX <= GridManager.instance.minGridSizeX
			&& GridManager.instance.gridSizeY <= GridManager.instance.minGridSizeY) {

			LessSizeButton.SetActive (false);
		} 
    }

    public void StartSim ()
	{
		GridManager.instance.Run ();

		PlayButton.SetActive (false);
		StopButton.SetActive (true);
		StepButton.SetActive (false);
	}

	public void StopSim ()
	{
		GridManager.instance.Stop ();

		PlayButton.SetActive (true);
		StopButton.SetActive (false);
		StepButton.SetActive (true);
	}

	public void NextStep()
	{
		GridManager.instance.UpdateNodes();
	}

    // Clear map now automatically stops the simulation.
	public void ClearMap ()
	{
        StopSim();
		GridManager.instance.ResetNodes ();
	}

	public void ChangeMapSize (Slider slider)
	{
		ClearMap ();
		GridManager.instance.RemoveGrid ();

		GridManager.instance.gridSizeX = (int)slider.value * 25;
		GridManager.instance.gridSizeY = (int)slider.value * 25;

		GridManager.instance.CreateGrid (GridManager.instance.gridSizeX, GridManager.instance.gridSizeY);
	}

	public void IncreaseMapSize ()
	{
		if (GridManager.instance.gridSizeX < GridManager.instance.maxGridSizeX
            && GridManager.instance.gridSizeY < GridManager.instance.maxGridSizeY) {

			ClearMap ();
			GridManager.instance.RemoveGrid ();

			GridManager.instance.gridSizeX += 25;
			GridManager.instance.gridSizeY += 25;

			GridManager.instance.CreateGrid (GridManager.instance.gridSizeX, GridManager.instance.gridSizeY);

            mapSizeText.text = "Map Size: " + GridManager.instance.gridSizeX + "x" + GridManager.instance.gridSizeY;
        }

		if (GridManager.instance.gridSizeX >= GridManager.instance.maxGridSizeX
            && GridManager.instance.gridSizeY >= GridManager.instance.maxGridSizeY) {

			MoreSizeButton.SetActive (false);
		} else {
			MoreSizeButton.SetActive (true);
			LessSizeButton.SetActive (true);
		}
	}

	public void DecreaseMapSize ()
	{
		if (GridManager.instance.gridSizeX > GridManager.instance.minGridSizeX
            && GridManager.instance.gridSizeY > GridManager.instance.minGridSizeY) {

			ClearMap ();
			GridManager.instance.RemoveGrid ();

			GridManager.instance.gridSizeX -= 25;
			GridManager.instance.gridSizeY -= 25;

			GridManager.instance.CreateGrid (GridManager.instance.gridSizeX, GridManager.instance.gridSizeY);

            mapSizeText.text = "Map Size: " + GridManager.instance.gridSizeX + "x" + GridManager.instance.gridSizeY;
        }

        if (GridManager.instance.gridSizeX <= GridManager.instance.minGridSizeX
            && GridManager.instance.gridSizeY <= GridManager.instance.minGridSizeY) {

			LessSizeButton.SetActive (false);
		} else {
			MoreSizeButton.SetActive (true);
			LessSizeButton.SetActive (true);
		}
	}

	public void ChangeUpdateInterval (Slider slider)
	{
		GridManager.instance.updateInterval = slider.value;

        updateIntervalText.text = "Time Step: " + Mathf.Round(GridManager.instance.updateInterval * 1000.0f) + "ms";
    }
}
