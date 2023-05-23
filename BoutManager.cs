using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutManager : MonoBehaviour
{
    public GameObject panelMain;
    public GameObject panelChargePhase;
    public GameObject panelEngagePhase;
    public Vector2 mapCentre;
    public float mapRadius;
    public float arenaRadius;
    public GameObject player;
    public GameObject enemy;
    public float boutRange;
    public float boutProjectionTime;
    public float chargePhaseSlowFactor;
    public float engagePhaseSlowFactor;
    private Player playerScript;
    private Enemy enemyScript;
    private bool inChargePhase;
    private bool inEngagePhase;

    // Start is called before the first frame update
    void Start()
    {
        panelChargePhase.SetActive(false);
        panelEngagePhase.SetActive(false);
        player.transform.position = player.transform.up * arenaRadius;
        enemy.transform.position = enemy.transform.up * arenaRadius;
        playerScript = player.GetComponent<Player>();
        enemyScript = enemy.GetComponent<Enemy>();
        playerScript.SetArenaRadius(arenaRadius);
        enemyScript.SetArenaRadius(arenaRadius);
        inChargePhase = false;
        inEngagePhase = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (inChargePhase || inEngagePhase)
        {
            Vector3 distance = player.transform.position.normalized - enemy.transform.position.normalized;
            playerScript.cam.transform.rotation = Quaternion.LookRotation(-distance, player.transform.up);
            if (inChargePhase && Mathf.Sqrt(distance.x * distance.x + distance.y * distance.y + distance.z * distance.z) <= boutRange / arenaRadius)
            {
                inChargePhase = false;
                inEngagePhase = true;
                panelChargePhase.SetActive(false);
                panelEngagePhase.SetActive(true);
                playerScript.SetSlowTimeFactor(engagePhaseSlowFactor);
                enemyScript.SetSlowTimeFactor(engagePhaseSlowFactor);
            }
            else if (inEngagePhase && Mathf.Sqrt(distance.x * distance.x + distance.y * distance.y + distance.z * distance.z) > boutRange / arenaRadius)
            {
                inEngagePhase = false;
                panelMain.SetActive(true);
                panelEngagePhase.SetActive(false);
                playerScript.SetInBout(false);
                playerScript.SetSlowTimeFactor(1);
                enemyScript.SetInBout(false);
                enemyScript.SetSlowTimeFactor(1);
            }
        }
        else
        {
            Quaternion rotation = transform.rotation;
            transform.rotation = Quaternion.LookRotation(player.transform.position, enemy.transform.position);
            Vector3 enemyPosition = transform.up;
            transform.rotation = Quaternion.LookRotation(player.transform.position, player.transform.forward);
            float angle = Vector3.Angle(transform.up, enemyPosition) + 90;
            enemyScript.SetMapSpritePosition(mapCentre + new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * Vector3.Angle(player.transform.position, enemy.transform.position) * mapRadius / 180f);
            transform.rotation = Quaternion.LookRotation(enemy.transform.position, player.transform.position);
            Vector3 playerPosition = transform.up;
            transform.rotation = Quaternion.LookRotation(enemy.transform.position, enemy.transform.forward);
            enemyScript.SetMapSpriteBearing(angle + Vector3.Angle(transform.up, playerPosition) + 180);
            transform.rotation = rotation;
            Vector3 futureDistance = playerScript.GetFuturePosition(boutProjectionTime).normalized - enemyScript.GetFuturePosition(boutProjectionTime).normalized;
            if (Mathf.Sqrt(futureDistance.x * futureDistance.x + futureDistance.y * futureDistance.y + futureDistance.z * futureDistance.z) <= boutRange / arenaRadius)
            {
                inChargePhase = true;
                panelMain.SetActive(false);
                panelChargePhase.SetActive(true);
                playerScript.SetInBout(true);
                playerScript.SetSlowTimeFactor(chargePhaseSlowFactor);
                enemyScript.SetInBout(true);
                enemyScript.SetSlowTimeFactor(chargePhaseSlowFactor);
            }
        }
    }
}
