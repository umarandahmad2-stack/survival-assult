using UnityEngine;
using OperationLostTrio;

public class CloudBuildBootstrap : MonoBehaviour
{
    public static Transform Player;
    void Start()
    {
        if (FindObjectOfType<VisualWorldBuilder>() == null)
        {
            var world = new GameObject("World Builder");
            var builder = world.AddComponent<VisualWorldBuilder>();
            builder.buildOnStart = true;
            builder.buildingRows = 5;
        }

        var player = new GameObject("Player");
        player.transform.position = new Vector3(0, 1.2f, 0);
        var controller = player.AddComponent<CharacterController>();
        controller.height = 1.8f; controller.radius = 0.35f; controller.center = new Vector3(0, 0, 0);
        player.AddComponent<FirstPersonController>();
        player.AddComponent<Health>();
        var cameraObject = new GameObject("Player Camera");
        cameraObject.transform.SetParent(player.transform, false);
        cameraObject.transform.localPosition = new Vector3(0, 0.55f, 0);
        var camera = cameraObject.AddComponent<Camera>();
        camera.fieldOfView = 78f;
        cameraObject.AddComponent<AudioListener>();
        var fps = player.GetComponent<FirstPersonController>();
        fps.cameraRoot = cameraObject.transform;
        var weapon = player.AddComponent<WeaponRaycast>();
        weapon.aimCamera = camera;
        var interact = player.AddComponent<InteractRaycaster>();
        interact.viewCamera = camera;
        Player = player.transform;

        var lightObject = new GameObject("Sun");
        var light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional; light.intensity = 1.15f;
        lightObject.transform.rotation = Quaternion.Euler(45, -30, 0);

        for (int i = 0; i < 5; i++) SpawnZombie(new Vector3((i % 2 == 0 ? -1 : 1) * Random.Range(8f, 16f), 1f, Random.Range(18f, 48f)));
    }

    void SpawnZombie(Vector3 position)
    {
        var zombie = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        zombie.name = "Infected";
        zombie.transform.position = position;
        zombie.transform.localScale = new Vector3(0.85f, 1.05f, 0.85f);
        var renderer = zombie.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = new Color(0.28f, 0.38f, 0.25f);
        zombie.AddComponent<Health>().maxHealth = 100f;
        zombie.AddComponent<CloudZombieAI>().target = Player;
    }

    void OnGUI()
    {
        var health = Player != null ? Player.GetComponent<Health>() : null;
        var weapon = Player != null ? Player.GetComponent<WeaponRaycast>() : null;
        GUI.color = Color.white;
        GUI.Box(new Rect(18, 18, 280, 96), "");
        GUI.Label(new Rect(30, 28, 260, 24), "DEAD ZONE  |  SURVIVAL");
        GUI.Label(new Rect(30, 52, 260, 22), "HEALTH: " + (health != null ? health.Current.ToString("0") : "100"));
        GUI.Label(new Rect(30, 74, 260, 22), "AMMO: " + (weapon != null ? weapon.AmmoInMagazine + " / " + weapon.reserveAmmo : "12 / 60"));
        GUI.Box(new Rect(18, Screen.height - 58, 390, 40), "WASD Move  |  Mouse Aim  |  Click Shoot  |  R Reload  |  Shift Sprint  |  Esc Unlock");
        if (health != null && health.IsDead)
        {
            GUI.color = Color.red;
            GUI.Box(new Rect(Screen.width / 2 - 160, Screen.height / 2 - 40, 320, 80), "YOU WERE OVERRUN  -  Refresh to retry");
        }
    }
}

public class CloudZombieAI : MonoBehaviour
{
    public Transform target;
    float nextAttack;
    void Update()
    {
        if (target == null) return;
        Vector3 delta = target.position - transform.position;
        delta.y = 0;
        float distance = delta.magnitude;
        if (distance > 1.35f)
        {
            transform.position += delta.normalized * (distance < 10f ? 2.1f : 1.2f) * Time.deltaTime;
            if (delta.sqrMagnitude > 0.01f) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(delta), 6f * Time.deltaTime);
        }
        else if (Time.time >= nextAttack)
        {
            nextAttack = Time.time + 1.1f;
            var health = target.GetComponent<Health>();
            if (health != null) health.Damage(8f);
        }
    }
}