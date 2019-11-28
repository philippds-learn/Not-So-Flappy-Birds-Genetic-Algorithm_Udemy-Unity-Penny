using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA {

	List<int> genes = new List<int>();
	int dnaLength = 0;
	int maxValues = 0;

	public DNA(int l, int v)
	{
		this.dnaLength = l;
		this.maxValues = v;
		SetRandom();
	}

	public void SetRandom()
	{
		this.genes.Clear();
		for(int i = 0; i < this.dnaLength; i++)
		{
			this.genes.Add(Random.Range(-this.maxValues, this.maxValues));
		}
	}

	public void SetInt(int pos, int value)
	{
		this.genes[pos] = value;
	}

	public void Combine(DNA d1, DNA d2)
	{
		for(int i = 0; i < this.dnaLength; i++)
		{
			this.genes[i] = Random.Range(0,10) < 5 ? d1.genes[i] : d2.genes[i];
		}
	}

	public void Mutate()
	{
		this.genes[Random.Range(0, this.dnaLength)] = Random.Range(-this.maxValues, this.maxValues);
	}

	public int GetGene(int pos)
	{
		return this.genes[pos];
	}
}
