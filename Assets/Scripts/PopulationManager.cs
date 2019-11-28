using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

	public GameObject botPrefab;
	public GameObject startingPos;
	public int populationSize = 50;
	List<GameObject> population = new List<GameObject>();
	public static float elapsed = 0;
	public float trialTime = 5;
	int generation = 1;

	GUIStyle guiStyle = new GUIStyle();
	void OnGUI()
	{
		this.guiStyle.fontSize = 25;
		this.guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup (new Rect (10, 10, 250, 150));
		GUI.Box (new Rect (0,0,140,140), "Stats", this.guiStyle);
		GUI.Label(new Rect (10,25,200,30), "Gen: " + this.generation, this.guiStyle);
		GUI.Label(new Rect (10,50,200,30), string.Format("Time: {0:0.00}", elapsed), this.guiStyle);
		GUI.Label(new Rect (10,75,200,30), "Population: " + this.population.Count, this.guiStyle);
		GUI.EndGroup ();
	}

	// Use this for initialization
	void Start () {
		for(int i = 0; i < this.populationSize; i++)
		{
			GameObject b = Instantiate(this.botPrefab, this.startingPos.transform.position, this.transform.rotation);
            b.transform.SetParent(this.transform);
			b.GetComponent<Brain>().Init();
			this.population.Add(b);
		}
        Time.timeScale = 5;
	}

	GameObject Breed(GameObject parent1, GameObject parent2)
	{
		GameObject offspring = Instantiate(this.botPrefab, this.startingPos.transform.position, this.transform.rotation);
        offspring.transform.SetParent(this.transform);
        Brain b = offspring.GetComponent<Brain>();
		if(Random.Range(0, 10) == 0) //mutate 1 in 100
		{
			b.Init();
			b.dna.Mutate();
		}
		else
		{ 
			b.Init();
			b.dna.Combine(parent1.GetComponent<Brain>().dna,parent2.GetComponent<Brain>().dna);
		}
		return offspring;
	}

	void BreedNewPopulation()
	{
		List<GameObject> sortedList = this.population.OrderBy(o => (o.GetComponent<Brain>().distanceTravelled - o.GetComponent<Brain>().crash)).ToList();
		
		this.population.Clear();
        // breed top 25%
		for (int i = (int) (3 * sortedList.Count / 4.0f) - 1; i < sortedList.Count - 1; i++)
		{
    		this.population.Add(Breed(sortedList[i], sortedList[i + 1]));
    		this.population.Add(Breed(sortedList[i + 1], sortedList[i]));
    		this.population.Add(Breed(sortedList[i], sortedList[i + 1]));
    		this.population.Add(Breed(sortedList[i + 1], sortedList[i]));
		}

		//destroy all parents and previous population
		for(int i = 0; i < sortedList.Count; i++)
		{
			Destroy(sortedList[i]);
		}
		this.generation++;
	}
	
	// Update is called once per frame
	void Update () {
		elapsed += Time.deltaTime;
		if(elapsed >= this.trialTime)
		{
			BreedNewPopulation();
			elapsed = 0;
		}
	}
}
