using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Shooting;

public class Shooting : MonoBehaviour
{
    public enum GunsToDrop
    {
        starterGun,
        assaultRifle,
        shotgun,
        sniperGun
    }
    [Header("For testing purposes, pick a gun to assign to the player.")]
    public GunsToDrop gunToSelect;
    [Header("This only works if the bool below is set to false.")]
    public bool useInventorySystem = true;
    [SerializeField] float bulletOffset;
    [SerializeField] GameObject hitScanLineTest;
    [Header("Bullets for the guns to fire should be put below in order of:")]
    [Header("Music Note Gun, Assault Rifle Gun, Soundwave/Shotgun, Sniper Gun.")]
    public List<GameObject> bullets = new List<GameObject>();
    public List<GameObject> muzzleFlashList = new List<GameObject>();
    private float timer;

    [SerializeField] private GameObject audioManager;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    //audio 0 is for shooting

    [System.Serializable]
    public struct Weapon
    {
        [Header("DON'T CHANGE NAME FROM THE EDITOR, DO SO IN THE SCRIPT.")]
        public string gunName;
        public int bulletDamage, pierceAmount;
        public float shootCooldown, lifeTime, bulletSpeed, randomBulletSpread, bulletSpread, spreadRange, offset;
        public bool isHitscan;
        public GameObject bulletToFire;
        public GameObject muzzleFlash;

        public Weapon(string gunName, float shootCooldown, float lifeTime, int bulletDamage, float bulletSpeed, float bulletSpread, float randomBulletSpread, float spreadRange, float offset, bool isHitscan, int pierceAmount, GameObject bulletToFire, GameObject muzzleFlash)
        {
            this.gunName = gunName;
            this.shootCooldown = shootCooldown;
            this.lifeTime = lifeTime;
            this.bulletDamage = bulletDamage;
            this.bulletSpeed = bulletSpeed;
            this.bulletSpread = bulletSpread;
            this.randomBulletSpread = randomBulletSpread;
            this.spreadRange = spreadRange;
            this.offset = offset;
            this.isHitscan = isHitscan;
            this.pierceAmount = pierceAmount;
            this.bulletToFire = bulletToFire;
            this.muzzleFlash = muzzleFlash;
        }
    }
    [HideInInspector] public Weapon currentlySelectedGun;
    [HideInInspector] public int currentlySelectedGunIndex = -1;
    [HideInInspector] public int previousGunIndex;

    // To add a new gun, simply define a new public Weapon variable, name it whatever you want to, and fill in the
    // variables in the following order: internal name, shooting cooldown, bullet damage, bullet speed, bullet spread,
    // random bullet spread, spread range, whether the weapon is hitscan or projectile, pierce, and the bullet object
    // the gun should fire, though the bullet specifically should be set in Awake(). Just set the default value to null.
    // Though these are public to be accessible from other scripts, be sure to edit their values
    // in the *script* when you're sure, not in the editor. Keeps everything nice and orderly.
    // (name, shootCooldown, lifeTime, bulletDamage, bulletSpeed, bulletSpread, randomBulletSpread, spreadRange, isHitscan, pierceAmount, null, null)
    public Weapon starterGun = new Weapon("startergun", 0.3f, 10, 3, 20f, 1, 0.2f, 180f, 0f, false, 0, null, null);
    public Weapon assaultRifle = new Weapon("assaultrifle", 0.1f, 10, 1, 0.6f, 1, 1, 180f, 0f, false, 0, null, null);
    public Weapon shotgun = new Weapon("shotgun", 1, 0.1f, 7, 0, 1, 0, 180f, 2f, false, -1, null, null);
    public Weapon sniperGun = new Weapon("sniper", 1.5f, 2.5f, 7, 1, 1, 0, 180f, 0f, true, 1, null, null);

    [HideInInspector] public List<Weapon> gunInventory = new();


