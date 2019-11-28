using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
	int DNALength = 5;
	public DNA dna;
    public GameObject eyes;
    bool seeDownWall = false;
    bool seeUpWall = false;
    bool seeBottom = false;
    bool seeTop = false;
    Vector3 startPosition;
    public float timeAlive = 0;
    public float distanceTravelled = 0;
    public int crash = 0;
    bool alive = true;
    Rigidbody2D rb;
    
	public void Init()
	{
		//initialise DNA
        //0 forward
        //1 upwall
        //2 downwall
        //3 normal upward
        this.dna = new DNA(this.DNALength,200);
        this.transform.Translate(Random.Range(-1.5f,1.5f),Random.Range(-1.5f,1.5f),0);
        this.startPosition = this.transform.position;
        this.rb = this.GetComponent<Rigidbody2D>();
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "dead" ||
            collision.gameObject.tag == "top" ||
            collision.gameObject.tag == "bottom" ||
            collision.gameObject.tag == "upwall" ||
            collision.gameObject.tag == "downwall")
        {
            this.crash++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "dead")
        {
            this.alive = false;
        }
    }

    void Update()
    {
        if(!this.alive) return;

        this.seeUpWall = false;
        this.seeDownWall = false;
        this.seeTop = false;
        this.seeBottom = false;
        Vector3 eyesPosition = this.eyes.transform.position;
        Vector3 eyesForward = this.eyes.transform.forward;
        Vector3 eyesUp = this.eyes.transform.up;

        RaycastHit2D hit = Physics2D.Raycast(eyesPosition, eyesForward, 1.0f);
        Debug.DrawRay(eyesPosition, eyesForward * 1.0f, Color.red);
        Debug.DrawRay(eyesPosition, eyesUp  * 1.0f, Color.red);
        Debug.DrawRay(eyesPosition, -eyesUp  * 1.0f, Color.red);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "upwall")
            {
                this.seeUpWall = true;
            }
            else if(hit.collider.gameObject.tag == "downwall")
            {
                this.seeDownWall = true;
            }
        }

		hit = Physics2D.Raycast(eyesPosition, eyesUp, 1.0f);
		if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "top")
            {
                this.seeTop = true;
            }
        }

        hit = Physics2D.Raycast(eyesPosition, -eyesUp, 1.0f);
		if (hit.collider != null)
        {    
            if(hit.collider.gameObject.tag == "bottom")
            {
                this.seeBottom = true;
            }
        }
        this.timeAlive = PopulationManager.elapsed;
    }


    void FixedUpdate()
    {
        if(!this.alive) return;
        
        // read DNA
        float upForce = 0;
        float forwardForce = 1.0f; //dna.GetGene(0);

        if(seeUpWall)
        {
            upForce = this.dna.GetGene(0);
        }
        else if(seeDownWall)
        {
            upForce = this.dna.GetGene(1);
        }
        else if(seeTop)
        {
            upForce = this.dna.GetGene(2);
        }        
        else if(seeBottom)
        {
            upForce = this.dna.GetGene(3);
        }
        else
        {
            upForce = this.dna.GetGene(4);
        }

        this.rb.AddForce(this.transform.right * forwardForce);
        this.rb.AddForce(this.transform.up * upForce * 0.1f);
        this.distanceTravelled = Vector3.Distance(this.startPosition, this.transform.position);
    }
}

