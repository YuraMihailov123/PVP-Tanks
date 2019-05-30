using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class plMove : Photon.MonoBehaviour
{
    public bool devTesting = false;
    public GameObject bullet;
    public PhotonView photonView;
    public float moveSpeed = 100f;
    public int dirY = 0, dirX = 0;
    private Vector3 selfPos;
    private int selfHp;
    private int Hp=5;
    public Sprite[] sprDir;
    public Transform[] spBulls = new Transform[4];
    private int currSpr;
    private int currSpBull;
    public bool isReady = false,isMap=false;
    public Vector3 prevDir = new Vector3(0, 0, 0);
    private GameObject sp1, sp2;
    
    private Quaternion selfQuat;
    private photonHandler pH;
    public float timeCantBeHit,timeToStart;
    private void Start()
    {
        pH = GameObject.Find("photonDontDestroy").GetComponent<photonHandler>();
        isReady = true;
        timeToStart = 3;
        if (pH.isPlayScene)
        {
            isReady =false;
            
        }
        //timeCantBeHit = 3;
        
        photonView = PhotonView.Get(this);
        selfQuat = transform.rotation;
        sp1 = GameObject.Find("sp1");
        sp2 = GameObject.Find("sp2");
        Hp = 5;       
       
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = photonView.owner.NickName+" Hp:"+Hp;        
       
        if (photonView.owner.ID % 2 == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        }else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
        }
        
        currSpr = 0;
        currSpBull = 0;
        prevDir.y = 1;
        
    }

    private void Update()
    {

        if (timeToStart > 0 && pH.isPlayScene && PhotonNetwork.room.PlayerCount == 2)
        {
            //pH.textStartObj.transform.GetChild(0).GetComponent<Text>().text = ((int)timeToStart).ToString();
            timeToStart -= Time.deltaTime;
        }
        if (timeToStart <= 0 && pH.isPlayScene && PhotonNetwork.room.PlayerCount == 2)
        {
            isReady = true;
            
        }
        if (isReady)
        {
            if (!devTesting)
            {

                if (photonView.isMine)
                {
                    checkInput();
                    if (PhotonNetwork.room.PlayerCount == 2  && !pH.isPlayScene)
                    {
                        pH.isPlayScene = true;
                        Debug.Log("destr");
                        //PhotonNetwork.Destroy(gameObject);
                        moveScenePlay();                            
                        isMap = true;
                    }
                }
                else
                {
                    smoothNetMovement();
                   
                }
            }
            else
            {
                checkInput();
            }
        }
    }

    public void moveScenePlay()
    {
        //PhotonNetwork.DestroyAll();
        PhotonNetwork.LoadLevel("mainGame");
    }


    private void Respawn()
    {
        transform.rotation = selfQuat;
        if (photonView.owner.ID % 2  == 0)
        {
            transform.position = sp1.transform.position;
        }else
        {
            transform.position = sp2.transform.position;
        }
        timeCantBeHit = 3;
        currSpr = 0;
        currSpBull = 0;
        prevDir.y = 1;
        Hp = 5;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = photonView.owner.NickName + " Hp:" + Hp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.isMine)
        {
            return;
        }

        if (collision.tag == "Finish")
        {
            ApplyDamage();
        }

    }
    

    public void ApplyDamage()
    {
        if (timeCantBeHit <= 0)
        {
            Hp--;
            gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = photonView.owner.NickName + " Hp:" + Hp;
        }
    }

    private void borderHit()
    {
        if (gameObject.GetComponent<SpriteRenderer>().enabled)
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        else gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void checkInput()
    {
        if (timeCantBeHit > 0 && pH.isPlayScene)
        {
            timeCantBeHit -= Time.deltaTime;
            borderHit();
        }
        transform.rotation = selfQuat;
        if (Input.GetKeyDown(KeyCode.W))
        {
            //gameObject.GetComponent<SpriteRenderer>().sprite = sprDir[0];
            currSpr = 0;
            dirY = 1;
            dirX = 0;
            currSpBull = 0;
            prevDir = new Vector3(dirX, dirY, 0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //gameObject.GetComponent<SpriteRenderer>().sprite = sprDir[2];
            currSpr = 2;
            dirY = -1;
            dirX = 0;
            currSpBull = 3;
            prevDir = new Vector3(dirX, dirY, 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //gameObject.GetComponent<SpriteRenderer>().sprite = sprDir[1];
            currSpr = 1;
            dirY = 0;
            dirX = -1;
            currSpBull = 2;
            prevDir = new Vector3(dirX, dirY, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //gameObject.GetComponent<SpriteRenderer>().sprite = sprDir[3];
            currSpr = 3;
            dirY = 0;
            dirX = 1;
            currSpBull = 1;
            prevDir = new Vector3(dirX, dirY, 0);
        }
        if (Input.GetKeyUp(KeyCode.Space) && pH.isPlayScene)
        {
            bullet.GetComponent<buMove>().dirX = prevDir.x;
            bullet.GetComponent<buMove>().dirY = prevDir.y;
            PhotonNetwork.Instantiate(bullet.name,spBulls[currSpBull].transform.position, bullet.transform.rotation, 0);
            
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (photonView.isMine)
            {
                PhotonNetwork.Destroy(gameObject);
                
                //PhotonNetwork.LeaveRoom(true);
                PhotonNetwork.Disconnect();
                
            }
        }
        if (!Input.anyKey)
        {
            //prevDir = new Vector3(dirX, dirY, 0);
            dirY = 0;
            dirX = 0;
        }
        //var moveHor = new Vector3(Input.GetAxis("Horizontal"),0);
        //moveHor = new Vector3( 0, Input.GetAxis("Vertical"));
        gameObject.GetComponent<SpriteRenderer>().sprite = sprDir[currSpr];
        transform.position += new Vector3(1*dirX,1*dirY,0) * moveSpeed * Time.deltaTime;
        if (Hp <= 0)
        {
            Respawn();
        }
    }
    private void smoothNetMovement()
    {
        //Hp = selfHp;
        
        if (Hp <= 0)
        {
            Respawn();
        }
        //gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = photonView.owner.NickName + " Hp:" + Hp;
        transform.rotation = selfQuat;
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprDir[currSpr];
        

    }
    private void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(currSpr);
            stream.SendNext(Hp);
            stream.SendNext(selfQuat);
           
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
            currSpr = (int)stream.ReceiveNext();
            Hp = (int)stream.ReceiveNext();
            selfQuat = (Quaternion)stream.ReceiveNext();
           
        }
    }
}