    private void Awake()
    {
        // Sets the bullets for the respective guns to fire, set the bullet objects in the editor.
        starterGun.bulletToFire = bullets[0];
        assaultRifle.bulletToFire = bullets[1];
        shotgun.bulletToFire = bullets[2];
        starterGun.muzzleFlash = muzzleFlashList[0];
        assaultRifle.muzzleFlash = muzzleFlashList[1];
        shotgun.muzzleFlash = muzzleFlashList[2];
    }
    private void Start()
    {
        // Adds the basic gun when you start a stage.
        gunInventory.Add(starterGun);
        ChangeHeldGun(0);
    }
    private void Update()
    {
        // When you scroll the mouse wheel up or down, switch  in the respective direction.
        if (Input.mouseScrollDelta.y < 0) ChangeHeldGun(currentlySelectedGunIndex + 1 <= gunInventory.Count - 1 ? currentlySelectedGunIndex + 1 : 0);
        if (Input.mouseScrollDelta.y > 0) ChangeHeldGun(currentlySelectedGunIndex - 1 >= 0 ? currentlySelectedGunIndex - 1 : gunInventory.Count - 1);

        // When you press any of the number keys, switch guns to the respective index.
        int indexNumber = -1;
        for(int i = 0; i < gunInventory.Count; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString())){
                indexNumber = i;
            }
        }
        if (indexNumber != -1) ChangeHeldGun(indexNumber);

        // Pressing the Q key switches to the previously held gun.
        if (Input.GetKeyDown(KeyCode.Q) && previousGunIndex != -1) ChangeHeldGun(previousGunIndex);

        timer += Time.deltaTime;
        if (timer > currentlySelectedGun.shootCooldown && Input.GetKey(KeyCode.Mouse0))
        {
            /*for(int i = 0; i < recoil; i++)
            {
                //this one calls the playerMovement script that's also on the player. It does this because I'm lazy and this is a easy way to do recoil.
                //I don't think recoil is going to be that important so it doesn't really matter.
                StartCoroutine(GetComponent<PlayerMovement>().Move(-transform.forward * 0.1f));
            }*/

            for (int i = 1; i <= currentlySelectedGun.bulletSpread; i++)
            {
                //Player was walking in to the bullet in certain cases, added offset to prevent this
                Vector3 offsetSpawn = new Vector3(transform.position.x, 2, transform.position.z) + transform.forward * currentlySelectedGun.offset;
                //this one instantiates the bullet and gives the correct values to the bullet based on what gun you have
                if(GetComponent<Losecondition>().conditionState == Losecondition.ConditionState.Playing)
                {
                    if (!currentlySelectedGun.isHitscan)
                    {
                        if (audioManager != null)
                        {
                            GameObject audioInstance = Instantiate(audioManager);
                            audioInstance.GetComponent<AudioSource>().clip = audioClips[0];
                            audioInstance.GetComponent<AudioManager>().valuesSet = true;
                        }
                        GameObject firedBullet = Instantiate(currentlySelectedGun.bulletToFire, offsetSpawn, transform.rotation * Quaternion.Euler(0, Random.Range(-currentlySelectedGun.randomBulletSpread, currentlySelectedGun.randomBulletSpread) + (currentlySelectedGun.spreadRange / (currentlySelectedGun.bulletSpread + 1) * i) - currentlySelectedGun.spreadRange / 2, 0));
                        firedBullet.GetComponent<Bulletmove>().isFromPlayer = true;
                        firedBullet.GetComponent<Bulletmove>().damage = currentlySelectedGun.bulletDamage;
                        firedBullet.GetComponent<Bulletmove>().speed = currentlySelectedGun.bulletSpeed;
                        firedBullet.GetComponent<Bulletmove>().pierceAmount = currentlySelectedGun.pierceAmount;
                        firedBullet.GetComponent<Bulletmove>().timeBeforeDelete = currentlySelectedGun.lifeTime;
                    }
                    else
                    {
                        int hitScanLayerMask = (1 << 9) | (1 << 10);
                        if (Physics.Raycast(new Vector3(transform.position.x, 1.5f, transform.position.z), transform.forward, out RaycastHit hit, Mathf.Infinity, hitScanLayerMask))
                        {
                            if (audioManager != null)
                            {
                                GameObject audioInstance = Instantiate(audioManager);
                                audioInstance.GetComponent<AudioSource>().clip = audioClips[0];
                                audioInstance.GetComponent<AudioManager>().valuesSet = true;
                            }
                            GameObject hSL = Instantiate(hitScanLineTest, Vector3.zero, Quaternion.identity);
                            hSL.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                            hSL.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                            Destroy(hSL, currentlySelectedGun.lifeTime);
                            if (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Boss"))
                            {
                                hit.collider.gameObject.GetComponent<HealthManager>().TakeDamage(currentlySelectedGun.bulletDamage);
                            }
                        }
                    }
                    MuzzleFlash(offsetSpawn);
                    timer = 0;
                }
            }
            
        }
    }

    private void FixedUpdate()
    {
        // Adds functionality to select a gun from the editor instead of having to pick them up in the scene.
        Weapon weapon = new() { gunName = "undefined" };
        switch (gunToSelect)
        {
            case GunsToDrop.starterGun:
                weapon = starterGun;
                break;
            case GunsToDrop.assaultRifle:
                weapon = assaultRifle;
                break;
            case GunsToDrop.shotgun:
                weapon = shotgun;
                break;
            case GunsToDrop.sniperGun:
                weapon = sniperGun;
                break;
        }
        if(weapon.gunName != "undefined" && currentlySelectedGun.gunName != weapon.gunName && !useInventorySystem)
        {
            AddNewGun(weapon);
        }
    }

    private void MuzzleFlash(Vector3 offsetPos)
    {
        if (currentlySelectedGun.muzzleFlash != null)
        {
            Quaternion lookDir = gameObject.transform.GetChild(0).transform.rotation;
            GameObject flash = Instantiate(currentlySelectedGun.muzzleFlash, offsetPos, lookDir);
            Destroy(flash, 2f);
        }
    }

    public void ChangeHeldGun(int gunIndex)
    {
        // Changes the gun the player's currently holding. Self explanatory.
        if(gunIndex <= gunInventory.Count - 1)
        {
            if(currentlySelectedGunIndex != -1) previousGunIndex = currentlySelectedGunIndex;
            currentlySelectedGun = gunInventory[gunIndex];
            currentlySelectedGunIndex = gunIndex;
            timer = currentlySelectedGun.shootCooldown / 1.2f;
        }
    }

    public void AddNewGun(Weapon gunToAdd)
    {
        // Adds a new gun to the player inventory as long as the player doesn't already have it,
        bool alreadyHasGun = false;
        int alreadyAcquiredGunIndex = -1;
        for(int i = 0; i < gunInventory.Count; i++)
        {
            if (gunInventory[i].gunName == gunToAdd.gunName) alreadyHasGun = true;
            alreadyAcquiredGunIndex = i;
        }
        if (!alreadyHasGun)
        {
            gunInventory.Add(gunToAdd);
            // and automatically swaps to the new gun.
            ChangeHeldGun(gunInventory.Count - 1);
        } else
        {
            ChangeHeldGun(alreadyAcquiredGunIndex);
        }
    }
}
