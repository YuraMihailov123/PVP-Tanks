using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buMove : Photon.MonoBehaviour
{
    public bool devTesting = false;
    private Vector3 selfPos;
    //public plMove MplMove;
    public float dirX, dirY;
    public float moveSpeed = 50f;
   
    
    // Update is called once per frame
    void Update()
    {

        
        if (!devTesting)
        {

            if (photonView.isMine)
            {
                transform.position += new Vector3(1 * dirX, 1 * dirY, 0) * moveSpeed * Time.deltaTime;                
            }
            else
            {
                smoothNetMovement();               
                
            }
        }
        else
        {
            transform.position += new Vector3(1 * dirX, 1 * dirY, 0) * moveSpeed * Time.deltaTime;
        }
    }



    private void smoothNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
        //gameObject.GetComponent<SpriteRenderer>().sprite = sprDir[currSpr];
    }
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            //stream.SendNext(currSpr);
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
            //currSpr = (int)stream.ReceiveNext();
        }
    }
}
