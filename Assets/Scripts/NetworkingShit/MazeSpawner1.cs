using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner1 : NetworkBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;



	public GameObject SpawnPoint;

	private BasicMazeGenerator mMazeGenerator = null;	

	[SyncVar]public int mapSeed = 0;
	[SyncVar]public int Rows = 0;
	[SyncVar]public int Columns = 0; 

	public override void OnStartServer()///on scene keist
	{
		mapSeed = Random.Range(0,1000);
		Rows = Random.Range(5, 10);
		Columns = Random.Range(5,10);
	}

	void Start () {
		
			Random.seed = mapSeed;
			//Debug.Log("dddddddddddddddddddddddddd    "+ mapSeed+"     "+Random.seed)
		
		switch (Algorithm) {
		case MazeGenerationAlgorithm.PureRecursive:
			mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveTree:
			mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RandomTree:
			mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.OldestTree:
			mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveDivision:
			mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
			break;
		}

		///////    sukurt i viena gameObject ir tada atspawnint.....prie sienu grindu pakeist server only,  gal seervas uzkrauna ir nusiuncia klientam
		mMazeGenerator.GenerateMaze ();
		for (int row = 0; row < Rows; row++) {
			for(int column = 0; column < Columns; column++){
				float x = column*(CellWidth+(AddGaps?.2f:0));
				float z = row*(CellHeight+(AddGaps?.2f:0));
				MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
				
				GameObject tmp;

				tmp = Instantiate(Floor,new Vector3(x,0.1f,z), Quaternion.Euler(0,0,0)) as GameObject;
				tmp.transform.parent = transform;
				if(cell.WallRight){
					tmp = Instantiate(Wall,new Vector3(x+CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,90,0)) as GameObject;// right
					tmp.transform.parent = transform;
				}
				if(cell.WallFront){
					tmp = Instantiate(Wall,new Vector3(x,0,z+CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,0,0)) as GameObject;// front
					tmp.transform.parent = transform;
				}
				if(cell.WallLeft){
					tmp = Instantiate(Wall,new Vector3(x-CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,270,0)) as GameObject;// left
					tmp.transform.parent = transform;
				}
				if(cell.WallBack){
					tmp = Instantiate(Wall,new Vector3(x,0,z-CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,180,0)) as GameObject;// back
					tmp.transform.parent = transform;
				}
                if (cell.IsGoal) //arba isvest lauk masyva su kordinatem arba bbz    registruot tik kelis
                {
                        tmp = Instantiate(SpawnPoint, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;

				}
				
				/* kamerai tikrint zaidejo vardo primas dvi raides
                /////padaryt kad galetum kokius keturis dadet skirtingus zmones
                if ((cell.IsGoal && play2 != null))   //pridet laukimo atstumaaaaaaa
                {/////padaryt kad skaiciuotu atstuma mmmmmmasmdasbjfgvadshfs
                    if (play1 != null)
                    {
                        tmp = Instantiate(play1, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;
                        play1 = null;
                    }
                    if (range >= 2) {
                        tmp = Instantiate(play2, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;
                        play2 = null;
                    }
                    range += 1;
				}*/
     
            }
		}
		if(Pillar != null){
			for (int row = 0; row < Rows+1; row++) {
				for (int column = 0; column < Columns+1; column++) {
					float x = column*(CellWidth+(AddGaps?.2f:0));
					float z = row*(CellHeight+(AddGaps?.2f:0));
					GameObject tmp = Instantiate(Pillar,new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.identity) as GameObject;
					tmp.transform.parent = transform;
				}
			}
		}




		
	}
}
