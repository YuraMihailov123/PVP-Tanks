using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mapPart : Photon.MonoBehaviour
{
    private int hp;
    public int get;
    public PhotonView photonView;
    public photonHandler pH;
    private Text wintext,serText;
    public GameObject winTexxt;
    // Start is called before the first frame update
    private void Start()
    {
        get = 0;
        pH = GameObject.Find("photonDontDestroy").GetComponent<photonHandler>();
        photonView = PhotonView.Get(this);
        wintext = GameObject.Find("finish.Text").GetComponent<Text>();
        Time.timeScale = 1;
        if (gameObject.tag == "brick")
            hp = 5;
        if (gameObject.tag == "flag")
            hp = 1;
        if (gameObject.tag == "const")
            hp = 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.isMine)
        {
            //
            return;
        }
        if (collision.tag == "Finish")
        {
            if (gameObject.tag != "const")
                hp--;
            if (hp <= 0)
            {
                
                
                if (gameObject.tag == "flag")
                {
                    
                    if (gameObject.name == "gerb1")
                    {
                       
                        winTexxt.transform.GetChild(0).GetComponent<Text>().text = "Green Team" + " won!";
                        //winText.GetComponent<Text>().text = "Green Team"+ " won!";
                    }
                    else
                    {
                        
                        winTexxt.transform.GetChild(0).GetComponent<Text>().text = "Red Team" + " won!";
                        //winText.GetComponent<Text>().text = "Red Team" + " won!";
                    }
                    GameObject winText = PhotonNetwork.Instantiate(winTexxt.name, winTexxt.transform.position, Quaternion.identity, 0);
                    //Time.timeScale = 0;
                }
                PhotonNetwork.Destroy(gameObject);
            }
            pH.DestroyBullet(collision.gameObject);
        }
    }


   

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(get);
        }
        else
        {
            get = (int)stream.ReceiveNext();
        }
    }
}
