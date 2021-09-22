using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public Sprite gunArtwork;
    public Transform attackPoint;
    public GameObject muzzleFlash;
    public GameObject pfBullet;
    private Camera fpsCam;

    public new string name;
    public int level;

    public float shootForce;

    public int magazineSize, bulletsPerTap;
    public float timeBetweenShots, spread, reloadTime, timeBetweenShooting;
    public bool allowButtonHold;

    private int bulletsLeft;

    private bool shooting, readyToShoot, reloading;

    public bool allowInvoke = true;

    void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Start() {
        fpsCam = PlayerHandler.fpsCam;
        attackPoint = transform.GetChild(0);
    }

    void Update()
    {
        MyInput();
    }

    public void MyInput() {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0) {
            Shoot();
        }
    }

    private void Shoot() {
        readyToShoot = false;

        bulletsLeft--;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(.5f, .5f, 0));

        Vector3 target;
        if (Physics.Raycast(ray, out var hit))
            target = hit.point;
        else
            target = ray.GetPoint(75);

        Vector3 directionWithoutSpread = target - attackPoint.position;

        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(spreadX, spreadY, 0);

        GameObject currentBullet = ObjectPool.Instance.GetPooledObject();

        if(currentBullet != null)
        {
            currentBullet.transform.position = attackPoint.transform.position;
            currentBullet.transform.rotation = attackPoint.transform.rotation;
            currentBullet.SetActive(true);
        }

        currentBullet.transform.forward = directionWithSpread.normalized;

        var bulletSpeed = currentBullet.GetComponent<Bullet>().speed;
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * (shootForce + bulletSpeed), ForceMode.Impulse);

        if (allowInvoke) {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void ResetShot() {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload() {
        reloading = true;
        Invoke("ReloadFinish", reloadTime);
    }

    private void ReloadFinish() {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
