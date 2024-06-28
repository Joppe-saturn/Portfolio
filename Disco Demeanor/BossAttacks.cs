using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    private GameObject playerObj;
    private bool isDoneWithAttack;
    private float timer;
    private int currentAttackIndex;

    [SerializeField] private GameObject audioManager;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    //Audio 0 will be for shooting
    //Audio 1 will be for dying

    [System.Serializable]
    public struct BossAttack
    {
        public GameObject bullet; //this is the bullet that will be shot
        public float attackTime; // the time it takes before the boss switches to another attack
        public int damage; //the damage every bullet does
        public float speed; //the speed the bullets travel
        public float randomBulletSpread; //this is just random bullet spread
        public float bulletSpread; //this is bullet spread 
        public float spreadRange; //this is how far the bullets spread from eachother
        public float intervalBetweenShots; //time between every round it shoots at you
        public float intervalBetweenRounds; //time between every bullet is shoots
        public int shotsPerRound; //how many times the boss shoots per round
        public float bulletLastTime; //how long before the bullets dissapear
        public float horizontalBulletOffset;
        public bool followPlayer; // Whether the boss looks at the player during attacks

        public BossAttack(GameObject bullet, float attackTime, int damage, float speed, float randomBulletSpread, float bulletSpread, float spreadRange, float intervalBetweenShots, float intervalBetweenRounds, int shotsPerRound, float bulletLastTime, float horizontalBulletOffset, bool followPlayer)
        {
            this.bullet = bullet;
            this.attackTime = attackTime;
            this.damage = damage;
            this.speed = speed;
            this.randomBulletSpread = randomBulletSpread;
            this.bulletSpread = bulletSpread;
            this.spreadRange = spreadRange;
            this.intervalBetweenShots = intervalBetweenShots;
            this.intervalBetweenRounds = intervalBetweenRounds;
            this.shotsPerRound = shotsPerRound;
            this.bulletLastTime = bulletLastTime;
            this.horizontalBulletOffset = horizontalBulletOffset;
            this.followPlayer = followPlayer;
        }
    }

    public List<BossAttack> attacks = new List<BossAttack>();
    private BossAttack currentAttack;

    private void OnDisable()
    {
        currentAttack = attacks[0];
        currentAttackIndex = 0;
        timer = 0;
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        // Gets the player object, so it doesn't have to do so every time it needs the player's position.
        playerObj = GameObject.FindGameObjectWithTag("Player");

        currentAttack = attacks[0];
        currentAttackIndex = 0;
        timer = 0;
        StartCoroutine(CombatShooting());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= currentAttack.attackTime)
        {
            isDoneWithAttack = true;
        }
    }

    public IEnumerator CombatShooting()
    {
        while(true)
        {
            while (!isDoneWithAttack)
            {
                int movingRight = 1;
                // Prevents a deadlock.
                yield return new WaitForFixedUpdate();

                // If the enemy is close enough to the enemy, it will start shooting at the player following
                // the set intervals. Only works for ranged enemies, not for melee ones, though it should be
                // easy to reuse for melee attacking code.
                yield return new WaitForSeconds(currentAttack.intervalBetweenRounds);
                if (audioManager != null)
                {
                    GameObject audioInstance = Instantiate(audioManager);
                    audioInstance.GetComponent<AudioSource>().clip = audioClips[0];
                    audioInstance.GetComponent<AudioSource>().volume = 0.5f;
                    audioInstance.GetComponent<AudioManager>().valuesSet = true;
                }
                for (int i = 0; i < currentAttack.shotsPerRound; i++)
                {
                    if (currentAttack.followPlayer) transform.LookAt(new Vector3(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z));
                    else transform.rotation = Quaternion.Euler(0, 180, 0);
                    Shoot();
                    if(currentAttackIndex == 1)
                    {
                        currentAttack.horizontalBulletOffset += 3 * movingRight;
                        if (currentAttack.horizontalBulletOffset > 40 || currentAttack.horizontalBulletOffset < -40) movingRight *= -1;
                    }
                    yield return new WaitForSeconds(currentAttack.intervalBetweenShots);
                }
            }
            isDoneWithAttack = false;
            currentAttackIndex += 1;
            if (currentAttackIndex == attacks.Count)
            {
                currentAttackIndex = 0;
            }
            currentAttack = attacks[currentAttackIndex];
            timer = 0;
        }
    }

    public void Shoot()
    {
        for (int i = 1; i <= currentAttack.bulletSpread; i++)
        {
            // Instantiates a bullet, shoots it in the direction the player is moving, then sets the values correctly.
            GameObject firedBullet = Instantiate(currentAttack.bullet, new Vector3(transform.position.x, 2, transform.position.z), transform.rotation * Quaternion.Euler(0, Random.Range(-currentAttack.randomBulletSpread, currentAttack.randomBulletSpread) + (currentAttack.spreadRange / (currentAttack.bulletSpread + 1) * i) - currentAttack.spreadRange / 2 + currentAttack.horizontalBulletOffset, 0));
            firedBullet.GetComponent<Bulletmove>().isFromPlayer = false;
            firedBullet.GetComponent<Bulletmove>().damage = currentAttack.damage;
            firedBullet.GetComponent<Bulletmove>().speed = currentAttack.speed;
            firedBullet.GetComponent<Bulletmove>().timeBeforeDelete = currentAttack.bulletLastTime;
        }
    }
}